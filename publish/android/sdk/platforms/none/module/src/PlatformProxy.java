package com.usdk.plugin;

import com.usdk.sdk.UsdkBase;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;

public class PlatformProxy extends UsdkBase {
	private static String TAG = "PlatformProxy";
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		Log.i(TAG, "OnCreate ");
	}

	@Override
	public void OnDestroy() {
		Log.i(TAG, "OnDestroy ");
	}
	
	public void callPlugin(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPlugin ");
		sendCallBack2Unity("callPlugin",a1);
	}
	
	public int callPluginRInt(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRInt ");
		sendCallBack2Unity("callPluginRInt",a1);
		return 1;
	}
	
	public String callPluginRString(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRString ");
		sendCallBack2Unity("callPluginRString",a1);
		return "callPluginRString";
	}
	
	public boolean callPluginRBool(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRBool ");
		sendCallBack2Unity("callPluginRBool",a1);
		return true;
	}
	
	public float callPluginRFloat(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRFloat ");
		sendCallBack2Unity("callPluginRFloat",a1);
		return 1.11f;
	}
}
