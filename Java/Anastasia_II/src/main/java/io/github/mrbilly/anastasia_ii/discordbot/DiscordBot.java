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
		System.out.printf("[%s] [#%s] %s: %s\n", Date.getTime(),
                event.getTextChannel().getName(), event.getAuthor().getUsername(),
                event.getMessage().getContent());
		
		String Prefix = "~";
		
		
		// Are you alive Anastasia? (testing message)
		if (event.getMessage().getContent().equalsIgnoreCase("Are you alive Anastasia?"))
		{
			event.getTextChannel().sendMessage("Yes I am " + event.getAuthor().getAsMention());
		}
		
		
		// ~anastasia testing
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "anastasia testing"))
		{
			event.getTextChannel().sendMessage("Test Received" + event.getAuthor().getAsMention());
		}
		
		
		// ~panda
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "panda"))
		{
			event.getTextChannel().sendMessage("**Panda, Panda, Panda, Panda, Panda, Panda, Panda, Panda**");
		}
		
		
		// ~ping
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "ping"))
		{
			event.getTextChannel().sendMessage("Pong!");
		}
		
		
		// ~roll
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "roll"))
		{
			int number = random.nextInt(6) + 1;
			event.getTextChannel().sendMessage("You rolled a " + number);
		}
		
		
		// ~anatasia time
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "anastasia time"))
		{
			event.getTextChannel().sendMessage("It is... " + Date.getDateTime());
		}
		
		
		// ~anastasia botinfo
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "anastasia botinfo"))
		{
			event.getTextChannel().sendMessage(""
					+ "**Anastasia II**\n"
					+ "`Created by cherry2003`");
		}
		
		
		// ~restartbot
		if (event.getMessage().getContent().equalsIgnoreCase(Prefix + "restartbot"))
		{
			if (event.getAuthor().getId().equals("145785306849738753"))
			{
				event.getTextChannel().sendMessage("Attempting Restart...");
				//Environment.restartBot();
				return;
			}
			event.getTextChannel().sendMessage("You are not permitted to use this command!");
		}

		
		  
		  	// ~mute
	        if (event.getMessage().getContent().toLowerCase().startsWith(Prefix + "mute")) {
	        	String ID = event.getAuthor().getId(); //Purpose of this is to shrink the check down
	        	if (ID.equals("145785306849738753")) {
	                String username = event.getMessage().getContent().substring(5).trim(); //Your Command size
	                for (User user : event.getGuild().getUsers()) {
	                    if (user.getUsername().equalsIgnoreCase(username)) {
	                        Role role = event.getGuild().getRoleById("195019723405131776");
	                        GuildManager gm = new GuildManager(event.getGuild());
	                        gm.addRoleToUser(user, role);
	                        gm.update();
	                        event.getTextChannel().sendMessage("User " + user.getAsMention() + " has been muted! :mute:");
	                        return;
	                    }
	                }
	        	}
	        }
	        
	        
	        // ~unmute
	        if (event.getMessage().getContent().toLowerCase().startsWith(Prefix + "unmute")) {
	        	String ID = event.getAuthor().getId(); //Purpose of this is to shrink the check down
	        	if (ID.equals("145785306849738753")) {
	        		String username = event.getMessage().getContent().substring(7).trim(); //the 
	        		for (User user : event.getGuild().getUsers()) {
	        			if (user.getUsername().equalsIgnoreCase(username)) {
	        				Role role = event.getGuild().getRoleById("195019723405131776");
	        				GuildManager gm = new GuildManager(event.getGuild());
	        				gm.removeRoleFromUser(user, role);
	        				gm.update();
	        				event.getTextChannel().sendMessage("User " + user.getAsMention() + " has been unmuted! :sound:");
	        				return;
	        			}
	        		}
	        	}
	        }
	        
	        
	        if (event.getMessage().getContent().equalsIgnoreCase("!botcommands"))
	        {
	        	event.getTextChannel().sendMessage("Visit here for list of commands\n"
	        			+ "https://github.com/MrBilly/Tr1pw1re-Bots/blob/master/commandlist.md");
	        }
	        
	        if (event.getMessage().getContent().equalsIgnoreCase("Hey Anastasia"))
	        {
	        	event.getTextChannel().sendMessage("Hey there! " + event.getAuthor().getAsMention());
	        }
	}
}
        

