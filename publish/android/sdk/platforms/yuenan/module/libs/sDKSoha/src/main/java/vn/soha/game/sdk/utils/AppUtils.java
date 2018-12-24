package vn.soha.game.sdk.utils;

import android.content.Context;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;

/**
 * @since November 07, 2014
 * @author hoangcaomobile
 *
 */
public class AppUtils {

	public static String getAppName(Context mContext) {
		String appName = "Sohagame";
		try {
			int stringId = mContext.getApplicationInfo().labelRes;
			appName = mContext.getString(stringId);
		} catch (Exception e) {
			e.printStackTrace();
		}
		return appName;
	}

	public static int getAppIcon(Context mContext) {
		int appIcon = android.R.drawable.ic_notification_overlay;
		try {
			appIcon = mContext.getApplicationInfo().icon;
		} catch (Exception e) {
			e.printStackTrace();
		}
		return appIcon;
	}

	public static String getAppPackage(Context mContext) {
		return mContext.getPackageName();
	}

	public static boolean isAppInstalled(Context mContext, String mAppPackage) {
		PackageManager pm = mContext.getPackageManager();
		try {
			pm.getPackageInfo(mAppPackage, PackageManager.GET_ACTIVITIES);
			return true;
		} catch (NameNotFoundException e) {
			return false;
		}
	}

	public static int getAppversionCode(Context mContext) {
		int versionCode = 1;
		try {
			versionCode = mContext.getPackageManager().getPackageInfo(mContext.getPackageName(), 0).versionCode;
		} catch (NameNotFoundException e) {
			e.printStackTrace();
		}
		return versionCode;
	}

	public static String getAppversionName(Context mContext) {
		String versionName = "1";
		try {
			versionName = mContext.getPackageManager().getPackageInfo(mContext.getPackageName(), 0).versionName;
		} catch (NameNotFoundException e) {
			e.printStackTrace();
		}
		return versionName;
	}

}
