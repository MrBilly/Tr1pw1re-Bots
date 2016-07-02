package io.github.lxgaming.discordbot.util;

import io.github.lxgaming.discordbot.DiscordBot;

public class MessageSender {

	public static void sendMessage(String message) {
		try {
			DiscordBot.API.getTextChannelById("192198356393656320").sendMessage(message);
		} catch (Exception ex) {
			System.out.println("Unable to send message!");
			System.out.println("Make sure 'DiscordBot.TextChannels.Bot' Is using an ID and not a name!");
			System.out.println("List of available TextChannels " + DiscordBot.API.getTextChannels());
		}
		return;
	}
}