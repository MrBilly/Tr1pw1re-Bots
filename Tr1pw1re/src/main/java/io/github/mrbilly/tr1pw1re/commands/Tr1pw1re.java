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
					+ "**Nicknames:**\n"
					+ "All nicknames will remain the same as their in-game name, no matter the roles of one.\n"
					+ "\nIf a member needs to change their names to their in-game name, Moderators+ will come by and assist you.\n"
					+ "\n**Bots:**\n"
					+ "`@Mee6` has a few commands for you to check out.\n"
					+ "When you talk in chat once per minute, you earn experiences that can help you `!rank`.\n"
					+ "Here is the link to the levels.\n"
					+ "<https://mee6.xyz/levels/191420210568232961>\n"
					+ "\nMusic Time! `@Tr1pw1re` allows you to play music in the Voice Channels.\n"
					+ "Some basic musics commands are available using `!music`\n"
					+ "\nAnd if you ever forget those commands, feel free to do `!help`\n"
					+ "\n**Word Filters:**\n"
					+ "Have you ever thought that when the owner is offline, you could be dirty as you want?\n"
					+ "Well too bad! There’s a word filter system!\n"
					+ "If you attempt to swear, the bot will remove your message and return you with a warning,\n"
					+ "telling you that one of the words you had in your previous message contained a filtered word.\n"
					+ "\nDon’t try.\n"
					+ "\n**Promotions:**\n"
					+ "Congratulations to QuanTBacon for earning Server Manager role!");
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
					+ "`https://github.com/MrBilly/Tr1pw1re-Bot`");
		}
	}
}
