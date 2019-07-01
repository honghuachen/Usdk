package com.usdk.sdk;

import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;
import java.util.Map;

import org.xmlpull.v1.XmlPullParser;

import com.unity3d.player.UnityPlayer;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.res.Configuration;
import android.os.Bundle;
import android.util.Log;
import android.util.Xml;
import android.view.KeyEvent;

public class UsdkBase implements IUsdkCallBack, IUsdkApplicationDelegate {
	protected String TAG = "usdk";
	private String callBackReceiverName = "UsdkCallBack";
	protected String packageName;
	protected String versionName;
	protected int versionCode;
	protected Activity mActivity;
	protected Map<String, String> configInfo;
	private String className;
	
	protected void info(String smg) {
		Log.i(TAG, "["+className+"]"+smg);
	}

	protected void debug(String smg) {
		Log.d(TAG, "["+className+"]"+smg);
	}

	protected void error(String smg) {
		Log.e(TAG, "["+className+"]"+smg);
	}

	public String getConfig(String key) {
		if(this.configInfo != null && this.configInfo.containsKey(key))
			return this.configInfo.get(key);
		
		return "";
	}

	protected void parseXmlConfig(String xmlName) {
		Map<String, String> argsMap = new HashMap<String, String>();
		try {
			InputStream xml = this.mActivity.getResources().getAssets()
					.open(xmlName);
			XmlPullParser pullParser = Xml.newPullParser();
			pullParser.setInput(xml, "UTF-8");
			int event = pullParser.getEventType();
			String name = null;
			String value = null;
			while (event != 1) {
				switch (event) {
				case 2:
					if ("data".equals(pullParser.getName())) {
						name = pullParser.getAttributeValue(0);
					}
					if ("value".equals(pullParser.getName())) {
						value = pullParser.nextText();
					}
					break;
				case 3:
					if ("data".equals(pullParser.getName())) {
						argsMap.put(name, value);
					}
					break;
				}

				event = pullParser.next();
			}
		} catch (IOException e) {
			e.printStackTrace();
		} catch (Exception e) {
			e.printStackTrace();
		}
		
		this.configInfo = argsMap;
	}
	
	protected void OnCreate(Activity activity, Bundle savedInstanceState){}
	protected void OnStart(){}
	protected void OnDestroy(){}
	protected void OnStop(){}
	protected void OnResume(){}
	protected void OnPause(){}
	protected void OnRestart(){}
	protected void OnActivityResult(int requestCode, int resultCode, Intent data){}
	protected void OnRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults){}
	protected void OnNewIntent(Intent intent){}
	protected void OnConfigurationChanged(Configuration arg0){}
	protected void OnSaveInstanceState(Bundle outState){}
	protected void OnBackPressed(){}
	protected void OnFinish(){}
	protected boolean OnKeyDown(int keyCode, KeyEvent event){return false;}
	protected boolean OnKeyUp(int keyCode, KeyEvent event){return false;}

	@Override
	public final void onCreate(Activity activity, Bundle savedInstanceState) {
		this.mActivity = activity;
		className = getClass().getName();
		
		this.packageName = this.mActivity.getPackageName();
		try {
			PackageInfo packageInfo = this.mActivity.getPackageManager().getPackageInfo(
					this.packageName, 0);
			this.versionName = packageInfo.versionName;
			this.versionCode = packageInfo.versionCode;
		} catch (Exception e) {
			
		}
		OnCreate(activity,savedInstanceState);
	}

	@Override
	public final void onStart() {
		OnStart();
	}

	@Override
	public final void onDestroy() {
		OnDestroy();
	}

	@Override
	public final void onStop() {
		OnStop();
	}

	@Override
	public final void onResume() {
		OnResume();
	}

	@Override
	public final void onPause() {
		OnPause();
	}

	@Override
	public final void onRestart() {
		OnRestart();
	}

	@Override
	public final void onActivityResult(int requestCode, int resultCode, Intent data) {
		OnActivityResult(requestCode,resultCode,data);
	}

	@Override
	public final void onRequestPermissionsResult(int requestCode,
			String[] permissions, int[] grantResults) {
		OnRequestPermissionsResult(requestCode,permissions,grantResults);
	}

	@Override
	public final void onNewIntent(Intent intent) {
		OnNewIntent(intent);
	}

	@Override
	public final void onConfigurationChanged(Configuration arg0) {
		OnConfigurationChanged(arg0);
	}

	@Override
	public final void onSaveInstanceState(Bundle outState) {
		OnSaveInstanceState(outState);
	}

	@Override
	public final void onBackPressed() {
		OnBackPressed();
	}

	@Override
	public final void finish() {
		OnFinish();
	}

	@Override
	public final boolean onKeyDown(int keyCode, KeyEvent event) {
		return OnKeyDown(keyCode,event);
	}

	@Override
	public final boolean onKeyUp(int keyCode, KeyEvent event) {
		return OnKeyUp(keyCode,event);
	}
	
	@Override
	public void sendCallBack2Unity(UsdkCallBackErrorCode errorCode) {
		sendCallBack2Unity(errorCode,null);
	}
	
	@Override
	public void sendCallBack2Unity(UsdkCallBackErrorCode errorCode, String paramString) {
		String retMsg = "errorCode=" + errorCode.name();
		if(paramString != null && paramString.length() > 0)
			retMsg = retMsg + "&paramString=" + paramString;
		UnityPlayer.UnitySendMessage(callBackReceiverName, "CallBack", retMsg);
	}
}
