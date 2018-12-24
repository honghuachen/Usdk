package vn.soha.game.sdk.utils;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.telephony.TelephonyManager;

/**
 * @since June 27, 2014
 * @author hoangcaomobile
 *
 */
public class NetworkUtils {

	public static boolean isInternetConnected(Context context) {
		ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
		android.net.NetworkInfo wifi = cm.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
		android.net.NetworkInfo mobile = cm.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
		if (wifi.isConnected() || mobile.isConnected()) return true;
		else return false;
	}

	public static boolean isWifiConnected(Context context) {
		ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
		android.net.NetworkInfo wifi = cm.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
		if (wifi.isConnected()) return true;
		else return false;
	}

	public static String getNetworkClass(Context context) {
		ConnectivityManager cm = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);      
		NetworkInfo info = cm.getActiveNetworkInfo();
		if(info==null || !info.isConnected())
			return "-"; //not connected
		if(info.getType() == ConnectivityManager.TYPE_WIFI)
			return "WIFI";
		if(info.getType() == ConnectivityManager.TYPE_MOBILE){
			int networkType = info.getSubtype();
			switch (networkType) {
			case TelephonyManager.NETWORK_TYPE_GPRS:
			case TelephonyManager.NETWORK_TYPE_EDGE:
			case TelephonyManager.NETWORK_TYPE_CDMA:
			case TelephonyManager.NETWORK_TYPE_1xRTT:
			case TelephonyManager.NETWORK_TYPE_IDEN: //api<8 : replace by 11
				return "2G";
			case TelephonyManager.NETWORK_TYPE_UMTS:
			case TelephonyManager.NETWORK_TYPE_EVDO_0:
			case TelephonyManager.NETWORK_TYPE_EVDO_A:
			case TelephonyManager.NETWORK_TYPE_HSDPA:
			case TelephonyManager.NETWORK_TYPE_HSUPA:
			case TelephonyManager.NETWORK_TYPE_HSPA:
			case TelephonyManager.NETWORK_TYPE_EVDO_B: //api<9 : replace by 14
			case TelephonyManager.NETWORK_TYPE_EHRPD:  //api<11 : replace by 12
			case TelephonyManager.NETWORK_TYPE_HSPAP:  //api<13 : replace by 15
				return "3G";
			case TelephonyManager.NETWORK_TYPE_LTE:    //api<11 : replace by 13
				return "4G";
			default:
				return "?";
			}
		}
		return "?";
	}

}
