package vn.soha.game.sdk.utils;

import java.net.URLEncoder;

import org.json.JSONObject;

import vn.sgame.sdk.R;
import android.annotation.SuppressLint;
import android.app.ActivityManager;
import android.app.ActivityManager.MemoryInfo;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Environment;
import android.os.Handler;
import android.os.StatFs;
import android.preference.PreferenceManager;
import android.provider.Settings.Secure;
import android.telephony.TelephonyManager;
import android.util.DisplayMetrics;
import android.view.WindowManager;

public class LogStat {

	/* ------------- Log Device Info ------------- */
	/* ------------- Log Device Info ------------- */
	/* ------------- Log Device Info ------------- */
	public static void logDeviceInfo(final Context context) {
		new Handler().postDelayed(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub

				new Thread(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						try {
							String params = "&app_id=%s&client_id=%s&clientname=%s&sdkver=%s&gver=%s&areaid=%s&roleid=%s&access_token=%s&device_info=%s";
							String appId = Utils.getAppId(context);
							String clientId = appId;

							String sdkVer = context.getString(R.string.sdkVersion);
							String clientName = Utils.getClientName(context);

							// get gver from manifest
							PackageInfo packageInfo = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);
							String gver = packageInfo.versionName;

							String areaId = "";
							String roleId = "";
							String accessToken = "";

							String deviceInfo = LogStat.getDeviceInfo(context);

							params = String.format(params, appId, clientId, clientName, sdkVer, gver, areaId, roleId, accessToken, deviceInfo);
							String apiUrl = NameSpace.API_LOG_DEVICE_INFO + params;

							String response = ServiceHelper.get(apiUrl);

							// mark that log open app already done
							SharedPreferences sharedPreferences = PreferenceManager.getDefaultSharedPreferences(context);
							sharedPreferences.edit().putString(NameSpace.SHARED_PREF_LOG_OPEN_APP, response).commit();
						} catch (Exception e) {
							// TODO: handle exception
							e.printStackTrace();
						}				
					}
				}).start();
			}
		}, 10 * 1000);
	}	

	public static String getAppVersionCode(Context context) {
		try {
			PackageInfo packageInfo = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);
			return packageInfo.versionCode + "";
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return "";
	}

	public static String getAppVersionName(Context context) {
		PackageInfo packageInfo;
		try {
			packageInfo = context.getPackageManager().getPackageInfo(context.getPackageName(), 0);
			return packageInfo.versionName;
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return "";
	}

	public static String getPhoneModel() {
		return android.os.Build.DEVICE;
	}

	public static String getBrand() {
		return android.os.Build.BRAND;
	}

	public static String getProduct() {
		return android.os.Build.PRODUCT;
	}

	public static String getAndroidVersion() {
		return android.os.Build.VERSION.RELEASE;
	}

	@SuppressWarnings("deprecation")
	public static String getTotalMemSize() {
		StatFs memStatus = new StatFs(Environment.getExternalStorageDirectory().getPath());
		return ((long) memStatus.getBlockSize() * (long) memStatus.getBlockCount()) + "";
	}

	@SuppressWarnings("deprecation")
	public static String getAvailableMemSize() {	        
		StatFs memStatus = new StatFs(Environment.getExternalStorageDirectory().getPath());
		return ((long) memStatus.getBlockSize() * (long) memStatus.getAvailableBlocks()) + "";
	}

	public static String getResolution(Context c) {
		DisplayMetrics displayMetrics = new DisplayMetrics();
		((WindowManager) c.getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay().getMetrics(displayMetrics);
		return displayMetrics.widthPixels + "x" + displayMetrics.heightPixels;
	}

	public static String getScale(Context context) {
		WindowManager windowManager = (WindowManager) context.getSystemService(Context.WINDOW_SERVICE);
		DisplayMetrics metrics = new DisplayMetrics();
		windowManager.getDefaultDisplay().getMetrics(metrics);
		return metrics.density + "";
	}

	public static String getDeviceID(Context context) {
		String deviceId = Secure.getString(context.getContentResolver(), Secure.ANDROID_ID);
		return deviceId;
	}

	public static String getMACAddress(Context context) {
		WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
		WifiInfo wifiInfo = wifiManager.getConnectionInfo();
		String macAddr = wifiInfo.getMacAddress();
		return macAddr;
	}

	@SuppressLint("NewApi")
	public static String getTotalRAMSize(Context context) {
		int apiLevel = android.os.Build.VERSION.SDK_INT;
		if (apiLevel < 16) {
			return "";
		}

		MemoryInfo mi = new MemoryInfo();
		ActivityManager activityManager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
		activityManager.getMemoryInfo(mi);

		return mi.totalMem + "";
	}

	public static String getAvailableRAMSize(Context context) {
		MemoryInfo mi = new MemoryInfo();
		ActivityManager activityManager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
		activityManager.getMemoryInfo(mi);
		return mi.availMem + "";
	}

	public static String getIMEI(Context context) {
		return ((TelephonyManager)context.getSystemService(Context.TELEPHONY_SERVICE)).getDeviceId();
	}

	public static String getDeviceInfo(Context context) {
		try {
			JSONObject deviceInfo = new JSONObject();
			deviceInfo.put("app_version_code", getAppVersionCode(context));
			deviceInfo.put("app_version_name", getAppVersionName(context));
			deviceInfo.put("phone_model", getPhoneModel());
			deviceInfo.put("brand", getBrand());
			deviceInfo.put("product", getProduct());
			deviceInfo.put("android_version", getAndroidVersion());
			deviceInfo.put("total_mem_size", getTotalMemSize());
			deviceInfo.put("available_mem_size", getAvailableMemSize());
			deviceInfo.put("total_ram_size", getTotalRAMSize(context));
			deviceInfo.put("available_ram_size", getAvailableRAMSize(context));
			deviceInfo.put("resolution", getResolution(context));
			deviceInfo.put("device_id", getDeviceID(context));
			deviceInfo.put("mac_address", getMACAddress(context));
			deviceInfo.put("imei", getIMEI(context));
			return URLEncoder.encode(deviceInfo.toString(), "utf-8");
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			return "";
		}
	}

}
