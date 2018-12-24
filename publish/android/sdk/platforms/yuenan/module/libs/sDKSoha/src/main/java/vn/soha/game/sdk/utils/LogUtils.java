package vn.soha.game.sdk.utils;

import java.util.ArrayList;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONObject;

import vn.soha.game.sdk.server.API;
import android.content.Context;
import android.util.Log;

public class LogUtils {
	public static final String GAME_STATE_LOGIN = "login";
	public static final String GAME_STATE_LOGOUT = "logout";
	public static final String GAME_STATE_STARTGAME = "start_game";
	public static final String GAME_STATE_ENDGAME = "end_game";

	public static void logDeviceId(final Context context, final String deviceId) {
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				try {
					List<NameValuePair> params = new ArrayList<NameValuePair>();
					String param = Utils.getParamsForGCM(context, deviceId);

					Log.e("gcm ", "__gcm param " + param);

					Log.e("gcm ", "__gcm url " + API.logGCM);
					params.add(new BasicNameValuePair("signed_request", param));

					JSONObject data = JsonParser.getJSONFromPostUrl(API.logGCM,
							params);
					Utils.setConfigGCM(context, true);
					Log.e("Log", "__gcm response" + data.toString());
				} catch (Exception e) {
					e.printStackTrace();
				}

			}
		}).start();
	}

	public static void logGameState(final Context context, final String state) {
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				String data = "app_key=" + Utils.getAppId(context)
						+ "&device_type=android" + "&user_email="
						+ Utils.getUserEmail(context) + "&type=" + state;
				ServiceHelper.post(NameSpace.API_LOG_GAME_STATE, data);
			}
		}).start();
	}

}
