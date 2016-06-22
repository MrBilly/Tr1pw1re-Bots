package io.github.lxgaming.discordbot.commands;

import java.util.Random;

import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class LoveCommand {
	
	private static Random RANDOM = new Random();
	
	public static void love(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		
		String SENDER = AUTHOR.getUsername();
		String NAME = "";
		
		if (COMMAND.toLowerCase().startsWith("kiss")) {
			if (COMMAND.length() > 4) {
				NAME = COMMAND.substring(4);
				int NUMBER = RANDOM.nextInt(3);
				if (NUMBER == 0) {
					CHANNEL.sendMessage(SENDER + " kissed" + NAME);
				} else if (NUMBER == 1) {
					CHANNEL.sendMessage("Awww" + NAME + " ran away, no kiss for you.");
				} else if (NUMBER == 2) {
					CHANNEL.sendMessage("Oh god, get a room you two!");
				}
			} else {
				CHANNEL.sendMessage("Got nobody to kiss?");
			}
		}
		
		if (COMMAND.toLowerCase().startsWith("hug")) {
			if (COMMAND.length() > 3) {
				NAME = COMMAND.substring(3);
				CHANNEL.sendMessage(SENDER + " hugged" + NAME);
			} else {
				CHANNEL.sendMessage("Got nobody to hug?");
			}
		}
		
		if (COMMAND.toLowerCase().startsWith("slap")) {
			if (COMMAND.length() > 4) {
				NAME = COMMAND.substring(4);
				int NUMBER = RANDOM.nextInt(2);
				if (NUMBER == 0) {
					CHANNEL.sendMessage(SENDER + " slapped" + NAME);
				} else if (NUMBER == 1) {
					NAME = COMMAND.substring(5);
					CHANNEL.sendMessage(NAME + " got RKO'D outta nowhere!");
				}
			} else {
				CHANNEL.sendMessage("Got nobody to slap?");
			}
		}
		
		if (COMMAND.toLowerCase().startsWith("lick")) {
			if (COMMAND.length() > 4) {
				NAME = COMMAND.substring(4);
				CHANNEL.sendMessage(SENDER + " licked" + NAME);
			} else {
				CHANNEL.sendMessage("Got nothing to lick?");
			}
		}
		return;
	}
}