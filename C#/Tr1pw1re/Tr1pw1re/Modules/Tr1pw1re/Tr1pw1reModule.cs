using Discord.Commands;
using Discord.Modules;
using System.Linq;
using Tr1pw1re;

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
                        var LoL = e.Server.FindChannels("league-of-legends").FirstOrDefault();
                        var anime = e.Server.FindChannels("anime").FirstOrDefault();
                        await e.Channel.SendMessage(""
                            + "```Quick Update:```\n"
                            + "1. Tr1pw1re server has been released to [OFFICIAL] state!\n"
                            + "2. We have a custom mood message implemented into every user's `!uinfo` page! To begin, `!evolva sm`, your mood message function will be generated. To change your mood message, `!evolva` sm [here].\n"
                            + "3. Lots of text channels has been removed!\n"
                            + "4. Adding on to announcement 3, we have created a few channels: " + LoL.Mention + " and " + anime.Mention + ". We have given those two channels seperate channel moderators to keep watch at all times.");
                    });

                cgb.CreateCommand(Prefix + "rules")
                    .Do(async e =>
                    {
                        var nsfw = e.Server.FindChannels("nsfw").FirstOrDefault();
                        await e.Channel.SendMessage(""
                            +"```Discord General Rules```\n"
                            + "• Change your **username to IGN**\n"
                            + "• Post NSFW contents in " + nsfw.Mention + "\n"
                            + "• Do not **threaten** others\n"
                            + "• You are **FREE to mention servers** but do not spam advertise\n"
                            + "• Racism is **not allowed** and any extreme religious talks are prohibited however short talks are fine\n"
                            + "• **Do not spam bots**\n"
                            + "• **Do not ask for roles**\n"
                            + "• **Be respectful** to staff members and everyone else\n"
                            + "• Any **purpose spamming** for earning experiences are bannable\n"
                            + "• Any message asking for **staff roles or permissions** will be denied and/or ignored, roles are to be earned by trust and responsibility\n"
                            + "• **Use common sense.** Decide what is right and what is wrong.\n"
                            + "```Discord Voice Rules```\n"
                            + "• Do not breathe into your mikes (this will lead to server mute)\n"
                            + "• Do not insult others (no matter indirectly or directly) in channels\n"
                            + "• Use PPT (Push to talk) when you have background noises or you don't wish to have the world hear your every bit of life\n"
                            + "• Do not play music through your mic since it causes loud noises, suggesting playing music through our music bots are the best choice\n");
                    });

                cgb.CreateCommand(Prefix + "staff")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage(""
                            + "**List of staff on Discord:**\n"
                            + "\n```xl\n"
                            + "• MrBilly - Global Admin\n"
                            + "• kspyro9988877 - Global Admin\n"
                            + "• cherry2003 - Global Admin\n"
                            + "• QuanTBacon - Senior Server Manager\n"
                            + "• joynilanjan2014 - Server Manager\n"
                            + "• Helga - Junior Server Manager\n"
                            + "• LeeCareGene - Junior Server Manager\n"
                            + "• JellyTipGamer - Anime Channel-Mod\n"
                            + "• MVGlad - LoL Channel-Mod```");
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
