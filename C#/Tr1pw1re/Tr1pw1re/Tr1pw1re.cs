using Discord;
using Discord.Commands;
using Discord.Modules;

using Tr1pw1re.Classes.JSONModels;
using Tr1pw1re.Modules.Tr1pw1re;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr1pw1re
{
    public class Tr1pw1re
    {
        public static DiscordClient Client { get; private set; }
        public static Credentials Creds { get; set; }
        //public static Configuration Config { get; set; }
        //public static LocalizedStrings Locale { get; set; } = new LocalizedStrings();
        public static string BotMention { get; set; } = "";
        public static bool Ready { get; set; } = false;
        public static Action OnReady { get; set; } = delegate { };

        private static List<Channel> OwnerPrivateChannels { get; set; }

        public static int tablesFlipped = 0;
        public static int tablesUnFlipped = 0;

        private static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            try
            {
                File.WriteAllText("credentials_example.json", JsonConvert.SerializeObject(new Credentials(), Formatting.Indented));
            }
            catch
            {
                Console.WriteLine("Failed writing credentials_example.json or data/config_example.json");
            }

            try
            {
                //load credentials from credentials.json
                Creds = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText("credentials.json"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load stuff from credentials.json, RTFM\n{ex.Message}");
                Console.ReadKey();
                return;
            }

            //if password is not entered, prompt for password
            if (string.IsNullOrWhiteSpace(Creds.Token))
            {
                Console.WriteLine("Token blank. Please enter your bot's token:\n");
                Creds.Token = Console.ReadLine();
            }

            BotMention = $"<@{Creds.BotId}>";

            //create new discord client and log
            Client = new DiscordClient(new DiscordConfigBuilder()
            {
                MessageCacheSize = 10,
                ConnectionTimeout = 120000,
                LogLevel = LogSeverity.Warning,
                LogHandler = (s, e) =>
                    Console.WriteLine($"Severity: {e.Severity}" +
                                      $"Message: {e.Message}" +
                                      $"ExceptionMessage: {e.Exception?.Message ?? "-"}"),
            });

            //create a command service
            var commandService = new CommandService(new CommandServiceConfigBuilder
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

            //reply to personal messages and forward if enabled.
            //Client.MessageReceived += Client_MessageReceived;

            //add command service
            Client.AddService<CommandService>(commandService);

            //create module service
            var modules = Client.AddService<ModuleService>(new ModuleService());

            //install modules
            modules.Add(new Tr1pw1reModule(), "Tr1pw1re", ModuleFilter.None);

            Client.MessageReceived += Client_MessageReceived;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[INFO] Registered modules.");
            Console.ResetColor();

            //run the bot
            Client.ExecuteAndWait(async () =>
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[INFO] Starting connection...");
                    await Client.Connect(Creds.Token).ConfigureAwait(false);
                    Console.WriteLine($"[INFO] Connected to server!");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token is wrong. Don't set a token if you don't have an official BOT account.");
                    Console.WriteLine(ex);
                    Console.ReadKey();
                    return;
                }

                await Task.Delay(90000).ConfigureAwait(false);

                //Console.WriteLine("-----------------");
                //Console.WriteLine(await NadekoStats.Instance.GetStats().ConfigureAwait(false));
                //Console.WriteLine("-----------------");

                Client.ClientAPI.SendingRequest += (s, e) =>
                {
                    var request = e.Request as Discord.API.Client.Rest.SendMessageRequest;
                    if (request == null) return;
                    // meew0 is magic
                    request.Content = request.Content?.Replace("@everyone", "@everyοne").Replace("@here", "@һere") ?? "_error_";
                    if (string.IsNullOrWhiteSpace(request.Content))
                        e.Cancel = true;
                };
                //PermissionsHandler.Initialize();
            });
            Console.WriteLine("Exiting...");
            Console.ReadKey();
        }

        private static async void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == "!tables")
            {
                await e.Channel.SendMessage("Table(s) flipped: **" + tablesFlipped + "**");
                await e.Channel.SendMessage("Table(s) unflipped: **" + tablesUnFlipped + "**");
            }

            if (e.Message.Text.Contains("(╯°□°）╯︵ ┻━┻"))
            {
                tablesFlipped++;

                //await e.Channel.SendMessage("Seems like " + tablesFlipped + " table(s) have been flipped");
            }

            if (e.Message.Text.Contains("┬─┬﻿ ノ( ゜-゜ノ)"))
            {
                tablesUnFlipped++;

                //await e.Channel.SendMessage("Seems like " + tablesUnFlipped + " table(s) have been unflipped");
            }
        }
    }
}
