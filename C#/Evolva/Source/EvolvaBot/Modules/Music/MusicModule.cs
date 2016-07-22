using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Modules;

using EvolvaBot.Classes;

namespace EvolvaBot.Modules.Music
{
    internal class MusicModule : Module
    {
        public static ConcurrentDictionary<Server, MusicPlayer> players = new ConcurrentDictionary<Server, MusicPlayer>();
        public static ConcurrentDictionary<ulong, float> DefaultMusicVolume = new ConcurrentDictionary<ulong, float>();

        public MusicModule()
        {

        }

        public override void Install(ModuleManager manager)
        {
            var client = EvolvaBot.Client;

            manager.CreateCommands("!em", cgb =>
            {

                cgb.CreateCommand("next")
                    .Alias("n")
                    .Description("Skips to the next song in queue.\n")
                    .Do(e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player)) return;
                        if (player.PlaybackVoiceChannel == e.User.VoiceChannel)
                            player.Next();
                    });

                cgb.CreateCommand("stop")
                    .Alias("s")
                    .Description("Stops and clears the playlist.\n")
                    .Do(async e =>
                    {
                        await Task.Run(() =>
                        {
                            MusicPlayer player;
                            if (!players.TryGetValue(e.Server, out player)) return;
                            if (e.User.VoiceChannel == player.PlaybackVoiceChannel)
                                player.Stop();
                        }).ConfigureAwait(false);
                    });

                cgb.CreateCommand("destroy")
                    .Alias("d")
                    .Description("Stops the music and removes the bot from the channel.\n")
                    .Do(async e =>
                    {
                        await Task.Run(() =>
                        {
                            MusicPlayer player;
                            if (!players.TryRemove(e.Server, out player)) return;
                            if (e.User.VoiceChannel == player.PlaybackVoiceChannel)
                                player.Destroy();
                        }).ConfigureAwait(false);
                    });

