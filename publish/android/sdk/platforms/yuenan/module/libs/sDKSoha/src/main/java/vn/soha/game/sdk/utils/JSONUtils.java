package vn.soha.game.sdk.utils;

import java.io.IOException;
import java.io.StringWriter;
import java.text.SimpleDateFormat;
import java.util.Date;

import org.json.simple.JSONObject;

import android.annotation.SuppressLint;
import android.content.Context;
import android.util.Base64;

/**
 * @since April 16, 2015
 * @author hoangcaomobile
 *
 */
public class JSONUtils {

	@SuppressWarnings({ "unchecked" })
	public static String getReceiptIAP(Context mContext, String purchaseData, String signature) {
		StringWriter out = new StringWriter();

		JSONObject obj = new JSONObject();
		obj.put("order_data", purchaseData);
		obj.put("signature", signature);

		try {
			obj.writeJSONString(out);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		Logger.e("jsonData = " + obj.toJSONString());
		String dataEncodedBase64 = Base64.encodeToString(obj.toString().getBytes(), Base64.DEFAULT).replaceAll("\n", "");
		Logger.e("dataEncodedBase64 = " + dataEncodedBase64);
		return dataEncodedBase64;
	}

	@SuppressLint("SimpleDateFormat")
	@SuppressWarnings("unchecked")
	public static String getLogJsonDataOne(Context context, String url, String params,
			String type, String step, String status, String response) {
		StringWriter out = new StringWriter();

		JSONObject obj = new JSONObject();
		obj.put("url", url);
		obj.put("params", params);
		obj.put("log_time", new SimpleDateFormat("HH:mm:ss").format(new Date()));
		obj.put("network_type", NetworkUtils.getNetworkClass(context));
		obj.put("type", type);
		obj.put("step", step);
		obj.put("status", status);
		obj.put("response", response);
		obj.put("info", "android");
		try {
			obj.writeJSONString(out);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return obj.toString();
	}

}
