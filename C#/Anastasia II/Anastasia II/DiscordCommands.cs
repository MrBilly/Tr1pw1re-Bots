using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Anastasia_II
{
    class DiscordCommands
    {
        public DiscordClient client;

        public async void Bot_Message(object sender, MessageEventArgs e)
        {
            client = new DiscordClient();

            var Prefix = "~";

            if (e.Message.IsAuthor) return;

            if (e.Message.Text == Prefix + "anastasia cherry")
            {
                await e.Channel.SendMessage(""
                    + "Cherry? Where did the Cherry go?\n"
                    + "I'm a little hungry.");
            }

            if (e.Message.Text == Prefix + "anastasia aye")
            {
                var bot = e.Server.FindUsers("Anastasia II").FirstOrDefault();
                await e.Channel.SendMessage("Aye there! I'm " + bot.Mention + "!");
            }

            if (e.Message.Text == Prefix + "anastasia")
            {
                await e.Channel.SendMessage(""
                    + "**`~anastasia`** = Displays this list\n"
                    + "**`~anastasia botinfo`** = Displays Bot Info\n"
                    + "**`~anastasia aye`** = Say hello to Anastasia!\n"
                    + "**`~anastasia cherry`** = Short message on Cherry... Random!"
                    + "**`~anastasia banana`** = Can you hold your breath through this whole passage?"
                    + "**`~anastasia time`** = Shows the time for Anastasia! Its GMT+8..."
                    + "**`~ping`** = Pong!);
            }
            
            if (e.Message.Text == Prefix + "anastasia banana")
            {
                await e.Channel.SendMessage (""
                    + "```You put a banana in a banana by putting one inside a banana so that the banana can stay within the banana because if you don't put the banana inside the banana then you cannot complete your theory about how to put a banana within a banana.```");
            }
        }
    }
}
