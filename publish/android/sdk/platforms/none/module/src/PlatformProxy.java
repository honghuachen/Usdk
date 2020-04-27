package com.usdk.plugin;

import com.usdk.platform.adapter.PlatformProxyBase;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;

public class PlatformProxy extends PlatformProxyBase {
	private static String TAG = "PlatformProxy";
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		Log.i(TAG, "OnCreate ");
	}

	@Override
	public void OnDestroy() {
		Log.i(TAG, "OnDestroy ");
	}

	@Override
	public void login(String custom_params_) {
		Log.i(TAG, "login ");
	}

	@Override
	public void logout(String custom_params_) {
		Log.i(TAG, "logout ");
	}

	@Override
	public void exit(String custom_params_) {
		Log.i(TAG, "exit ");
	}

	@Override
	public void pay(String pay_info_) {
		Log.i(TAG, "pay ");
	}

	@Override
	public void releaseSdkResource(String custom_params_) {
		Log.i(TAG, "releaseSdkResource ");
	}

	@Override
	public void switchAccount(String custom_params_) {
		Log.i(TAG, "switchAccount ");
	}

	@Override
	public void openUserCenter(String custom_params_) {
		Log.i(TAG, "openUserCenter ");
	}

	@Override
	public void openAppstoreComment(String appid) {
		Log.i(TAG, "openAppstoreComment ");
	}
	
	public void callPlugin(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPlugin ");
	}
	
	public int callPluginRInt(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRInt ");
		return 1;
	}
	
	public String callPluginRString(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRString ");
		return "callPluginRString";
	}
	
	public boolean callPluginRBool(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRBool ");
		return true;
	}
	
	public float callPluginRFloat(String a1,String a2,String a3,String a4) {
		Log.i(TAG, "callPluginRFloat ");
		return 1.11f;
	}
}
