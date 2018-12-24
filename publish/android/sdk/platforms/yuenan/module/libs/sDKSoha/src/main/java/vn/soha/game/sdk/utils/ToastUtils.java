package vn.soha.game.sdk.utils;

import vn.sgame.sdk.R;
import android.content.Context;
import android.widget.Toast;

/**
 * @since January 27, 2015
 * @author hoangcaomobile
 *
 */
public class ToastUtils {

	public static void vToastShort(Context mContext, String text) {
		Toast.makeText(mContext, text, Toast.LENGTH_SHORT).show();
	}

	public static void vToastLong(Context mContext, String text) {
		Toast.makeText(mContext, text, Toast.LENGTH_LONG).show();
	}

	public static void vToastErrorTryAgain(Context mContext) {
		vToastShort(mContext, mContext.getString(R.string.toastErrorTryAgain));
	}

	public static void vToastErrorNetwork(Context mContext) {
		vToastShort(mContext, mContext.getString(R.string.toastErrorNetwork));
	}

	public static void vToastErrorServer(Context mContext) {
		vToastShort(mContext, mContext.getString(R.string.toastErrorServer));
	}
}
