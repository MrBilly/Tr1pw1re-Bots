package io.github.lxgaming.discordbot.listeners;

import io.github.lxgaming.discordbot.DiscordBot;
import io.github.lxgaming.discordbot.util.Date;
import io.github.lxgaming.discordbot.util.MessageSender;
import net.dv8tion.jda.events.user.UserAvatarUpdateEvent;
import net.dv8tion.jda.events.user.UserGameUpdateEvent;
import net.dv8tion.jda.events.user.UserNameUpdateEvent;
import net.dv8tion.jda.events.user.UserOnlineStatusUpdateEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;

public class UserListener extends ListenerAdapter {
	
	private String USERAVATARUPDATE = DiscordBot.CONFIG.getString("UserAvatarUpdate");
	private String USERGAMEUPDATE = DiscordBot.CONFIG.getString("UserGameUpdate");
	private String USERNAMEUPDATE = DiscordBot.CONFIG.getString("UserNameUpdate");
	private String USERONLINESTATUSUPDATE = DiscordBot.CONFIG.getString("UserOnlineStatusUpdate");
	@Override
	public void onUserAvatarUpdate(UserAvatarUpdateEvent UAU) {
		if (USERAVATARUPDATE.toLowerCase().equals("true")) {
			MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UAU.getUser().getUsername() + "** ``New Avatar:`` **" + UAU.getUser().getAvatarId() + "**");
		}
		return;
	}
	
	@Override
	public void onUserGameUpdate(UserGameUpdateEvent UGU) {
		if (USERGAMEUPDATE.toLowerCase().equals("true") &&!(UGU.getUser().getCurrentGame() == null)) {
			MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UGU.getUser().getUsername() + "** ``Game:`` **" + UGU.getUser().getCurrentGame() + "**");
		}
		return;
	}
	
	@Override
	public void onUserNameUpdate(UserNameUpdateEvent UNU) {
		if (USERNAMEUPDATE.toLowerCase().equals("true")) {
			MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UNU.getPreviousUsername() + "** ``New name:`` **" + UNU.getUser().getUsername() + "**");
		}
		return;
	}
	
	@Override
	public void onUserOnlineStatusUpdate(UserOnlineStatusUpdateEvent UOSU) {
		if (USERONLINESTATUSUPDATE.toLowerCase().equals("true")) {
			if (UOSU.getUser().getOnlineStatus().toString().equals("ONLINE")) {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UOSU.getUser().getUsername() + "** ``Status:`` **Online**");
			}
			if (UOSU.getUser().getOnlineStatus().toString().equals("AWAY")) {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UOSU.getUser().getUsername() + "** ``Status:`` **Away**");
			}
			if (UOSU.getUser().getOnlineStatus().toString().equals("OFFLINE")) {
				MessageSender.sendMessage("``Time:`` **" + Date.getTime() + "** ``User:`` **" + UOSU.getUser().getUsername() + "** ``Status:`` **Offline**");
			}
		}
		return;
	}
}