package com.usdk.plugin;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;

import com.usdk.platform.adapter.PlatformProxyBase;
import com.usdk.sdk.UsdkBase;
import com.usdk.sdk.Usdk;
import com.xsj.crasheye.Crasheye;

public class CrasheyeProxy extends UsdkBase implements ICrasheye{
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		super.onCreate(activity, savedInstanceState);
		this.parseXmlConfig("CrasheyeConfig.xml");	
		PlatformProxyBase proxy = Usdk.getPlatform();
		
		String crasheyeAppkey = this.getConfig("AppKey");
		Crasheye.enableDebug();
		Crasheye.setChannelID(proxy.getConfig("ChannelId")+"_"+proxy.getConfig("AdId"));
		Crasheye.setLogging(100, "Unity");
		Crasheye.initWithNativeHandleUserspaceSig(activity.getApplicationContext(), crasheyeAppkey);
		Crasheye.initWithMonoNativeHandle(activity.getApplicationContext(), crasheyeAppkey);
	}

	@Override
	public String getSDKVersion() {
		return Crasheye.getSDKVersion();
	}

	@Override
	public void initWithNativeHandleUserspaceSig(Context context, String appKey) {
		Crasheye.initWithNativeHandleUserspaceSig(context, appKey);
	}

	@Override
	public void setAppVersion(String YourAppVersion) {
		Crasheye.setAppVersion(YourAppVersion);
	}

	@Override
	public void setFlushOnlyOverWiFi(boolean enabled) {
		Crasheye.setFlushOnlyOverWiFi(enabled);
	}

	@Override
	public void setLogging(int lines) {
		Crasheye.setLogging(lines);
	}

	@Override
	public void setLogging(String filter) {
		Crasheye.setLogging(filter);
	}

	@Override
	public void setLogging(int lines, String filter) {
		Crasheye.setLogging(lines, filter);
	}

	@Override
	public void setUserIdentifier(String userIdentifier) {
		Crasheye.setUserIdentifier(userIdentifier);
	}
}
