package vn.soha.game.sdk.utils;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Iterator;
import java.util.Map;
import java.util.Map.Entry;

import org.apache.commons.io.IOUtils;
import org.json.JSONException;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.content.res.Configuration;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.Bitmap.CompressFormat;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.graphics.Rect;
import android.media.ExifInterface;
import android.net.ConnectivityManager;
import android.net.Uri;
import android.os.Environment;
import android.preference.PreferenceManager;
import android.provider.ContactsContract;
import android.provider.Settings;
import android.util.Base64;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager;
import android.view.inputmethod.InputMethodManager;
import android.widget.Toast;

import com.mesglog.sdk.MesgLog;

public class Utils {

	public static int SHOW_CONNECT_TIME = 600;
	private static String isShowConnectAccount = "isShowConnectAcc";

	
	// ducnm dashboard new
	
	
	
	public static void setConfigGCM(Context mContext, Boolean isConfiged) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putBoolean("isConfigGcm", isConfiged);
		editor.commit();
	}
	public static Boolean getConfigGCM(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean("isConfigGcm", false);

	}
		public static void vSaveCurrDashboardVersion(Context mContext, int version) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putInt("currDashboardVersion", version);
			editor.commit();
		}

		
		public static void vSaveIsShowDashboard(Context mContext, Boolean isShowing) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putBoolean("isShowDashBoard", isShowing);
			editor.commit();
		}
		public static Boolean getIsShowDashBoard(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getBoolean("isShowDashBoard", false);

		}

		public static void vSaveNewDashboardVersion(Context mContext, int version) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putInt("newDashboardVersion", version);
			editor.commit();
		}

		public static void vSaveDashboardData(Context mContext, String data) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putString("dashboardData", data);
			editor.commit();
		}

		public static int getCurrDashBoardVersion(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getInt("currDashboardVersion", -1);

		}

		public static int getNewDashBoardVersion(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getInt("newDashboardVersion", -1);

		}

		public static String getDashBoardData(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getString("dashboardData", "");

		}

		// end
		// ducnm end
	public static void setIsShowConnectAccount(Context context, Boolean data) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putBoolean(isShowConnectAccount, data).commit();
	}

	public static Boolean isShowConnectAccount(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getBoolean(isShowConnectAccount, false);
	}

	private static String uAvatar = "uAvatar";

	public static void saveUserAvatar(Context context, String userAvatar) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(uAvatar, userAvatar).commit();
	}

	public static String getUserAvatar(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(uAvatar, "");
	}

	private static String newUser = "newUser";

	public static void saveNewUser(Context context, boolean isNew) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putBoolean(newUser, isNew).commit();
	}

	public static boolean isNewUser(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getBoolean(newUser, false);
	}

	public static   String puId = "puId";

	public static void savePUserId(Context context, String userId) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(puId, userId).commit();
	}

	public static String getPUserId(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(puId, "");
	}

	public static final String EMPTY = "";

	public static void saveString(Context context, String key, String value) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(key, value).commit();
	}

	public static String getString(Context context, String key) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(key, null);
	}

	public static boolean checkInternet(Context c) {
		ConnectivityManager cm = (ConnectivityManager) c
				.getSystemService(Context.CONNECTIVITY_SERVICE);
		android.net.NetworkInfo wifi = cm
				.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
		android.net.NetworkInfo mobile = cm
				.getNetworkInfo(ConnectivityManager.TYPE_MOBILE);
		if (wifi.isConnected() || mobile.isConnected())
			return true;
		else
			return false;
	}

	public static void toast(final Activity a, final String message) {
		a.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				Toast.makeText(a, message, Toast.LENGTH_SHORT).show();
			}
		});
	}

	public static void checkInternetAndToast(Activity a) {
		if (checkInternet(a))
			toast(a, "Download error");
		else

			toast(a, "Can not connect.Check internet");
	}

	public static String MD5(String text) throws NoSuchAlgorithmException,
			UnsupportedEncodingException {
		MessageDigest md;
		md = MessageDigest.getInstance("MD5");
		byte[] md5hash = new byte[32];
		md.update(text.getBytes("iso-8859-1"), 0, text.length());
		md5hash = md.digest();
		return convertToHex(md5hash);
	}

	private static String convertToHex(byte[] data) {
		StringBuffer buf = new StringBuffer();
		for (int i = 0; i < data.length; i++) {
			int halfbyte = (data[i] >>> 4) & 0x0F;
			int two_halfs = 0;
			do {
				if ((0 <= halfbyte) && (halfbyte <= 9))
					buf.append((char) ('0' + halfbyte));
				else
					buf.append((char) ('a' + (halfbyte - 10)));
				halfbyte = data[i] & 0x0F;
			} while (two_halfs++ < 1);
		}
		return buf.toString();
	}

	public static String checkSum(String input) {
		try {
			return MD5(input);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "";
		}
	}

	public static void toggleSoftKeyboard(Context context) {
		InputMethodManager imm = (InputMethodManager) context
				.getSystemService(Context.INPUT_METHOD_SERVICE);
		imm.toggleSoftInput(0, 0);
	}

	public static String getOSVersion() {
		return android.os.Build.VERSION.RELEASE;
	}

	public static String getDeviceName() {
		return android.os.Build.MANUFACTURER + " " + android.os.Build.MODEL;
	}

	public static String getResolution(Context c) {
		DisplayMetrics displayMetrics = new DisplayMetrics();
		((WindowManager) c.getSystemService(Context.WINDOW_SERVICE))
				.getDefaultDisplay().getMetrics(displayMetrics);
		return displayMetrics.widthPixels + "x" + displayMetrics.heightPixels;
	}

	public static String getScale(Context context) {
		WindowManager windowManager = (WindowManager) context
				.getSystemService(Context.WINDOW_SERVICE);
		DisplayMetrics metrics = new DisplayMetrics();
		windowManager.getDefaultDisplay().getMetrics(metrics);
		return metrics.density + "";
	}

	public static float getScaleFloat(Context context) {
		WindowManager windowManager = (WindowManager) context
				.getSystemService(Context.WINDOW_SERVICE);
		DisplayMetrics metrics = new DisplayMetrics();
		windowManager.getDefaultDisplay().getMetrics(metrics);
		return metrics.density;
	}

	public static Bitmap resizeBitmap(String filePath, int requiredWidth) {
		// create resized bitmap
		BitmapFactory.Options options = new BitmapFactory.Options();
		options.inJustDecodeBounds = true;
		BitmapFactory.decodeFile(filePath, options);

		int originalWidth;
		int rotatationAngle = getCameraPhotoOrientation(filePath);
		if (rotatationAngle == 0 || rotatationAngle == 180) {
			originalWidth = options.outWidth;
		} else {
			originalWidth = options.outHeight;
		}

		options.inSampleSize = (int) Math.floor(originalWidth * 1.0
				/ requiredWidth);
		if (options.inSampleSize <= 1) {
			return BitmapFactory.decodeFile(filePath);
		}

		options.inJustDecodeBounds = false;
		Bitmap resizedBitmap = BitmapFactory.decodeFile(filePath, options);
		return resizedBitmap;
	}

	public static File saveBitmapToFile(Bitmap bitmap, File file) {
		try {
			FileOutputStream fos = new FileOutputStream(file);
			bitmap.compress(CompressFormat.JPEG, 90, fos);
			return file;
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			return null;
		}
	}

	public static Bitmap rotateBitmapToNormalState(Bitmap inputBitmap,
			int rotationAngle) {
		Matrix matrix = new Matrix();
		matrix.postRotate(rotationAngle);
		Bitmap rotatedBitmap = Bitmap.createBitmap(inputBitmap, 0, 0,
				inputBitmap.getWidth(), inputBitmap.getHeight(), matrix, false);
		return rotatedBitmap;
	}

	public static int getCameraPhotoOrientation(String imagePath) {
		int rotate = 0;
		try {
			File imageFile = new File(imagePath);
			ExifInterface exif = new ExifInterface(imageFile.getAbsolutePath());
			int orientation = exif.getAttributeInt(
					ExifInterface.TAG_ORIENTATION,
					ExifInterface.ORIENTATION_NORMAL);
			switch (orientation) {
			case ExifInterface.ORIENTATION_ROTATE_270:
				rotate = 270;
				break;
			case ExifInterface.ORIENTATION_ROTATE_180:
				rotate = 180;
				break;
			case ExifInterface.ORIENTATION_ROTATE_90:
				rotate = 90;
				break;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return rotate;
	}

	/**
	 * Obtains the contact list for the currently selected account.
	 * 
	 * @return A cursor for for accessing the contact list.
	 */
	public static String[] getContactsEmail(Context c) {
		// Run query
		Uri uri = ContactsContract.CommonDataKinds.Email.CONTENT_URI;
		String[] projection = new String[] { ContactsContract.CommonDataKinds.Email.DATA };
		String selection = ContactsContract.Contacts.IN_VISIBLE_GROUP + "='1'";
		String[] selectionArgs = null;
		String sortOrder = ContactsContract.Contacts.DISPLAY_NAME
				+ " COLLATE LOCALIZED ASC";

		Cursor cursor = c.getContentResolver().query(uri, projection,
				selection, selectionArgs, sortOrder);
		String[] listEmails = new String[cursor.getCount()];

		for (int i = 0; i < cursor.getCount(); i++) {
			cursor.moveToPosition(i);
			listEmails[i] = cursor.getString(0);
		}

		cursor.close();
		return listEmails;
	}

	public static String getSdkVersion(Context context) {
		return context.getString(R.string.sdkVersion);
	}

	public static void saveClientName(Context context, String clientName) {
		context.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).edit()
				.putString(NameSpace.SHARED_PREF_CLIENT_NAME, clientName)
				.commit();
	}

	public static String getClientName(Context context) {
		// check if saved clientName before
		String clientName = context.getSharedPreferences(
				NameSpace.SHARED_PREF_NAME, Context.MODE_PRIVATE).getString(
				NameSpace.SHARED_PREF_CLIENT_NAME, "");

		// if not save yet, read clientName from META-INF/client.txt
		if (clientName.equals("")) {
			try {
				InputStream configInputStream = context.getClass()
						.getClassLoader()
						.getResourceAsStream("META-INF/client.txt");
				String configString = IOUtils.toString(configInputStream);
				JSONObject configJSON = new JSONObject(configString);
				clientName = configJSON.getString("client_name");
				return clientName;
			} catch (Exception e) {
				// TODO: handle exception
				// e.printStackTrace();
				Logger.e("Can Not Read client_name From META-INF/client.txt");
				// if can not read from META-INF/client.txt => read from
				// assets/client.txt
				try {
					InputStream is = context.getAssets().open("client.txt");
					JSONObject config = new JSONObject(IOUtils.toString(is));
					String clientName2 = config.getString("client_name");
					return clientName2;
				} catch (Exception e2) {
					// TODO: handle exception
					// e2.printStackTrace();
					Logger.e("Can Not Read client_name From assets/client.txt");
					// if can not read from assets/client.txt => return default
					return "sohagame";
				}
			}
		} else {
			return clientName;
		}
	}

	public static void saveAppId(Context context, String appId) {
		context.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).edit()
				.putString(NameSpace.SHARED_PREF_APP_ID, appId).commit();
	}

	public static String getAppId(Context context) {
		// check if saved appId
		String appId = context.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).getString(NameSpace.SHARED_PREF_APP_ID,
				"");
		Logger.e("appID__"+appId);
		// if not save appId yet, read from META-INF/client.txt
		if (appId.equals("")) {
			try {
				InputStream configInputStream = context.getClass()
						.getClassLoader()
						.getResourceAsStream("META-INF/client.txt");
				String configString = IOUtils.toString(configInputStream);
				JSONObject configJSON = new JSONObject(configString);
				appId = configJSON.getString("app_id");
				
				Logger.e("appID__meta"+appId);
				saveAppId(context, appId);
				return appId;
			} catch (Exception e) {
				// TODO: handle exception
				// e.printStackTrace();
				Logger.e("Can Not Read app_id From META-INF/client.txt");
				// if can not read from META-INF/client.txt => read from
				// assets/client.txt
				try {
					InputStream is = context.getAssets().open("client.txt");
					JSONObject config = new JSONObject(IOUtils.toString(is));
					String appId2 = config.getString("app_id");
					Logger.e("appID__asset"+appId);
					saveAppId(context, appId2);
					return appId2;
				} catch (Exception e2) {
					// TODO: handle exception
					// e2.printStackTrace();
					Logger.e("Can Not Read app_id From assets/client.txt");
					// if can not read from assets/client.txt => return default
					// app id
					return "21005e7c6680a5d2e8ee2ce1512e13d1";
				}
			}
		} else {
			return appId;
		}
	}

	// Kh�i
	public static void saveAppIdFacebook(Context context, String appIdFacebook) {
		context.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE)
				.edit()
				.putString(NameSpace.SHARED_PREF_APP_ID_FACEBOOK, appIdFacebook)
				.commit();
	}

	// Kh�i
	public static String getAppIdFacebook(Context context) {
		// check if saved appIdFacebook
		String appIdFacebook = context.getSharedPreferences(
				NameSpace.SHARED_PREF_NAME, Context.MODE_PRIVATE).getString(
				NameSpace.SHARED_PREF_APP_ID_FACEBOOK, "");

		// if not save appIdFacebook yet, read from META-INF/client.txt
		if (appIdFacebook.equals("")) {
			try {
				InputStream configInputStream = context.getClass()
						.getClassLoader()
						.getResourceAsStream("META-INF/client.txt");
				String configString = IOUtils.toString(configInputStream);
				JSONObject configJSON = new JSONObject(configString);
				appIdFacebook = configJSON.getString("app_id_facebook");
				saveAppIdFacebook(context, appIdFacebook);
				return appIdFacebook;
			} catch (Exception e) {
				// TODO: handle exception
				// e.printStackTrace();
				Log.e("SohaSDK",
						"Can Not Read app_id_facebook From META-INF/client.txt");
				// if can not read from META-INF/client.txt => read from
				// assets/client.txt
				try {
					InputStream is = context.getAssets().open("client.txt");
					JSONObject config = new JSONObject(IOUtils.toString(is));
					String appIdFacebook2 = config.getString("app_id_facebook");
					saveAppIdFacebook(context, appIdFacebook2);
					return appIdFacebook2;
				} catch (Exception e2) {
					// TODO: handle exception
					// e2.printStackTrace();
					Log.e("SohaSDK",
							"Can Not Read app_id_facebook From assets/client.txt");
					// if can not read from assets/client.txt => return default
					// app id
					return "300024010098297";
				}
			}
		} else {
			return appIdFacebook;
		}
	}

	public static String getGameVersion(Context c) {
		try {
			return c.getPackageManager().getPackageInfo(c.getPackageName(), 0).versionName;
		} catch (NameNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return "";
		}
	}
	
	public static void saveAccessTokenForPush(Context context,
			String soapAccessToken) {
		PreferenceManager
				.getDefaultSharedPreferences(context)
				.edit()
				.putString(NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN+"push",
						soapAccessToken).commit();
	}

	public static String getAccessTokenForPush(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN+"push", "");
	}

	public static void saveSoapAccessToken(Context context,
			String soapAccessToken) {
		PreferenceManager
				.getDefaultSharedPreferences(context)
				.edit()
				.putString(NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN,
						soapAccessToken).commit();
	}

	public static String getSoapAccessToken(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN, "");
	}

	public static void setPendingLoginMySoha(Context context, boolean pending) {
		PreferenceManager
				.getDefaultSharedPreferences(context)
				.edit()
				.putBoolean(NameSpace.SHARED_PREF_PENDING_LOGIN_MYSOHA, pending)
				.commit();
	}

	public static boolean getPendingLoginMySoha(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getBoolean(NameSpace.SHARED_PREF_PENDING_LOGIN_MYSOHA, false);
	}

	// public static String getMySohaAccessToken(Context c) {
	// return c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
	// Context.MODE_PRIVATE).getString(NameSpace.SHARED_PREF_MYSOHA_ACCESS_TOKEN,
	// "");
	// }

	public static String getAreaId(Context c) {
		return c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).getString(NameSpace.SHARED_PREF_AREAID,
				"");
	}

	public static String getRoleId(Context c) {
		return c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).getString(NameSpace.SHARED_PREF_ROLEID,
				"");
	}

	public static String getRoleName(Context c) {
		String roleName = Base64.encodeToString(
				c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
						Context.MODE_PRIVATE)
						.getString(NameSpace.SHARED_PREF_ROLENAME, "")
						.getBytes(), Base64.DEFAULT).trim();
		// try {
		// roleName =
		// URLEncoder.encode(c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
		// Context.MODE_PRIVATE).getString(NameSpace.SHARED_PREF_ROLENAME,
		// ""), "UTF-8");
		// } catch (UnsupportedEncodingException e) {
		// // TODO Auto-generated catch block
		// e.printStackTrace();
		// }
		return roleName;
	}

	public static String getRoleLevel(Context c) {
		return c.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
				Context.MODE_PRIVATE).getString(
				NameSpace.SHARED_PREF_ROLELEVEL, "");
	}

	public static void saveUserId(Context context, String userId) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(NameSpace.SHARED_PREF_USER_ID, userId).commit();
	}

	public static String getUserId(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(NameSpace.SHARED_PREF_USER_ID, "");
	}

	public static void saveUserEmail(Context context, String userEmail) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(NameSpace.SHARED_PREF_USER_EMAIL, userEmail)
				.commit();
	}

	public static void saveUserType(Context context, String type) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString("userType", type).commit();
	}

	public static String getUserType(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString("userType", "");
	}

	public static String getUserEmail(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(NameSpace.SHARED_PREF_USER_EMAIL, "");
	}

	public static void saveUserName(Context context, String userName) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putString(NameSpace.SHARED_PREF_USER_NAME, userName).commit();
	}

	public static String getUserName(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getString(NameSpace.SHARED_PREF_USER_NAME, "");
	}

	
	public static String getDeviceIDVCC(Context context)
	{
		/*String localId = Settings.Secure.getString(context.getContentResolver(),
				Settings.Secure.ANDROID_ID);*/
		String id;
		try {
			 id = MesgLog.getDeviceID(context);
			
			if(id==null ||id.equalsIgnoreCase(""))
			{
				id = Settings.Secure.getString(context.getContentResolver(),
						Settings.Secure.ANDROID_ID);
			}
		} catch (Exception e) {
			// TODO: handle exception
			id = Settings.Secure.getString(context.getContentResolver(),
					Settings.Secure.ANDROID_ID);
		}
	
		
	   return id;
	
	//	return localId;
	}
	public static String createDefaultSOAPParams(Context context) {
		String params = "&app_id=%s&areaid=%s&roleid=%s&rolename=%s&is_encode_character=1&rolelevel=%s&gver=%s&sdkver=%s&clientname=%s&access_token=%s&device_id_vcc=%s&bundle_id=%s";
		return String.format(params, Utils.getAppId(context),
				Utils.getAreaId(context), Utils.getRoleId(context),
				Utils.getRoleName(context), Utils.getRoleLevel(context),
				AppUtils.getAppversionName(context),
				Utils.getSdkVersion(context), Utils.getClientName(context),
				Utils.getSoapAccessToken(context),
				getDeviceIDVCC(context), AppUtils.getAppPackage(context));
	}

	// May 16, 2017 by Hoang Cao Dev
	public static String publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCU+1bLfPmcY7qrF/dTbAtuJlv4R/FVc1WEH9HK"
			+ "U0jQjX/n/db9vz/x0i3te/bKLNEcwUhBu+PWPnOt/qVURG9BUT6RsCRFUn0CyGiUKoy45o9K/mJA"
			+ "HmbrNtrUB6ckrYLF75Y50nUNsBVHUDw8yQymmiOBT1gc/KM5s1xTz44LMwIDAQAB";

	public static String getDefaultParams(Context context) {
		JSONObject obj = new JSONObject();
		try {
			
			obj.put("app_id", getAppId(context));
			obj.put("area_id", getAreaId(context));
			obj.put("role_id", getRoleId(context));
			obj.put("role_name", getRoleName(context));
			obj.put("role_level", getRoleLevel(context));
			obj.put("gver", AppUtils.getAppversionName(context));
			obj.put("sdkver", getSdkVersion(context));
			obj.put("clientname", getClientName(context));
			obj.put("access_token", getSoapAccessToken(context));
			obj.put("device_id_vcc", getDeviceIDVCC(context));
			obj.put("bundle_id", context.getApplicationContext().getPackageName());
			
			
			checkAdId(context);
			obj.put("device_id",getADVERT_ID(context));
			obj.put("device_id_vcc", getDeviceIDVCC(context));
			obj.put("redirect_uri", "uri_login");
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return "signed_request="
				+ EncryptorEngine.encryptData(obj.toString(), publicKey);
	}
	
	static void checkAdId(Context context)
	{
		/*if(Utils.getADVERT_ID(context).equals(""))
		{
		int count =0;
		while (true) {
			
			if(Utils.getADVERT_ID(context).equals(""))
			{
				try {
					count+=200;
					Log.e("sleep","Sleeping____count "+count);
					Thread.sleep(200);
					
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				if(count>2000)
				{
					saveADVERT_ID(context, getDeviceIDVCC(context));
					break;}
			}
			else
			{
				Log.e("addId","Sleeping____time out get local ID  "+Utils.getADVERT_ID(context));
				break;
			}
		
		}
		}*/
		
	}
	
	public static String getParamsLoginOtp(Context context,String token,String mess,String syntax,String phone) {
		JSONObject obj = new JSONObject();
		try {
			
			obj.put("app_id", getAppId(context));
			obj.put("area_id", getAreaId(context));
			obj.put("role_id", getRoleId(context));
			obj.put("role_name", getRoleName(context));
			obj.put("role_level", getRoleLevel(context));
			obj.put("gver", AppUtils.getAppversionName(context));
			obj.put("sdkver", getSdkVersion(context));
			obj.put("clientname", getClientName(context));
			obj.put("access_token", getSoapAccessToken(context));
			obj.put("device_id_vcc", getDeviceIDVCC(context));
			obj.put("bundle_id", context.getApplicationContext().getPackageName());
			checkAdId(context);
			obj.put("device_id",getADVERT_ID(context));
			obj.put("device_id_vcc", getDeviceIDVCC(context));
			obj.put("redirect_uri", "uri_login");
			obj.put("confirm_otp", "1");
			obj.put("otp_token", token);
			obj.put("message", mess);
			obj.put("syntax", syntax);
			obj.put("phone_number", phone);
			
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return "signed_request="
				+ EncryptorEngine.encryptData(obj.toString(), publicKey);
	}

	public static String getParamsForGCM(Context context, String deviceId) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("redirect_uri", "uri_login");

			obj.put("device_token", deviceId);

			obj.put("type", "android");

		} catch (JSONException e) {
			e.printStackTrace();
		}

		 
		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}



	public static void createDefaultParam(Context context,JSONObject obj) {
		try {
			
/*			obj.put("app_id", getAppId(context));
			obj.put("areaid", getAreaId(context));
			obj.put("roleid", getRoleId(context));
			obj.put("rolename", getRoleName(context));
			obj.put("rolelevel", getRoleLevel(context));
			obj.put("gver", AppUtils.getAppversionName(context));
			obj.put("sdkver", getSdkVersion(context));
			obj.put("clientname", getClientName(context));
			obj.put("access_token", getSoapAccessToken(context));
			obj.put("areaid", getAreaId(context));
			obj.put("roleid", getRoleId(context));
			obj.put("rolename", getRoleName(context));
			obj.put("rolelevel", getRoleLevel(context));*/
			obj.put("app_id", getAppId(context));
			obj.put("area_id", getAreaId(context));
			obj.put("role_id", getRoleId(context));
			obj.put("role_name", getRoleName(context));
			obj.put("role_level", getRoleLevel(context));
			obj.put("gver", AppUtils.getAppversionName(context));
			obj.put("sdkver", getSdkVersion(context));
			obj.put("clientname", getClientName(context));
			obj.put("access_token", getSoapAccessToken(context));
			checkAdId(context);
			obj.put("device_id",getADVERT_ID(context));
			obj.put("device_id_vcc", getDeviceIDVCC(context));
			obj.put("bundle_id", context.getApplicationContext().getPackageName());
			obj.put("client_id", Utils.getMQTTClientId(context));
		} catch (JSONException e) {
			e.printStackTrace();
		}
		
		
	}
	public static String getDefaultParamsPost(Context context) {
		JSONObject obj = new JSONObject();

			createDefaultParam(context, obj);
		

		Log.e("default", "default_" + obj.toString());
		
		String param = EncryptorEngine.encryptDataNoURLEn(obj.toString(),
				publicKey);
				
		Log.e("singed","default_"+ param);
				
				
		return param;
	}

	public static String getParamsLoginBig4(Context context,
			String big4_access_token, String big4_type) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("big4_access_token", big4_access_token);
			obj.put("big4_type", big4_type);
		} catch (JSONException e) {
			e.printStackTrace();
		}

		Log.e("big4", obj.toString());
		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}

	public static String getParamsLoginBig4ConnectAcc(Context context,
			String big4_access_token, String big4_type) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("big4_access_token", big4_access_token);
			obj.put("big4_type", big4_type);
			obj.put("connect_account", 1);
		} catch (JSONException e) {
			e.printStackTrace();
		}

		Log.e("big4", obj.toString());
		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}

	// --

	// -------- Log Info Account

