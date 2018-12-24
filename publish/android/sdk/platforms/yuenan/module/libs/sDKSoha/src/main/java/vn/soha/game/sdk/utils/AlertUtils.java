package vn.soha.game.sdk.utils;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;

/**
 * @since March 17, 2015
 * @author hoangcaomobile
 *
 */
public class AlertUtils {

	public static void vOpenAlert(Activity activity, String message, DialogInterface.OnClickListener onClickListenerOk) {
		AlertDialog.Builder builder = new AlertDialog.Builder(activity);
		builder.setMessage(message);
		builder.setPositiveButton(android.R.string.ok, onClickListenerOk);
		builder.setNegativeButton(android.R.string.cancel, null);

		builder.create().show();
	}	

	public static void vOpenAlert1Button(Activity activity, String message, DialogInterface.OnClickListener onClickListenerOk) {
		AlertDialog.Builder builder = new AlertDialog.Builder(activity);
		builder.setMessage(message);
		builder.setPositiveButton(android.R.string.ok, onClickListenerOk);

		builder.create().show();
	}	

}
