package io.github.lxgaming.discordbot;

import org.json.JSONObject;

import io.github.lxgaming.discordbot.listeners.BotListener;
import io.github.lxgaming.discordbot.listeners.MessageListener;
import io.github.lxgaming.discordbot.listeners.UserListener;
import io.github.lxgaming.discordbot.listeners.VoiceListener;
import net.dv8tion.jda.JDA;
import net.dv8tion.jda.JDABuilder;

public class DiscordBot {
	
	public static JSONObject CONFIG = Configuration.loadConfig();
	public static JDA API;
	public static String DBVERSION = "0.6.1 ('Forest')";
	public static String APIVERSION = "JDA v2.1.0, Build 293 - Recompiled";
	
	public static void main(String[] args) {
		System.out.println("DiscordBot v" + DBVERSION);
		System.out.println("API - " + APIVERSION);
		System.out.println("Author - Alex Thomson and MrBilly");
		loadDiscord();
	}
	
	public static void loadDiscord() {
		try {
			API = new JDABuilder()
					.setBotToken(CONFIG.getString("BotToken"))
					.addListener(new BotListener())
					.addListener(new MessageListener())
					.addListener(new UserListener())
					.addListener(new VoiceListener())
					.setAudioEnabled(false)
					.buildAsync();
		} catch (Exception ex) {
			System.out.println("Connection Failed! Invaild BotToken");
			return;
		}
	}
}