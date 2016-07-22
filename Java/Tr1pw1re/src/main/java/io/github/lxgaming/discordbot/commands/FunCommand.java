package io.github.lxgaming.discordbot.commands;

import java.util.Random;

import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class FunCommand {
	
	private static Random RANDOM = new Random();
	
	public static void fun(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		if (COMMAND.equalsIgnoreCase("number")) {
			int NUMBER = RANDOM.nextInt(100) + 1;
			CHANNEL.sendMessage("Your lucky number is " + NUMBER + "/100!");
		}
		
		if (COMMAND.equalsIgnoreCase("roll")) {
			int NUMBER = RANDOM.nextInt(6) + 1;
			CHANNEL.sendMessage("You rolled a " + NUMBER);
		}
		
		if (COMMAND.equalsIgnoreCase("coin")) {
			int NUMBER = RANDOM.nextInt(2);
			if (NUMBER == 0) {
				CHANNEL.sendMessage("Heads\n" + "https://goo.gl/Pg5RQN");
			} else if (NUMBER == 1) {
				CHANNEL.sendMessage("Tails\n" + "https://goo.gl/wgHmwb");
			}
		}
		
		if (COMMAND.equalsIgnoreCase("version")) {
			CHANNEL.sendMessage("Version " + String.valueOf(RANDOM.nextInt(10)) + "." + String.valueOf(RANDOM.nextInt(10)) + "." + String.valueOf(RANDOM.nextInt(10)));
		}
		return;
	}
}