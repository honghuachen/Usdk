package vn.soha.game.sdk.utils;

import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.impl.client.DefaultHttpClient;

import com.nostra13.universalimageloader.core.download.ImageDownloader.Scheme;

public class CustomHttpClient extends DefaultHttpClient {

//	public CustomHttpClient() {
//		super();
//		SSLSocketFactory socketFactory = SSLSocketFactory.getSocketFactory();
//		socketFactory.setHostnameVerifier(new CustomHostnameVerifier());
//		Scheme scheme = (new Scheme("https", socketFactory, 443));
//		getConnectionManager().getSchemeRegistry().register(scheme);
//	}
}