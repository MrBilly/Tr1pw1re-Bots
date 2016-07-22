using System;
using System.IO;
using System.Text;

using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Modules;

using Newtonsoft.Json;

using EvolvaBot.Classes.JSON;
using EvolvaBot.Modules.Music;
using EvolvaBot.Modules.RoleManagement;

namespace EvolvaBot
{
    public class EvolvaBot
    {
        public static DiscordClient Client { get; private set; }

        // Version name should be [major].[minor].[patch].[version name]
        // Version name should be MMDDYY
        private static string VERSION = "2.0.1.071016";
        private static string CODENAME = "Beryl 緑柱石";

        public static Credentials Creds { get; set; }
        public static Configuration Config { get; set; }
        
        public EvolvaBot()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Write credentials and config
            try
            {
                if(!File.Exists("json/credentials.json"))
                {
                    File.WriteAllText("json/credentials.json", JsonConvert.SerializeObject(new Credentials(), Formatting.Indented));
                }
                else
                {
                    PrintInfo("'credentials.json' exists!");
                }
                File.WriteAllText("json/config.json", JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
                PrintInfo("Written to 'credentials.json' and 'config.json'");
            }
            catch (Exception e)
            {
                PrintError("Couldn't write to 'credentials.json' and/or 'config.json'");
                Console.WriteLine("Reason: " + e.Message);
            }
            
            // Load credentials
            try
            {
                Creds = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText("json/credentials.json"));
                PrintInfo("Loaded 'credentials.json'");
            }
            catch(Exception e)
            {
                PrintError("Couldn't load 'credentials.json'"); 
                Console.WriteLine(e.Message);
            }

            Client = new DiscordClient(new DiscordConfigBuilder()
            {
                MessageCacheSize = 10,
                ConnectionTimeout = 120000,
                LogLevel = LogSeverity.Warning,
                LogHandler = (s, e) =>
                PrintError($"Severity: {e.Severity}" +
                           $"Message: {e.Message}" +
                           $"ExceptionMessage: {e.Exception.Message ?? "-"}")
            });

            var cmdService = new CommandService(new CommandServiceConfigBuilder
            {
                AllowMentionPrefix = false,
                CustomPrefixHandler = m => 0,
                HelpMode = HelpMode.Disabled,
                ErrorHandler = async (s, e) =>
                {
                    if (e.ErrorType != CommandErrorType.BadPermissions)
                        return;
                    if (string.IsNullOrWhiteSpace(e.Exception?.Message))
                        return;
                    try
                    {
                        await e.Channel.SendMessage(e.Exception.Message).ConfigureAwait(false);
                    }
                    catch { }
                }
            });

            Client.AddService<CommandService>(cmdService);

            var modules = Client.AddService<ModuleService>(new ModuleService());

            Client.AddService<AudioService>(new AudioService(new AudioServiceConfigBuilder()
            {
                Channels = 2,
                EnableEncryption = false,
                Bitrate = 128,
            }));

            modules.Add(new MusicModule(), "Music", ModuleFilter.None);
            modules.Add(new RMModule(), "RoleManagement", ModuleFilter.None);

            PrintInfo("Registered modules.");

            Client.MessageReceived += Client_MessageReceived;
            Client.MessageSent += Client_MessageSent;

            PrintInfo("Starting connection...");
            Client.Connect(Creds.Token);

            PrintInfo($"Connected to server!");

            Client.Wait();
            
            
        }
        

        private void Client_MessageSent(object sender, MessageEventArgs e)
        {
            
        }

        private void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            Console.WriteLine("[" + DateTime.UtcNow.ToString("hh:mm:ss") + " UTC]" + "< {0} | {1} > {2}", e.Channel, e.User.Name, e.Message.Text);
            if (e.Message.Text == "!evolva info" || e.Message.Text.Equals("Who are you Evolva?", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva info] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                e.Channel.SendMessage(
                    "Evolva `[RELEASE]`" + "\n"
                    + "Version " + VERSION + " (" + CODENAME + ")\n"
                    + "Written by QuanTBacon, in C# using Discord.NET" + "\n"
                    + "\nEvolva is an intelligent assistant that aims to be like Cortana on Windows 10 & Siri on iOS, but at a lesser degree."
                    + "\nThis project is open source, and can be found here: <https://github.com/MrBilly/Tr1pw1re-Bots/tree/master/Evolva>");
            }

