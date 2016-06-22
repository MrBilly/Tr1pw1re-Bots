package io.github.lxgaming.discordbot;

import java.io.File;
import java.nio.file.Files;
import java.nio.file.Paths;

import org.json.JSONObject;

public class Configuration {
	
	public static File CONFIG = new File("config.json");
	
	public static JSONObject loadConfig() {
		if (!CONFIG.exists()) {
			saveConfig();
		}
		
		try {
			JSONObject OBJECT = new JSONObject(new String(Files.readAllBytes(Paths.get(CONFIG.getPath())), "UTF-8"));
			System.out.println("Loaded config.");
			return OBJECT;
		} catch (Exception ex) {
			System.out.println("Failed to load config!");
		}
		return null;
	}
	
	private static void saveConfig() {
		try {
			Files.write(Paths.get(CONFIG.getPath()), new JSONObject()
					.put("BotToken", "")
					.put("BotChannel", "")
					.put("OwnerID", "")
					.put("CommandPrefix", "!")
					.put("ConsoleOutput", "true")
					.put("UserAvatarUpdate", "true")
					.put("UserGameUpdate", "true")
					.put("UserNameUpdate", "true")
					.put("UserOnlineStatusUpdate", "true")
					.put("VoiceServerDeaf", "true")
					.put("VoiceServerMute", "true")
					.toString(4).getBytes());
			System.out.println("Saved config.");
		} catch (Exception ex) {
			System.out.println("Failed to save config!");
		}
	}
}