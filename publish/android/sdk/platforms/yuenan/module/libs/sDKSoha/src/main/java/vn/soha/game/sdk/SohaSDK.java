package vn.soha.game.sdk;

import java.io.File;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Random;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.eclipse.paho.client.mqttv3.IMqttActionListener;
import org.eclipse.paho.client.mqttv3.IMqttToken;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import shg.vn.track.MQTTListener;
import shg.vn.track.TrackController;
import vn.sgame.sdk.R;
import vn.sgame.sdk.view.DashboardButton;
import vn.sgame.sdk.view.NotifyConnect;
import vn.sgame.sdk.view.WarningButton;
import vn.soha.game.sdk.dialogs.DialogConnectAccount;
import vn.soha.game.sdk.dialogs.DialogConnectAccount.InteractConnectDialog;
import vn.soha.game.sdk.dialogs.DialogDetailDashBoard;
import vn.soha.game.sdk.dialogs.DialogDetailDashBoard.onFileAvatarChoosed;
import vn.soha.game.sdk.dialogs.DialogDetailDashBoard.onFileCompleteChosed;
import vn.soha.game.sdk.dialogs.DialogFail;
import vn.soha.game.sdk.dialogs.DialogLogin;
import vn.soha.game.sdk.dialogs.DialogNotifyUpdate;
import vn.soha.game.sdk.dialogs.DialogPayment;
import vn.soha.game.sdk.dialogs.DialogPayment.OnPaymentListener;
import vn.soha.game.sdk.dialogs.DialogPermisionExplain;
import vn.soha.game.sdk.dialogs.DialogPermissionAcceptDashboard;
import vn.soha.game.sdk.interact.OnSDKBackClickListener;
import vn.soha.game.sdk.model.DashBoardItem;
import vn.soha.game.sdk.model.MQTTAction;
import vn.soha.game.sdk.server.API;
import vn.soha.game.sdk.utils.AlertUtils;
import vn.soha.game.sdk.utils.DialogUtils;
import vn.soha.game.sdk.utils.ImageFilePath;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.LogUtils;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.MQTTUtils;
import vn.soha.game.sdk.utils.MyThread;
import vn.soha.game.sdk.utils.NameSpace;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.TextUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;
import android.Manifest;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.ActivityNotFoundException;
import android.content.BroadcastReceiver;
import android.content.ComponentName;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.ServiceConnection;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.os.Handler;
import android.os.IBinder;
import android.provider.MediaStore;
import android.provider.Settings;
import android.support.annotation.NonNull;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.Base64;
import android.util.Log;
import android.webkit.CookieManager;
import android.webkit.CookieSyncManager;
import android.webkit.ValueCallback;
import android.widget.Toast;

import com.android.vending.billing.IInAppBillingService;
import com.facebook.CallbackManager;
import com.facebook.FacebookSdk;
import com.facebook.appevents.AppEventsLogger;
import com.facebook.login.LoginManager;
import com.google.android.gcm.GCMRegistrar;
import com.google.android.gms.ads.identifier.AdvertisingIdClient;
import com.mesglog.sdk.MesgLog;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;

/**
 * @since January 26, 2015
 * @author Sohagame SDK Team
 * 
 */
public class SohaSDK {

	private static Activity mActivity;
	private static WarningButton mWarningButton;
	public static AppEventsLogger logger;
	static DialogLogin.OnLogoutListener logoutListener;
	static DialogLogin.OnLoginListener onLoginListener;
	static OnSDKBackClickListener onBackListener;
	public static onFileCompleteChosed onFileChosedCallback = null;
	public static onFileAvatarChoosed onAvatarChosed = null;
	public static final int REQUEST_DRAW_OVERLAY = 12;
	Boolean isOnDestroy =false;

	public SohaSDK(Activity activity,
			DialogLogin.OnLoginListener onLoginListener,
			DialogLogin.OnLogoutListener logoutListener) {
		mActivity = activity;
		isEnableLogin = true;
		this.onLoginListener = onLoginListener;
		this.logoutListener = logoutListener;

		refreshSharePreference(activity);

		Utils.saveSessionId(mActivity, "");
		String unixTime = System.currentTimeMillis() + "";
		// int randomNum = ThreadLocalRandom.current().nextInt(10000, 99998 +
		// 1);
		Random rn = new Random();
		int randomNum = rn.nextInt(99999 - 10000 + 1) + 10000;
		if (Utils.getSessionId(mActivity).equals("")) {
			Utils.saveSessionId(mActivity, unixTime + "_" + randomNum);
			Log.e("SESSION_ID", "==" + Utils.getSessionId(mActivity));
		}
		if (Utils.getMQTTClientId(mActivity).equals("")) {

			String clientId = Utils.getMQTTClientCode(mActivity) + "_"
					+ Utils.getAppId(mActivity) + "_"
					+ System.currentTimeMillis()
					+ MQTTUtils.getDeviceId(mActivity);
			Utils.saveMQTTClientId(mActivity, clientId);

			Utils.setAppInstalledMQTT(mActivity, false);

		}
		Utils.setAppOpened(mActivity, false);
		if (!Utils.isAppInstalledMQTT(mActivity)) {
			sendLog(MQTTUtils.ACTION_INSTALL, "");
			Utils.setAppInstalledMQTT(mActivity, true);
		}
		sendLog(MQTTUtils.ACTION_OPEN, "");
		vInitSDK();
	
		// showConnectView();

	}

	public boolean called = false;
	static Boolean isPause = false;
	Boolean isPauseMQTT = false;
	public static Boolean isPendingShowConnectAcc = true;
	public static NotifyConnect connectAccountView;

	static MyThread connectThread;
	static Handler h = new Handler();

	static List<BroadcastReceiver> listReceiver = new ArrayList<BroadcastReceiver>();