            if(e.Message.Text == "!evolva help")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva help] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                e.Channel.SendMessage(
                    "List of usable commands: " + "\n"
                    + "\nNOTE: They do not ignore question marks or commas!" + "\n"
                    + "`!evolva info | or | Who are you Evolva? -`" + "\n"
                    + "    See the info about Evolva." + "\n"
                    + "`!evolva time | or | Hey Evolva, what's the time now? -`" + "\n"
                    + "    Gets the current time." + "\n"
                    + "`!evolva notes | or | What's new Evolva?`" + "\n"
                    + "    Shows the latest release notes for Evolva." + "\n"
                    + "`!evolva modules`" + "\n"
                    + "    Shows list of modules loaded." + "\n"
                    + "`!evolva music`" + "\n"
                    + "    Shows the list of music commands." + "\n"
                    + "`!evolva rm`" + "\n"
                    + "    Shows the list of role management commands.");
            }

            if(e.Message.Text == "!evolva time" || e.Message.Text.Equals("Hey Evolva, what's the time now?", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva time] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                var utcBase = DateTime.UtcNow;

                string myTime = utcBase.AddHours(8).ToString();
                string ausTime = utcBase.AddHours(10).ToString();
                string nzTime = utcBase.AddHours(12).ToString();
                string vcvrTime = utcBase.AddHours(-7).ToString();
                e.Channel.SendFile("./Resources/evolva-expression-happy.png");
                e.Channel.SendMessage(
                    "It is currently `" + myTime + "` in Malaysia/Singapore." + "\n"
                    + "It is currently `" + ausTime + "` in Australia." + "\n"
                    + "It is currently `" + nzTime  + "` in New Zealand." + "\n"
                    + "It is currently `" + utcBase + "` UTC time." + "\n"
                    + "It is currently `" + vcvrTime + "` in Vancouver, Canada.");
            }

            if(e.Message.Text == "!evolva notes" || e.Message.Text.Equals("What's new Evolva?", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva notes] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                e.Channel.SendMessage(
                    "Release notes for 6/22/2016 `(Version 0.1.0.062216)`: " + "\n"
                    + "• Added new `!evolva help`, `!evolva info`, `!evolva time` and `!evolva myinfo` commands." + "\n"
                    + "• Started on `!evolva sendDM` command" + "\n"
                    + "Release notes for 7/04/2016 `(Version 1.1.0.070416[BETA])`: " + "\n"
                    + "• Support for Discord Modules" + "\n"
                    + "• Added 2 new modules, Music`[WIP]` and RoleManagement" + "\n"
                    + "Release notes for 7/10/2016 `(Version 2.0.0.071016[RELEASE])`: " + "\n"
                    + "• Evolva should now be on the server 24/7(unless a network or system failure), thanks to MrBilly & Spyro" + "\n"
                    + "• Time command should now display proper times, also added a Vancouver time" + "\n"
                    + "• Used JSON files for storing credentials and configuration" + "\n"
                    + "• Evolva is now open-source!");
            }

            if(e.Message.Text == "!evolva" || e.Message.Text.Equals("Hey Evolva", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                Random rand = new Random();
                int next = rand.Next(3);
                switch(next)
                {
                    case 0:
                        e.Channel.SendMessage(
                    "Hello there " + e.User.Name + ", what can I do for you?" + "\n"
                    + "Do `!evolva help` for the list of commands.");
                        break;
                    case 1:
                        e.Channel.SendMessage(
                    "Hi there " + e.User.Name + "! Nice day, isn't it?" + "\n"
                    + "Do `!evolva help` for the list of commands.");
                        break;
                    case 2:
                        e.Channel.SendMessage(
                    "Hello " + e.User.Name + "! How may I help you?" + "\n"
                    + "Do `!evolva help` for the list of commands.");
                        break;
                    default:
                        e.Channel.SendMessage(
                    "Greetings!" + "\n"
                    + "Do `!evolva help` for the list of commands.");
                        break;

                }
                
            }

            if (e.Message.Text == "!evolva modules")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***Received command [!evolva modules] from [{0}]*****", e.User.Name);
                Console.ResetColor();
                e.Channel.SendMessage("Here are the list of loaded modules:" + "\n"
                    + "```- Evolva Music Module, Version 1.0.0" + "\n"
                    + "- Evolva Role Management Module, Version 1.0.0```");
            }

            if(e.Message.Text == "!evolva disconnect")
            {
                if(e.User.Id == (ulong)Creds.OwnerId)
                {
                    e.Channel.SendMessage("I'm shutting down...");
                    Console.WriteLine("\nDisconnecting...");
                    Client.Disconnect();
                    
                }
                else
                {
                    e.Channel.SendMessage("You don't tell me what to do!");
                }
            }

            if(e.Message.Text == "!evolva music")
            {
                e.Channel.SendMessage("Here are a list of music commands that are usable:" + "\n"
                    + "`!em queue [keyword/link]` | Alias: `!em q[keyword/link]`" + "\n"
                    + "    Queues a song using keywords or a link. The bot will join your voice channel." + "\n"
                    + "`!em next` | Alias: `!em n`" + "\n"
                    + "    Skips to the next song in queue." + "\n"
                    + "`!em stop` | Alias: `!em s`" + "\n"
                    + "    Stops and clears the playlist." + "\n"
                    + "`!em destroy` | Alias: `!em d`" + "\n"
                    + "    Stops the music and removes the bot from the channel." + "\n"
                    + "`!em pause` | Alias: `!em p`" + "\n"
                    + "    Pauses/Unpauses the song." + "\n"
                    + "`!em list [page]` | Alias: `!em l [page]`" + "\n"
                    + "    Lists 15 songs currently in queue." + "\n"
                    + "`!em current` | Alias: `!em cr`" + "\n"
                    + "    Shows the song currently playing." + "\n"
                    + "`!em volume [value]` | Alias: `!em v [value]`" + "\n"
                    + "    Sets the music volume between 0-100%." + "\n"
                    + "`!em defvolume [value]` | Alias: `!em dv [value]`" + "\n"
                    + "    Sets the default music volume when music playback is started between 0-100%. **Does not persist through restart!**" + "\n"
                    + "`!em mute`" + "\n"
                    + "    Sets the music volume to 0%." + "\n"
                    + "`!em max`" + "\n"
                    + "    Sets the music volume to the 100%." + "\n"
                    + "`!em shuffle` | Alias: `!em sh`" + "\n"
                    + "    Shuffles the current playlist." + "\n"
                    + "`!em playlist[keyword/link]` | Alias: `!em pl [keyword/link]`" + "\n"
                    + "    Queues up to 50 songs from a youtube playlist, specified by either a link or keywords." + "\n"
                    + "`!em move` | Alias: `!em mv`" + "\n"
                    + "    Moves the bot to your voice channel. Only works if music is already playing." + "\n"
                    + "`!em remove [pos/all]` | Alias: `!em rm [pos/all]`" + "\n"
                    + "    Removes a song by its position in queue, or 'all' to remove the whole queue." + "\n"
                    + "\n\n```Evolva Music Module, Version 1.0.0" + "\n"
                    + "Created by QuanTBacon```");
            }
            
            if(e.Message.Text == "!evolva rm")
            {
                e.Channel.SendMessage("Here are a list of role management commands that are usable:" + "\n"
                    + "`!rm addRole [@username] [role]` | Alias: `!rm ar [@username] [role]`" + "\n"
                    + "    `[STAFF ONLY]` Adds a specific role to the specified user." + "\n"
                    + "`!rm removeRole [@username] [role]` | Alias: `!rm rr [@username] [role]`" + "\n"
                    + "    `[STAFF ONLY]` Removes a specific role from the specified user." + "\n"
                    + "`!rm createRole [name]` | Alias: `!rm cr [name]`" + "\n"
                    + "    `[STAFF ONLY]` Creates a role with the specified name." + "\n"
                    + "\n\n```Evolva Role Management Module, Version 1.0.0" + "\n"
                    + "Created by QuanTBacon```");
            }
            
        }

        static void Main(string[] args)
        {
            
            Console.Title = $"Evolva {VERSION} | {CODENAME} | Heap size: " + getHeap() + " | Started on: " + DateTime.Now.ToString("hh:mm:ss [dd-MM-yyyy]");
            Console.WriteLine("Evolva [RELEASE]");
            Console.WriteLine("Version " + VERSION);
            Console.WriteLine("OS: " + Environment.OSVersion);
            Console.WriteLine("Written by QuanTBacon, in C# with Discord.NET\n");
            EvolvaBot theClient = new EvolvaBot();
            
        }

        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {msg}");
            Console.ResetColor();
        }

        public static void PrintWarn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[WARN] {msg}");
            Console.ResetColor();
        }

        public static void PrintInfo(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[INFO] {msg}");
            Console.ResetColor();
        }

        public static string getHeap(bool pass = true) => Math.Round((double)GC.GetTotalMemory(pass) / 10240, 2).ToString();
    }
}