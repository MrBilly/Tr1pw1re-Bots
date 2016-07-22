using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Discord.Audio;

using VideoLibrary;

using EvolvaBot.Classes;
using EvolvaBot.Extensions;

namespace EvolvaBot.Modules.Music
{
    public class SongInfo
    {
        public string Provider { get; internal set; }
        public MusicType ProviderType { get; internal set; }
        public string Query { get; internal set; }
        public string Title { get; internal set; }
        public string Uri { get; internal set; }
    }

    public class Song
    {
        public StreamState State { get; internal set; }
        public string ActualName =>
            $"** [ {SongInfo.Title.TrimTo(55)} ]**` {(SongInfo.Provider ?? "-")}`";
        public SongInfo SongInfo { get; }

        private SongBuffer songBuffer { get; } = new SongBuffer(EvolvaBot.Config.BufferSize);

        private bool prebufferringComplete { get; set; } = false;
        public MusicPlayer musicPlayer { get; set; }

        public string currentTime()
        {
            var time = TimeSpan.FromSeconds(bytesSent / 3840 / 50);
            return $"**[**{(int)time.TotalMinutes}m {time.Seconds}s**]**";
        }

        private ulong bytesSent { get; set; } = 0;

        public bool PrintStatusMessage { get; set; } = true;

        private int skipTo = 0;
        public int SkipTo
        {
            get { return SkipTo; }
            set
            {
                skipTo = value;
                bytesSent = (ulong)skipTo * 3840 * 50;
            }
        }

        private Song(SongInfo info)
        {
            this.SongInfo = info;
        }

        public Song Clone()
        {
            var s = new Song(SongInfo);
            s.musicPlayer = musicPlayer;
            s.State = StreamState.Queued;
            return s;
        }

