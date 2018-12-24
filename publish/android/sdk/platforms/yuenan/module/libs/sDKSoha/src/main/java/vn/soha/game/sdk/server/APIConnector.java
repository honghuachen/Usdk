package vn.soha.game.sdk.server;

import java.util.ArrayList;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONObject;

import vn.soha.game.sdk.utils.InviteUtils;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.Utils;
import android.annotation.SuppressLint;
import android.content.Context;
import android.util.Base64;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
@SuppressLint("SimpleDateFormat")
public class APIConnector {

	public static final int TIMEOUT_CONNECTION = 15000;
	public static final int TIMEOUT_SOCKET = 15000;

	// post
	private static JsonParser jsonParser = new JsonParser();

	


	
/*	public static JSONObject paymentIAPCreateNew(Context mContext, String orderInfo) {
		Logger.e(API.paymentIAPCreate);
		List<NameValuePair> params = new ArrayList<NameValuePair>();
		params.add(new BasicNameValuePair("signed_request", Utils.postCreateIAP(mContext, orderInfo)));

		String apiUrl = API.paymentIAPCreate;
		Logger.e(apiUrl);

		//JSONObject json = jsonParser.getJSONFromUrl(apiUrl, params);

		return null;
	}
	*/



	public static JSONObject inviteFriendFacebookVerify(Context mContext, List<String> invitedFriends) {
		List<NameValuePair> params = new ArrayList<NameValuePair>();
		params.add(new BasicNameValuePair("access_token", Utils.getSoapAccessToken(mContext)));
		params.add(new BasicNameValuePair("role_id", Utils.getRoleId(mContext)));
		params.add(new BasicNameValuePair("area_id", Utils.getAreaId(mContext)));
		params.add(new BasicNameValuePair("type", "2"));
		String data = Base64.encodeToString(InviteUtils.getFacebookIFJsonData(mContext, invitedFriends).getBytes(), Base64.DEFAULT);
		params.add(new BasicNameValuePair("data", data));
		//JSONObject json = jsonParser.getJSONFromUrl(API.inviteFriendFacebookVerify, params);
		return null;
	}
	// --
}
