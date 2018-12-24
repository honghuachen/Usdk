package vn.soha.game.sdk.utils;

import android.content.Context;
import android.media.MediaScannerConnection;
import android.net.Uri;

/**
 * @since July 11, 2014
 * @author hoangcaomobile
 *
 */
public class AndroidUtils {

	public static interface MediaScannerCallback {
		public void mediaScannerCompleted(String scanPath, Uri scanURI);
	}

	public static void vScanSavedMediaFile(final Context context, final String path, final MediaScannerCallback callback) {
		// silly array hack so closure can reference scannerConnection[0] before it's created 
		final MediaScannerConnection[] scannerConnection = new MediaScannerConnection[1];
		try {
			MediaScannerConnection.MediaScannerConnectionClient scannerClient = new MediaScannerConnection.MediaScannerConnectionClient() {
				public void onMediaScannerConnected() {
					scannerConnection[0].scanFile(path, null);
				}

				public void onScanCompleted(String scanPath, Uri scanURI) {
					scannerConnection[0].disconnect();
					if (callback!=null) {
						callback.mediaScannerCompleted(scanPath, scanURI);
					}
				}
			};
			scannerConnection[0] = new MediaScannerConnection(context, scannerClient);
			scannerConnection[0].connect();
		}
		catch(Exception ignored) {}
	}

	public static void vScanSavedMediaFile(final Context context, final String path) {
		vScanSavedMediaFile(context, path, null);
	}

	public static int getOSVersion() {
		return android.os.Build.VERSION.SDK_INT;
	}
}