	public static void registerBroadCast(BroadcastReceiver broadcast,
			IntentFilter intentFilter) {
		try {
			mActivity.registerReceiver(broadcast, intentFilter);
			listReceiver.add(broadcast);
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

	}

	public static void unregistBroadcast(BroadcastReceiver broadcast) {
		try {
			mActivity.unregisterReceiver(broadcast);
			if (listReceiver != null && listReceiver.size() > 0) {
				listReceiver.remove(broadcast);
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

	}

	void unregistAllBroadcast() {
		try {
			for (int i = 0; i < listReceiver.size(); i++) {
				mActivity.unregisterReceiver(listReceiver.get(i));

			}
			listReceiver.clear();
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

	}

	public static void startConnectAccountThread() {
		// Utils.setIsShowConnectAccount(mActivity, true);
		try {

			// Toast.makeText(mActivity, "connect time"
			// +Utils.SHOW_CONNECT_TIME, 1000).show();
			if ((connectThread == null || !connectThread.isRuning())
					&& Utils.SHOW_CONNECT_TIME > 0) {
				// connectThread.setRuning(false);
				// connectThread.interrupt();
				connectThread = new MyThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub

						while (connectThread != null
								&& connectThread.isRuning()) {
							try {
								Log.e("connect_view",
										"connect_view call sleep "
												+ Utils.SHOW_CONNECT_TIME
												* 1000);
								Thread.sleep(Utils.SHOW_CONNECT_TIME * 1000);

								h.post(new Runnable() {

									@Override
									public void run() {
										// TODO Auto-generated method stub
										if (connectThread != null
												&& connectThread.isRuning()
												&& !Utils.getSoapAccessToken(
														mActivity).equals("")) {
											// Log.e("connect_view",
											// "connect_view "+connectThread.getMyId()+"  "+Utils.SHOW_CONNECT_TIME);
											// Toast.makeText(mActivity,
											// "show time: "+connectThread.getId(),
											// 1000).show();
											showConnectView();

										}

									}
								});

							} catch (Exception e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
					}
				});
				connectThread.start();
			}
		} catch (Exception ex) {
			// TODO: handle exception
			ex.printStackTrace();
		}

	}

	public static void showConnectView() {
		try {
			Boolean isShowConnect = Utils.isShowConnectAccount(mActivity);
			if (isShowConnect
					&& isPendingShowConnectAcc && Utils.SHOW_CONNECT_TIME > 0&&!DialogDetailDashBoard.isShowingProfile&&!DialogConnectAccount.isShowConnectPopup) {
				if (connectAccountView != null) {

					if (!connectAccountView.isShowing) {
						connectAccountView.onDestroy();
						connectAccountView = new NotifyConnect(mActivity,
								isPause);
					}

				} else {
					connectAccountView = new NotifyConnect(mActivity, isPause);
				}

			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			// Toast.makeText(mActivity, "show connect viewexception",
			// 1000).show();
		}

	}

	public static void destroyConnectView() {
		Utils.setIsShowConnectAccount(mActivity, false);

		if (connectAccountView != null) {
			connectAccountView.onDestroy();
			connectAccountView = null;
		}
	}
	int count = 0;
	// String demo
	// ="eyJyb2xlbmFtZSI6IlkyaGhjbUZqZEdWeUlERT0iLCJkZXZpY2VfaWRfdmNjIjoiMzUxODMxMDYzNDU1ODU0Iiwicm9sZWlkIjoiMSIsInNka3ZlciI6IjMuNS43Iiwicm9sZWxldmVsIjoiMTAiLCJhcmVhaWQiOiIxIiwiZGV2aWNlX2lkIjoiMzUxODMxMDYzNDU1ODU0IiwiYXBwX2lkIjoiMjEwMDVlN2M2NjgwYTVkMmU4ZWUyY2UxNTEyZTEzZDEiLCJhY2Nlc3NfdG9rZW4iOiJjMjloY0hSdmEyVnVNQzQwTlRrMk1EZ3dNQ0F4TkRrMU1qWTJORFE0S3pZNE1Ua3hPRGcwTVE9PSIsImd2ZXIiOiIxLjIuMCIsImNsaWVudG5hbWUiOiJzb2hhZ2FtZSJ9";
	private void vInitSDK() {
		initAdTracking(mActivity);
		if (Utils.getADVERT_ID(mActivity).equals("")) {
			new Thread(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					count =0;
					while (advertId == null) {
						
						try {
							Thread.sleep(200);
						} catch (InterruptedException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						count += 200;
						
						if(count==1200)
						{
							Log.e("init Tracking", "Sleeping____time out  "+advertId);
							Utils.saveADVERT_ID(mActivity, Utils.getDeviceIDVCC(mActivity));
							break;
						}
					}
					
					mActivity.runOnUiThread(new Runnable() {
						
						@Override
						public void run() {
							// TODO Auto-generated method stub
							  Log.e("init Tracking", "Sleeping____call other init  "+advertId);
							initApterAdIdOk();
							if(waitingForLogIn)
							{
								login();
								waitingForLogIn =false;
								try {
									DialogUtils.vDialogLoadingDismiss();
								} catch (Exception e) {
									// TODO: handle exception
									e.printStackTrace();
								}
								
							}
						}
					});

					
				}
			}).start();
		}
		else
		{
			initApterAdIdOk();
		}
		
	}

	void initApterAdIdOk()
	{
		vInitFacebook();
		vInitMysohaConfig();
		vInitImageLoader();
		vInitGCM();
		vInitOther();

		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils
						.getDefaultParamsPost(mActivity)));
				// Log.e("param appinfor ", "__appInfor : " +
				// Utils.getDefaultParamsPost(mActivity));
				Log.e("param appinfor ",
						"__appInfor : " + Utils.getDefaultParamsPost(mActivity));
				final JSONObject json = JsonParser.getJSONFromPostUrl(
						API.getAppInfo, params);
				mActivity.runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						DialogUtils.vDialogLoadingDismiss();
						Log.e("param appinfor ", "__appInfor" + json + "");
						// Toast.makeText(mActivity, json.toString(),
						// 3000).show();

						vSetupAppInfo(json);

					}
				});
			}
		}).start();
		
		//requestLuncherPermision(mActivity);
	}
	public static String advertId = null;

	void initAdTracking(final Activity activity) {

		 Log.e("init Tracking", "Sleeping____Init ad ");
        if(Utils.getADVERT_ID(mActivity).equals(""))
        {
    		new Thread(new Runnable() {
    			@Override
    			public void run() {
    				// TODO Auto-generated method stub
    				try {

    					SharedPreferences share = mActivity
    							.getPreferences(Activity.MODE_PRIVATE);

    					
    					if (!share.getBoolean("isCofigAdTracking", false)) {
    						
    						 Log.e("init Tracking", "Sleeping____call to get ad ");
    						AdvertisingIdClient.Info idInfo;

    						idInfo = AdvertisingIdClient
    								.getAdvertisingIdInfo(mActivity);
    						// final String advertId = idInfo.getId();
    						advertId = idInfo.getId();
    						
    						
    						  if(Utils.getADVERT_ID(mActivity).equals(""))
    					        {
    								if (advertId.equals("") || advertId == null) {
    	    							advertId = MQTTUtils.getDeviceId(mActivity);
    	    						}
    	    						Utils.saveADVERT_ID(mActivity, advertId);
    	    						Log.e("Google Ad Id", "Sleeping____ get ad finish" + advertId);
    	    						if (Utils.getADVERT_ID(mActivity).equals("")) {
    	    							Utils.saveADVERT_ID(mActivity,
    	    									MQTTUtils.getDeviceId(mActivity));
    	    						}
    					        }
    						  Log.e("init Tracking", "Sleeping____finish get ad  "+advertId);

    					} else {
    						Log.e("Tracking Ad", "Sleeping____ad already tracked success "+Utils.getADVERT_ID(mActivity));
    						
    						
    					}

    				} catch (Exception e) {
    					// TODO Auto-generated catch block
    					Log.e("init Tracking fail", "");
    					e.printStackTrace();
    				}

    			}
    		}).start();
        }
        else
        {
        	Log.e("Tracking Ad", "Sleeping____ad cache  "+Utils.getADVERT_ID(mActivity));
        }
		

	}

	public void vInitMysohaConfig() {
		if (Utils.getString(mActivity, NameSpace.PACKAGE_MYSOHA) == null) {
			Utils.saveString(mActivity, NameSpace.PACKAGE_MYSOHA,
					NameSpace.PACKAGE_MYSOHA);
		}

		if (Utils.getString(mActivity, NameSpace.CLASS_LOGIN_MYSOHA) == null) {
			Utils.saveString(mActivity, NameSpace.CLASS_LOGIN_MYSOHA,
					NameSpace.CLASS_LOGIN_MYSOHA);
		}

		if (Utils.getString(mActivity, NameSpace.DOWNLOAD_PAGE) == null) {
			Utils.saveString(mActivity, NameSpace.DOWNLOAD_PAGE,
					NameSpace.DOWNLOAD_PAGE);
		}
	}

	public static void showWarningButton(Activity activity) {
		Log.e("", "__w__show warning button");

		if (Utils.isEnableWarningAge(activity)) {
			if (mWarningButton != null) {
				mWarningButton.onDestroy();
			}
			mWarningButton = new WarningButton(activity);

		}
	}

	public static void hideWarningButton() {
		Log.e("", "__w hide warning button");
		if (mWarningButton != null) {
			mWarningButton.onDestroy();
			mWarningButton = null;
		}
	}

	// Facebook SDK
	public static CallbackManager callbackManager;

	public void vInitFacebook() {
		FacebookSdk.setApplicationId(Utils.getAppIdFacebook(mActivity));
		FacebookSdk.sdkInitialize(mActivity);
		AppEventsLogger.activateApp(mActivity);
		callbackManager = CallbackManager.Factory.create();
		logger = AppEventsLogger.newLogger(mActivity);
		fuckLog("open_app", "");
		SohaApplication.getInstance().trackEvent("sdk", "open_app", "");
		if (!Utils.isAppInstalled(mActivity)) {
			fuckLog("install", "");
			SohaApplication.getInstance().trackEvent("sdk", "install", "");
			Utils.setAppInstalled(mActivity, true);
		}
		Utils.getKeyhash(mActivity);
		Logger.e("inited Facebook SDK 4.5.1");
	}

	// --

	@SuppressWarnings("deprecation")
	public void vInitImageLoader() {
		ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(
				mActivity)
				.threadPriority(Thread.NORM_PRIORITY - 2)
				.memoryCacheSize(2 * 1024 * 1024)
				// 2.0
				// Mb
				.memoryCache(new WeakMemoryCache())
				.denyCacheImageMultipleSizesInMemory()
				.discCacheFileNameGenerator(new Md5FileNameGenerator())
				.tasksProcessingOrder(QueueProcessingType.FIFO).build();
		ImageLoader.getInstance().init(config);
	}

	public void vInitOther() {
		// init cookies manager
		CookieSyncManager.createInstance(mActivity);

		// init storage path
		if (Environment.getExternalStorageState().equals(
				Environment.MEDIA_MOUNTED)) {
			NameSpace.STORAGE_PATH = Environment.getExternalStorageDirectory()
					+ "/SDK";
		} else {
			NameSpace.STORAGE_PATH = mActivity.getCacheDir().getPath() + "/SDK";
		}

		File file = new File(NameSpace.STORAGE_PATH);
		if (!file.exists()) {
			file.mkdir();
		}

		isInAppBillingConnected = false;
	}

	/* ------------- Function called from game -------------- */
	public void onPause() {
		try {
			Logger.e("SGameSDK.onPause()");

			isPauseMQTT = true;
			isPause = true;
			if (Build.VERSION.SDK_INT >= 23) {
				if (Settings.canDrawOverlays(mActivity)) {
					if (mDoashboardButton != null) {
						mDoashboardButton.setPendingShow(true);
						mDoashboardButton.hide();
					}
					if (mWarningButton != null) {
						mWarningButton.setPendingShow(true);
						Log.e("", "__w hide warning button2");
                        mWarningButton.hide();
					}
				}
			} else {
				if (mDoashboardButton != null) {
					mDoashboardButton.setPendingShow(true);
                    mDoashboardButton.hide();
				}
				if (mWarningButton != null) {
					mWarningButton.setPendingShow(true);
					Log.e("", "__w hide warning button2");
                    mWarningButton.hide();
				}
			}

			if (connectAccountView != null) {

				connectAccountView.hide();
			}
			LogUtils.logGameState(mActivity, LogUtils.GAME_STATE_ENDGAME);
			sendLog("hide_app", "");
			SohaApplication.getInstance().trackEvent("sdk", "hide_app", "");
			fuckLog("hide_app", "");
			new Thread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					try {
						MesgLog.sendLogAction(
								mActivity.getApplicationContext(), "1", "");
					} catch (Exception e) {
						// TODO: handle exception
						e.printStackTrace();
					}
				}
			}).start();
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

		// EasyTracker.getInstance(mActivity).activityStop(mActivity);
	}

	public static boolean isShowProfile = false;

	public void onResume(Intent intent) {
		isOnDestroy =false;
		SohaApplication.getInstance().trackScreenView("ingame");
		if (isPauseMQTT) {
			// call mqtt here

			fuckLog("resume_app", "");
			SohaApplication.getInstance().trackEvent("sdk", "resume_app", "");
			sendLog("resume_app", "");
			isPauseMQTT = false;
		}
		isPause = false;

		if (intent.hasExtra("fromGCM")) {
			SohaSDK.fuckLog("open_notifi", "");
			SohaApplication.getInstance().trackEvent("sdk", "open_notifi", "");
		}
		Logger.e("SGameSDK.onResume()");

		if (Build.VERSION.SDK_INT >= 23) {
			if (Settings.canDrawOverlays(mActivity)) {
				if (mDoashboardButton != null
						&& mDoashboardButton.getPendingShow()) {
					mDoashboardButton.setPendingShow(false);
					mDoashboardButton.show();
					if (isPendingFromOpenFanpage) {
						showDashboard(mActivity);
					}

				}
			}

		} else {
			if (mDoashboardButton != null && mDoashboardButton.getPendingShow()) {
				mDoashboardButton.setPendingShow(false);
				mDoashboardButton.show();
				if (isPendingFromOpenFanpage) {

					showDashboard(mActivity);
				}

			}
		}

		if (connectAccountView != null) {
			connectAccountView.show();
		}

		if (Build.VERSION.SDK_INT >= 23) {
			if (Settings.canDrawOverlays(mActivity)) {
				if (Utils.isEnableWarningAge(mActivity)
						&& mWarningButton != null
						&& mWarningButton.getPendingShow()
						&& mWarningButton.dialogProfile == null) {
					mWarningButton.setPendingShow(false);
					mWarningButton.show();
				}

			}
		} else {
			if (Utils.isEnableWarningAge(mActivity) && mWarningButton != null
					&& mWarningButton.getPendingShow()
					&& mWarningButton.dialogProfile == null) {
				mWarningButton.setPendingShow(false);
				mWarningButton.show();
			}

		}

		LogUtils.logGameState(mActivity, LogUtils.GAME_STATE_STARTGAME);

		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				try {
					MesgLog.sendLogAction(mActivity.getApplicationContext(),
							"0", "");
				} catch (Exception e) {
					// TODO: handle exception
					e.printStackTrace();
				}
			}
		}).start();
		// EasyTracker.getInstance(mActivity).activityStart(mActivity);
	}

	private static final int REQUEST_CODE_CANCEL_IAP = 1001;

	public void onActivityResult(int requestCode, int resultCode, Intent data) {
		Logger.e("SGameSDK.onActivityResult()");
		// TODO Auto-generated method stub
		callbackManager.onActivityResult(requestCode, resultCode, data);
		vInitGoogleBillingOnActivityResult(requestCode, resultCode, data);


		if (requestCode == PICKFILE_REQUEST_CODE2) {
			Uri result = data == null || resultCode != Activity.RESULT_OK ? null
					: data.getData();
			Uri[] resultsArray = new Uri[1];
			resultsArray[0] = result;

			if (resultsArray[0] != null) {

				lastResultNew = resultsArray;
				filePathCallback2.onReceiveValue(resultsArray);
				filePathCallback2 = null;

			} else {
				if (filePathCallback2 != null) {
					if (lastResultNew != null) {
						filePathCallback2.onReceiveValue(lastResultNew);
					} else {
						filePathCallback2.onReceiveValue(new Uri[] {});
					}

					filePathCallback2 = null;
				}

			}
//vn.shg.mobi.mongvolam
		}
		if (requestCode == PICKFILE_REQUEST_CODE) {

			Uri result = data == null || resultCode != Activity.RESULT_OK ? null
					: data.getData();
			if (result != null) {

				lastResultOld = result;
				filePathCallback.onReceiveValue(result);
				filePathCallback = null;

			} else {

				if (lastResultOld != null) {
					filePathCallback.onReceiveValue(lastResultOld);
				} else {
					filePathCallback.onReceiveValue(null);
				}

				filePathCallback = null;
			}

		}
		if (requestCode == 9001 && data != null) {

			// File myFile = new File(data.getData().getPath());
			// String[] path =ImageFilePath.getPath(mActivity, data.getData());
			// String path[] =
			// String imagePath = ImageFilePath.getPath(mActivity,
			// data.getData());
			List<String> path = new ArrayList<String>();
			if (data.getClipData() != null) {
				int count = data.getClipData().getItemCount();
				int currentItem = 0;
				while (currentItem < count) {
					Uri imageUri = data.getClipData().getItemAt(currentItem)
							.getUri();
					String link = ImageFilePath.getPath(mActivity, imageUri);
					path.add(link);
					Log.e("______link image", link);
					currentItem = currentItem + 1;
				}
			} else if (data.getData() != null) {
				String imagePath = ImageFilePath.getPath(mActivity,
						data.getData());
				path.add(imagePath);
				Log.e("______link image", imagePath);
			}
			if (onFileChosedCallback != null) {
				onFileChosedCallback.onFileChosed(path);
			}

		}
		if (requestCode == 9000 && data != null) {
			File myFile = new File(data.getData().getPath());
			String path = ImageFilePath.getPath(mActivity, data.getData());
			onAvatarChosed.onAvatarChosed(path);
		}
		if (requestCode == REQUEST_DRAW_OVERLAY) {

			if (Build.VERSION.SDK_INT >= 23) {
				if (!Settings.canDrawOverlays(mActivity)) {
					DialogPermissionAcceptDashboard dialog = new DialogPermissionAcceptDashboard(
							mActivity, new DialogPermissionAcceptDashboard.clickSetting() {

						@Override
						public void click() {
							// TODO Auto-generated method stub
							if (Build.VERSION.SDK_INT != 27) {
								Intent intent = new Intent(
										Settings.ACTION_MANAGE_OVERLAY_PERMISSION,
										Uri.parse("package:"
												+ mActivity
												.getPackageName()));
								mActivity.startActivityForResult(
										intent, REQUEST_DRAW_OVERLAY);
							} else {
								Intent myAppSettings = new Intent(
										Settings.ACTION_APPLICATION_DETAILS_SETTINGS,
										Uri.parse("package:"
												+ mActivity
												.getPackageName()));
								mActivity.startActivityForResult(
										myAppSettings,
										REQUEST_DRAW_OVERLAY);
							}
						}
					});
				}
			}

		}
	}

	private String getRealPathFromURI(Uri contentURI) {
		String result;
		String[] filePath = { MediaStore.Images.Media.DATA };
		Cursor cursor = mActivity.getContentResolver().query(contentURI,
				filePath, null, null, null);
		if (cursor == null) { // Source is Dropbox or other similar local file
								// path
			result = contentURI.getPath();
		} else {
			cursor.moveToFirst();
			int idx = cursor
					.getColumnIndex(MediaStore.Images.ImageColumns.DATA);
			result = cursor.getString(idx);
			cursor.close();
		}
		return result;
	}

	int countToKillApp = 0;

	public void onDestroy() {
		try {
			isShowDashBoardDetail =false;
			isPauseMQTT =false;
			countToKillApp =1000;
		

			h.postDelayed(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					countToKillApp = 0;
					if (mqttController != null) {
						try {
							mqttController.disconnect();
							mqttController=null;
						} catch (Exception e) {
							// TODO: handle exception
						}
						
					}
				}

			}, 1000);
			
			
			sendLog("kill_app", "");
			
