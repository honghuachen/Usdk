package vn.soha.game.sdk.utils;

import java.io.IOException;
import java.io.StringWriter;
import java.util.LinkedList;
import java.util.List;

import org.json.simple.JSONObject;

import android.content.Context;
import android.util.Log;

import com.facebook.AccessToken;

public class InviteUtils {

	public static String getFacebookIFJsonData(Context mContext, List<String> invitedFriends) {
		// {"app_id":2,"user_id_facebook":1,"users_invite":[3,4,5]}
		StringWriter out = new StringWriter();

		JSONObject obj = new JSONObject();
		obj.put("user_id_facebook", AccessToken.getCurrentAccessToken().getUserId());
		obj.put("app_id", Utils.getAppIdFacebook(mContext));
		LinkedList l1 = new LinkedList();
		for (int i = 0; i < invitedFriends.size(); i++) {
			l1.add(invitedFriends.get(i));
		}
		obj.put("users_invite", l1);

		try {
			obj.writeJSONString(out);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		Log.e("InviteUtils", obj.toJSONString());
		return obj.toJSONString();
	}

}
