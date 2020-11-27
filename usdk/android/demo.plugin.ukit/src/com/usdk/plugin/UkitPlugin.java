package com.usdk.plugin;

import java.util.ArrayList;

import com.usdk.sdk.UsdkBase;
import com.uuid.tool.DeviceUuidFactory;

import android.app.Activity;
import android.os.Bundle;
import net.agasper.unitynotification.NotificationAction;
import net.agasper.unitynotification.UnityNotificationManager;

public class UkitPlugin extends UsdkBase{
	DeviceUuidFactory uuidKit;
	int notificationId = 0;
	@Override
	protected void OnCreate(Activity activity, Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.OnCreate(activity, savedInstanceState);

	}
	

	public boolean hasNotch() {
		return Utils.hasNotch(mActivity);
	}

	public int getNotchHeight() { return Utils.getNotchHeight(mActivity);}
	
	public String getUuid() {
		if(uuidKit == null)
			uuidKit = new DeviceUuidFactory(mActivity);
		return uuidKit.getUuid().toString();
	}
	
    public void showNotification(long delayMs, String title, String message) {
    	notificationId = notificationId + 1;
    	UnityNotificationManager.SetNotification(mActivity, notificationId, delayMs, title, message, message, 1, null, 1, 1, "", "app_icon", 16729156, mActivity.getPackageName(), "default", null);
    }
    
    public void showRepeatingNotification(long delayMs, long rep, String title, String message) {
    	notificationId = notificationId + 1;
    	UnityNotificationManager.SetRepeatingNotification(mActivity,notificationId, delayMs, title, message, message, rep, 1, null, 1, 1, "", "app_icon", 16729156, mActivity.getPackageName(), "default", null);
    }
    
    public void CancelNotification(int id) {
    	UnityNotificationManager.CancelPendingNotification(mActivity, id);
    }
    
    public void clearAllNotification() {
    	notificationId = 0;
    	UnityNotificationManager.ClearShowingNotifications(mActivity);
    }
}
