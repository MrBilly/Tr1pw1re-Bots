using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Anastasia_II
{
    class Anastasia_II
    {
        static void Main(string[] args) => new Anastasia_II().Start();

        public DiscordClient client;

        public void Start()
        {
            client = new DiscordClient();

            client.MessageReceived += Bot_Message;

            Console.Title = "Anastasia II";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Anastasia II"
                + "\nWritten in C# with Discord.Net\n");
            Console.ResetColor();

            client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            client.ExecuteAndWait(async () =>
            {
                await client.Connect("");
            });
        }

        public void Bot_Message(object sender, MessageEventArgs e)
        {
            if (e.Message.IsAuthor) return;

            Console.WriteLine("[" + DateTime.Now.ToString("hh:mm:ss | dd/MM/yyyy") + "] " + "< #{0} | {1} > {2}", e.Channel, e.User.Name, e.Message.Text);

            DiscordCommands commandClass = new DiscordCommands();
            commandClass.Bot_Message(sender, e);
        }
    }
}
