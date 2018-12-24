package vn.soha.game.sdk.utils;

import android.util.Log;

public class Logger {

	static boolean ENABLE_LOG = true;

	public static void e(String msg) {
		if (ENABLE_LOG) {
			 Log.e("SGameSDK", msg);
		}
	}
}
