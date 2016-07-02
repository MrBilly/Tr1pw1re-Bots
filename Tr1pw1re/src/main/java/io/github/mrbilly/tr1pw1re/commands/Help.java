package io.github.mrbilly.tr1pw1re.commands;

import io.github.lxgaming.discordbot.DiscordBot;
import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class Help {
	
	private static String COMMANDPREFIX = DiscordBot.CONFIG.getString("CommandPrefix");
	
	public static void help(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		
		// MrBilly Tr1pw1re Commands
		
		if (COMMAND.equalsIgnoreCase("help !cooldown")) {
			CHANNEL.sendMessage(""
					+ "`Help for '" + COMMANDPREFIX + "cooldown':`\n"
					+ "**Usage**: !cooldown <user>");
		}
		
		if (COMMAND.equalsIgnoreCase("help !uncooldown")) {
			CHANNEL.sendMessage(""
					+ "`Help for '" + COMMANDPREFIX + "uncooldown':`\n"
					+ "**Usage**: !uncooldown <user>");
		}
		
		// LX_Gaming DiscordBot Commands
	}

}
