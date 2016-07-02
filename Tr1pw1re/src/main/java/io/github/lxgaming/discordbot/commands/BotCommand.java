package io.github.lxgaming.discordbot.commands;

import io.github.lxgaming.discordbot.DiscordBot;
import io.github.lxgaming.discordbot.util.EnvironmentManager;
import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class BotCommand {
	
	private static String COMMANDPREFIX = DiscordBot.CONFIG.getString("CommandPrefix");
	
	public static void bot(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		if (COMMAND.equalsIgnoreCase("ping")) {
			CHANNEL.sendMessage("Pong!");
		}
		
		if (COMMAND.equalsIgnoreCase("botinfo")) {
			CHANNEL.sendMessage("DiscordStandAlone, Version " + DiscordBot.DBVERSION 
					+ "\nCreated by LX_Gaming, With modification from MrBilly"
					+ "\n• API - " + DiscordBot.APIVERSION 
					+ "\n• OS - " + EnvironmentManager.getOS() + ", Java - " + EnvironmentManager.getJavaVersion());
		}
		
		if (COMMAND.equalsIgnoreCase("restartbot")) {
			if (AUTHOR.getId().equals(DiscordBot.CONFIG.getString("OwnerID"))) {
				CHANNEL.sendMessage("Attempting Restart...");
				EnvironmentManager.restartBot();
				return;
			}
			CHANNEL.sendMessage("You are not permitted to use this command!");
		}
		
		if (COMMAND.equalsIgnoreCase("request")) {
			CHANNEL.sendMessage("*https://trello.com/c/DGx9tron* \nComment with your Request!");
		}
		
		if (COMMAND.equalsIgnoreCase("website")) {
			CHANNEL.sendMessage("*http://lxgaming.github.io/*");
		}
		
		if (COMMAND.startsWith("bothelp")) {
			String HELPOPTION = COMMAND.substring(7).trim();
			if (HELPOPTION.equalsIgnoreCase("bot")) {
				CHANNEL.sendMessage(""
						+ "`" + COMMANDPREFIX + "ping`\n" 
						+ "		Ping the bot.\n"
						+ "`" + COMMANDPREFIX + "botinfo`\n" 
						+ "		Display bot information.\n"
						+ "`" + COMMANDPREFIX + "request`\n" 
						+ "		Request a feature.\n"
						+ "`" + COMMANDPREFIX + "website`\n" 
						+ "		Link to LX's website.\n");
				return;
			} else if (HELPOPTION.equalsIgnoreCase("fun")) {
				CHANNEL.sendMessage(""
						+ "`" + COMMANDPREFIX + "number`\n" 
						+ "		What's your lucky number?\n"
						+ "`" + COMMANDPREFIX + "roll`\n" 
						+ "		Roll the dice.\n"
						+ "`" + COMMANDPREFIX + "coin`\n" 
						+ "		Flip a coin.\n"
						+ "`" + COMMANDPREFIX + "version`\n" 
						+ "		All the versions.\n");
				return;
			} else if (HELPOPTION.equalsIgnoreCase("love")) {
				CHANNEL.sendMessage(""
						+ "`" + COMMANDPREFIX + "kiss`\n" 
						+ "		Kiss your loved one.\n"
						+ "`" + COMMANDPREFIX + "hug`\n" 
						+ "		Embrace another.\n"
						+ "`" + COMMANDPREFIX + "slap`\n" 
						+ "		Slap a user!\n"
						+ "`" + COMMANDPREFIX + "lick`\n" 
						+ "		Claim it as yours!\n");
				return;
			} else if (HELPOPTION.equalsIgnoreCase("tr1pw1re")) {
				CHANNEL.sendMessage(""
						+ "`" + COMMANDPREFIX + "music`\n"
						+ "		Shows basic music commands.\n"
						+ "`" + COMMANDPREFIX + "rules`\n"
						+ "		Shows a list of rules.\n"
						+ "`" + COMMANDPREFIX + "announcement`\n"
						+ "		Shows latest announcements.\n"
						+ "`" + COMMANDPREFIX + "staff`\n"
						+ "		Get a list of staff.\n");
			} else {
				CHANNEL.sendMessage(""
						+ "`" + COMMANDPREFIX + "bothelp bot`\n" 
						+ "		List bot commands.\n"
						+ "`" + COMMANDPREFIX + "bothelp fun`\n" 
						+ "		List fun commands.\n"
						+ "`" + COMMANDPREFIX + "bothelp love`\n" 
						+ "		List love commands.\n"
						+ "`" + COMMANDPREFIX + "bothelp tr1pw1re`\n"
						+ "		List custom commands.");
			}
		}
		return;
	}
}