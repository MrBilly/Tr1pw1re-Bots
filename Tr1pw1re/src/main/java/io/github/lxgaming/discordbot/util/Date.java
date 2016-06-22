package io.github.lxgaming.discordbot.util;

import java.text.SimpleDateFormat;
import java.util.Calendar;

public class Date {
	
	public static String getDateTime() {
		return getDate() + " " + getTime();
	}
	
	public static String getDate() {
		Calendar CAL = Calendar.getInstance();
		SimpleDateFormat SDF = new SimpleDateFormat("dd/MM/yyyy");
		return SDF.format(CAL.getTime());
	}
	
	public static String getTime() {
		Calendar CAL = Calendar.getInstance();
		SimpleDateFormat SDF = new SimpleDateFormat("HH:mm:ss");
		return SDF.format(CAL.getTime());
	}
}
