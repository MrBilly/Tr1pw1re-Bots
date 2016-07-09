package io.github.mrbilly.tr1pw1re.commands;

import io.github.lxgaming.discordbot.DiscordBot;
import net.dv8tion.jda.entities.TextChannel;
import net.dv8tion.jda.entities.User;

public class Tr1pw1re {
	
	private static String COMMANDPREFIX = DiscordBot.CONFIG.getString("CommandPrefix");
	
	public static void tr1pw1re(TextChannel CHANNEL, String COMMAND, User AUTHOR) {
		
		if (COMMAND.equalsIgnoreCase("dev")) {
			CHANNEL.sendTyping();
			CHANNEL.sendMessage("Sending DM!");
			AUTHOR.getPrivateChannel().sendMessage("Hello, " + AUTHOR.getUsername());
			CHANNEL.sendTyping();
			CHANNEL.sendMessage("DM Sent!");
		}
		
		if (COMMAND.equalsIgnoreCase("music")) {
			CHANNEL.sendMessage(""
					+ "Hello I'm Tr1pw1re, I allow you to play music in the Voice Channels." + "\n"
					+ "Some basic music commands are below:" + "\n"
					+ "`!m q <music here> = Queues music from Youtube`" + "\n"
					+ "`!m lq = Lists queued music`" + "\n"
					+ "`!m vol <0 ~ 100> = Changes volume of current music`" + "\n"
					+ "`!m dv <0 ~ 100> = Sets the default music volume when music playback is started.` **Does not persist through restart**" + "\n"
					+ "`!m n = Skips to the next song in queue`" + "\n"
					+ "`!m s = Stops all music, playing and queued`" + "\n"
					+ "`!m p = Pauses/Unpauses music player`" + "\n"
					+ "`!m rm <queue position> = Removes song from order specified`" + "\n"
					+ "`!m rm all = Removes all songs in queue, but not the one currently playing`" + "\n"
					+ "`!m np = Shows the song currently playing`" + "\n"
					+ "`!m sh = Shuffles all songs in queue`" + "\n"
					+ "`!m pl <playlist here> = Queues up to 50 songs froma Youtube playlist`" + "\n"
					+ "`!m scpl <link here> = Queues a Soundcloud playlist using a link`" + "\n"
					+ "`!m gl = Gives the link of the current song`");
		}
		
		if (COMMAND.equalsIgnoreCase("announcement")) {
			CHANNEL.sendMessage(""
					+ "```Update from July 4th to July 6th```"
					+ "1. Applications for Staff Roles such as bot developers and server managers have been enhanced and posted in #application! The Tr1pw1re Discord team is looking forward to seeing the brand new applications!\n"
					+ "Remember, don't ask about it or you will be denied and ignored immediately!\n"
					+ "2. Server Manager @QuanTBacon#9845 has a survey for TW members. Click the link below to fill it out. http://www.strawpoll.me/10669417 \n"
					+ "3. Due to issues, Honored role will only be awarded at lvl 31. Sorry for the inconvenience.");
		}
		
		if (COMMAND.equalsIgnoreCase("pokegame")) {
			CHANNEL.sendMessage(""
					+ "Introducing the PokeGame update!\n"
					+ "You now have access to commands that you can use to battle against others!\n"
					+ "`>settype` = Sets your element type, can do >settype . to check the list of types available\n"
					+ "`>ml` = Lists moves that you can use\n"
					+ "`>attack <move name> @User` = Attacks specified user with specified move\n"
					+ "`>heal @User` = Heals user to full HP\n"
					+ "\nAny queries feel free to PM staff.\n");
		}
		
		if (COMMAND.equalsIgnoreCase("pokebattle")) {
			CHANNEL.sendMessage(""
					+ "Here you can play the PokeBattle game.\n"
					+ "• If you are interested in signing up for a battle, contact one of the Discord Staffs.\n"
					+ "• Once you are accepted, only you and your pals have access to the channel.\n"
					+ "• Everyone else that is not included will only be able to view but unable to leave comments.\n");
		}
		
		if (COMMAND.equalsIgnoreCase("rules")) {
			CHANNEL.sendMessage(""
					+ "```Discord General Rules```\n"
					+ "• Do not use custom names, use your in-game name instead\n"
					+ "• Post NSFW contents in `#nsfw`\n"
					+ "• Do not threaten others\n"
					+ "• Do not spam bots, that makes them crash easily and our bot manager is not always on\n"
					+ "• Roles will be given by the developers and moderators, so please do not complain\n"
					+ "• Be respectful to staff members and everyone else\n"
					+ "• Use common sense. Decide what is right and what is wrong.\n"
					+ "\n"
					+ "```Discord Voice Rules```\n"
					+ "• Do not breathe into your mikes (this will lead to server mute)\n"
					+ "• Do not insult others (no matter indirectly or directly) in channels\n"
					+ "• Use PPT (Push to talk) when you have background noises or you don't wish to have the world hear your every bit of life\n"
					+ "• Do not play music through your mic since it causes loud noises, suggesting playing music through our music bots are the best choice\n");
		}
		
		if (COMMAND.equalsIgnoreCase("staff")) {
			CHANNEL.sendMessage(""
					+ "`" + COMMANDPREFIX + "staff discord`\n"
					+ "		List of staff on Discord.\n"
					+ "`" + COMMANDPREFIX + "staff server`\n"
					+ "		List of staff on Server.\n");
		}
		
		if (COMMAND.equalsIgnoreCase("staff discord")) {
			CHANNEL.sendMessage(""
					+ "List of staff on Discord:\n"
					+ "\n"
					+ "```xl\n"
					+ "• MrBilly - Developer\n"
					+ "• cherry2003 - Developer\n"
					+ "• QuanTBacon - Server Manager```");
		}
		
		if (COMMAND.equalsIgnoreCase("staff server")) {
			CHANNEL.sendMessage(""
					+ "List of staff on Server:\n"
					+ "\n"
					+ "```xl\n"
					+ "• kjburr - Owner\n"
					+ "• Kelly - Co-Owner\n"
					+ "• Arustyred - Moderator\n"
					+ "• Haydeezx - Moderator\n"
					+ "• Kawaii_Potatoes - Moderator\n"
					+ "• SiiM - Moderator\n"
					+ "• MVGlad - Moderator```");
		}
		
		if (COMMAND.equalsIgnoreCase("vote")) {
			CHANNEL.sendMessage("Vote for Tr1pw1re here!: <http://tr1pw1re.com/vote>");
		}
		
		if (COMMAND.equalsIgnoreCase("github")) {
			CHANNEL.sendMessage(""
					+ "You can contribute in the development of the Tr1pw1re bot in Github, or make a suggestion for more commands here!:" + "\n"
					+ "<https://github.com/MrBilly/Tr1pw1re-Bot>");
		}
	}
}
