package com.usdk.plugin;

import android.content.Context;

public interface ICrasheye {
	public abstract void initWithNativeHandleUserspaceSig(Context context, String appKey);
	public abstract void setAppVersion(String YourAppVersion);
	public abstract String getSDKVersion();
	public abstract void setUserIdentifier(String YourAppVersion);
	public abstract void setLogging(int lines);
	public abstract void setLogging(String filter);
	public abstract void setLogging(int lines, String filter);
	public abstract void setFlushOnlyOverWiFi(boolean enabled);
}