/*	public static String postInfoAccount(Context context) {
		JSONObject obj = new JSONObject();
		try {
			
			obj.put("app_id", getAppId(context));
			obj.put("areaid", getAreaId(context));
			obj.put("roleid", getRoleId(context));
			obj.put("rolename", getRoleName(context));
			obj.put("rolelevel", getRoleLevel(context));
			obj.put("gver", AppUtils.getAppversionName(context));
			obj.put("sdkver", getSdkVersion(context));
			obj.put("clientname", getClientName(context));
			obj.put("access_token", getSoapAccessToken(context));
			obj.put("areaid", getAreaId(context));
			obj.put("roleid", getRoleId(context));
			obj.put("rolename", getRoleName(context));
			obj.put("rolelevel", getRoleLevel(context));

		} catch (JSONException e) {
			e.printStackTrace();
		}

		return "signed_request="
				+ EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);

	}*/

	// ----- payment
	// death_note----------------------------------------------------------------

	public static String getParamsPayment(Context context) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("redirect_uri", "uri_payment");
		} catch (JSONException e) {
			e.printStackTrace();
		}

		return "signed_request="
				+ EncryptorEngine.encryptData(obj.toString(), publicKey);
	}

	public static String postCreateIAP(Context context, String order_info) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("order_info", order_info);
		} catch (JSONException e) {
			e.printStackTrace();
		}

		Log.d("signed_request_______________", "" + obj.toString());

		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}

	public static String postPlayStore(Context context, String transId,
			String purchaseData, String signature) {
		JSONObject obj = new JSONObject();
		try {
			createDefaultParam(context, obj);
			obj.put("platform", "android");
			obj.put("trans_id", transId);
			obj.put("receipt",
					JSONUtils.getReceiptIAP(context, purchaseData, signature));

		} catch (JSONException e) {
			e.printStackTrace();
		}

		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}

	public static String postSetUserConfig(Context context) {
		JSONObject obj = new JSONObject();
		
			createDefaultParam(context, obj);
		

		return EncryptorEngine.encryptDataNoURLEn(obj.toString(), publicKey);
	}

	// -----------------------------------------------------------------------

	public static String checkMySohaAppInstalled(Context context) {
		if (context.getPackageManager().getLaunchIntentForPackage(
				getString(context, NameSpace.PACKAGE_MYSOHA)) == null) {
			return "0";
		} else {
			return "1";
		}
	}

	public static void getKeyhash(Context context) {
		try {
			PackageInfo info = context.getPackageManager().getPackageInfo(
					context.getApplicationContext().getPackageName(),
					PackageManager.GET_SIGNATURES);
			for (android.content.pm.Signature signature : info.signatures) {
				MessageDigest md = MessageDigest.getInstance("SHA");
				md.update(signature.toByteArray());
				Log.e("KEY HASH:",
						Base64.encodeToString(md.digest(), Base64.DEFAULT));
			}
		} catch (NameNotFoundException e) {
			e.printStackTrace();
		} catch (NoSuchAlgorithmException e) {
			e.printStackTrace();
		}
	}

	public static boolean isFullScreen(Activity activity) {
		Rect rectangle = new Rect();
		Window window = activity.getWindow();
		window.getDecorView().getWindowVisibleDisplayFrame(rectangle);
		int statusBarHeight = rectangle.top;
		if (statusBarHeight == 0)
			return true;
		else
			return false;
	}

	public static boolean isTablet(Context context) {
		return (context.getResources().getConfiguration().screenLayout & Configuration.SCREENLAYOUT_SIZE_MASK) >= Configuration.SCREENLAYOUT_SIZE_LARGE;
	}

	public static boolean isStringNull(String str) {
		if (str == null || str.equals(""))
			return true;
		return false;
	}

	public static Bitmap takeScreenshot(Activity activity) {
		ViewGroup decor = (ViewGroup) activity.getWindow().getDecorView();
		ViewGroup decorChild = (ViewGroup) decor.getChildAt(0);
		decorChild.setDrawingCacheEnabled(true);
		decorChild.buildDrawingCache();
		Bitmap drawingCache = decorChild.getDrawingCache(true);
		Bitmap bitmap = Bitmap.createBitmap(drawingCache);
		decorChild.setDrawingCacheEnabled(false);
		return bitmap;
	}

	public static String join(Iterator<?> iterator, char separator) {
		// handle null, zero and one elements before building a buffer
		if (iterator == null) {
			return null;
		}
		if (!iterator.hasNext()) {
			return EMPTY;
		}
		Object first = iterator.next();
		if (!iterator.hasNext()) {
			return first == null ? "" : first.toString();
		}

		// two or more elements
		StringBuilder buf = new StringBuilder(256); // Java default is 16,
		// probably too small
		if (first != null) {
			buf.append(first);
		}

		while (iterator.hasNext()) {
			buf.append(separator);
			Object obj = iterator.next();
			if (obj != null) {
				buf.append(obj);
			}
		}

		return buf.toString();
	}

	public static String join(Map<?, ?> map, char separator,
			char valueStartChar, char valueEndChar) {
		// handle null, zero and one elements before building a buffer
		if (map == null) {
			return null;
		}
		if (map.size() == 0) {
			return EMPTY;
		}

		// two or more elements
		StringBuilder buf = new StringBuilder(256); // Java default is 16,
		// probably too small

		boolean isFirst = true;
		for (Entry<?, ?> entry : map.entrySet()) {
			if (isFirst) {
				buf.append(entry.getKey());
				buf.append(valueStartChar);
				buf.append(entry.getValue());
				buf.append(valueEndChar);
				isFirst = false;
			} else {
				buf.append(separator);
				buf.append(entry.getKey());
				buf.append(valueStartChar);
				buf.append(entry.getValue());
				buf.append(valueEndChar);
			}
		}

		return buf.toString();
	}

	public static boolean isSDCardOK(String folderName) {

		File file = new File(Environment.getExternalStorageDirectory() + "/"
				+ folderName);

		if (file.exists()) {

			return true;

		} else if (file.mkdirs()) {

			return true;

		}

		return false;
	}

	public static void openAlertCheckInternet(Activity activity,
			final DialogInterface.OnClickListener onClickListener) {
		AlertDialog.Builder builder = new AlertDialog.Builder(activity);
		builder.setMessage(activity
				.getString(R.string.textviewCheckInternetTryAgain));

		DialogInterface.OnClickListener mOnClickListener = new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				// TODO Auto-generated method stub
				dialog.dismiss();
				onClickListener.onClick(dialog, which);
			}
		};

		builder.setPositiveButton(activity.getString(R.string.buttonTryAgain),
				mOnClickListener);
		builder.create().show();
	}

	public static void openAlert(Activity activity,
			final DialogInterface.OnClickListener onClickListener,
			String message, String button) {
		AlertDialog.Builder builder = new AlertDialog.Builder(activity);
		builder.setMessage(message);

		DialogInterface.OnClickListener mOnClickListener = new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				// TODO Auto-generated method stub
				dialog.dismiss();
				onClickListener.onClick(dialog, which);
			}
		};

		builder.setPositiveButton(button, mOnClickListener);
		builder.create().show();
	}

	public static int getAppversionCode(Context mContext) {
		int versionCode = 1;
		try {
			versionCode = mContext.getPackageManager().getPackageInfo(
					mContext.getPackageName(), 0).versionCode;
		} catch (NameNotFoundException e) {
			e.printStackTrace();
		}
		return versionCode;
	}

	public static String TAG = "Utils";
	private static String versionCode = "versionCode";

	public static void vSaveRecentAppVersionCode(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt(versionCode, getAppversionCode(mContext));
		editor.commit();
	}

	public static int getRecentAppVersionCode(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getInt(versionCode, 1);
	}

	private static String enableWarning = "enableWarning";
	private static String imageAge = "imageAge";
	private static String warningTimeMessage = "warningTimeMessage";

	public static void vSaveWarningInfo(Context mContext, String mImageAge,
			String mWarningTimeMessage, boolean isEnable) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		Logger.e("_____IS show warning__" + isEnable);
		editor.putBoolean(enableWarning, isEnable);
		editor.putString(imageAge, mImageAge);
		editor.putString(warningTimeMessage, mWarningTimeMessage);
		editor.commit();
	}
	public static void vSaveIconDB(Context mContext, String url) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		Log.e("wwarning url", "___" + url);
		editor.putString("icon_db", url);
		editor.commit();
	}

	public static String getIconDB(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString("icon_db", "");
	}
	public static void vSaveWarningURL(Context mContext, String url) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		Log.e("wwarning url", "___" + url);
		editor.putString("warningUrl", url);
		editor.commit();
	}

	public static String getWarningURL(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString("warningUrl", "");
	}

	public static String getWarningImageAge(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(imageAge, "");
	}

	public static String getWarningTimeMessage(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(warningTimeMessage, "");
	}

	public static boolean isEnableWarning(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean(enableWarning, true);
	}

	// enable warning age
	private static String enableWarningAge = "enableWarningAge";
	public static void saveMqttLimit(Context mContext, int size) {

		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt("mqtt_limit_connect", size);
		editor.commit();
	}

	public static int getMqttLimit(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getInt("mqtt_limit_connect", 100);
	}
	public static void saveWarningWidth(Context mContext, int size) {

		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt("image_warning_width", size);
		editor.commit();
	}

	public static int getWarningWidth(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getInt("image_warning_width", 100);
	}

	public static void saveWarningHeight(Context mContext, int size) {

		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt("image_warning_height", size);
		editor.commit();
	}

	public static int getWarningHeight(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getInt("image_warning_height", 45);
	}


	public static void vSaveWarningAge(Context mContext, boolean isEnable) {
    
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putBoolean(enableWarningAge, isEnable);
		editor.commit();
	}

	public static boolean isEnableWarningAge(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean(enableWarningAge, false);
	}

	// --

	private static String iconGame = "iconGame";

	public static void vSaveIconGame(Context mContext, String mIconGame) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putString(iconGame, mIconGame);
		editor.commit();
	}

	public static String getIconGame(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(iconGame, "");
	}

	private static String loginBig4Param = "loginBig4Param";

	public static void vSaveLoginBig4Param(Context mContext,
			String mLoginBig4Param) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putString(loginBig4Param, mLoginBig4Param);
		editor.commit();
	}

	public static String getLoginBig4Param(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(loginBig4Param, "").replaceAll("sohagame", "");
		// return "";
	}

	// mySoha button position
	private static String mySohaButtonPositionX = "mySohaButtonPositionX";
	private static String mySohaButtonPositionY = "mySohaButtonPositionY";

	public static void vSaveMySohaButtonPosition(Context mContext, int x, int y) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt(mySohaButtonPositionX, x);
		editor.putInt(mySohaButtonPositionY, y);
		editor.commit();
	}

	public static int getMySohaButtonPositionX(Activity activity) {
		SharedPreferences sp = activity.getSharedPreferences(TAG, 0);
		return sp.getInt(mySohaButtonPositionX, 0);
	}

	public static int getMySohaButtonPositionY(Activity activity) {
		SharedPreferences sp = activity.getSharedPreferences(TAG, 0);
		return sp.getInt(mySohaButtonPositionY,
				ScreenUtils.getScreenHeight(activity) / 2);
	}

	// --

	// warningButton position
	private static String warningButtonPositionX = "warningButtonPositionX";
	private static String warningButtonPositionY = "warningButtonPositionY";

	public static void vSaveWarningButtonPosition(Context mContext, int x, int y) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putInt(warningButtonPositionX, x);
		editor.putInt(warningButtonPositionY, y);
		editor.commit();
	}

	public static int getWarningButtonPositionX(Activity activity) {
		SharedPreferences sp = activity.getSharedPreferences(TAG, 0);
		return sp.getInt(warningButtonPositionX, 0);
	}

	public static int getWarningButtonPositionY(Activity activity) {
		SharedPreferences sp = activity.getSharedPreferences(TAG, 0);
		return sp.getInt(warningButtonPositionY,
				ScreenUtils.getScreenHeight(activity) / 3);
	}

	// --

	// enable dialog warning
	private static String isEnableDialogWarning = "isEnableDialogWarning";

	public static void vDisableDialogWarning(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putBoolean(isEnableDialogWarning, false);
		editor.commit();
	}

	public static boolean isEnableDialogWarning(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean(isEnableDialogWarning, true);
	}

	// --

	public static int dpToPx(Context context, int dp) {
		DisplayMetrics displayMetrics = context.getResources()
				.getDisplayMetrics();
		int px = Math.round(dp
				* (displayMetrics.xdpi / DisplayMetrics.DENSITY_DEFAULT));
		return px;
	}

	// /////
	private static String timeCache = "timeCache";

	public static void vSavetimeCache(Context mContext, String mtimeCache) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putString(timeCache, mtimeCache);
		editor.commit();
	}

	public static String gettimeCache(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(timeCache, "hi");
		// return "";
	}

	// ///////
	private static String isUpdateWebview = "isUpdateWebview";

	public static void vSaveUpdateWebview(Context mContext, boolean b) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putBoolean(isUpdateWebview, b);
		editor.commit();
	}

	public static boolean isUpdateWebview(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean(isUpdateWebview, true);
	}

	public static String getMQTTClientCode(Context context) {
		// check if saved clientName before
		String clientCode = context.getSharedPreferences(
				NameSpace.SHARED_PREF_NAME, Context.MODE_PRIVATE).getString(
				NameSpace.SHARED_PREF_CLIENT_NAME, "");

		// if not save yet, read clientName from META-INF/client.txt
		if (clientCode.equals("")) {
			try {
				InputStream configInputStream = context.getClass()
						.getClassLoader()
						.getResourceAsStream("META-INF/client.txt");
				String configString = IOUtils.toString(configInputStream);
				JSONObject configJSON = new JSONObject(configString);
				clientCode = configJSON.getString("client_code");
				return clientCode;
			} catch (Exception e) {
				// TODO: handle exception
				// e.printStackTrace();
				Logger.e("Can Not Read client_name From META-INF/client.txt");
				// if can not read from META-INF/client.txt => read from
				// assets/client.txt
				try {
					InputStream is = context.getAssets().open("client.txt");
					JSONObject config = new JSONObject(IOUtils.toString(is));
					String clientName2 = config.getString("client_code");
					return clientName2;
				} catch (Exception e2) {
					// TODO: handle exception
					// e2.printStackTrace();
					Logger.e("Can Not Read client_name From assets/client.txt");
					// if can not read from assets/client.txt => return default
					return "MobileAppDemo";
				}
			}
		} else {
			return clientCode;
		}
	}

	private static String MQTT_CLIENTID = "MQTT_CLIENTID_AUTHEN";

	public static void saveMQTTClientId(Context mContext, String clientId) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putString(MQTT_CLIENTID+Utils.getAppId(mContext)+Utils.getSdkVersion(mContext), clientId);
		editor.commit();
	}

	public static String getMQTTClientId(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(MQTT_CLIENTID+Utils.getAppId(mContext)+Utils.getSdkVersion(mContext), "");
	}

	public static void setAppInstalled(Context mContext, boolean isInstalled) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putBoolean(NameSpace.SHARED_PREF_IS_INSTALLED_APP, isInstalled);
		editor.commit();
	}

	public static boolean isAppInstalled(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getBoolean(NameSpace.SHARED_PREF_IS_INSTALLED_APP, false);
	}

	public static String ADVERT_ID = "ADVERT_ID";

	public static void saveADVERT_ID(Context mContext, String clientId) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		SharedPreferences.Editor editor = sp.edit();
		editor.putString(ADVERT_ID, clientId);
		editor.commit();
	}

	public static String getADVERT_ID(Context mContext) {
		SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
		return sp.getString(ADVERT_ID, "");
	}
	public static void setAppOpened(Context context, boolean isOpened) {
		PreferenceManager.getDefaultSharedPreferences(context).edit()
				.putBoolean(NameSpace.SHARED_PREF_IS_OPENED_APP, isOpened)
				.commit();
	}

	public static boolean isAppOpened(Context context) {
		return PreferenceManager.getDefaultSharedPreferences(context)
				.getBoolean(NameSpace.SHARED_PREF_IS_OPENED_APP, false);
	}
	// MQTT HERE
		private static String CONNECTMQTTAUTHEN = "CONNECTMQTTAUTHEN";
		public static boolean isConnectedMQTTWithAuthen(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getBoolean(CONNECTMQTTAUTHEN, false);
		}

		public static void setConnectedMQTTWithAuthen(Context mContext,
				boolean isEnable) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putBoolean(CONNECTMQTTAUTHEN, isEnable);
			editor.commit();
		}
		private static String deviceTokenSoap = "deviceTokenSoap";

		public static void vSaveDeviceTokenSoap(Context c, String mDeviceTokenSoap) {
			SharedPreferences.Editor editor = c.getSharedPreferences(TAG, 0).edit();
			editor.putString(deviceTokenSoap, mDeviceTokenSoap).commit();
		}

		public static String getDeviceTokenSoap(Context c) {
			return c.getSharedPreferences(TAG, 0).getString(deviceTokenSoap, "");
		}
		private static String MQTT_SESSION_ID = "MQTT_SESSION_ID";

		public static void saveSessionId(Context mContext, String sessionId) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putString(MQTT_SESSION_ID, sessionId);
			editor.commit();
		}

		public static String getSessionId(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getString(MQTT_SESSION_ID, "");
		}
		public static void saveEnName(Context mContext, String enName) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			SharedPreferences.Editor editor = sp.edit();
			editor.putString(EN_NAME, enName);
			editor.commit();
		}

		private static String EN_NAME = "EN_NAME";

		public static String getEnName(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(TAG, 0);
			return sp.getString(EN_NAME, "");
		}
		public static void setAppInstalledMQTT(Context mContext, boolean isInstalled) {
			mContext.getSharedPreferences(NameSpace.SHARED_PREF_NAME+mContext.getPackageName()+"global3",
					Context.MODE_PRIVATE)
					.edit()
					.putBoolean(NameSpace.SHARED_PREF_IS_INSTALLED_MQTT+mContext.getPackageName()+"3", isInstalled)
					.commit();
		}

		public static boolean isAppInstalledMQTT(Context mContext) {
			SharedPreferences sp = mContext.getSharedPreferences(
					NameSpace.SHARED_PREF_NAME+mContext.getPackageName()+"global3", Context.MODE_PRIVATE);
			Boolean test = sp.getBoolean(NameSpace.SHARED_PREF_IS_INSTALLED_MQTT+mContext.getPackageName()+"3", false);
			return test;
		}
}
