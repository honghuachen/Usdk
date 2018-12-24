package vn.soha.game.sdk.utils;

import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.List;

import org.apache.commons.io.IOUtils;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.mime.MultipartEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.json.JSONArray;
import org.json.JSONObject;

public class ServiceHelper {
	public static final int TIMEOUT_CONNECTION = 15000;
	public static final int TIMEOUT_SOCKET = 15000;

	public static void getWithoutResponse(String apiUrl) {
		String responseString = "";
		try {
			apiUrl = apiUrl.trim();

			HttpParams params = new BasicHttpParams();
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_CONNECTION);
			HttpConnectionParams.setSoTimeout(params, TIMEOUT_SOCKET);

			HttpClient client = new DefaultHttpClient(params);
			HttpGet get = new HttpGet(apiUrl);
			HttpResponse response = client.execute(get);
			responseString = IOUtils.toString(response.getEntity().getContent());

			if (!apiUrl.contains("services/api/apiMobile.php?act=notify")) {
				Logger.e("apiurl=" + apiUrl);
				Logger.e("responseString=" + responseString);				
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	}	

	public static String get(String apiUrl) {
		String responseString = "";
		try {
			apiUrl = apiUrl.trim();

			HttpParams params = new BasicHttpParams();
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_CONNECTION);
			HttpConnectionParams.setSoTimeout(params, TIMEOUT_SOCKET);

			HttpClient client = new DefaultHttpClient(params);
			HttpGet get = new HttpGet(apiUrl);
			HttpResponse response = client.execute(get);
			responseString = IOUtils.toString(response.getEntity().getContent());

			Logger.e("apiUrl=" + apiUrl);
			Logger.e("response=" + responseString);

			return responseString;
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			
			return e.getMessage();
		}
	}	

	public static Object get(String apiUrl, boolean isJsonObject) {
		String responseString = "";
		try {
			apiUrl = apiUrl.trim();

			HttpParams params = new BasicHttpParams();
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_CONNECTION);
			HttpConnectionParams.setSoTimeout(params, TIMEOUT_SOCKET);

			HttpClient client = new DefaultHttpClient(params);
			HttpGet get = new HttpGet(apiUrl);
			HttpResponse response = client.execute(get);
			responseString = IOUtils.toString(response.getEntity().getContent());

			if (!apiUrl.contains("services/api/apiMobile.php?act=notify")) {
				Logger.e("apiurl=" + apiUrl);
				Logger.e("responseString=" + responseString);				
			}

			if (isJsonObject)
				return new JSONObject(responseString);
			else
				return new JSONArray(responseString);
		} catch (Exception e) {
			// TODO: handle exception

			e.printStackTrace();
			return null;
		}
	}

	public static void get(String apiUrl, boolean isJsonObject, ServiceHelperListener listener) {
		String responseString = "";
		try {
			apiUrl = apiUrl.trim();

			HttpParams params = new BasicHttpParams();
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_CONNECTION);
			HttpConnectionParams.setSoTimeout(params, TIMEOUT_SOCKET);

			HttpClient client = new DefaultHttpClient(params);
			HttpGet get = new HttpGet(apiUrl);
			HttpResponse response = client.execute(get);
			responseString = IOUtils.toString(response.getEntity().getContent());

			if (isJsonObject) {
				listener.onSuccess(new JSONObject(responseString));
			} else {
				listener.onSuccess(new JSONArray(responseString));
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			listener.onFailed();
		} finally {
			//			if (!apiUrl.contains("notify")) {
			//				Logger.e("apiUrl=" + apiUrl);
			//				Logger.e("apiResponse=" + responseString);				
			//			}
		}
	}	


	public static Object post(String apiUrl, MultipartEntity entity, boolean isJsonObject) {
		try {
			HttpParams params = new BasicHttpParams();
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_CONNECTION);
			HttpConnectionParams.setConnectionTimeout(params, TIMEOUT_SOCKET);

			HttpClient client = new DefaultHttpClient(params);
			HttpPost post = new HttpPost(apiUrl);
			post.setEntity(entity);

			HttpResponse response = client.execute(post);
			String textResponse = IOUtils.toString(response.getEntity().getContent());

			if (isJsonObject) {
				return new JSONObject(textResponse);
			} else {
				return new JSONArray(textResponse);
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			return null;
		}
	}

	public static void post(String apiUrl, List<NameValuePair> nameValuePairs) {
		HttpClient client = new DefaultHttpClient();
		HttpPost post = new HttpPost(apiUrl);

		try {
			post.setEntity(new UrlEncodedFormEntity(nameValuePairs));
			HttpResponse response = client.execute(post);

			Logger.e("apiUrl POSTTTTTTTTTTTT=" + apiUrl);
			Logger.e("apiResponse=" + IOUtils.toString(response.getEntity().getContent()));
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	}

	public static String post(String apiUrl, String dataString) {
		try {
			URL url = new URL(apiUrl);
			HttpURLConnection connection = (HttpURLConnection) url.openConnection();
			connection.setDoOutput(true);
			connection.setRequestMethod("POST");
			connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");

			OutputStream outputStream = connection.getOutputStream();
			outputStream.write(dataString.getBytes());
			outputStream.close();

			String response = IOUtils.toString(connection.getInputStream());

			

			return response;
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			return null;
		}
	}

	public interface ServiceHelperListener {
		public void onSuccess(Object apiResponse);
		public void onFailed();
	}


}
