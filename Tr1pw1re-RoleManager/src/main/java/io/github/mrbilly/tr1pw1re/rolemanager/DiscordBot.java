package io.github.mrbilly.tr1pw1re.rolemanager;

import javax.security.auth.login.LoginException;

import net.dv8tion.jda.JDA;
import net.dv8tion.jda.JDABuilder;
import net.dv8tion.jda.entities.Role;
import net.dv8tion.jda.entities.User;
import net.dv8tion.jda.events.message.MessageReceivedEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;
import net.dv8tion.jda.managers.GuildManager;

public class DiscordBot extends ListenerAdapter
{
    public static void main(String[] args) throws LoginException, IllegalArgumentException, InterruptedException
    {
        JDA jda = new JDABuilder().setBotToken("").buildBlocking();
        jda.addEventListener(new DiscordBot());
    }

    @Override
    public void onMessageReceived(MessageReceivedEvent event) {
        if (event.getAuthor().getId().equals(event.getJDA().getSelfInfo().getId())) {
            return; //We don't want the to be processing the bots messages, Prevent loops.
        }
        
        if (event.getMessage().getContent().toLowerCase().startsWith("!cooldown")) {
        	String ID = event.getAuthor().getId(); //Purpose of this is to shrink the check down
        	if (ID.equals("145785306849738753") 
        			|| ID.equals("98386227161464832") || ID.equals("116896231103528968") 
        			|| ID.equals("193579405849657363") || ID.equals("165882067115180032")) {
                String username = event.getMessage().getContent().substring(9).trim(); //Your Command size
                for (User user : event.getGuild().getUsers()) {
                    if (user.getUsername().equalsIgnoreCase(username)) {
                        Role role = event.getGuild().getRoleById("195019723405131776");
                        GuildManager gm = new GuildManager(event.getGuild());
                        gm.addRoleToUser(user, role);
                        gm.update();
                        event.getTextChannel().sendMessage(user.getAsMention() + " you should really calm down!");
                        return;
                    }
                }
        	}
        }
        
        if (event.getMessage().getContent().toLowerCase().startsWith("!uncooldown")) {
        	String ID = event.getAuthor().getId(); //Purpose of this is to shrink the check down
        	if (ID.equals("145785306849738753") 
        			|| ID.equals("98386227161464832") || ID.equals("116896231103528968") 
        			|| ID.equals("193579405849657363") || ID.equals("165882067115180032")) {
        		String username = event.getMessage().getContent().substring(11).trim(); //Your Command size
        		for (User user : event.getGuild().getUsers()) {
        			if (user.getUsername().equalsIgnoreCase(username)) {
        				Role role = event.getGuild().getRoleById("195019723405131776");
        				GuildManager gm = new GuildManager(event.getGuild());
        				gm.removeRoleFromUser(user, role);
        				gm.update();
        				event.getTextChannel().sendMessage(user.getAsMention() + " you're good to go!");
        				return;
        			}
        		}
        	}
        }
    }
}