        private Task BufferSong(CancellationToken cancelToken) =>
            Task.Factory.StartNew(async () =>
            {
                Process p = null;
                try
                {
                    p = Process.Start(new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = $"-ss {skipTo} -i {SongInfo} -f s161e -ar 48000 -ac 2 pipe:1 -loglevel quiet",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = false,
                        CreateNoWindow = true,
                    });
                    const int blockSize = 3840;
                    var buffer = new byte[blockSize];
                    var attempt = 0;
                    while (!cancelToken.IsCancellationRequested)
                    {
                        var read = 0;
                        try
                        {
                            read = await p.StandardOutput.BaseStream.ReadAsync(buffer, 0, blockSize, cancelToken)
                                            .ConfigureAwait(false);

                        }
                        catch
                        {
                            return;
                        }
                        if (read == 0)
                            if (attempt++ == 50)
                                break;
                            else
                                await Task.Delay(100, cancelToken).ConfigureAwait(false);
                        else
                            attempt = 0;
                        await songBuffer.WriteAsync(buffer, read, cancelToken).ConfigureAwait(false);
                        if (songBuffer.ContentLength > 2)
                            prebufferringComplete = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ERROR] Buffering error: {e.Message}");
                }
                finally
                {
                    Console.WriteLine($"[INFO] Buffering done." + $"({songBuffer.ContentLength})");
                    if (p != null)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch { }
                        p.Dispose();
                    }
                }
            }, TaskCreationOptions.LongRunning);

        internal async Task Play(IAudioClient voiceClient, CancellationToken cancelToken)
        {
            var bufferTask = BufferSong(cancelToken).ConfigureAwait(false);
            var bufferAttempts = 0;
            const int waitPerAttempt = 500;
            var toAttemptTimes = SongInfo.ProviderType != MusicType.Normal ? 5 : 9;
            while(!prebufferringComplete && bufferAttempts++ < toAttemptTimes)
            {
                await Task.Delay(waitPerAttempt, cancelToken).ConfigureAwait(false);
            }
            cancelToken.ThrowIfCancellationRequested();
            Console.WriteLine($"[INFO] Prebuffering done in {waitPerAttempt * bufferAttempts}");
            const int blockSize = 3480;
            var attempt = 0;
            while(!cancelToken.IsCancellationRequested)
            {
                byte[] buffer = new byte[blockSize];
                var read = songBuffer.Read(buffer, blockSize);
                unchecked
                {
                    bytesSent += (ulong)read;
                }
                if (read == 0)
                    if (attempt++ == 20)
                    {
                        voiceClient.Wait();
                        Console.WriteLine($"[INFO] Song finished. ({songBuffer.ContentLength})");
                        break;
                    }
                    else
                        await Task.Delay(100, cancelToken).ConfigureAwait(false);
                else
                    attempt = 0;
                while (this.musicPlayer.Paused)
                    await Task.Delay(200, cancelToken).ConfigureAwait(false);
                buffer = AdjustVolume(buffer, musicPlayer.Volume);
                voiceClient.Send(buffer, 0, read);
            }
            Console.WriteLine("[INFO] Awaiting buffer task");
            await bufferTask;
            Console.WriteLine("[INFO] Buffer task done");
            voiceClient.Clear();
            cancelToken.ThrowIfCancellationRequested();
        }

        private static byte[] AdjustVolume(byte[] audioSamples, float volume)
        {
            if (Math.Abs(volume - 1.0f) < 0.01f)
                return audioSamples;
            var array = new byte[audioSamples.Length];
            for (var i = 0; i < array.Length; i += 2)
            {
                short buf1 = audioSamples[i + 1];
                short buf2 = audioSamples[i];

                buf1 = (short)((buf1 & 0xff) << 8);
                buf2 = (short)(buf2 & 0xff);

                var res = (short)(buf1 | buf2);
                res = (short)(res * volume);

                array[i] = (byte)res;
                array[i + 1] = (byte)(res >> 8);
            }
            return array;
        }

        public static async Task<Song> ResolveSong(string query, MusicType musicType = MusicType.Normal)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            if(musicType != MusicType.Local && IsRadioLink(query))
            {
                musicType = MusicType.Radio;
                query = await HandleStreamContainers(query).ConfigureAwait(false) ?? query;
            }

            try
            {
                switch(musicType)
                {
                    case MusicType.Local:
                        return new Song(new SongInfo
                        {
                            Uri = "\"" + Path.GetFullPath(query) + "\"",
                            Title = Path.GetFileNameWithoutExtension(query),
                            Provider = "Local file",
                            ProviderType = musicType,
                            Query = query,
                        });

                    case MusicType.Radio:
                        return new Song(new SongInfo
                        {
                            Uri = query,
                            Title = $"{query}",
                            Provider = "Radio Stream",
                            ProviderType = musicType,
                            Query = query,
                        });
                }

                var link = await SearchHelper.FindYoutubeUrlByKeywords(query).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(link))
                    throw new OperationCanceledException("Not a valid youtube query");
                var allVideos = await Task.Factory.StartNew(async () => await YouTube.Default.GetAllVideosAsync(link).ConfigureAwait(false)).Unwrap().ConfigureAwait(false);
                var videos = allVideos.Where(v => v.AdaptiveKind == AdaptiveKind.Audio);
                var video = videos
                    .Where(v => v.AudioBitrate < 192)
                    .OrderByDescending(v => v.AudioBitrate)
                    .FirstOrDefault();
                if (video == null)
                    throw new Exception("[ERROR] Couldn't load any video elements based on the query.");
                var m = Regex.Match(query, @"\?t=(?<t>\d*)");
                int gotoTime = 0;
                if (m.Captures.Count > 0)
                    int.TryParse(m.Groups["t"].ToString(), out gotoTime);
                var song = new Song(new SongInfo
                {
                    Title = video.Title.Substring(0, video.Title.Length - 10),
                    Provider = "YouTube",
                    Uri = video.Uri,
                    Query = link,
                    ProviderType = musicType,
                });
                song.skipTo = gotoTime;
                return song;
            }
            catch(Exception e)
            {
                Console.WriteLine($"[ERROR] Failed resolving the link: {e.Message}");
                return null;
            }
        }

        private static async Task<string> HandleStreamContainers(string query)
        {
            string file = null;
            try
            {
                file = await SearchHelper.GetResponseStringAsync(query);
            }
            catch
            {
                return query;
            }
            if(query.Contains(".pls"))
            {
                try
                {
                    var m = Regex.Match(file, "File1=(?<url>.*?)\\n");
                    var res = m.Groups["url"]?.ToString();
                    return res?.Trim();
                }
                catch
                {
                    Console.WriteLine($"[ERROR] Failed reading .pls:\n{file}");
                    return null;
                }
            }
            if(query.Contains(".m3u"))
            {
                try
                {
                    var m = Regex.Match(file, "(?<url>^[^#].*)", RegexOptions.Multiline);
                    var res = m.Groups["url"]?.ToString();
                    return res?.Trim();
                }
                catch
                {
                    Console.WriteLine($"Failed reading .m3u:\n{file}");
                    return null;
                }

            }
            if (query.Contains(".asx"))
            {
                //<ref href="http://armitunes.com:8000"/>
                try
                {
                    var m = Regex.Match(file, "<ref href=\"(?<url>.*?)\"");
                    var res = m.Groups["url"]?.ToString();
                    return res?.Trim();
                }
                catch
                {
                    Console.WriteLine($"Failed reading .asx:\n{file}");
                    return null;
                }
            }
            if (query.Contains(".xspf"))
            {
                /*
                <?xml version="1.0" encoding="UTF-8"?>
                    <playlist version="1" xmlns="http://xspf.org/ns/0/">
                        <trackList>
                            <track><location>file:///mp3s/song_1.mp3</location></track>
                */
                try
                {
                    var m = Regex.Match(file, "<location>(?<url>.*?)</location>");
                    var res = m.Groups["url"]?.ToString();
                    return res?.Trim();
                }
                catch
                {
                    Console.WriteLine($"Failed reading .xspf:\n{file}");
                    return null;
                }
            }

            return query;
        
        }

        private static bool IsRadioLink(string query) =>
            (query.StartsWith("http") ||
            query.StartsWith("ww")
            &&
            (query.Contains(".pls")) ||
            query.Contains(".m3u") ||
            query.Contains(".asx") ||
            query.Contains(".xspf"));
    }
}