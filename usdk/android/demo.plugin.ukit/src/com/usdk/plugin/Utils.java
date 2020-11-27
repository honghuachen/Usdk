
package com.usdk.plugin;

import java.io.File;
import java.lang.reflect.Method;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Locale;

import android.annotation.SuppressLint;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.ClipData;
import android.content.ClipboardManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Environment;
import android.os.StatFs;
import android.support.v4.content.ContextCompat;
import android.support.v4.content.FileProvider;
import android.telephony.TelephonyManager;
import android.text.TextUtils;
import android.util.Log;
import android.view.DisplayCutout;
import android.view.View;
import android.view.WindowInsets;
import android.widget.Toast;

import notchtools.geek.com.notchtools.helper.SystemProperties;
import org.json.JSONException;
import org.json.JSONObject;

public class Utils {

	private static final String TAG = "ukit";

	@TargetApi(Build.VERSION_CODES.HONEYCOMB)
	public static void copyTextToClipboard(Activity activity, final String text) {
		ClipboardManager clipboard = (ClipboardManager) activity.getSystemService(Context.CLIPBOARD_SERVICE);
		ClipData textCd = ClipData.newPlainText("Label", text);
		clipboard.setPrimaryClip(textCd);
	}

	public static void jumpToApp(Activity activity, String uri, String webUrl) {
		Intent intent = jumpToApp(activity.getPackageManager(), uri);
		if (intent != null) {
			activity.startActivity(intent);
		}
		else {
			intent = jumpToApp(activity.getPackageManager(), webUrl);
			activity.startActivity(intent);
		}
	}

	public static boolean CheckPermission(Context context, String permission, String FailText) {
		int i = ContextCompat.checkSelfPermission(context, permission);
		if (i != PackageManager.PERMISSION_GRANTED) {
			Toast.makeText(context, FailText, Toast.LENGTH_SHORT).show();
			return false;
		}
		return true;
	}

	@SuppressLint("NewApi")
	public static void restartApplication(Activity context) {
		PackageManager packageManager = context.getPackageManager();
		Intent intent = packageManager.getLaunchIntentForPackage(context.getPackageName());
		ComponentName componentName = intent.getComponent();
		Intent mainIntent = Intent.makeRestartActivityTask(componentName);
		mainIntent.addFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
		context.startActivity(mainIntent);
		System.exit(0);
	}

	public static String getSimOperatorName(Context context) {
		TelephonyManager telManager = (TelephonyManager) context.getSystemService(Context.TELEPHONY_SERVICE);
		String operator = telManager.getSimOperator();
		return operator;
	}

