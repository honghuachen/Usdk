package vn.soha.game.sdk.utils;

import java.io.InputStream;

import org.apache.commons.io.IOUtils;
import org.json.JSONObject;

import android.content.Context;
import android.util.Log;

/**
 * @since June 02, 2014
 * @author hoangcaomobile
 *
 */
public class TextUtils {

	// account and password
	public static boolean isAccountPasswordOk(String text) {
		boolean isOk = true;
		if (text.matches("[a-z0-9_]*")) {
			isOk = true;
		} else {
			isOk = false;
		}
		return isOk;
	}
	// --

	//ducnm start   linh tinh
	public static boolean checkAccount(String text) {
		if(text!=null&&!text.trim().equals("")&&text.length()>6)
		{
			return true;
		}
		return false;
	}
	public static boolean checkPassWord(String text) {
		if(text!=null&&!text.trim().equals("")&&text.length()>6)
		{
			return true;
		}
		return false;
	}
	public static boolean checkEmail(String text) {
		if (text.contains("@") && text.contains(".") && text.length() > 10) {
			return true;
		}
		return false;
	}
	//ducnm end

	// check if string is int
	public static boolean isInt(String string) {  
		try {  
			int i = Integer.parseInt(string);
			Log.i("TextUtils", "is int: " + Integer.toString(i));
		}  
		catch(NumberFormatException nfe) {  
			return false;  
		}  
		return true;  
	}
	// --

	// check if string is null
	public static boolean isStringNull(String str) {
		if (str == null || str.equals(""))
			return true;
		return false;
	}

	// -- 

	// get email
	//	public static String getEmail(Context context) {
	//		Pattern emailPattern = Patterns.EMAIL_ADDRESS;
	//		Account[] accounts = AccountManager.get(context).getAccounts();
	//		for (Account account : accounts) {
	//			if (emailPattern.matcher(account.name).matches()) {
	//				if (account.name.contains("@gmail.com") && account.type.contains("com.google")) {
	//					return account.name;
	//				}
	//			}
	//		}
	//		return "";
	//	}
	// --

	public static String getAppId(Context context) {
		try {
			InputStream configInputStream = context.getClass().getClassLoader().getResourceAsStream("META-INF/client.txt");
			String configString = IOUtils.toString(configInputStream);
			JSONObject configJSON = new JSONObject(configString);
			return configJSON.getString("app_id");
		} catch (Exception e) {
			// TODO: handle exception
			//					 e.printStackTrace();
			Log.e("stk", "Can Not Read app_id From META-INF/client.txt");
			// if can not read from META-INF/client.txt => read from assets/client.txt
			try {
				InputStream is = context.getAssets().open("client.txt");
				JSONObject config = new JSONObject(IOUtils.toString(is));
				return config.getString("app_id");
			} catch (Exception e2) {
				// TODO: handle exception
				//						e2.printStackTrace();
				Log.e("stk", "Can Not Read app_id From assets/client.txt");
				// if can not read from assets/client.txt => return default app id
				return "21005e7c6680a5d2e8ee2ce1512e13d1";	
			}
		}
	}

}
