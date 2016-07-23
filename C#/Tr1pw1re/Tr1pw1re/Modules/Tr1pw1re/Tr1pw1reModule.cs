using Discord.Commands;
using Discord.Modules;
using System.Linq;

namespace Tr1pw1re.Modules.Tr1pw1re
{
    internal class Tr1pw1reModule : DiscordModule
    {
        public override string Prefix { get; } = "!";

        public override void Install(ModuleManager manager)
        {
            manager.CreateCommands("", cgb =>
            {
                //cgb.AddCheck(PermissionChecker.Instance);
                //commands.ForEach(com => com.Init(cgb));

                cgb.CreateCommand(Prefix + "dev")
                    .Do(async e =>
                    {
                        await e.Channel.SendIsTyping();
                        await e.Channel.SendMessage("Sending DM!");
                        await e.User.PrivateChannel.SendMessage("Hello, " + e.User.Name);
                        await e.Channel.SendIsTyping();
                        await e.Channel.SendMessage("DM Sent!");
                    });

                cgb.CreateCommand(Prefix + "music")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(""
                            + "Hi I'm Tr1pw1re, and I allow you to play music in the Voice Channels.\n"
                            + "Some basic music commands are below:\n"
                            + "`!m q <music here> = Queues music from Youtube`\n"
                            + "`!m lq = Lists queued music`\n"
                            + "`!m vol <0 ~ 100> = Changes volume of current music`\n"
                            + "`!m dv <0 ~ 100> = Sets the default music volume when music playback is started.`\n"
                            + "`!m n = Skips to the next song in queue`\n"
                            + "`!m s = Stops all music, playing and queued`\n"
                            + "`!m p = Pauses/Unpauses music player`\n"
                            + "`!m rm <queue position> = Removes song from order specified`\n"
                            + "`!m rm all = Removes all songs in queue, but not the one currently playing`\n"
                            + "`!m np = Shows the song currently playing`\n"
                            + "`!m sh = Shuffles all songs in queue`\n"
                            + "`!m pl <playlist here> = Queues up to 50 songs froma Youtube playlist`\n"
                            + "`!m scpl <link here> = Queues a Soundcloud playlist using a link`\n"
                            + "`!m gl = Gives the link of the current song`");
                    });

                cgb.CreateCommand(Prefix + "announcements")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(""
                            + "```Update from July 4th to July 6th```"
                            + "1. Applications for Staff Roles such as bot developers and server managers have been enhanced and posted in #application! The Tr1pw1re Discord team is looking forward to seeing the brand new applications!\n"
                            + "Remember, don't ask about it or you will be denied and ignored immediately!\n"
                            + "2. Server Manager @QuanTBacon#9845 has a survey for TW members. Click the link below to fill it out. <http://www.strawpoll.me/10669417>\n"
                            + "3. Due to issues, Honored role will only be awarded at lvl 31. Sorry for the inconvenience.");
                    });

                cgb.CreateCommand(Prefix + "rules")
                    .Do(async e =>
                    {
                        var nsfw = e.Server.FindChannels("nsfw").FirstOrDefault();
                        await e.Channel.SendMessage(""
                            +"```Discord General Rules```\n"
                            + "• Do not use custom names, use your in-game name instead\n"
                            + "• Post NSFW contents in " + nsfw.Mention + "\n"
                            + "• Do not threaten others\n"
                            + "• Do not spam bots, that makes them crash easily and our bot manager is not always on\n"
                            + "• Roles will be given by the developers and moderators, so please do not complain\n"
                            + "• Be respectful to staff members and everyone else\n"
                            + "• Use common sense. Decide what is right and what is wrong.\n"
                            + "\n"
                            + "```Discord Voice Rules```\n"
                            + "• Do not breathe into your mikes (this will lead to server mute)\n"
                            + "• Do not insult others (no matter indirectly or directly) in channels\n"
                            + "• Use PPT (Push to talk) when you have background noises or you don't wish to have the world hear your every bit of life\n"
                            + "• Do not play music through your mic since it causes loud noises, suggesting playing music through our music bots are the best choice\n");
                    });

                cgb.CreateCommand(Prefix + "staff discord")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(""
                            + "List of staff on Discord:\n"
                            + "\n```xl\n"
                            + "• MrBilly - Developer\n"
                            + "• kspyro998877 - Developer\n"
                            + "• cherry2003 - Developer\n"
                            + "• QuanTBacon - Server Manager\n"
                            + "• Helga - Anime Moderator\n"
                            + "• CheeseChris - NSFW Moderator\n"
                            + "• LeeCareGene - Bot Developer```");
                    });

                cgb.CreateCommand(Prefix + "github")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(""
                            + "You can contribute in the development of the Tr1pw1re bot in Github, or make a suggestion for more commands here!:\n"
                            + "<https://github.com/MrBilly/Tr1pw1re-Bot>");
                    });
            });
        }
    }
}