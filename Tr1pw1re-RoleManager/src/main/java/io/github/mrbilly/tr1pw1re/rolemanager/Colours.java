package io.github.mrbilly.tr1pw1re.rolemanager;

import net.dv8tion.jda.entities.Role;
import net.dv8tion.jda.events.message.MessageReceivedEvent;
import net.dv8tion.jda.hooks.ListenerAdapter;
import net.dv8tion.jda.managers.GuildManager;

public class Colours extends ListenerAdapter{
	
	public void onMessageReceived(MessageReceivedEvent event)
    {
		
        if (event.getMessage().getContent().toLowerCase().startsWith("!colours")) {
        	event.getTextChannel().sendMessage(""
        			+ "```"
        			+ "1. Red\n"
        			+ "2. Orange\n"
        			+ "3. Yellow\n"
        			+ "4. Green\n"
        			+ "5. Turquoise\n"
        			+ "6. Blue\n"
        			+ "7. Indigo\n"
        			+ "8. Violet\n"
        			+ "9. Black\n"
        			+ "You can get these colours for your name by typing !<colour>```");
        }
        
        if (event.getMessage().getContent().toLowerCase().startsWith("!colors")) {
        	event.getTextChannel().sendMessage(""
        			+ "```"
        			+ "1. Red\n"
        			+ "2. Orange\n"
        			+ "3. Yellow\n"
        			+ "4. Green\n"
        			+ "5. Turquoise\n"
        			+ "6. Blue\n"
        			+ "7. Indigo\n"
        			+ "8. Violet\n"
        			+ "9. Black\n"
        			+ "You can get these colors for your name by typing !<color>```");
        }
        
        if (event.getMessage().getContent().toLowerCase().startsWith("!red")) {
        	Role role = event.getGuild().getRoleById("193707590662881281");
        	GuildManager gm = new GuildManager(event.getGuild());
        	gm.addRoleToUser(event.getAuthor(), role);
        	gm.update();
        	event.getTextChannel().sendMessage(event.getAuthor().getAsMention() + " your name is now red!");
        }
        
        if (event.getMessage().getContent().toLowerCase().startsWith("!orange")) {
        	Role role = event.getGuild().getRoleById("");
        	GuildManager gm = new GuildManager(event.getGuild());
        	gm.addRoleToUser(event.getAuthor(), role);
        	gm.update();
        	event.getTextChannel().sendMessage(event.getAuthor().getAsMention() + " your name is now orange!");
        }
    }
}