	private static Intent jumpToApp(PackageManager pm, String uri) {
		if (!TextUtils.isEmpty(uri)) {
			Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(uri));
			if (intent.resolveActivity(pm) != null) return intent;
		}
		return null;
	}

	@SuppressWarnings("deprecation")
	public static int GetDiskFreeSizeMB() {
		StatFs statfs = new StatFs(Environment.getExternalStorageDirectory().getAbsolutePath());
		long size = statfs.getBlockSize();
		long count = statfs.getAvailableBlocks();
		int rst = (int) (size * count / 1024 / 1024);
		return rst;
	}

	public static void install(Context context, String filePath) {
		File apkFile = new File(filePath);
		Intent intent = new Intent(Intent.ACTION_VIEW);
		intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
			intent.setFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
			Uri contentUri = FileProvider.getUriForFile(context, context.getPackageName() + ".fileprovider", apkFile);
			Log.i(TAG, "android n path: " + contentUri.toString());
			intent.setDataAndType(contentUri, "application/vnd.android.package-archive");
		}
		else {
			Log.i(TAG, "android path: " + apkFile.getPath());
			intent.setDataAndType(Uri.fromFile(apkFile), "application/vnd.android.package-archive");
		}
		context.startActivity(intent);
	}

	public static boolean hasNotch(Activity activity) {
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
			boolean result = hasNotchInAndroidP(activity);
			if (result) {
				return true;
			}
		}

		String brand = Build.BRAND.toLowerCase(Locale.ROOT);
		Log.d(TAG, "hasNotch: " + brand);
		if (brand.equals("huawei") || brand.equals("honor")) {
			return hasNotchInHuawei(activity);
		} else if (brand.equals("oppo")) {
			return hasNotchInOppo(activity);
		} else if (brand.equals("vivo")) {
			return hasNotchInVivo(activity);
		} else if (brand.equals("xiaomi")) {
			return hasNotchInMi(activity);
		} else {
			return false;
		}
	}

	public static int getNotchHeight(Activity activity) {
		if (!hasNotch(activity)) {
			return 0;
		}

		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
			int result = getNotchHeightInAndroidP(activity);
			if (result > 0) {
				return result;
			}
		}

		String brand = Build.BRAND.toLowerCase(Locale.ROOT);
		Log.d(TAG, "getNotchHeight: " + brand);
		if (brand.equals("huawei") || brand.equals("honor")) {
			return getNotchHeightInHuawei(activity);
		} else if (brand.equals("oppo")) {
			return getNotchHeightInOppo(activity);
		} else if (brand.equals("vivo")) {
			return getNotchHeightInVivo(activity);
		} else if (brand.equals("xiaomi")) {
			return getNotchHeightInMi(activity);
		} else {
			return 0;
		}
	}

	/**
	 * OPPO
	 */
	private static boolean hasNotchInOppo(Context context) {
		return context.getPackageManager().hasSystemFeature("com.oppo.feature.screen.heteromorphism");
	}

	/**
	 * VIVO
	 */
	private static boolean hasNotchInVivo(Context context) {
		boolean hasNotch = false;
		try {
			ClassLoader cl = context.getClassLoader();
			Class ftFeature = cl.loadClass("android.util.FtFeature");
			Method[] methods = ftFeature.getDeclaredMethods();
			if (methods != null) {
				for (int i = 0; i < methods.length; i++) {
					Method method = methods[i];
					if (method.getName().equalsIgnoreCase("isFeatureSupport")) {
						hasNotch = (Boolean) method.invoke(ftFeature, 0x00000020);
						break;
					}
				}
			}
		}
		catch (Exception e) {
			e.printStackTrace();
			hasNotch = false;
		}
		return hasNotch;
	}

	/**
	 * HUAWEI 
	 */
	private static boolean hasNotchInHuawei(Context context) {
		boolean hasNotch = false;
		try {
			ClassLoader cl = context.getClassLoader();
			Class HwNotchSizeUtil = cl.loadClass("com.huawei.android.util.HwNotchSizeUtil");
			Method get = HwNotchSizeUtil.getMethod("hasNotchInScreen");
			hasNotch = (Boolean) get.invoke(HwNotchSizeUtil);
		}
		catch (Exception e) {
			e.printStackTrace();
		}
		return hasNotch;
	}

	private static boolean hasNotchInMi(Context context) {
		boolean hasNotch = SystemProperties.getInstance().get("ro.miui.notch").equals("1");
//		try {
//			ClassLoader cl = context.getClassLoader();
//			Class c = cl.loadClass("android.os.SystemProperties");
//			Method get = c.getMethod("get", String.class, Integer.class);
//			hasNotch = ((Integer) get.invoke("ro.miui.notch", 0) == 1);
//		}
//		catch (Exception e) {
//			e.printStackTrace();
//		}
		return hasNotch;
	}

	@TargetApi(28)
	private static boolean hasNotchInAndroidP(Activity activity) {
		boolean hasNotch = true;

		try {
			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
				View decorView = activity.getWindow().getDecorView();
				if (decorView != null) {
					WindowInsets windowinsets = decorView.getRootWindowInsets();
					if (windowinsets != null) {
						DisplayCutout cutout = windowinsets.getDisplayCutout();
						if (cutout.getSafeInsetTop() > 0) {
							hasNotch = true;
						} else {
							hasNotch = false;
						}
					}
				}
			}
		} catch(Exception e) {
			e.printStackTrace();
		}

		return hasNotch;
	}

	public static int getStatusBarHeight(Context context) {
		int statusBarHeight = 0;

		try {
			int resourceId = context.getResources().getIdentifier("status_bar_height", "dimen", "android");
			if (resourceId > 0) {
				statusBarHeight = context.getResources().getDimensionPixelSize(resourceId);
			}
		} catch(Exception e) {
			e.printStackTrace();
		}

		return statusBarHeight;
	}

	private static int getNotchHeightInHuawei(Context context) {
		int height = 0;

		try {
			ClassLoader cl = context.getClassLoader();
			Class<?> HwNotchSizeUtil = cl.loadClass("com.huawei.android.util.HwNotchSizeUtil");
			Method get = HwNotchSizeUtil.getMethod("getNotchSize");
			int[] ret = (int []) get.invoke(HwNotchSizeUtil);
			height = ret[1];
		} catch(Exception e) {
			e.printStackTrace();
		}

		return height;
	}

	private static int getNotchHeightInOppo(Context context) {
		return getStatusBarHeight(context);
	}

	private static int getNotchHeightInVivo(Context context) {
		return getStatusBarHeight(context);
	}

	private static int getNotchHeightInMi(Context context) {
		return getStatusBarHeight(context);
	}

	@TargetApi(28)
	@SuppressLint("NewApi")
	private static int getNotchHeightInAndroidP(Activity activity) {
		int height = 0;

		try {
			if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
				View decorView = activity.getWindow().getDecorView();
				if (decorView != null) {
					WindowInsets windowinsets = decorView.getRootWindowInsets();
					if (windowinsets != null) {
						DisplayCutout cutout = windowinsets.getDisplayCutout();
						height = cutout.getSafeInsetTop();
					}
				}
			}
		} catch(Exception e) {
			e.printStackTrace();
		}

		return height;
	}

	public static HashMap<String,Object> decodeJsonToMap(String json)
	{
		HashMap<String, Object> hashMap = new HashMap<String, Object>();
		try {
			JSONObject go = new JSONObject(json);
			Iterator<String> collection = go.keys();
			while (collection.hasNext()) {
				String jkey = collection.next();
				hashMap.put(jkey, go.get(jkey));
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return hashMap;
	}
}
