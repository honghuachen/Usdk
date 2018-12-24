package vn.soha.game.sdk;

import vn.soha.game.sdk.utils.Utils;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;

public class InstallReceiver extends BroadcastReceiver{
	public static final String REFERRER_VALUE = "referrer_value";
	public static final String LOG_INSTALL_SUCCESSFUL = "log_install_successful";


	@Override
	public void onReceive(Context context, Intent intent) {
		// TODO Auto-generated method stub
		Log.e("stk", "InstallReceiver : onReceiveeeeeeeeeeeeeee");

		SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(context);

		try {
			// get referrer value
			Bundle extras = intent.getExtras();
			String referrerValue = extras.getString("referrer");
			sharedPreferences.edit().putString(REFERRER_VALUE, referrerValue).commit();
			Log.e("stk", "InstallReceiver : referrerValue=" + referrerValue);

			String clientName = referrerValue.split("=")[1];
			Utils.saveClientName(context, clientName);

			Log.e("stk", "InstallReceiver : clientName=" + clientName);
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	}

}