/*			mqttController.send("kill_app", Utils.getEnName(mActivity),
					Utils.getAppId(mActivity), mActivity.getPackageName(),
					Utils.getGameVersion(mActivity),
					Utils.getSdkVersion(mActivity), Utils.getRoleId(mActivity),
					new String(Base64.decode(Utils.getRoleName(mActivity),

					Base64.DEFAULT)), Utils.getRoleLevel(mActivity),
					Utils.getAreaId(mActivity), Utils.getUserId(mActivity),

					Utils.getPUserId(mActivity), "",
					Utils.getDeviceTokenSoap(mActivity),
					Utils.getSessionId(mActivity),
					Utils.getMQTTClientCode(mActivity), Utils

					.getADVERT_ID(mActivity),
					System.currentTimeMillis() / 1000L, null);*/
			unregistAllBroadcast();
			if (mDoashboardButton != null) {
				mDoashboardButton.onDestroy();
				mDoashboardButton = null;
			}

			if (connectThread != null && connectThread.isRuning()) {
				connectThread.setRuning(false);
				connectThread = null;
			}
			fuckLog(MQTTUtils.ACTION_KILL_APP, "");
			SohaApplication.getInstance().trackEvent("sdk",
					MQTTUtils.ACTION_KILL_APP, "");

		/*	while (true) {
				if (countToKillApp == 0) {
					break;
				}
				countToKillApp--;

			}*/

			

			DialogLogin.mDialog = null;
			Logger.e("SGameSDK.onDestroy()");
		} catch (Exception e) {
			e.printStackTrace();
		}

	}
	
	static Boolean waitingForLogIn =false; 

	/* ------------- All main functions here --------------- */

	public void login() {


		//DialogProfile dialogProfile = new DialogProfile(mActivity,null);
		
		
		if (Utils.getADVERT_ID(mActivity).equals("")) {
			try {
				DialogUtils.vDialogLoadingShowProcessing(mActivity, false);
			} catch (Exception e) {
				// TODO: handle exception
			}
			
			waitingForLogIn =true;
			return;
		}else
		{
			waitingForLogIn =false;
		}
	
		if (NetworkUtils.isInternetConnected(mActivity)) {

			if (Utils.getADVERT_ID(mActivity).equals("")) {
				new Thread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						int count = 0;
						while (advertId == null) {
							// Log.e("Pending time", "pending___"+count);
							try {
								Thread.sleep(200);
							} catch (InterruptedException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
							count += 200;
						}

						// Log.e("Pending time", "pending___"+count + "__" +
						// advertId);
					}
				}).start();
			}


			if (isEnableLogin) {
				if (DialogLogin.mDialog != null
						&& DialogLogin.mDialog.isShowing()) {
					DialogLogin.mDialog.dismiss();
				}
				new DialogLogin(mActivity, onLoginListener);
				fuckLog("login", "");
				SohaApplication.getInstance().trackEvent("sdk", "login", "");
				sendLog(MQTTUtils.ACTION_LOGIN_OPEN, "");

				// showWarningButton(mActivity);
				//
				// new DialogLoginForConnectNew(mActivity);
			}
			vSendLogConfirm();
		} else {
			AlertDialog.Builder adb = new AlertDialog.Builder(mActivity);
			adb.setMessage(mActivity.getString(R.string.errorConnect));
			
			adb.setNegativeButton(mActivity.getString(R.string.retry),
					new OnClickListener() {

						public void onClick(DialogInterface dialog, int which) {
							// TODO Auto-generated method stub
							login();
						}
					});
			
			
			
			adb.setPositiveButton(mActivity.getString(R.string.cancel),
					new OnClickListener() {

						@Override
						public void onClick(DialogInterface dialog, int which) {
							// TODO Auto-generated method stub
							mActivity.finish();
						}
					});
			adb.setCancelable(false);
			adb.show();
		}
	}

	public static void fuckLog(String action, String ext) {
		logger.logEvent(action, getParameter(ext));
	}

	private boolean isEnableLogin = true;

	private void vSetupAppInfo(JSONObject jsonObject) {
		if (Build.VERSION.SDK_INT >= 23) {
			if (!Settings.canDrawOverlays(mActivity)) {
				DialogPermissionAcceptDashboard dialog = new DialogPermissionAcceptDashboard(
						mActivity, new DialogPermissionAcceptDashboard.clickSetting() {

					@Override
					public void click() {
						// TODO Auto-generated
						// method stub
						if (Build.VERSION.SDK_INT != 27) {
							Intent intent = new Intent(
									Settings.ACTION_MANAGE_OVERLAY_PERMISSION,
									Uri.parse("package:"
											+ mActivity
											.getPackageName()));
							mActivity.startActivityForResult(intent,
									REQUEST_DRAW_OVERLAY);
						} else {
							Intent myAppSettings = new Intent(
									Settings.ACTION_APPLICATION_DETAILS_SETTINGS,
									Uri.parse("package:"
											+ mActivity
											.getPackageName()));
							mActivity
									.startActivityForResult(
											myAppSettings,
											REQUEST_DRAW_OVERLAY);
						}
					}
				});
				// dialog.show();
			}
		}
		if (jsonObject != null) {
			Log.e("da", jsonObject.toString());
			try {

				if (jsonObject.getString("status").equals("success")) {
					Utils.vSaveDeviceTokenSoap(mActivity, jsonObject
							.getJSONObject("data").getString("device_token"));
					Utils.saveEnName(mActivity, jsonObject
							.getJSONObject("data").getString("e_name"));

					JSONObject jsonData = jsonObject.getJSONObject("data");

					// app info co the bi goi nhieu lan nen phai check mqtt
					// connect thanh cong roi thi ko connect lai nua(du meo biet
					// biet tai sao goi nhieu lan)
					if (jsonData.getInt("active_mqtt") == 1
							&& !Utils.isAppOpened(mActivity) && !called) {
						// Toast.makeText(mActivity, "connect mqtt",
						// 1000).show();

						String port = jsonData.getString("port_mqtt");
						String domain = jsonData.getString("domain_mqtt");
						MQTTAuthAddress = "tcp://" + domain + ":" + port;
						h.postDelayed(new Runnable() {
							
							@Override
							public void run() {
								// TODO Auto-generated method stub
								vInitMQTT();
							}
						}, 3000);
					
					}

					// init dashboard version
				
					Utils.vSaveNewDashboardVersion(mActivity, jsonObject
							.getJSONObject("data").getInt("dashboard_ver"));

					int isShowDasBoard = jsonObject.getJSONObject("data")
							.getInt("hidden_dashboard");
					Utils.vSaveIsShowDashboard(mActivity, isShowDasBoard == 0);

					Utils.vSaveWarningAge(
							mActivity,
							jsonData.getString("show_warning_ingame").equals(
									"1"));
					
					int width = jsonData.getInt("size_image_age_width");
					int height = jsonData.getInt("size_image_age_height");
					int limitConnect =jsonData.getInt("limit_reconnect_mqtt"); 
					Utils.saveMqttLimit(mActivity, limitConnect);
					Utils.saveWarningWidth(mActivity, width);
					Utils.saveWarningHeight(mActivity, height);
					if (Utils.isEnableWarningAge(mActivity)) {
						String imageAge = jsonData.getString("image_age");
						String warningTimeMessage = jsonData
								.getString("warning_time_message");
						Utils.vSaveWarningInfo(mActivity, imageAge,
								warningTimeMessage, true);

					}
					
					String iconDB = jsonData.getString("icon_db");
					Utils.vSaveIconDB(mActivity, iconDB);
					String urlWarning = jsonData.getString("url_warning");
					Utils.vSaveWarningURL(mActivity, urlWarning);
					// --

					Utils.SHOW_CONNECT_TIME = jsonData
							.getInt("warning_time_connect");
					Log.e("TIme connect", "__time " + Utils.SHOW_CONNECT_TIME);

					Utils.vSaveWarningAge(
							mActivity,
							jsonData.getString("show_warning_ingame").equals(
									"1"));

					// notify update
					JSONObject joNotifi = jsonData.getJSONObject("notifi");
					if (joNotifi.getString("status").equals("1")
							&& joNotifi.getString("force").equals("0")) {
						// need to update (not required)
						new DialogNotifyUpdate(mActivity,
								joNotifi.getString("link"), true);
					} else if (joNotifi.getString("status").equals("1")) {
						// need to update (required)
						new DialogNotifyUpdate(mActivity,
								joNotifi.getString("link"), false);
					}
					// --

				} else if (jsonObject.getString("status").equals("fail")) {
					new DialogFail(mActivity, jsonObject.getString("message"));
				}
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

	}

	public static void logoutForBlock() {

		if (NetworkUtils.isInternetConnected(mActivity)) {

			SohaSDK.fuckLog(MQTTUtils.ACTION_LOGOUT, "");
			SohaApplication.getInstance().trackEvent("sdk",
					MQTTUtils.ACTION_LOGOUT, "");
			Log.e("mqtt", "__mqtt____send log logout");
			sendLog(MQTTUtils.ACTION_LOGOUT, "");
			Utils.saveSoapAccessToken(mActivity, "");
			Utils.saveUserAvatar(mActivity, "");
			Utils.saveUserId(mActivity, "");
			SharedPreferences mSharedPreferences = mActivity
					.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
							Context.MODE_PRIVATE);
			mSharedPreferences.edit()
					.putString(NameSpace.SHARED_PREF_AREAID, "").commit();
			mSharedPreferences.edit()
					.putString(NameSpace.SHARED_PREF_ROLEID, "").commit();
			mSharedPreferences.edit()
					.putString(NameSpace.SHARED_PREF_ROLENAME, "").commit();
			mSharedPreferences.edit()
					.putString(NameSpace.SHARED_PREF_ROLELEVEL, "").commit();
			Utils.saveUserId(mActivity, "");
			Utils.savePUserId(mActivity, "");
		
			Utils.setIsShowConnectAccount(mActivity, false);
			Utils.saveSoapAccessToken(mActivity, "");

			CookieManager cookieManager = CookieManager.getInstance();
			cookieManager.removeAllCookie();

			// logout facebook
			LoginManager.getInstance().logOut();
			// --
			Log.e("mqtt", "__mqtt____calll logout cusess");
			logoutListener.onLogoutSuccessful();

			hideWarningButton();
			hideDashboard();

			LogUtils.logGameState(mActivity, LogUtils.GAME_STATE_LOGOUT);

		}
	}

	int pendingTime = 800;
	Boolean isPendingLogout = false;

	/**
	 * Clear user token saved in shared preferences
	 */
	public void logout() {

		if (!isPendingLogout) {
			isPendingLogout = true;
			if (NetworkUtils.isInternetConnected(mActivity)) {

				if (!Utils.getUserType(mActivity).equalsIgnoreCase("play_now")) {
					SohaSDK.fuckLog(MQTTUtils.ACTION_LOGOUT, "");
					SohaApplication.getInstance().trackEvent("sdk",
							MQTTUtils.ACTION_LOGOUT, "");
					Log.e("mqtt", "__mqtt____send log logout");
					sendLog(MQTTUtils.ACTION_LOGOUT, "");
					Utils.saveSoapAccessToken(mActivity, "");
					Utils.saveUserAvatar(mActivity, "");
					Utils.saveUserId(mActivity, "");
					SharedPreferences mSharedPreferences = mActivity
							.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
									Context.MODE_PRIVATE);
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_AREAID, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLEID, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLENAME, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLELEVEL, "")
							.commit();
					Utils.saveUserId(mActivity, "");
					Utils.savePUserId(mActivity, "");
					
					Utils.setIsShowConnectAccount(mActivity, false);
					Utils.saveSoapAccessToken(mActivity, "");

					CookieManager cookieManager = CookieManager.getInstance();
					cookieManager.removeAllCookie();

					// logout facebook
					LoginManager.getInstance().logOut();
					// --
					Log.e("mqtt", "__mqtt____calll logout cusess");
					logoutListener.onLogoutSuccessful();

					hideWarningButton();
					hideDashboard();

					LogUtils.logGameState(mActivity, LogUtils.GAME_STATE_LOGOUT);
				} else {
					new DialogConnectAccount(mActivity,
							new InteractConnectDialog() {

								@Override
								public void onCancel() {
									// TODO Auto-generated method stub
									Utils.setIsShowConnectAccount(mActivity,
											false);
									Utils.saveSoapAccessToken(mActivity, "");
									CookieManager cookieManager = CookieManager
											.getInstance();
									cookieManager.removeAllCookie();
									// logout facebook
									LoginManager.getInstance().logOut();
									Utils.saveUserAvatar(mActivity, "");
									SohaSDK.fuckLog(MQTTUtils.ACTION_LOGOUT, "");
									SohaApplication.getInstance().trackEvent(
											"sdk", MQTTUtils.ACTION_LOGOUT, "");
									Log.e("mqtt", "__mqtt____send log logout");
									sendLog(MQTTUtils.ACTION_LOGOUT, "");
									LogUtils.logGameState(mActivity,
											LogUtils.GAME_STATE_LOGOUT);
									Utils.saveSoapAccessToken(mActivity, "");
									Utils.saveUserAvatar(mActivity, "");
									Utils.saveUserId(mActivity, "");
									SharedPreferences mSharedPreferences = mActivity
											.getSharedPreferences(
													NameSpace.SHARED_PREF_NAME,
													Context.MODE_PRIVATE);
									mSharedPreferences
											.edit()
											.putString(
													NameSpace.SHARED_PREF_AREAID,
													"").commit();
									mSharedPreferences
											.edit()
											.putString(
													NameSpace.SHARED_PREF_ROLEID,
													"").commit();
									mSharedPreferences
											.edit()
											.putString(
													NameSpace.SHARED_PREF_ROLENAME,
													"").commit();
									mSharedPreferences
											.edit()
											.putString(
													NameSpace.SHARED_PREF_ROLELEVEL,
													"").commit();
									Utils.saveUserId(mActivity, "");
									Utils.savePUserId(mActivity, "");
									logoutListener.onLogoutSuccessful();

								

									hideWarningButton();
									hideDashboard();
									Utils.setIsShowConnectAccount(mActivity,
											false);
									destroyConnectView();

								}
							});

				}

			} else {
				ToastUtils.vToastErrorNetwork(mActivity);
			}
			h.postDelayed(new Runnable() {

				public void run() {
					// TODO Auto-generated method stub
					isPendingLogout = false;
				}
			}, pendingTime);
		}

	}
	
	public static void logoutFromServer() {

	
		

					SohaSDK.fuckLog(MQTTUtils.ACTION_LOGOUT, "");
					SohaApplication.getInstance().trackEvent("sdk",
							MQTTUtils.ACTION_LOGOUT, "");
					Log.e("mqtt", "__mqtt____send log logout");
					sendLog(MQTTUtils.ACTION_LOGOUT, "");
					Utils.saveSoapAccessToken(mActivity, "");
					Utils.saveUserAvatar(mActivity, "");
					Utils.saveUserId(mActivity, "");
					SharedPreferences mSharedPreferences = mActivity
							.getSharedPreferences(NameSpace.SHARED_PREF_NAME,
									Context.MODE_PRIVATE);
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_AREAID, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLEID, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLENAME, "")
							.commit();
					mSharedPreferences.edit()
							.putString(NameSpace.SHARED_PREF_ROLELEVEL, "")
							.commit();
					Utils.saveUserId(mActivity, "");
					Utils.savePUserId(mActivity, "");
				
					Utils.setIsShowConnectAccount(mActivity, false);
					Utils.saveSoapAccessToken(mActivity, "");

					CookieManager cookieManager = CookieManager.getInstance();
					cookieManager.removeAllCookie();

					// logout facebook
					LoginManager.getInstance().logOut();
					// --
					Log.e("mqtt", "__mqtt____calll logout cusess");
					logoutListener.onLogoutSuccessful();

					hideWarningButton();
					hideDashboard();

					LogUtils.logGameState(mActivity, LogUtils.GAME_STATE_LOGOUT);
		
			
	
	}


	public void pay(final OnPaymentListener onPaymentListener) {

		Log.e("payment ", "Pay_ begin call pay");
		if (TextUtils.isStringNull(Utils.getRoleId(mActivity))) {
			Log.e("payment ", "Pay_ not set role id");
			AlertUtils
					.vOpenAlert1Button(
							mActivity,
							"You need call SohaSDK.setUserConfig(params) before call the payment method, please see the document to get more details",
							null);
		} else {
			Log.e("payment ", "Pay_ create payment dialog");
			new DialogPayment(mActivity, onPaymentListener);
		}
	}

	public void payMonthly(final OnPaymentListener onPaymentListener,
			String price) {
		if (TextUtils.isStringNull(Utils.getRoleId(mActivity))) {
			AlertUtils
					.vOpenAlert1Button(
							mActivity,
							"You need call SohaSDK.setUserConfig(params) before call the payment method, please see the document to get more details",
							null);
		} else {
			new DialogPayment(mActivity, onPaymentListener);
		}
	}

	public void payWithId(final OnPaymentListener onPaymentListener, String id) {
		if (TextUtils.isStringNull(Utils.getRoleId(mActivity))) {
			AlertUtils
					.vOpenAlert1Button(
							mActivity,
							"You need call SohaSDK.setUserConfig(params) before call the payment method, please see the document to get more details",
							null);
		} else {
			new DialogPayment(mActivity, onPaymentListener);
		}
	}

	
	
	void refreshSharePreference(Context mContext )
	{
		if(!NameSpace.SHARED_PREF_NAME.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_NAME +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}

		if(!NameSpace.SHARED_PREF_AREAID.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_AREAID +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}

		if(!NameSpace.SHARED_PREF_ROLEID.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_ROLEID +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!NameSpace.SHARED_PREF_ROLENAME.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_ROLENAME +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!NameSpace.SHARED_PREF_ROLELEVEL.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_ROLELEVEL +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!Utils.ADVERT_ID.contains(Utils.getAppId(mContext)))
		{
			Utils.ADVERT_ID +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!Utils.TAG.contains(Utils.getAppId(mContext)))
		{
			Utils.TAG +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!NameSpace.SHARED_PREF_IS_INSTALLED_APP.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_IS_INSTALLED_APP +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		
		if(!NameSpace.SHARED_PREF_USER_ID.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_USER_ID +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!Utils.puId.contains(Utils.getAppId(mContext)))
		{
			Utils.puId +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}
		
		if(!NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN.contains(Utils.getAppId(mContext)))
		{
			NameSpace.SHARED_PREF_SOAP_ACCESS_TOKEN +=Utils.getAppId(mContext)+Utils.getSdkVersion(mContext);
		}


	}

	/**
	 * Set user config after login successful
	 * 
	 * @param areaId
	 * @param roleId
	 */
	public void setUserConfig(String areaId, String roleId, String roleName,
			String roleLevel) {
		
	
		SharedPreferences mSharedPreferences = mActivity.getSharedPreferences(
				NameSpace.SHARED_PREF_NAME, Context.MODE_PRIVATE);
		mSharedPreferences.edit()
				.putString(NameSpace.SHARED_PREF_AREAID, areaId).commit();
		mSharedPreferences.edit()
				.putString(NameSpace.SHARED_PREF_ROLEID, roleId).commit();
		mSharedPreferences.edit()
				.putString(NameSpace.SHARED_PREF_ROLENAME, roleName).commit();
		mSharedPreferences.edit()
				.putString(NameSpace.SHARED_PREF_ROLELEVEL, roleLevel).commit();

		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils
						.postSetUserConfig(mActivity)));
				Log.e("param set user info", Utils.postSetUserConfig(mActivity));
				JsonParser.getJSONFromPostUrl(API.logPlayer, params);
			}
		}).start();
		sendLog(MQTTUtils.ACTION_SET_ROLE, "");
		fuckLog(MQTTUtils.ACTION_SET_ROLE, "");
		SohaApplication.getInstance().trackEvent("sdk",
				MQTTUtils.ACTION_SET_ROLE, "");
	}

	/**
	 * get SDK version
	 */
	public String getSDKVersion() {
		return mActivity.getString(R.string.sdkVersion);
	}

	/**
	 * send log comfirm
	 */
	private void vSendLogConfirm() {
		final int finalAppVersionCode = Utils.getAppversionCode(mActivity);
		if (finalAppVersionCode != Utils.getRecentAppVersionCode(mActivity)) {
			new Thread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					try {
						boolean logInstall = MesgLog.sendLogConfirm(mActivity,
								Utils.getPUserId(mActivity));
						Logger.e("Send log confirm = " + logInstall);
						if (logInstall) {
							Utils.vSaveRecentAppVersionCode(mActivity);
						}
					} catch (Exception e) {
						// TODO: handle exception
						e.printStackTrace();
					}
				}
			}).start();
		}
	}

	// handler Google In App Purchase
	public static boolean isInAppBillingConnected = false;
	public static IInAppBillingService mService;
	private static ServiceConnection mServiceConn = new ServiceConnection() {
		@Override
		public void onServiceDisconnected(ComponentName name) {
			mService = null;
		}

		@Override
		public void onServiceConnected(ComponentName name, IBinder service) {
			mService = IInAppBillingService.Stub.asInterface(service);
			isInAppBillingConnected = true;
			Logger.e("In App Billing connected");
		}
	};

	public void vInitGCM() {
		Log.e("SohaSDK", "gcm_registerin..1");
		try {
			GCMRegistrar.checkDevice(mActivity);
			GCMRegistrar.checkManifest(mActivity);
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

		String regId = GCMRegistrar.getRegistrationId(mActivity);
		if (regId == null || regId.equals("")) {
			GCMRegistrar.register(mActivity,
					mActivity.getString(R.string.gcm_project_id));
			Log.e("SohaSDK", "gcm_ call gcm regist2");
		} else {

			if (!Utils.getAccessTokenForPush(mActivity).equals(
					Utils.getSoapAccessToken(mActivity))) {
				Utils.saveAccessTokenForPush(mActivity,
						Utils.getSoapAccessToken(mActivity));
				LogUtils.logDeviceId(mActivity, regId);
			
				Logger.e("SohaSDK; gcm_ registered device3; regId="
						+ regId);
			}
			else
			{
				if(!Utils.getConfigGCM(mActivity))
				{
					GCMRegistrar.register(mActivity,mActivity.getString(R.string.gcm_project_id));
				}
			}
			
			
			Logger.e("SohaSDK; gcm_  old token "+Utils.getAccessTokenForPush(mActivity) +" curent soap "+Utils.getSoapAccessToken(mActivity));
		}
	}

	private void vInitGoogleBillingOnActivityResult(int requestCode,
			int resultCode, Intent data) {
		if (requestCode == 1001 && data != null) {
			final String mPurchaseData = data
					.getStringExtra("INAPP_PURCHASE_DATA");
			final String mDataSignature = data
					.getStringExtra("INAPP_DATA_SIGNATURE");
			Logger.e("purchaseData = " + mPurchaseData);
			Logger.e("dataSignature = " + mDataSignature);
			if (resultCode == Activity.RESULT_OK) {
				try {
					JSONObject jo = new JSONObject(mPurchaseData);
					String sku = jo.getString("productId");
					String purchaseToken = jo.getString("purchaseToken");
					int response = mService.consumePurchase(3,
							mActivity.getPackageName(), purchaseToken);
					Logger.e(response + "You have bought the " + sku
							+ ". Excellent choice, adventurer!");
					String developerPayload = jo.getString("developerPayload");
					Logger.e("developerPayload = " + developerPayload);
					vVerifyGooglePay(developerPayload, mPurchaseData,
							mDataSignature);
				} catch (Exception e) {
					ToastUtils.vToastErrorTryAgain(mActivity);
					e.printStackTrace();
				}
			}
			else
			{

					JSONObject obj = new JSONObject();
					try {
						obj.put("status", "fail");
						obj.put("message", "user cancel");
					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
					SohaApplication.getInstance().trackEvent(
							"sdk",
							"MQTTUtils.ACTION_IAP_END",
							Base64.encodeToString(obj.toString().getBytes(),
									Base64.DEFAULT));
					SohaSDK.fuckLog(MQTTUtils.ACTION_IAP_END, Base64.encodeToString(obj
							.toString().getBytes(), Base64.DEFAULT));
					sendLog(MQTTUtils.ACTION_IAP_END, Base64.encodeToString(obj
							.toString().getBytes(), Base64.DEFAULT));

			}
		}
	}

	private void vVerifyGooglePay(final String developerPayload,
			final String purchaseData, final String dataSignature) {
		DialogUtils.vDialogLoadingShowProcessing(mActivity, true);
		new Thread(new Runnable() {
			@Override
			public void run() {
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils
						.postPlayStore(mActivity, developerPayload,
								purchaseData, dataSignature)));
				Log.e("paramVerifyStore", Utils.postPlayStore(mActivity,
						developerPayload, purchaseData, dataSignature));
				final JSONObject json = JsonParser.getJSONFromPostUrl(
						API.paymentIAPVerify, params);

				mActivity.runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						DialogUtils.vDialogLoadingDismiss();
						Log.e("final JSON", json + "");
						if (json != null) {
							try {
								JSONObject obj = new JSONObject(purchaseData);
								String itemId = obj.getString("productId");
								if (json.getString("status").equals("success")) {
									String orderId = json.getString("order_id");
									DialogPayment.mOnPaymentListener
											.onSuccessful(orderId);
									obj = new JSONObject();
									obj.put("trans_id", orderId);
									obj.put("item_id", itemId);
									obj.put("status", "success");
									obj.put("receipt", "");
									obj.put("message", "");
									sendLog(MQTTUtils.ACTION_IAP_END,
											Base64.encodeToString(obj
													.toString().getBytes(),
													Base64.DEFAULT));
									SohaApplication.getInstance().trackEvent(
											"sdk",
											MQTTUtils.ACTION_IAP_END,
											Base64.encodeToString(obj
													.toString().getBytes(),
													Base64.DEFAULT));
									SohaSDK.fuckLog(MQTTUtils.ACTION_IAP_END,
											Base64.encodeToString(obj
													.toString().getBytes(),
													Base64.DEFAULT));

									DialogPayment.mDialog.dismiss();

									Logger.e("final orderId = " + orderId);
								} else {
									AlertUtils.vOpenAlert1Button(
											mActivity,
											json.getString("message"),
											new DialogInterface.OnClickListener() {

												@Override
												public void onClick(
														DialogInterface dialog,
														int which) {
												}
											});
								}
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								ToastUtils.vToastErrorTryAgain(mActivity);
								e.printStackTrace();
							}
						} else {
							/*
							 * PaymentFailedDB db = new
							 * PaymentFailedDB(mActivity);
							 * db.vInsert(developerPayload, purchaseData,
							 * dataSignature); db.close();
							 */
							ToastUtils.vToastErrorServer(mActivity);
						}
					}
				});
			}
		}).start();

	}

	public static void vInitGoogleBillingStart(Activity mActivity) {
		Intent serviceIntent = new Intent(
				"com.android.vending.billing.InAppBillingService.BIND");
		serviceIntent.setPackage("com.android.vending");
		mActivity.bindService(serviceIntent, mServiceConn,
				Context.BIND_AUTO_CREATE);
	}

	public static void vInitGoogleBillingStop(Activity mActivity) {
		if (mService != null) {
			mActivity.unbindService(mServiceConn);
		}
	}

	// --
	public static Bundle getParameter(String ext) {
		Bundle parameters = new Bundle();
		parameters.putString("ai", Utils.getAppId(mActivity));
		parameters.putString("bdi", mActivity.getPackageName());
		parameters.putString("clientid", Utils.getMQTTClientId(mActivity));
		parameters.putString("db", MQTTUtils.getDB());
		parameters.putString("di", Utils.getADVERT_ID(mActivity));
		Log.e("Google Advertising Id", "= " + Utils.getADVERT_ID(mActivity));

		parameters.putString("dm", MQTTUtils.getDM());
		parameters.putString("dn", MQTTUtils.getDeviceName());
		parameters.putString("ext", ext);
		parameters.putString("gv", Utils.getGameVersion(mActivity));
		parameters.putString("ipl", MQTTUtils.getIPLan(true));
		parameters.putString("lang", MQTTUtils.getDeviceLang());
		parameters.putString("lv", "N/A");
		parameters.putString("mf", MQTTUtils.getMF());
		parameters.putString("nt", MQTTUtils.getNetworkType(mActivity));
		parameters.putString("os", "Android");
		parameters.putString("osv", MQTTUtils.getOSVersion());
		parameters.putString("rd", MQTTUtils.isEmulator());
		parameters.putString("sdkv", Utils.getSdkVersion(mActivity));
		return parameters;
	}

	// ducnm file choser start
	public static Uri[] lastResultNew;
	public static Uri lastResultOld;
	public static ValueCallback<Uri> filePathCallback;
	public static ValueCallback<Uri[]> filePathCallback2;
	public static int PICKFILE_REQUEST_CODE = 1011;
	public static int PICKFILE_REQUEST_CODE2 = 1012;

	static int MY_PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE = 201;
	static int MY_PERMISSIONS_REQUEST_READ_PHONE_STATE = 203;
	static int PERMISSION_REQUEST_CODE_READ_EXTERNAL = 204;
	static int PERMISSION_REQUEST_CODE_READ_EXTERNAL1 = 205;
	static Intent chooserIntent;

	public static void requestPermision(Intent intent) {
		chooserIntent = intent;
		if (android.os.Build.VERSION.SDK_INT >= 23) {
			// only for gingerbread and newer versions
			if (ContextCompat.checkSelfPermission(mActivity,
					Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
				try {
					mActivity.startActivityForResult(chooserIntent,
							SohaSDK.PICKFILE_REQUEST_CODE2);
					chooserIntent = null;
				} catch (ActivityNotFoundException e) {
					SohaSDK.filePathCallback2 = null;
					Toast.makeText(mActivity.getApplicationContext(),
							"Cannot Open File Chooser", Toast.LENGTH_LONG)
							.show();

				}

			} else {

				ActivityCompat
						.requestPermissions(
								mActivity,
								new String[] { Manifest.permission.WRITE_EXTERNAL_STORAGE },
								MY_PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE);

			}
		} else {
			mActivity.startActivityForResult(chooserIntent,
					SohaSDK.PICKFILE_REQUEST_CODE2);
			chooserIntent = null;
		}
	}

/*	public void requestLuncherPermision(Activity activity) {
		// chooserIntent = intent;
		if (android.os.Build.VERSION.SDK_INT >= 23) {
			// only for gingerbread and newer versions
			if (ContextCompat.checkSelfPermission(activity,
					Manifest.permission.READ_PHONE_STATE) != PackageManager.PERMISSION_GRANTED) {
				// Should we show an explanation?

				ActivityCompat.requestPermissions(activity,
						new String[] { Manifest.permission.READ_PHONE_STATE },
						MY_PERMISSIONS_REQUEST_READ_PHONE_STATE);

			}
		}
	}
*/
	public void onRequestPermissionsResult(int requesCode,
			@NonNull String[] arg1, @NonNull int[] grantResult) {
		

		if (requesCode == MY_PERMISSIONS_REQUEST_READ_PHONE_STATE) {
			if (grantResult.length > 0
					&& grantResult[0] == PackageManager.PERMISSION_GRANTED) {
				
			} else {

				// show explain

				DialogPermisionExplain dialog = new DialogPermisionExplain(
						mActivity);

			}

		}
		
		
		if (requesCode == MY_PERMISSIONS_REQUEST_WRITE_EXTERNAL_STORAGE) {
			if (grantResult.length > 0
					&& grantResult[0] == PackageManager.PERMISSION_GRANTED) {

				try {
					mActivity.startActivityForResult(chooserIntent,
							PICKFILE_REQUEST_CODE2);
					chooserIntent = null;
				} catch (ActivityNotFoundException e) {
					filePathCallback2.onReceiveValue(new Uri[] {});
					filePathCallback2 = null;
					Toast.makeText(mActivity.getApplicationContext(),
							"Cannot Open File Chooser", Toast.LENGTH_LONG)
							.show();

				}
			} else {
				filePathCallback2.onReceiveValue(new Uri[] {});
				filePathCallback2 = null;
			}

		}

		if (requesCode == PERMISSION_REQUEST_CODE_READ_EXTERNAL) {
			if (grantResult.length > 0
					&& grantResult[0] == PackageManager.PERMISSION_GRANTED) {
				Intent intent = new Intent();
				intent.setType("image/*");
				intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
				intent.setAction(Intent.ACTION_GET_CONTENT);
				mActivity.startActivityForResult(
						Intent.createChooser(intent, "Select Picture"),
						9001);
			} else {

				// show explain

				DialogPermisionExplain dialog = new DialogPermisionExplain(
						mActivity);

			}

		}
		if (requesCode == PERMISSION_REQUEST_CODE_READ_EXTERNAL1) {
			if (grantResult.length > 0
					&& grantResult[0] == PackageManager.PERMISSION_GRANTED) {
				Intent intent = new Intent();
				intent.setType("image/*");
				intent.setAction(Intent.ACTION_GET_CONTENT);
				mActivity
						.startActivityForResult(Intent.createChooser(
								intent, "Select Avatar"), 9000);
				
			} else {

				// show explain

				DialogPermisionExplain dialog = new DialogPermisionExplain(
						mActivity);

			}
		}

	}

	// ducnm file choser end

	// ducnm dashboard start

	private static DashboardButton mDoashboardButton;
	public static boolean isShowDashBoardDetail = false;
	public static JSONObject cacheDashboardData = null;

	static boolean isPendingFromOpenFanpage = false;

	public static void showPendingDashBoard() {
		isPendingFromOpenFanpage = true;
	}

	public static void showDashboard(final Activity activity) {
		if (Utils.getIsShowDashBoard(activity)) {
			isHidedDashBoard = false;
			isPendingFromOpenFanpage = false;
			if (Build.VERSION.SDK_INT >= 23) {
				if (Settings.canDrawOverlays(mActivity)) {
					if (!mDoashboardButton.isUserCloseDasBoard
							&& !isShowDashBoardDetail) {

						if (mDoashboardButton != null) {

							mDoashboardButton.onDestroy();
						}
						final int newDBVer = Utils
								.getNewDashBoardVersion(activity);
						int curDbVer = Utils.getCurrDashBoardVersion(activity);
						String saveDashBoadData = Utils
								.getDashBoardData(activity);
						Log.e("__Save dashboard data", "__" + saveDashBoadData);
						/*
						 * if (curDbVer == newDBVer && saveDashBoadData != null
						 * && !saveDashBoadData.equals("")) {
						 *
						 * try { JSONObject jsondata = new
						 * JSONObject(saveDashBoadData);
						 * initDashBoardFromData(jsondata, activity); } catch
						 * (Exception e) { // TODO Auto-generated catch block
						 * e.printStackTrace(); } } else {
						 *
						 * }
						 */

						// new version dashboad moi connect api de updat config
						new Thread(new Runnable() {

							@Override
							public void run() {
								// TODO Auto-generated method stub
								List<NameValuePair> params = new ArrayList<NameValuePair>();
								params.add(new BasicNameValuePair(
										"signed_request",
										Utils.getDefaultParamsPost(mActivity)));
								Log.e("param",
										Utils.getDefaultParamsPost(mActivity));
								final JSONObject jsonData = JsonParser
										.getJSONFromPostUrl(
												API.getDashboardConfig, params);

								try {
									if (jsonData != null) {
										String status = jsonData
												.getString("status");
										Log.e("sdk dashborad",
												jsonData.toString());
										if (status.equalsIgnoreCase("success")) {
											Utils.vSaveDashboardData(activity,
													jsonData.toString());
											Utils.vSaveCurrDashboardVersion(
													activity, newDBVer);
											initDashBoardFromData(jsonData,
													activity);
											cacheDashboardData = jsonData;
										} else {
											if (cacheDashboardData != null) {
												initDashBoardFromData(
														cacheDashboardData,
														activity);

											}
										}
									} else {
										if (cacheDashboardData != null) {
											initDashBoardFromData(
													cacheDashboardData,
													activity);

										}
									}

								} catch (Exception e) {
									// TODO Auto-generated catch block
									e.printStackTrace();

								}

							}
						}).start();
					}

				}
			} else {
				if (!mDoashboardButton.isUserCloseDasBoard
						&& !isShowDashBoardDetail) {

					if (mDoashboardButton != null) {

						mDoashboardButton.onDestroy();
					}
					final int newDBVer = Utils.getNewDashBoardVersion(activity);
					int curDbVer = Utils.getCurrDashBoardVersion(activity);
					String saveDashBoadData = Utils.getDashBoardData(activity);
					Log.e("__Save dashboard data", "__" + saveDashBoadData);
					/*
					 * if (curDbVer == newDBVer && saveDashBoadData != null &&
					 * !saveDashBoadData.equals("")) {
					 *
					 * try { JSONObject jsondata = new
					 * JSONObject(saveDashBoadData);
					 * initDashBoardFromData(jsondata, activity); } catch
					 * (Exception e) { // TODO Auto-generated catch block
					 * e.printStackTrace(); } } else {
					 *
					 * }
					 */

					// new version dashboad moi connect api de updat config
					new Thread(new Runnable() {

						@Override
						public void run() {
							// TODO Auto-generated method stub
							List<NameValuePair> params = new ArrayList<NameValuePair>();
							params.add(new BasicNameValuePair("signed_request",
									Utils.getDefaultParamsPost(mActivity)));
							Log.e("param",
									Utils.getDefaultParamsPost(mActivity));
							final JSONObject jsonData = JsonParser
									.getJSONFromPostUrl(API.getDashboardConfig,
											params);

							try {
								if (jsonData != null) {
									String status = jsonData
											.getString("status");
									Log.e("sdk dashborad", jsonData.toString());
									if (status.equalsIgnoreCase("success")) {
										Utils.vSaveDashboardData(activity,
												jsonData.toString());
										Utils.vSaveCurrDashboardVersion(
												activity, newDBVer);
										initDashBoardFromData(jsonData,
												activity);
										cacheDashboardData = jsonData;
									} else {
										if (cacheDashboardData != null) {
											initDashBoardFromData(
													cacheDashboardData,
													activity);

										}
									}
								} else {
									if (cacheDashboardData != null) {
										initDashBoardFromData(
												cacheDashboardData, activity);

									}
								}

							} catch (Exception e) {
								// TODO Auto-generated catch block
								e.printStackTrace();

							}

						}
					}).start();
				}

			}

		}

	}

	public static void updateNotify(final Activity activity) {

		// new version dashboad moi connect api de updat config
		new Thread(new Runnable() {

			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils
						.getDefaultParamsPost(mActivity)));
				Log.e("param db :", Utils.getDefaultParamsPost(mActivity));
				final JSONObject jsonData = JsonParser.getJSONFromPostUrl(
						API.getDashboardConfig, params);

				try {
					if (jsonData != null) {
						String status = jsonData.getString("status");
						Log.e("sdk dashborad", jsonData.toString());
						if (status.equalsIgnoreCase("success")) {
							final int newDBVer = Utils
									.getNewDashBoardVersion(activity);
							Utils.vSaveDashboardData(activity,
									jsonData.toString());
							Utils.vSaveCurrDashboardVersion(activity, newDBVer);
							initDashBoardFromData(jsonData, activity);
							cacheDashboardData = jsonData;
						}
					}

				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

			}
		}).start();
	}

	static void initDashBoardFromData(JSONObject jsonData,
									  final Activity activity) {
		final List<DashBoardItem> listItem = new ArrayList<DashBoardItem>();
		try {
			int total = 0;
			JSONArray data = jsonData.getJSONArray("data");
			for (int i = 0; i < data.length(); i++) {
				JSONObject item = data.getJSONObject(i);
				DashBoardItem temp = new DashBoardItem();
				temp.setTab(item.getInt("tab"));
				temp.setTitle(item.getString("title"));
				temp.setType(item.getString("type"));
				temp.setIcon(item.getString("icon"));
				temp.setUrl(item.getString("url"));
				total += item.getInt("notify");
				temp.setNotify(item.getInt("notify"));
				if (temp.getTab() == 1) {
					JSONArray tablist = item.getJSONArray("data");
					for (int j = 0; j < tablist.length(); j++) {
						JSONObject subItem = tablist.getJSONObject(j);
						DashBoardItem subTemp = new DashBoardItem();
						subTemp.setTitle(subItem.getString("title"));
						subTemp.setType(subItem.getString("type"));
						subTemp.setIcon(subItem.getString("icon"));
						subTemp.setUrl(subItem.getString("url"));
						temp.getListSubTab().add(subTemp);
					}
				}
				try {
					temp.setActive(item.getInt("active"));
					temp.setMessageActive(item.getString("mess_active"));
					temp.setAutoOpen(item.getInt("auto_open"));

					temp.setPageId(item.getString("id_page"));
				} catch (Exception e) {
					// TODO: handle exception
				}
				listItem.add(temp);
			}
			final int notify = total;
			activity.runOnUiThread(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub

					Boolean isAutoOpen = false;
					if (DashboardButton.checkToShowAuto) {
						for (int i = 0; i < listItem.size(); i++) {
							if (listItem.get(i).getAutoOpen() == 1) {
								DialogDetailDashBoard dialog = new DialogDetailDashBoard(
										mActivity, listItem.get(i).getUrl());
								SohaSDK.hideDashboard();
								isAutoOpen = true;
								break;
							}
						}
						DashboardButton.checkToShowAuto = false;

					}
					if (!isAutoOpen) {
						if (Build.VERSION.SDK_INT >= 23) {
							if (Settings.canDrawOverlays(mActivity)) {
								mDoashboardButton = new DashboardButton(
										activity, listItem, notify);
							}

						} else {
							mDoashboardButton = new DashboardButton(activity,
									listItem, notify);
						}
					}

					/*
					 * mDoashboardButton = new DashboardButton(activity,
					 * listItem, notify);
					 */

				}
			});
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	/*
	 * public static void showDashboard(Activity activity, int positionX, int
	 * positionY) { if (mDoashboardButton != null) {
	 * mDoashboardButton.onDestroy(); }
	 * 
	 * mDoashboardButton = new DashboardButton(activity, positionX, positionY);
	 * }
	 */
	public static Boolean isHidedDashBoard = false;

	public static void hideDashboard() {
		if (Build.VERSION.SDK_INT >= 23) {
			if (Settings.canDrawOverlays(mActivity)) {
				isHidedDashBoard = true;
				if (mDoashboardButton != null) {

					mDoashboardButton.onDestroy();
					mDoashboardButton = null;
				}
			}
		} else {
			isHidedDashBoard = true;
			if (mDoashboardButton != null) {

				mDoashboardButton.onDestroy();
				mDoashboardButton = null;
			}
		}
	}

	public static void setCurrentDashBoard(DashboardButton dashboard) {
		if (Build.VERSION.SDK_INT >= 23) {
			if (Settings.canDrawOverlays(mActivity)) {
				mDoashboardButton = dashboard;
			}
		} else {
			mDoashboardButton = dashboard;
		}
	}

	// ducnm dashboard end

	/*
	 * static void runQueue() { if (Utils.isAppOpened(mActivity) &&
	 * NetworkUtils.isInternetConnected(mActivity) && !isRunningQueue) {
	 * isRunningQueue = true; new Thread(new Runnable() {
	 * 
	 * @Override public void run() { // TODO Auto-generated method stub while
	 * (listAction.size() > 0) {
	 * 
	 * if (Utils.isAppOpened(mActivity) &&
	 * NetworkUtils.isInternetConnected(mActivity)) { // try {
	 * 
	 * MQTTAction action = listAction.get(0); String roleName = new
	 * String(Base64.decode( action.getRoleName(), Base64.DEFAULT)); String
	 * deviceToken = Utils .getDeviceTokenSoap(mActivity); String ssi =
	 * Utils.getSessionId(mActivity); String adverID =
	 * Utils.getADVERT_ID(mActivity); String mqttCode = Utils
	 * .getMQTTClientCode(mActivity);
	 * 
	 * mqttController.send(action.getAction(), Utils.getEnName(mActivity),
	 * Utils.getAppId(mActivity), mActivity.getPackageName(),
	 * Utils.getGameVersion(mActivity), Utils.getSdkVersion(mActivity),
	 * action.getRoleId(), roleName, action.getRoleLevel(), action.getAreaId(),
	 * action.getUserId(), action.getvId(), action.getExtra(), deviceToken, ssi,
	 * mqttCode, adverID, action.getTime(),null); listAction.remove(0); // }
	 * catch (Exception e) { // // TODO: handle exception //
	 * e.printStackTrace(); // }
	 * 
	 * } else { break; } } isRunningQueue = false; }
	 * 
	 * }).start();
	 * 
	 * } }
	 */
	public static void setDomain(String domain) {
		NameSpace.DOMAIN = domain;
		API.refreshDomain();
		NameSpace.refreshDomain();
	}

	// MQTT HERE
	private static TrackController mqttController;
	static private String MQTTAuthAddress = "";// "tcp://track.shg.vn:1883";
	private static String mqttClientAuthen = "";

	public static List<MQTTAction> listAction = new ArrayList<MQTTAction>();
	public static Boolean isRunningQueue = false;
	int countOfMqttConnect =0;
	long lastTimeConnect =0;

	private MQTTListener listener = new MQTTListener() {

		@Override
		public void onDisconnected() {
			// TODO Auto-generated method stub
			Utils.setAppOpened(mActivity, false);
			// mqttController.disconnect();
			// Toast.makeText(mActivity, "disconnect", 1000).show();
		}

		@Override
		public void onConnectSuccess() {
			// TODO Auto-generated method stub
			
			Utils.setConnectedMQTTWithAuthen(mActivity, true);
			Date currentTime = Calendar.getInstance().getTime();
			if(lastTimeConnect==0)
			{
				//lan dau connect thanh cong
				lastTimeConnect = currentTime.getTime();
			}
			else
			{
				//neu chua qua 60s , tang bien dem so lan connect trong 60s
				if(currentTime.getTime()<lastTimeConnect+60000)
				{
					countOfMqttConnect++;
				}
				else
				{
					//neu qua 60 s reset lai so lan connect trong phut va updat lai thoi gian
					countOfMqttConnect=0;
					lastTimeConnect = currentTime.getTime();
				}
			}
			//neu qua 100 lan connect trong 1 phut, disconnect
			if(countOfMqttConnect>=Utils.getMqttLimit(mActivity))
			{
				mqttController.disconnect();
				mqttController =null;
			}else
			{
				Utils.setAppOpened(mActivity, true);
				runQueue(false);
			}
			// Toast.makeText(mActivity, "connect", 1000).show();
	

		}

		@Override
		public void onConnectFailure() {
			// TODO Auto-generated method stub

		}

	};

	static IMqttActionListener deliveriedListener = new IMqttActionListener() {

		@Override
		public void onSuccess(IMqttToken arg0) {
			if (listAction.size() > 0) {
				Log.e("end quue", "___que call remove "
						+ listAction.get(0).getAction());
				listAction.remove(0);

			}

			if (listAction.size() == 0) {
				Log.e("end quue", "___que call empty");
				isRunningQueue = false;
			} else {
				runQueue(true);
			}

		}

		@Override
		public void onFailure(IMqttToken arg0, Throwable arg1) {
			isRunningQueue = false;
			// isSentFail =true;

		}
	};

	static int countOfInitMqtt = 0;

	public void vInitMQTT() {
		// if (listAction.size() > 0)
		try {
			mqttController = new TrackController(mActivity, "shg",
					"WE8Yax5ndKApNmJAQpqQAsB",
					Utils.getMQTTClientId(mActivity), MQTTAuthAddress,
					listener, true);
			called = true;

			if (countOfInitMqtt < 3) {
				h.postDelayed(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						if (!Utils.isAppOpened(mActivity)) {
							// Toast.makeText(mActivity, "call init again",
							// 1000).show();
							countOfInitMqtt++;
							vInitMQTT();
						}
					}
				}, 180000);
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

	}

	public static void sendLog(String action, String extra) {
		// if(!Utils.isAppOpened(mActivity)){
		// vInitMQTT();
		// }
		Log.e("mqtt", "__mqtt____action: " + action);
		MQTTAction actionModel = new MQTTAction(action, extra,
				System.currentTimeMillis() / 1000L);
		actionModel.setAreaId(Utils.getAreaId(mActivity));
		actionModel.setRoleName(Utils.getRoleName(mActivity));
		actionModel.setRoleLevel(Utils.getRoleLevel(mActivity));
		actionModel.setRoleId(Utils.getRoleId(mActivity));
		actionModel.setUserId(Utils.getUserId(mActivity));
		actionModel.setvId(Utils.getPUserId(mActivity));
		listAction.add(actionModel);
		runQueue(false);
	}

	static void runQueue(Boolean isCallFromOrigin) {
		Log.e("end quue", "___que call que____" + isCallFromOrigin);
		// isSentFail = false;
		if (Utils.isAppOpened(mActivity)
				&& NetworkUtils.isInternetConnected(mActivity)
				&& (!isRunningQueue || isCallFromOrigin)) {

			Log.e("end quue", "___que call start que" + isCallFromOrigin);
			isRunningQueue = true;
			new Thread(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub

					if (Utils.isAppOpened(mActivity)
							&& NetworkUtils.isInternetConnected(mActivity)
							&& listAction != null && listAction.size() > 0) {

						MQTTAction action = listAction.get(0);

						Log.e("end quue",
								"___que call send action " + action.getAction());
						String roleName = new String(Base64.decode(
								action.getRoleName(), Base64.DEFAULT));
						String deviceToken = Utils
								.getDeviceTokenSoap(mActivity);
						String ssi = Utils.getSessionId(mActivity);
						String adverID = Utils.getADVERT_ID(mActivity);
						String mqttCode = Utils.getMQTTClientCode(mActivity);
						try {
							if(mqttController!=null)
							{
								mqttController.send(action.getAction(),
										Utils.getEnName(mActivity),
										Utils.getAppId(mActivity),
										mActivity.getPackageName(),
										Utils.getGameVersion(mActivity),
										Utils.getSdkVersion(mActivity),
										action.getRoleId(), roleName,
										action.getRoleLevel(), action.getAreaId(),
										action.getUserId(),
										Utils.getPUserId(mActivity),
										action.getExtra(), deviceToken, ssi,
										mqttCode, adverID, action.getTime(),
										deliveriedListener);
							}

							
							// listAction.remove(0);
						} catch (Exception e) {
							// TODO: handle exception
							e.printStackTrace();
						}

					}

					// isRunningQueue =false;
				}

			}).start();

		} else {
			Log.e("end quue", "___que call que is running" + isCallFromOrigin);
		}

	}

	public static OnSDKBackClickListener getOnBackListener() {
		return onBackListener;
	}

	public static void setOnBackListener(OnSDKBackClickListener onBackListener) {
		SohaSDK.onBackListener = onBackListener;
	}

	public static onFileCompleteChosed getOnFileChosedCallback() {
		return onFileChosedCallback;
	}

	public static void setOnFileChosedCallback(
			onFileCompleteChosed onFileChosedCallback) {
		SohaSDK.onFileChosedCallback = onFileChosedCallback;
	}

	public static void setOnAvatarChosedCallback(
			onFileAvatarChoosed onFileAvatarChoosed) {
		SohaSDK.onAvatarChosed = onFileAvatarChoosed;
	}
}
