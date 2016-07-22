using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.Audio;

namespace EvolvaBot.Modules.Music
{
    public enum MusicType
    {
        Radio,
        Normal,
        Local
    }

    public enum StreamState
    {
        Resolving,
        Queued,
        Buffering,
        Playing,
        Completed
    }

    public class MusicPlayer
    {
        public static int MAX_PLAYLIST_SIZE => 50;
        private IAudioClient AudioClient { get; set; }
        private readonly List<Song> playlist = new List<Song>();
        public IReadOnlyCollection<Song> Playlist => playlist;
        private readonly object PLAYLIST_LOCK = new object();

        public Song CURRENT_SONG { get; set; } = default(Song);
        private CancellationTokenSource SongCancelSource { get; set; }
        private CancellationToken CANCEL_TOKEN { get; set; }

        public bool Paused { get; set; }
        public float Volume { get; private set; }

        public event EventHandler<Song> OnComplete = delegate { };
        public event EventHandler<Song> OnStarted = delegate { };

        public Channel PlaybackVoiceChannel { get; private set; }

        private bool Destroyed { get; set; } = false;
        public bool RepeatSong { get; private set; } = false;
        public bool RepeatPlaylist { get; private set; }

        public MusicPlayer(Channel startingVoiceChannel, float? defaultVolume)
        {
            if(startingVoiceChannel == null)
            {
                throw new ArgumentNullException(nameof(startingVoiceChannel));
            }
            if(startingVoiceChannel.Type != ChannelType.Voice)
            {
                throw new ArgumentNullException("Channel must be a voice channel!");
            }
            Volume = defaultVolume ?? 1.0f;

            PlaybackVoiceChannel = startingVoiceChannel;
            SongCancelSource = new CancellationTokenSource();
            CANCEL_TOKEN = SongCancelSource.Token;
            Task.Run(async () =>
            {
                while (!Destroyed)
                {
                    try
                    {
                        if (AudioClient?.State != ConnectionState.Connected)
                            AudioClient = await PlaybackVoiceChannel.JoinAudio().ConfigureAwait(false);
                    }
                    catch
                    {
                        await Task.Delay(1000).ConfigureAwait(false);
                        continue;
                    }
                    CURRENT_SONG = GetNextSong();
                    var currSong = CURRENT_SONG;
                    if (currSong != null)
                    {
                        try
                        {
                            OnStarted(this, currSong);
                            await currSong.Play(AudioClient, CANCEL_TOKEN).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine("Song canceled");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Exception in PlaySong: {e}");
                        }
                        OnComplete(this, currSong);
                        currSong = CURRENT_SONG;
                        if (currSong != null)
                            if (RepeatSong)
                                playlist.Insert(0, currSong);
                            else if (RepeatPlaylist)
                                playlist.Insert(playlist.Count, currSong);
                        SongCancelSource = new CancellationTokenSource();
                        CANCEL_TOKEN = SongCancelSource.Token;
                    }
                    await Task.Delay(1000).ConfigureAwait(false);
                }
               
            });
            
        }

        public void Next()
        {
            lock(PLAYLIST_LOCK)
            {
                Paused = false;
                SongCancelSource.Cancel();
            }
        }

        public void Stop()
        {
            lock(PLAYLIST_LOCK)
            {
                playlist.Clear();
                CURRENT_SONG = null;
                RepeatPlaylist = false;
                RepeatSong = false;
                if (!SongCancelSource.IsCancellationRequested)
                    SongCancelSource.Cancel();
            }
        }

        public void TogglePause() => Paused = !Paused;

        public void Shuffle()
        {
            lock(PLAYLIST_LOCK)
            {
                //playlist.Shuffle();
            }
        }

        public int SetVolume(int volume)
        {
            if (volume < 0)
                volume = 0;
            if (volume > 100)
                volume = 100;

            Volume = volume / 100.0f;
            return volume;
        }

        private Song GetNextSong()
        {
            lock(PLAYLIST_LOCK)
            {
                if (playlist.Count == 0)
                    return null;
                var toReturn = playlist[0];
                playlist.RemoveAt(0);
                return toReturn;
            }
        }

        public void AddSong(Song s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            lock(PLAYLIST_LOCK)
            {
                playlist.Add(s);
            }
        }

        public void AddSong(Song s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            lock(PLAYLIST_LOCK)
            {
                playlist.Insert(index, s);
            }
        }

        public void RemoveSong(Song s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            lock(PLAYLIST_LOCK)
            {
                playlist.Remove(s);
            }
        }

        public void RemoveSongAt(int index)
        {
            lock(PLAYLIST_LOCK)
            {
                if (index < 0 || index >= playlist.Count)
                    throw new ArgumentException("Invalid index");
                playlist.RemoveAt(index);
            }
        }

        internal Task MoveToVoiceChannel(Channel voiceChannel)
        {
            if (AudioClient?.State != ConnectionState.Connected)
                throw new InvalidOperationException("Can't move while bot is not connected to voice channel.");
            PlaybackVoiceChannel = voiceChannel;
            return PlaybackVoiceChannel.JoinAudio();
        }

        internal void ClearQueue()
        {
            lock(PLAYLIST_LOCK)
            {
                playlist.Clear();
            }
        }

        public void Destroy()
        {
            lock(PLAYLIST_LOCK)
            {
                playlist.Clear();
                Destroyed = true;
                CURRENT_SONG = null;
                if (!SongCancelSource.IsCancellationRequested)
                    SongCancelSource.Cancel();
                AudioClient.Disconnect();
            }
        }

        internal bool ToggleRepeatSong() => this.RepeatSong = !this.RepeatSong;

        internal bool ToggleRepeatPlaylist() => this.RepeatPlaylist = !this.RepeatPlaylist;

    }
}