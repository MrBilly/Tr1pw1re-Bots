package io.github.lxgaming.discordbot.listeners;

import io.github.lxgaming.discordbot.DiscordBot;
import io.github.lxgaming.discordbot.util.Date;
import io.github.lxgaming.discordbot.util.MessageSender;
import net.dv8tion.jda.events.voice.VoiceServerDeafEvent;
import net.dv8tion.jda.events.voice.VoiceServerMuteEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;

public class VoiceListener extends ListenerAdapter {
	
	private String VOICESERVERDEAF = DiscordBot.CONFIG.getString("VoiceServerDeaf");
	private String VOICESERVERMUTE = DiscordBot.CONFIG.getString("VoiceServerMute");
	
	@Override
	public void onVoiceServerDeaf(VoiceServerDeafEvent VSD) {
		if (VOICESERVERDEAF.toLowerCase().equals("true")) {
			if (VSD.getVoiceStatus().isServerDeaf() == true) {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + VSD.getUser().getUsername() + "** ``Voice:`` **Server Deafened**");
			} else {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + VSD.getUser().getUsername() + "** ``Voice:`` **Server Undeafened**");
			}
		}
		return;
	}
	
	@Override
	public void onVoiceServerMute(VoiceServerMuteEvent VSM) {		
		if (VOICESERVERMUTE.toLowerCase().equals("true")) {
			if (VSM.getVoiceStatus().isServerMuted() == true) {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + VSM.getUser().getUsername() + "** ``Voice:`` **Server Muted**");
			} else {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + VSM.getUser().getUsername() + "** ``Voice:`` **Server Unmuted**");
			}
		}
		return;
	}
}