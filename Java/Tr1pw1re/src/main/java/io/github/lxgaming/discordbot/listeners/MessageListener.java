package io.github.lxgaming.discordbot.listeners;

import io.github.lxgaming.discordbot.DiscordBot;
import io.github.lxgaming.discordbot.commands.BotCommand;
import io.github.lxgaming.discordbot.commands.FunCommand;
import io.github.lxgaming.discordbot.commands.LoveCommand;
import io.github.mrbilly.tr1pw1re.commands.Help;
import io.github.mrbilly.tr1pw1re.commands.Tr1pw1re;
import net.dv8tion.jda.entities.Message;
import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;
import net.dv8tion.jda.events.message.MessageReceivedEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;

public class MessageListener extends ListenerAdapter {
	
	private static String COMMANDPREFIX = DiscordBot.CONFIG.getString("CommandPrefix");
	private static String CONSOLEOUTPUT = DiscordBot.CONFIG.getString("ConsoleOutput");
	
	@Override
	public void onMessageReceived(MessageReceivedEvent MR) {
		TextChannel CHANNEL = MR.getTextChannel();
		Message MESSAGE = MR.getMessage();
		User AUTHOR = MR.getAuthor();
		
		if (AUTHOR.getId().equals(DiscordBot.API.getSelfInfo().getId())) {
			return;
		}
		
		if ((MESSAGE.getContent().startsWith(COMMANDPREFIX) || MESSAGE.getContent().startsWith("/")) && !AUTHOR.getId().equals(DiscordBot.API.getSelfInfo().getId())) {
			String COMMAND = MESSAGE.getContent().substring(COMMANDPREFIX.length());
			BotCommand.bot(CHANNEL, COMMAND, AUTHOR);
			FunCommand.fun(CHANNEL, COMMAND, AUTHOR);
			LoveCommand.love(CHANNEL, COMMAND, AUTHOR);
			Tr1pw1re.tr1pw1re(CHANNEL, COMMAND, AUTHOR);
			Help.help(CHANNEL, COMMAND, AUTHOR);
		}
		
		if (CONSOLEOUTPUT.equalsIgnoreCase("true")) {
			System.out.println("#" + CHANNEL.getName() + " - " + CHANNEL.getGuild());
			System.out.println(AUTHOR.getUsername() + ": " + MESSAGE.getContent());
		}
		return;
	}
}