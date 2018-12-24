package vn.soha.game.sdk.utils;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.graphics.Point;
import android.os.Build;
import android.view.Display;
import android.view.View;
import android.view.WindowManager;

/**
 * @since June 25, 2014
 * @author hoangcaomobile
 *
 */
public class ScreenUtils {

	// private static String TAG = "SohagameSDK @" + ScreenUtils.class.getSimpleName();

	// get size of screen width
	@SuppressWarnings("deprecation")
	@SuppressLint("NewApi")
	public static int getScreenWidth(Activity mActivity) {
		int screenWidth = 0;  
		//		int screenHeight = 0;  
		Point size = new Point();
		WindowManager w = mActivity.getWindowManager();

		if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)    {
			w.getDefaultDisplay().getSize(size);
			screenWidth = size.x;
			//			screenHeight = size.y; 
		} else {
			Display d = w.getDefaultDisplay(); 
			screenWidth = d.getWidth(); 
			//			screenHeight = d.getHeight(); 
		}
		return screenWidth;

	}
	// --

	// get size of screen height
	@SuppressWarnings("deprecation")
	@SuppressLint("NewApi")
	public static int getScreenHeight(Activity mActivity) {
		int screenHeight = 0;  
		//		int screenHeight = 0;  
		Point size = new Point();
		WindowManager w = mActivity.getWindowManager();

		if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)    {
			w.getDefaultDisplay().getSize(size);
			screenHeight = size.y;
			//			screenHeight = size.y; 
		} else {
			Display d = w.getDefaultDisplay(); 
			screenHeight = d.getHeight(); 
			//			screenHeight = d.getHeight(); 
		}
		return screenHeight;

	}
	// --

	public static boolean isPortrait(Activity mActivity) {
		if (getScreenWidth(mActivity) < getScreenHeight(mActivity)) {
			return true;
		}
		return false;
	}

	public static void vSetPadding(Activity mActivity, View v) {
		int padding = 10;
		if (ScreenUtils.isPortrait(mActivity)) {
			padding = (ScreenUtils.getScreenWidth(mActivity) / 100) * 10;
		} else {
			int widthOfPortrait = (ScreenUtils.getScreenHeight(mActivity) / 100) * 90;
			padding = (ScreenUtils.getScreenWidth(mActivity) - widthOfPortrait) / 2;
		}
		v.setPadding(padding, 0, padding, 0);
		// Log.e(TAG, "is Portrait = " + ScreenUtils.isPortrait(mActivity));
		// Log.e(TAG, "width = " + Integer.toString(ScreenUtils.getScreenWidth(mActivity) - (padding * 2)));
	}

	public static void vSetPadding(Activity mActivity, View v, int top, int bottom) {
		int padding = 10;
		if (ScreenUtils.isPortrait(mActivity)) {
			padding = (ScreenUtils.getScreenWidth(mActivity) / 100) * 10;
		} else {
			int widthOfPortrait = (ScreenUtils.getScreenHeight(mActivity) / 100) * 90;
			padding = (ScreenUtils.getScreenWidth(mActivity) - widthOfPortrait) / 2;
		}
		v.setPadding(padding, top, padding, bottom);
		// Log.e(TAG, "is Portrait = " + ScreenUtils.isPortrait(mActivity));
		// Log.e(TAG, "width = " + Integer.toString(ScreenUtils.getScreenWidth(mActivity) - (padding * 2)));
	}

	public static boolean isTablet(Context context) {
		return (context.getResources().getConfiguration().screenLayout & Configuration.SCREENLAYOUT_SIZE_MASK) >= Configuration.SCREENLAYOUT_SIZE_LARGE;
	}
}