                cgb.CreateCommand("pause")
                    .Alias("p")
                    .Description("Pauses/Unpauses the song.\n")
                    .Do(async e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player)) return;
                        if (e.User.VoiceChannel != player.PlaybackVoiceChannel)
                            return;
                        player.TogglePause();
                        if (player.Paused)
                            await e.Channel.SendMessage("Music paused.").ConfigureAwait(false);
                        else
                            await e.Channel.SendMessage("Music unpaused.").ConfigureAwait(false);
                    });

                cgb.CreateCommand("queue")
                    .Alias("q")
                    .Description("Queues a song using keywords or a link. The bot will join your voice channel.")
                    .Parameter("query" + ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage("From MusicModule.cs");
                        await QueueSong(e.Channel, e.User.VoiceChannel, e.GetArg("query")).ConfigureAwait(false);
                        if(e.Server.CurrentUser.GetPermissions(e.Channel).ManageMessages)
                        {
                            await Task.Delay(10000).ConfigureAwait(false);
                            await e.Message.Delete().ConfigureAwait(false);
                        }
                    });

                cgb.CreateCommand("list")
                    .Alias("l")
                    .Description("Lists 15 songs currently in queue.\n")
                    .Parameter("page", ParameterType.Optional)
                    .Do(async e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                        {
                            await e.Channel.SendMessage("No active music player.").ConfigureAwait(false);
                            return;
                        }

                        int pg;
                        if (!int.TryParse(e.GetArg("page"), out pg) || pg <= 0)
                        {
                            pg = 1;
                        }

                        var currSong = player.CURRENT_SONG;
                        if (currSong == null)
                            return;
                        var toSend = $"Currently playing: {currSong.ActualName} " + $"**[**{currSong.currentTime()}**]**\n";
                        if (player.RepeatSong)
                            toSend += "🔂";
                        else if (player.RepeatPlaylist)
                            toSend += "🔁";
                        toSend += $"`{player.Playlist.Count}` songs currently queued. Show page `{pg}`";
                        if (player.Playlist.Count >= MusicPlayer.MAX_PLAYLIST_SIZE)
                            toSend += "**Song queue is full!**\n";
                        else
                            toSend += "\n";
                        const int itemsPerPage = 15;
                        int startAt = itemsPerPage * (pg - 1);
                        var number = 1 + startAt;
                        await e.Channel.SendMessage(toSend + string.Join("\n", player.Playlist.Skip(startAt).Take(15).Select(v => $"`{number++}.` {v.ActualName}"))).ConfigureAwait(false);
                    });

                cgb.CreateCommand("current")
                    .Alias("cr")
                    .Description("Shows the song currently playing.\n")
                    .Do(async e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        var curr = player.CURRENT_SONG;
                        if (curr == null)
                            return;
                        await e.Channel.SendMessage($"Currently playing: {curr.ActualName} " + $"**[**{curr.currentTime()}**]**\n");
                    });

                cgb.CreateCommand("volume")
                    .Alias("v")
                    .Description("Sets the music volume between 0-100%.\n")
                    .Parameter("value", ParameterType.Required)
                    .Do(async e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        if (e.User.VoiceChannel != player.PlaybackVoiceChannel)
                            return;
                        var arg = e.GetArg("value");
                        int volume;
                        if (!int.TryParse(arg, out volume))
                        {
                            await e.Channel.SendMessage("The volume value is invalid.").ConfigureAwait(false);
                            return;
                        }
                        volume = player.SetVolume(volume);
                        await e.Channel.SendMessage($"Volume set to {volume}%").ConfigureAwait(false);
                    });

                cgb.CreateCommand("defvolume")
                    .Alias("dv")
                    .Description("Sets the default music volume when music playback is started between 0-100%. \n"
                        + "**Does not persist through restart!** \n")
                    .Parameter("value", ParameterType.Required)
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(e.GetArg("value"));
                        var arg = e.GetArg("value");
                        float volume;
                        if (!float.TryParse(arg, out volume) || volume < 0 || volume > 100)
                        {
                            await e.Channel.SendMessage("The volume value is invalid.").ConfigureAwait(false);
                            return;
                        }
                        DefaultMusicVolume.AddOrUpdate(e.Server.Id, volume / 100, (key, newval) => volume / 100);
                        await e.Channel.SendMessage($"Default volume set to {volume}%").ConfigureAwait(false);
                    });

                cgb.CreateCommand("mute")
                    .Description("Sets the music volume to 0%.\n")
                    .Do(e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        if (e.User.VoiceChannel != player.PlaybackVoiceChannel)
                            return;
                        player.SetVolume(0);
                    });

                cgb.CreateCommand("max")
                    .Description("Sets the music volume to the 100%.\n")
                    .Do(e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        if (e.User.VoiceChannel != player.PlaybackVoiceChannel)
                            return;
                        player.SetVolume(100);
                    });

                cgb.CreateCommand("shuffle")
                    .Alias("sh")
                    .Description("Shuffles the current playlist.\n")
                    .Do(async e =>
                    {
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        if (e.User.VoiceChannel != player.PlaybackVoiceChannel)
                            return;
                        if (player.Playlist.Count < 2)
                        {
                            await e.Channel.SendMessage("There's not enough songs in the playlist to perform a shuffle").ConfigureAwait(false);
                            return;
                        }
                        player.Shuffle();
                        await e.Channel.SendMessage("The songs has been shuffled.").ConfigureAwait(false);
                    });

                cgb.CreateCommand("playlist")
                    .Alias("pl")
                    .Description("Queues up to 50 songs from a youtube playlist, specified by either a link or keywords.\n")
                    .Parameter("playlist", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        var arg = e.GetArg("playlist");
                        if (string.IsNullOrWhiteSpace(arg))
                            return;
                        if(e.User.VoiceChannel?.Server != e.Server)
                        {
                            await e.Channel.SendMessage("In order for me to play songs, you must join a voice channl on this server.");
                            return;
                        }
                        var plId = await SearchHelper.GetPlaylistIdByKeyword(arg).ConfigureAwait(false);
                        if(plId == null)
                        {
                            await e.Channel.SendMessage("I couldn't find any playlists from that query.");
                            return;
                        }
                        var ids = await SearchHelper.GetVideoIDs(plId, 500).ConfigureAwait(false);
                        if(ids == null || ids.Count == 0)
                        {
                            await e.Channel.SendMessage("I couldn't find any songs.");
                            return;
                        }
                        var idArray = ids as string[] ?? ids.ToArray();
                        var count = idArray.Length;
                        var msg =
                            await e.Channel.SendMessage($"Attempting to queue {count} songs...").ConfigureAwait(false);
                        foreach(var id in idArray)
                        {
                            try
                            {
                                await QueueSong(e.Channel, e.User.VoiceChannel, id, true).ConfigureAwait(false);
                            }
                            catch { }
                        }
                        await msg.Edit("Successfully queued a playlist.").ConfigureAwait(false);
                    });

                /*cgb.CreateCommand("localplaylist")
                    .Alias("lp")
                    .Description("**Bot Owner only**: Queues all songs from a specified directory.\n")
                    .Parameter("dir", ParameterType.Unparsed)
                    .AddCheck(Sim)*/

                cgb.CreateCommand("move")
                    .Alias("mv")
                    .Description("Moves the bot to your voice channel. Only works if music is already playing.\n")
                    .Do(e =>
                    {
                        MusicPlayer player;
                        var vceChnl = e.User.VoiceChannel;
                        if (vceChnl == null || vceChnl.Server != e.Server || players.TryGetValue(e.Server, out player))
                            return;
                        player.MoveToVoiceChannel(vceChnl);
                    });

                cgb.CreateCommand("remove")
                    .Alias("rm")
                    .Description("Removes a song by its position in queue, or 'all' to remove the whole queue.\n")
                    .Parameter("pos", ParameterType.Required)
                    .Do(async e =>
                    {
                        var arg = e.GetArg("pos");
                        MusicPlayer player;
                        if (!players.TryGetValue(e.Server, out player))
                            return;
                        if (arg?.ToLower() == "all")
                        {
                            player.ClearQueue();
                            await e.Channel.SendMessage("Queue cleared.").ConfigureAwait(false);
                            return;
                        }
                        int pos;
                        if (!int.TryParse(arg, out pos))
                        {
                            return;
                        }
                        if (pos <= 0 || pos > player.Playlist.Count)
                            return;
                        var song = (player.Playlist as List<Song>)?[pos - 1];
                        player.RemoveSongAt(pos - 1);
                        await e.Channel.SendMessage($"Track {song.ActualName} at position #{pos} has been removed").ConfigureAwait(false);
                    });


            });
        }

        private async Task QueueSong(Channel txtChnl, Channel vceChnl, string query, bool silent = false, MusicType type = MusicType.Normal)
        {
            if(vceChnl == null || vceChnl.Server != txtChnl.Server)
            {
                if (!silent)
                    await txtChnl.SendMessage("In order for me to play songs, you must join a voice channl on this server.").ConfigureAwait(false);
                throw new ArgumentNullException(nameof(vceChnl));
            }
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3)
                throw new ArgumentException("Invalid query for queue song.", nameof(query));

            var player = players.GetOrAdd(txtChnl.Server, server =>
            {
                float? vol = null;
                float throwAway;
                if (DefaultMusicVolume.TryGetValue(server.Id, out throwAway))
                    vol = throwAway;
                var mp = new MusicPlayer(vceChnl, vol);

                Message playingMsg = null;
                Message lastFinishedMsg = null;
                mp.OnComplete += async (s, song) =>
                {
                    if (song.PrintStatusMessage)
                    {
                        try
                        {
                            if (lastFinishedMsg != null)
                                await lastFinishedMsg.Delete().ConfigureAwait(false);
                            if (playingMsg != null)
                                await playingMsg.Delete().ConfigureAwait(false);
                            lastFinishedMsg = await txtChnl.SendMessage($"Finished song: {song.ActualName}").ConfigureAwait(false);
                        }
                        catch { }

                    }
                };
                mp.OnStarted += async (s, song) =>
                {
                    if (song.PrintStatusMessage)
                    {
                        var sender = s as MusicPlayer;
                        if (sender == null)
                            return;

                        try
                        {
                            var txt = $"Currently playing: {song.ActualName} @ Volume: {(int)(sender.Volume * 100)}%";
                            playingMsg = await txtChnl.SendFile(txt).ConfigureAwait(false);
                        }
                        catch { }
                    }
                };
                return mp;
            });

            var resolvedSong = await Song.ResolveSong(query, type).ConfigureAwait(false);
            resolvedSong.musicPlayer = player;

            player.AddSong(resolvedSong);
            if(!silent)
            {
                var queuedMsg = await txtChnl.SendMessage($"Queued {resolvedSong.ActualName} @ No. {player.Playlist.Count}");
#pragma warning disable CS4014

                Task.Run(async () =>
                {
                    await Task.Delay(10000).ConfigureAwait(false);
                    try
                    {
                        await queuedMsg.Delete().ConfigureAwait(false);
                    }
                    catch { }
                }).ConfigureAwait(false);

#pragma warning restore CS4014
            }
        }

    }
}