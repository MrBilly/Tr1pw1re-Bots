package io.github.lxgaming.discordbot.util;

public class EnvironmentManager {
	
	public static String getOS() {
		return System.getProperty("os.name");
	}
	
	public static String getJavaVersion() {
		return System.getProperty("java.version");
	}
	
	public static void restartBot() {
		if (!getOS().toLowerCase().contains("windows")) {
			MessageSender.sendMessage("Unsupported System!");
			return;
		}
		try {
			Runtime.getRuntime().exec("cmd /c restart.bat");
		} catch (Exception ex) {
			MessageSender.sendMessage("Restart Failed!");
		}
	}
}