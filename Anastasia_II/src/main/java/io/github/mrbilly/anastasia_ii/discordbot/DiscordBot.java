package io.github.mrbilly.anastasia_ii.discordbot;

import javax.security.auth.login.LoginException;

import net.dv8tion.jda.JDA;
import net.dv8tion.jda.JDABuilder;
import net.dv8tion.jda.entities.User;
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
    	if (event.getMessage().getContent().equalsIgnoreCase("~anastasia cherry")) {
    		event.getTextChannel().sendMessage(""
    				+ "Cherry? Where did the Cherry go?\n"
    				+ "I'm a little hungry.");
    	}
    	
        if (event.getMessage().getContent().toLowerCase().startsWith("~anastasia info")) {
                String username = "cherry2003";
                for (User user : event.getGuild().getUsers()) {
                    if (user.getUsername().equalsIgnoreCase(username)) {
                    	event.getTextChannel().sendMessage("This bot is created by " + user.getAsMention() + " and inspired by Discord/Billy :stuck_out_tongue:");
                    }
                }
        }
        
        if (event.getMessage().getContent().equalsIgnoreCase("~anastasia aye")) {
        	event.getTextChannel().sendMessage("Aye there! I'm " + event.getJDA().getSelfInfo().getAsMention() + "!");
        }
        
        if (event.getMessage().getContent().equalsIgnoreCase("~anastasia")) {
        	event.getTextChannel().sendMessage(""
        			+ "~anastasia = Displays this list\n"
        			+ "~anastasia info = Displays Bot Info\n"
        			+ "~anastasia aye = Say hello to Anastasia!\n"
        			+ "~anastasia cherry = Short message on Cherry... Random!");
        }
    }
}