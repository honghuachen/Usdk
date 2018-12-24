package vn.soha.game.sdk;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.utils.LogUtils;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.Utils;

import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.media.RingtoneManager;
import android.os.Build;
import android.os.Bundle;
import android.provider.Settings;
import android.support.v4.app.NotificationCompat;

import com.google.android.gcm.GCMBaseIntentService;

public class GCMIntentService extends GCMBaseIntentService {

	@Override
	protected String[] getSenderIds(Context context) {
		// TODO Auto-generated method stub
		String[] ids = new String[1];
		ids[0] = "754061351551";
		return ids;
	}

	@Override
	protected void onError(Context context, String arg1) {
		// TODO Auto-generated method stub
		Logger.e("__gcm err; string=" + arg1);
	}

	@Override
	protected void onMessage(Context context, Intent intent) {
		// TODO Auto-generated method stub
		Logger.e("__gcm onMessage GCMBaseIntentService");

		String title = intent.getExtras().getString("title");
		String message = intent.getExtras().getString("message");
		Logger.e("title=" + title + "; message=" + message);

		if (title != null && message != null) {
			generateNotification(context, title, message);
		}
	}

	@Override
	protected void onRegistered(Context context, String regId) {
		// TODO Auto-generated method stub
		Logger.e("__gcm on registed GCMBaseIntentService;__gcm String=" + regId);
		LogUtils.logDeviceId(context, regId);
	}

	@Override
	protected void onUnregistered(Context context, String arg1) {
		// TODO Auto-generated method stub
		Logger.e("__gcm unregisted String=" + arg1);
	}

	public static void generateNotification(Context context, String title, String message) {
		Intent intent = context.getPackageManager().getLaunchIntentForPackage(context.getPackageName());
		Bundle bundle = new Bundle();
		bundle.putBoolean("fromGCM", true);
		intent.putExtra("fromGCM", bundle);
		intent.addCategory(Intent.CATEGORY_LAUNCHER);
		PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, intent, PendingIntent.FLAG_UPDATE_CURRENT);

		if (Build.VERSION.SDK_INT < 26) {
			NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
					.setSmallIcon(R.drawable.ic_sohagame)
					.setLargeIcon(BitmapFactory.decodeResource(context.getResources(), context.getApplicationInfo().icon))
					.setContentTitle(title)
					.setContentText(message)
					.setContentIntent(pendingIntent)
					.setAutoCancel(true)
					.setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION))
					.setLights(Color.GREEN, 3000, 3000)
					.setVibrate(new long[]{600, 800, 1000});

			((NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE)).notify(0, builder.build());
		} else {
			NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context, Utils.getAppId(context));
			mBuilder.setSmallIcon(R.drawable.ic_sohagame);
			mBuilder.setContentTitle(title)
					.setContentText(message)
					.setAutoCancel(false)
					.setSound(Settings.System.DEFAULT_NOTIFICATION_URI)
					.setContentIntent(pendingIntent);

			NotificationManager mNotificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

			if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.O) {
				int importance = NotificationManager.IMPORTANCE_HIGH;
				NotificationChannel notificationChannel = new NotificationChannel(Utils.getAppId(context), Intent.ACTION_LOCALE_CHANGED, importance);
				notificationChannel.enableLights(true);
				notificationChannel.setLightColor(Color.RED);
				notificationChannel.enableVibration(true);
				notificationChannel.setVibrationPattern(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400});
				assert mNotificationManager != null;
				mBuilder.setChannelId(Utils.getAppId(context));
				mNotificationManager.createNotificationChannel(notificationChannel);
			}
			assert mNotificationManager != null;
			mNotificationManager.notify(0 /* Request Code */, mBuilder.build());
		}
	}
}
