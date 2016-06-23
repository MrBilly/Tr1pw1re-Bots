package io.github.mrbilly.anastasia_ii.discordbot;

import javax.security.auth.login.LoginException;

import net.dv8tion.jda.JDA;
import net.dv8tion.jda.JDABuilder;
import net.dv8tion.jda.events.message.MessageReceivedEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;

public class DiscordBot extends ListenerAdapter
{
	// Anastasia II
	// Created on Thursday, 23 June 2016
	// Created by MrBilly & cherry2003
	
    public static void main(String[] args) throws LoginException, IllegalArgumentException, InterruptedException
    {
        JDA jda = new JDABuilder().setBotToken("").buildBlocking();
        jda.addEventListener(new DiscordBot());
    }

    @Override
    public void onMessageReceived(MessageReceivedEvent event)
    {
    	if (event.getMessage().getContent().equalsIgnoreCase("!cherry")) {
    		event.getTextChannel().sendMessage(""
    				+ "Cherry? Where did the Cherry go?\n"
    				+ "I'm a little hungry.");
    	}
    	if (COMMAND.equalsIgnoreCase("anastasia ")) {
		 CHANNEL.sendMessage(""
            + "This bot is created by @cherry2003 and inspired by Discord/Billy :P")
		}

	if (COMMAND.euqlsIgnoreCase("anastasia aye")) {
		CHANNEL.sendMessage(""
			+ "Aye there!")
		}
    	}
    }
}
