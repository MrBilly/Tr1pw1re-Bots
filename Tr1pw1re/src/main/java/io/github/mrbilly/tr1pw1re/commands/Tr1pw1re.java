package io.github.mrbilly.tr1pw1re.commands;

import io.github.lxgaming.discordbot.DiscordBot;
import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class Tr1pw1re {
	
	private static String COMMANDPREFIX = DiscordBot.CONFIG.getString("CommandPrefix");
	
	public static void tr1pw1re(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		
		if (COMMAND.equalsIgnoreCase("dev")) {
			CHANNEL.sendTyping();
			CHANNEL.sendMessage("Sending DM!");
			AUTHOR.getPrivateChannel().sendMessage("Hello, " + AUTHOR.getUsername());
			CHANNEL.sendTyping();
			CHANNEL.sendMessage("DM Sent!");
		}
		
		if (COMMAND.equalsIgnoreCase("music")) {
			CHANNEL.sendMessage(""
					+ "Hello I'm Tr1pw1re, I allow you to play music in the Voice Channels." + "\n"
					+ "Some basic music commands are below:" + "\n"
					+ "`!m q <music here> = Queues music from Youtube`" + "\n"
					+ "`!m lq = Lists queued music`" + "\n"
					+ "`!m vol <0 ~ 100> = Changes volume of current music`" + "\n"
					+ "`!m s / !m n = Stops all music, playing and queued`" + "\n"
					+ "`!m rm <queue position> = Removes song from order specified`");
		}
		
		if (COMMAND.equalsIgnoreCase("announcement")) {
			CHANNEL.sendMessage(""
					+ "```Update on June 23rd to June 24th.```
					+ "1. We have added quite a few bots. They are:\n"
					+ "@MathBot from a while ago,\n"
					+ "@Onee-chan today,\n"
					+ "@Ruby by @LeeCareGene,\n"
					+ "@Septapus from some random XD,\n"
					+ "@Evolva by @QuanTBacon our Server Manager, a bot developed by his own hands (or PC),\n"
					+ "@Anastasia2 Can now play music like @Tr1pw1re, it acts as a Second Music Bot. Those have the same commands, except Anastasia bot uses ]m Instead. Queries feel free to PM !staff discord.\n"
					+ "\n"
					+ "2. Please do not share this Discord to the Enjin forum for Tr1pw1re. The server is not yet complete and we wish this to be private, even though we have invited several members of the community. We hope to announce the server.\n"
					+ "Wun day. :wink:\n"
					+ "\n"
					+ "3. Please do not ask for roles/staff title. All requests related will be denied and ignored. Further annoyance will lead to mute/softban.");
		}
		
		if (COMMAND.equalsIgnoreCase("rules")) {
			CHANNEL.sendMessage(""
					+ "• Do not use custom names, use your in-game name instead.\n"
					+ "• Post NSFW contents in `#nsfw`\n"
					+ "• Do not threaten others\n"
					+ "• Do not spam bots, that makes them crash easily and our bot manager is not always on\n"
					+ "• Roles will be given by the developers and moderators, so please do not complain\n"
					+ "• Be respectful to staff members\n"
					+ "• Any queries do not hesitate to ask the moderators and developers");
		}
		
		if (COMMAND.equalsIgnoreCase("staff")) {
			CHANNEL.sendMessage(""
					+ "`" + COMMANDPREFIX + "staff discord`\n"
					+ "		List of staff on Discord.\n"
					+ "`" + COMMANDPREFIX + "staff server`\n"
					+ "		List of staff on Server.\n");
		}
		
		if (COMMAND.equalsIgnoreCase("staff discord")) {
			CHANNEL.sendMessage(""
					+ "List of staff on Discord:\n"
					+ "\n"
					+ "```xl\n"
					+ "• MrBilly - Developer\n"
					+ "• cherry2003 - Developer\n"
					+ "• QuanTBacon - Server Manager```");
		}
		
		if (COMMAND.equalsIgnoreCase("staff server")) {
			CHANNEL.sendMessage(""
					+ "List of staff on Server:\n"
					+ "\n"
					+ "```xl\n"
					+ "• kjburr - Owner\n"
					+ "• Kelly - Co-Owner\n"
					+ "• Arustyred - Moderator\n"
					+ "• Haydeezx - Moderator\n"
					+ "• Kawaii_Potatoes - Moderator\n"
					+ "• SiiM - Moderator\n"
					+ "• MVGlad - Moderator```");
		}
		
		if (COMMAND.equalsIgnoreCase("vote")) {
			CHANNEL.sendMessage("Vote for Tr1pw1re here!: <http://tr1pw1re.com/vote>");
		}

		if (COMMAND.equalsIgnoreCase("github")) {
			CHANNEL.sendMessage(""
					+ "You can contribute in the development of the Tr1pw1re bot in Github, or make a suggestion for more commands here!:" + "\n"
					+ "`<https://github.com/MrBilly/Tr1pw1re-Bot>`");
		}
	}
}
