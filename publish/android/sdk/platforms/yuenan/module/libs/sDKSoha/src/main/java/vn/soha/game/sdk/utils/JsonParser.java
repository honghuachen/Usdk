package vn.soha.game.sdk.utils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.List;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.utils.URLEncodedUtils;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.protocol.HTTP;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Base64;
import android.util.Log;

public class JsonParser {

	static InputStream is = null;
	static JSONObject jObj = null;
	static String json = "";

	// constructor
	public JsonParser() {

	}

	
	/*
	public static JSONObject getJsonStringWithGet(String urlLink, List<NameValuePair> params) {
		jObj =null;
		json="";
		URL url = null;
		
		if(params!=null&&params.size()>0)
		{
			String paramEncode = URLEncodedUtils.format(params, HTTP.UTF_8);
			urlLink+="?"+paramEncode;
		}
		
		//urlLink+="1?"
		Log.e("url", "___get Link :"+urlLink);

		InputStream mInputStream = null;
		String result = null;
		
		HttpURLConnection connector = null;
		try {
			
			url = new URL(urlLink);
		
			connector = (HttpURLConnection) url.openConnection();
			connector.setRequestMethod("GET");
			
			connector.setRequestProperty("Content-Type",
					"application/json;charset=UTF-8");
			connector.setRequestProperty("Content-Type",
			"application/x-www-form-urlencoded;charset=UTF-8");
                  
			connector.setConnectTimeout(15000);
			
			
			mInputStream = connector.getInputStream();
			BufferedReader br = new BufferedReader(new InputStreamReader(mInputStream));
			StringBuffer sb = new StringBuffer();
			String line = "";
			while ((line = br.readLine()) != null) {
				sb.append(line);
			}
			result = sb.toString();
			Log.e("url", "___"+urlLink);


			jObj = new JSONObject(result);   
			
			String data = jObj.getString("signed_request");
			Log.e("resPonse_base", "___"+data);
			data = new String( Base64.decode(data.getBytes(), Base64.DEFAULT));
			jObj = new JSONObject(data);
			Log.e("resPonse", "___"+data);
		} catch (Exception ex) {
			ex.printStackTrace();
			// This is currently being printout
	
		} finally {
			if (mInputStream != null) {
				try {
					//dong inputstream
					mInputStream.close();
				} catch (IOException e) {
				}
				//dong connection
				connector.disconnect();
			}
		}
		return jObj;
	}*/

	public static JSONObject getJSONFromPostUrl(String url, List<NameValuePair> params) {
		Log.e("url", url);
		jObj =null;
		json="";
		// Making HTTP request
		try {
			// defaultHttpClient
			DefaultHttpClient httpClient = new DefaultHttpClient();
			HttpPost httpPost = new HttpPost(url);
			httpPost.setEntity(new UrlEncodedFormEntity(params, HTTP.UTF_8)); 
			// httpPost.setEntity(new UrlEncodedFormEntity(params));		

			HttpResponse httpResponse = httpClient.execute(httpPost);
			HttpEntity httpEntity = httpResponse.getEntity();
			is = httpEntity.getContent();

		

			BufferedReader reader = new BufferedReader(new InputStreamReader(
					is, "iso-8859-1"), 8);
			StringBuilder sb = new StringBuilder();
			String line = null;
			while ((line = reader.readLine()) != null) {
				sb.append(line);
			}
			is.close();
			json = sb.toString();
			
//			Log.e("DATA1", json);
	

		// try parse the string to a JSON object
		Log.e("Response", json);
	
			jObj = new JSONObject(json);         
			
			String data = jObj.getString("signed_request");
			Log.e("resPonse_base", "___"+data);
			data = new String( Base64.decode(data.getBytes(), Base64.DEFAULT));
			jObj = new JSONObject(data);
			//Log.e("resPonse", "___"+data);
		
		} catch (Exception e) {
			e.printStackTrace();
			
		}
		// return JSON String
		return jObj;
	}

}
