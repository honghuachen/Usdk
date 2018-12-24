package vn.soha.game.sdk;

import android.content.Context;

import com.google.android.gcm.GCMBroadcastReceiver;

public class MyGCMBroadCast extends GCMBroadcastReceiver{
	@Override
	protected String getGCMIntentServiceClassName(Context context) {
		// TODO Auto-generated method stub
		return "vn.soha.game.sdk.GCMIntentService";
	}

}
