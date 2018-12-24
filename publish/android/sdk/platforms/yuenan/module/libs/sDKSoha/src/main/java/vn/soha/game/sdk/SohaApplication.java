package vn.soha.game.sdk;

import org.json.JSONObject;

import vn.soha.game.sdk.utils.LogStat;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.NameSpace;
import vn.soha.game.sdk.utils.ServiceHelper;
import vn.soha.game.sdk.utils.Utils;
import android.app.Application;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.pm.PackageManager.NameNotFoundException;
import android.preference.PreferenceManager;
import android.support.multidex.MultiDex;

import com.google.android.gms.analytics.GoogleAnalytics;
import com.google.android.gms.analytics.HitBuilders;
import com.google.android.gms.analytics.StandardExceptionParser;
import com.google.android.gms.analytics.Tracker;
import com.mesglog.sdk.MesgLog;

/**
 * Do not modify
 * 
 * @author Sohagame SDK Team
 * 
 */
public class SohaApplication extends Application {

	int retryGetConfig = 0;
	private static SohaApplication mInstance;

	@Override
	public void onCreate() {
		// TODO Auto-generated method stub
		super.onCreate();
		mInstance = this;
		MultiDex.install(this);
		AnalyticsTrackers.initialize(this);
		AnalyticsTrackers.getInstance().get(AnalyticsTrackers.Target.APP);
		logInstallSOAP();
		logInstallTracking();
		getMysohaConfig();

	}

	public static synchronized SohaApplication getInstance() {
		return mInstance;
	}

	public synchronized Tracker getGoogleAnalyticsTracker() {
		AnalyticsTrackers analyticsTrackers = AnalyticsTrackers.getInstance();
		return analyticsTrackers.get(AnalyticsTrackers.Target.APP);
	}
	public synchronized Tracker getGATracker() {
		AnalyticsTrackers analyticsTrackers = AnalyticsTrackers.getInstance();
		return analyticsTrackers.get(AnalyticsTrackers.Target.GAME);
	}
	/***
	 * Tracking screen view
	 * 
	 * @param screenName
	 *            screen name to be displayed on GA dashboard
	 */
	public void trackScreenView(String screenName) {
		Tracker tSDK = getGoogleAnalyticsTracker();
		Tracker tGame = getGATracker();
		
		// Set screen name.
		tSDK.setScreenName(screenName);
		tGame.setScreenName(screenName);
		// Send a screen view.
		tSDK.send(new HitBuilders.ScreenViewBuilder().build());
		tGame.send(new HitBuilders.ScreenViewBuilder().build());

		GoogleAnalytics.getInstance(this).dispatchLocalHits();
	}

	/***
	 * Tracking exception
	 * 
	 * @param e
	 *            exception to be tracked
	 */
	public void trackException(Exception e) {
		if (e != null) {
			Tracker t = getGoogleAnalyticsTracker();

			t.send(new HitBuilders.ExceptionBuilder()
					.setDescription(
							new StandardExceptionParser(this, null)
									.getDescription(Thread.currentThread()
											.getName(), e)).setFatal(false)
					.build());
		}
	}

	/***
	 * Tracking event
	 * 
	 * @param category
	 *            event category
	 * @param action
	 *            action of the event
	 * @param label
	 *            label
	 */
	public void trackEvent(String category, String action, String label) {
		Tracker tSDK = getGoogleAnalyticsTracker();
		Tracker tGAME = getGATracker();
		// Build and send an Event.
		tSDK.send(new HitBuilders.EventBuilder().setCategory(category)
				.setAction(action).setLabel(label).build());
		tGAME.send(new HitBuilders.EventBuilder().setCategory(category)
				.setAction(action).setLabel(label).build());
	}

	public void logInstallSOAP() {
		SharedPreferences sharedPreferences = PreferenceManager
				.getDefaultSharedPreferences(getApplicationContext());
		if (sharedPreferences.getString(NameSpace.SHARED_PREF_LOG_OPEN_APP,
				null) == null) {
			Logger.e("SohaApplication; Log OPEN APP first time");
			LogStat.logDeviceInfo(getApplicationContext());
		} else {
			Logger.e("SohaApplication; Log OPEN APP already done");
		}
	}

	public void logInstallTracking() {
		// LOG INSTALL with LIB TRACKING
		String versionCode = "";
		String versionLibChecking = "";
		try {
			versionCode = getPackageManager().getPackageInfo(getPackageName(),
					0).versionCode
					+ "";
		} catch (NameNotFoundException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		try {
			versionLibChecking = MesgLog.getLibVersion();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		final String finalVersionCode = versionCode;
		Logger.e("SohaApplication: versionCode = " + versionCode);
		Logger.e("SohaApplication: libChecking = " + versionLibChecking);

		if (Utils.getString(getApplicationContext(),
				NameSpace.SHARED_PREF_LOG_INSTALL_APP) == null
				|| !Utils.getString(getApplicationContext(),
						NameSpace.SHARED_PREF_LOG_INSTALL_APP).equals(
						finalVersionCode)) {

			new Thread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					try {
						boolean logInstall = MesgLog.sendLogInstall(
								getApplicationContext(), "");
						Logger.e("SohaApplication; Log INSTALL APP; response="
								+ logInstall);
						if (logInstall) {
							Utils.saveString(getApplicationContext(),
									NameSpace.SHARED_PREF_LOG_INSTALL_APP,
									finalVersionCode);
						}
					} catch (Exception e) {
						// TODO: handle exception
						e.printStackTrace();
					}
				}
			}).start();
		}
	}

	public void getMysohaConfig() {
		// GET MYSOHA CONFIG
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				final String response = ServiceHelper
						.get(NameSpace.API_GET_CONFIG);

				if (response != null) {
					try {
						JSONObject responseJSON = new JSONObject(response);
						// get mysoha config
						String package_mysoha = responseJSON
								.getJSONObject("result")
								.getJSONObject("activity")
								.getString("package_mysoha");
						String class_login_mysoha = responseJSON
								.getJSONObject("result")
								.getJSONObject("activity")
								.getString("class_login_mysoha");
						String link_download = responseJSON.getJSONObject(
								"result").getString("link_download");

						Utils.saveString(getApplicationContext(),
								NameSpace.PACKAGE_MYSOHA, package_mysoha);
						Utils.saveString(getApplicationContext(),
								NameSpace.CLASS_LOGIN_MYSOHA,
								class_login_mysoha);
						Utils.saveString(getApplicationContext(),
								NameSpace.DOWNLOAD_PAGE, link_download);
					} catch (Exception e) {
						// TODO: handle exception
						e.printStackTrace();
					}
				} else {
					if (retryGetConfig < 3) {
						retryGetConfig++;
						getMysohaConfig();
					}
				}
			}
		}).start();
	}
	protected void attachBaseContext(Context base) {
		super.attachBaseContext(base);
		MultiDex.install(this);
	}
}
