package vn.soha.game.sdk.dialogs;

import java.io.ByteArrayOutputStream;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.net.URLEncoder;
import java.util.List;

import org.apache.commons.io.IOUtils;

import shg.vn.track.MQTTUtils;
import vn.sgame.sdk.R;
import vn.sgame.sdk.view.DashboardButton;
import vn.sgame.sdk.view.KeyboardHeightObserver;
import vn.sgame.sdk.view.KeyboardHeightProvider;
import vn.soha.game.sdk.SohaApplication;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.dialogs.DialogLoginForConnectNew.ConnectCallBack;
import vn.soha.game.sdk.utils.DialogUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;
import android.Manifest;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Build;
import android.os.Handler;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.Window;
import android.webkit.JavascriptInterface;
import android.webkit.ValueCallback;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

public class DialogDetailDashBoard implements KeyboardHeightObserver {

	private Activity mActivity;
	public static Dialog mDialog;
	private WebView wv;
	private String mPageErrorHtml;
	LinearLayout lnErr;
	String mFailingUrl;
	public static Boolean isShowingProfile = false;
	private String url, cacheUrl;
	private TextView tvPadding;
	private IntentFilter mIntentFilter = new IntentFilter(
			"android.net.conn.CONNECTIVITY_CHANGE");
	private KeyboardHeightProvider keyboardHeightProvider;
	private static int PERMISSION_REQUEST_CODE = 204;
	private static int PERMISSION_REQUEST_CODE1 = 205;

	public DialogDetailDashBoard(Activity activity, String mUrl) {

		this.mActivity = activity;
		this.url = mUrl;
		SohaSDK.isShowDashBoardDetail = true;

		Log.d("________________________________url", url);
		SohaSDK.registerBroadCast(mBroadcastReceiver, mIntentFilter);
		init();
	}

	private BroadcastReceiver mBroadcastReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			ConnectivityManager connectivityManager = (ConnectivityManager) context
					.getSystemService(Context.CONNECTIVITY_SERVICE);
			NetworkInfo activeNetworkInfo = connectivityManager
					.getActiveNetworkInfo();
			if (activeNetworkInfo == null) {

				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
				lnErr.setVisibility(View.VISIBLE);

			} else {
				lnErr.setVisibility(View.GONE);
				// mDialog.findViewById(R.id.rlCheckInternet).setVisibility(View.GONE);
				// wv.reload();
			}
		}
	};
	Handler h = new Handler();

	@SuppressLint("NewApi")
	@SuppressWarnings("deprecation")
	public void init() {

		mDialog = new Dialog(mActivity,
				android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		// mDialog.getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_ADJUST_RESIZE);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_detail_dashboard);
		mDialog.setCancelable(true);
		mDialog.setOnKeyListener(new Dialog.OnKeyListener() {

			@Override
			public boolean onKey(DialogInterface arg0, int keyCode,
					KeyEvent event) {
				// TODO Auto-generated method stub
				switch (keyCode) {
				case KeyEvent.KEYCODE_BACK:
					if (event.getAction() == KeyEvent.ACTION_UP) {

						Log.e("check", "___click back");
						wv.loadUrl("javascript:onclick_back()");
					}
					return true;
				}
				return false;
			}
		});
		mDialog.show();

		tvPadding = (TextView) mDialog.findViewById(R.id.tvPadding);
		keyboardHeightProvider = new KeyboardHeightProvider(mActivity);

		// make sure to start the keyboard height provider after the onResume
		// of this activity. This is because a popup window must be initialised
		// and attached to the activity root view.
		View view = mDialog.findViewById(R.id.activitylayout);
		view.post(new Runnable() {
			public void run() {
				try {
					keyboardHeightProvider.start();
				} catch (Exception e) {
					// TODO: handle exception
					e.printStackTrace();
				}

			}
		});
		keyboardHeightProvider.setKeyboardHeightObserver(this);
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
			WebView.setWebContentsDebuggingEnabled(true);

		}

		mDialog.findViewById(R.id.header).setVisibility(View.GONE);

		lnErr = (LinearLayout) mDialog.findViewById(R.id.lnErr);
		// load page error
		try {
			mPageErrorHtml = IOUtils.toString(mActivity.getAssets().open(
					"page-error.html"));
			// mPageErrorHtml = mPageErrorHtml.replace("[BUTTONSTYLE]",
			// "btnTryAgainGreen");
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}

		wv = (WebView) mDialog.findViewById(R.id.wvDb);
		wv.setBackgroundColor(0x00000000);
		/*
		 * mDialog.setOnKeyListener(new Dialog.OnKeyListener() {
		 * 
		 * @Override public boolean onKey(DialogInterface arg0, int keyCode,
		 * KeyEvent event) { // TODO Auto-generated method stub switch (keyCode)
		 * { case KeyEvent.KEYCODE_BACK: if
		 * (NetworkUtils.isInternetConnected(mActivity)) { if (wv.canGoBack() &&
		 * wv.isFocused()) { wv.goBack(); } } else {
		 * ToastUtils.vToastErrorNetwork(mActivity); } return true; } return
		 * false; } });
		 */
		mDialog.setOnDismissListener(new OnDismissListener() {

			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				try {
					isShowingProfile = false;
					SohaSDK.isShowDashBoardDetail = false;
					DashboardButton.isPendingClick = false;
					if (isShowDashBoadAgain) {
						SohaSDK.showDashboard(mActivity);
					}

					SohaSDK.fuckLog(MQTTUtils.ACTION_CLOSE_DB, "");
					SohaSDK.sendLog(MQTTUtils.ACTION_CLOSE_DB, "");
					SohaApplication.getInstance().trackEvent("sdk",
							MQTTUtils.ACTION_CLOSE_DB, "");
					SohaSDK.unregistBroadcast(mBroadcastReceiver);
				} catch (Exception e) {
					Log.e("unregisterReceiver", e.getMessage());
				}
			}
		});

		initWebSetting(wv);

		wv.addJavascriptInterface(new CloseJavaScriptInterface(),
				"CloseDialogInterface");

		wv.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
				Log.e("URL", url);
			}

			@Override
			public void onPageFinished(WebView view, String url) {
				// TODO Auto-generated method stub
				super.onPageFinished(view, url);
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
				wv.getSettings().setCacheMode(WebSettings.LOAD_DEFAULT);
				cacheUrl = url;
				if (url.contains("#account")) {
					isShowingProfile = true;
					if (SohaSDK.connectAccountView != null) {
						SohaSDK.connectAccountView.hide();
					}
					// SohaSDK.hide

				} else {
					isShowingProfile = false;
				}
				wv.loadUrl("javascript:function AppSDKexecute(method,value) {CloseDialogInterface.closedashboard(method,value);}");
			}

			@Override
			public void onReceivedError(WebView view, int errorCode,
					String description, String failingUrl) {
				// TODO Auto-generated method stub
				super.onReceivedError(view, errorCode, description, failingUrl);
				mFailingUrl = failingUrl;
				view.loadDataWithBaseURL(null,
						mPageErrorHtml.replace("[RELOADLINK]", failingUrl),
						"text/html", "utf-8", null);
				mFailingUrl = failingUrl;
			}
		});
		// String demo =
		// "https://auth.shg.vn/ticket?app_id=dd1f8869f1813609309b35178ae78c7e&app_id=dd1f8869f1813609309b35178ae78c7e&areaid=1&roleid=1&rolename=Y2hhcmFjdGVyIDE=&is_encode_character=1&rolelevel=10&gver=1.0.1&sdkver=1.0.3&clientname=&access_token=c29hcHRva2VuMC44NzA3MDAwMCAxNDk3OTI3MzM5KzE1MjExNzg4NDA=&device_id=351831063455854&device_token_soap=c29hcHRva2VuMC42OTIzODUwMCAxNDkzODA3MDIxKzExNDMwMzg4NTI=&lang=vi-VN#/app/ticket/";
		// wv.loadUrl(demo);
		wv.loadUrl(url + "?signed_request="
				+ URLEncoder.encode(Utils.getDefaultParamsPost(mActivity)));
		SohaSDK.setOnAvatarChosedCallback(new onFileAvatarChoosed() {

			@Override
			public void onAvatarChosed(String url) {
				// TODO Auto-generated method stub
				Log.e("CHECK_URL_AVATAR", url);
				ByteArrayOutputStream baos = new ByteArrayOutputStream();
				Bitmap bitmap = BitmapFactory.decodeFile(url);
				if (bitmap != null) {
					double ratio = (bitmap.getWidth() * 1.0)
							/ bitmap.getHeight();
					if (bitmap.getWidth() > 480) {
						int width = 300;
						int height = (int) (300 / ratio);

						bitmap = Bitmap.createScaledBitmap(bitmap, width,
								height, false);
					}
					bitmap.compress(Bitmap.CompressFormat.JPEG, 70, baos);
					byte[] imageBytes = baos.toByteArray();
					String imageString = Base64.encodeToString(imageBytes,
							Base64.DEFAULT);
					String content = "javascript: returnSelectedAvatar(['data:image/png;base64,"
							+ imageString + "'])";
					Log.e("CF_URL_AVATAR", content);
					wv.loadUrl(content);
				} else {
					mActivity.runOnUiThread(new Runnable() {
						public void run() {
							Toast.makeText(mActivity,
									mActivity.getString(R.string.bitmapNull),
									500).show();
						}
					});
				}
			}
		});
		SohaSDK.setOnFileChosedCallback(new onFileCompleteChosed() {

			@Override
			public void onFileChosed(final List<String> url) {
				// dialogUtils.vDialogLoadingShow(mActivity, "", false);
				// TODO Auto-generated method stub
				DialogUtils.vDialogLoadingShowProcessing(mActivity, false);

				new Thread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						String content = "javascript: returnSelectedPhoto([";
						for (int i = 0; i < url.size(); i++) {
							ByteArrayOutputStream baos = new ByteArrayOutputStream();

							Bitmap bitmap = BitmapFactory
									.decodeFile(url.get(i));
							if (bitmap != null) {
								double ratio = (bitmap.getWidth() * 1.0)
										/ bitmap.getHeight();
								if (bitmap.getWidth() > 480) {
									int width = 300;
									int height = (int) (300 / ratio);

									bitmap = Bitmap.createScaledBitmap(bitmap,
											width, height, false);
									bitmap.compress(Bitmap.CompressFormat.JPEG,
											70, baos);
									byte[] imageBytes = baos.toByteArray();
									String imageString = Base64.encodeToString(
											imageBytes, Base64.DEFAULT);
									String a = "ABCD";
									if (i != url.size() - 1) {
										content = content
												+ "'data:image/png;base64,"
												+ imageString + "',";
									} else {
										content = content
												+ "'data:image/png;base64,"
												+ imageString + "'])";
									}
									final String contentEnd = content;
									h.post(new Runnable() {

										@Override
										public void run() {
											// TODO Auto-generated method stub
											Log.e("CHECK_URL_IMAGE", contentEnd);
											wv.loadUrl(contentEnd);
											DialogUtils.vDialogLoadingDismiss();
										}
									});
								}
							} else {
								mActivity.runOnUiThread(new Runnable() {

									@Override
									public void run() {
										// TODO Auto-generated method stub
										DialogUtils.vDialogLoadingDismiss();
										Toast.makeText(
												mActivity,
												mActivity
														.getString(R.string.bitmapListNull),
												500).show();
									}
								});
							}

						}
					}
				}).start();

			}
		});

	}

	@SuppressLint("NewApi")
	void initWebSetting(final WebView wv) {
		WebSettings ws = wv.getSettings();
		ws.setJavaScriptEnabled(true);
		ws.setSupportZoom(false);
		ws.setAllowFileAccess(true);
		ws.setAllowFileAccess(true);
		ws.setAppCacheEnabled(false);
		ws.setAllowContentAccess(true);
		ws.setAllowFileAccessFromFileURLs(true);
		ws.setAllowUniversalAccessFromFileURLs(true);
		ws.setCacheMode(WebSettings.LOAD_NO_CACHE);
		ws.setDomStorageEnabled(true);

		wv.setWebChromeClient(new WebChromeClient() {
			// For Android 3.0-
			public void openFileChooser(ValueCallback<Uri> uploadMsg) {
				// Toast.makeText(mActivity, "getcontent 1",1000).show();

				SohaSDK.filePathCallback = uploadMsg;
				Intent i = new Intent(Intent.ACTION_GET_CONTENT);
				i.addCategory(Intent.CATEGORY_OPENABLE);
				i.setType("image/*");
				mActivity.startActivityForResult(
						Intent.createChooser(i, "File Chooser"),
						SohaSDK.PICKFILE_REQUEST_CODE);

			}

			// For Android 3.0+
			public void openFileChooser(ValueCallback uploadMsg,
					String acceptType) {
				// Toast.makeText(mActivity, "getcontent 2",1000).show();

				SohaSDK.filePathCallback = uploadMsg;
				Intent i = new Intent(Intent.ACTION_GET_CONTENT);
				i.addCategory(Intent.CATEGORY_OPENABLE);
				i.setType("*/*");
				mActivity.startActivityForResult(
						Intent.createChooser(i, "File Browser"),
						SohaSDK.PICKFILE_REQUEST_CODE);
			}

			// For Android 4.1
			public void openFileChooser(ValueCallback<Uri> uploadMsg,
					String acceptType, String capture) {
				// Toast.makeText(mActivity, "getcontent 3",1000).show();

				SohaSDK.filePathCallback = uploadMsg;
				Intent i = new Intent(Intent.ACTION_GET_CONTENT);
				// i.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
				i.addCategory(Intent.CATEGORY_OPENABLE);
				i.setType("image/*");
				mActivity.startActivityForResult(
						Intent.createChooser(i, "File Chooser"),
						SohaSDK.PICKFILE_REQUEST_CODE);

			}

			public void showFileChooser(
					ValueCallback<String[]> filePathCallback,
					String acceptType, boolean paramBoolean) {
				// Toast.makeText(mActivity, "getcontent 5",1000).show();
				// TODO Auto-generated method stub
			}

			public void showFileChooser(
					ValueCallback<String[]> uploadFileCallback,
					FileChooserParams fileChooserParams) {
				// TODO Auto-generated method stub
				// Toast.makeText(mActivity, "getcontent 6",1000).show();
			}

			// For Lollipop 5.0+ Devices
			public boolean onShowFileChooser(WebView mWebView,
					ValueCallback<Uri[]> filePathCallback,
					WebChromeClient.FileChooserParams fileChooserParams) {
				// Toast.makeText(mActivity, "getcontent 4",1000).show();

				if (SohaSDK.filePathCallback2 != null) {
					SohaSDK.filePathCallback2.onReceiveValue(null);
					SohaSDK.filePathCallback2 = null;
				}

				SohaSDK.filePathCallback2 = filePathCallback;

				Intent intent = fileChooserParams.createIntent();
				// mActivity.startActivityForResult(intent,
				// SohaSDK.PICKFILE_REQUEST_CODE );
				SohaSDK.requestPermision(intent);
				/*
				 * try { mActivity.startActivityForResult(intent,
				 * SohaSDK.PICKFILE_REQUEST_CODE2 ); } catch
				 * (ActivityNotFoundException e) { SohaSDK.filePathCallback2 =
				 * null; Toast.makeText(mActivity.getApplicationContext(),
				 * "Cannot Open File Chooser", Toast.LENGTH_LONG).show(); return
				 * false; }
				 */
				return true;
			}

		});

		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.ECLAIR) {
			try {

				Method m1 = WebSettings.class.getMethod("setDomStorageEnabled",
						new Class[] { Boolean.TYPE });
				m1.invoke(ws, Boolean.TRUE);

				Method m2 = WebSettings.class.getMethod("setDatabaseEnabled",
						new Class[] { Boolean.TYPE });
				m2.invoke(ws, Boolean.TRUE);

				Method m3 = WebSettings.class.getMethod("setDatabasePath",
						new Class[] { String.class });
				m3.invoke(ws, "/data/data/" + mActivity.getPackageName()
						+ "/databases/");

				Method m4 = WebSettings.class.getMethod("setAppCacheMaxSize",
						new Class[] { Long.TYPE });
				m4.invoke(ws, 1024 * 1024 * 8);

				Method m5 = WebSettings.class.getMethod("setAppCachePath",
						new Class[] { String.class });
				m5.invoke(ws, "/data/data/" + mActivity.getPackageName()
						+ "/cache/");

				Method m6 = WebSettings.class.getMethod("setAppCacheEnabled",
						new Class[] { Boolean.TYPE });
				m6.invoke(ws, Boolean.TRUE);

			} catch (NoSuchMethodException e) {
				e.printStackTrace();
			} catch (InvocationTargetException e) {
				e.printStackTrace();
			} catch (IllegalAccessException e) {
				e.printStackTrace();
			}
		}
	}

	Boolean isShowDashBoadAgain = true;

	private class CloseJavaScriptInterface {
		@JavascriptInterface
		public void closedashboard(final String method, final String value) {

			if (method.equalsIgnoreCase("logout")) {
				h.post(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						isShowDashBoadAgain = false;
						mDialog.dismiss();
						SohaSDK.logoutFromServer();
					}
				});

			}
			if (method.equalsIgnoreCase("close_db"))
				mDialog.dismiss();

			if (method.equalsIgnoreCase("select_photo")) {
				if (checkPermission()) {
					Intent intent = new Intent();
					intent.setType("image/*");
					intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
					intent.setAction(Intent.ACTION_GET_CONTENT);
					mActivity.startActivityForResult(
							Intent.createChooser(intent, "Select Picture"),
							9001);
				} else {
					ActivityCompat
							.requestPermissions(
									mActivity,
									new String[] { Manifest.permission.READ_EXTERNAL_STORAGE },
									PERMISSION_REQUEST_CODE);
				}

			}
			if (method.equalsIgnoreCase("select_avatar")) {
				Log.e("ABCDEF", checkPermission() + " 123");
				if (checkPermission()) {
					Intent intent = new Intent();
					intent.setType("image/*");
					intent.setAction(Intent.ACTION_GET_CONTENT);
					mActivity
							.startActivityForResult(Intent.createChooser(
									intent, "Select Avatar"), 9000);
				} else {
					ActivityCompat
							.requestPermissions(
									mActivity,
									new String[] { Manifest.permission.READ_EXTERNAL_STORAGE },
									PERMISSION_REQUEST_CODE1);
				}

			}
			if (method.equals("onclick_back")) {
				if (value.equalsIgnoreCase("0")) {
					mDialog.dismiss();
				}
			}
			if (method.equalsIgnoreCase("connect_account")) {
				mActivity.runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						new DialogLoginForConnectNew(mActivity,
								new ConnectCallBack() {

									@Override
									public void onConnectSuccess() {
										// TODO Auto-generated method stub

										// wv.clearCache(true);
										DialogConnectAccount
												.closeConnectDialog();
										wv.loadUrl("javascript: location.reload();");
										ToastUtils
												.vToastShort(
														mActivity,
														mActivity
																.getResources()
																.getString(
																		R.string.connectSucess));
										Log.e("url", "cacheUrl" + cacheUrl);
										// mDialog.dismiss();
										// Toast.makeText(mActivity, cacheUrl,
										// 1000).show();
									}
								});
					}
				});

			}
		}
	}

	public interface onFileCompleteChosed {

		void onFileChosed(List<String> url);
	}

	public interface onFileAvatarChoosed {
		void onAvatarChosed(String url);
	}

	@Override
	public void onKeyboardHeightChanged(int height, int orientation) {
		// TODO Auto-generated method stub
		String or = orientation == Configuration.ORIENTATION_PORTRAIT ? "portrait"
				: "landscape";
		Log.e("_________height____", "onKeyboardHeightChanged in pixels: "
				+ height + " " + or);

		// TextView tv = (TextView)findViewById(R.id.height_text);
		// tv.setText(Integer.toString(height) + " " + or);

		// color the keyboard height view, this will stay when you close the
		// keyboard
		// View view = mDialog.findViewById(R.id.keyboard);
		RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams) tvPadding
				.getLayoutParams();
		params.height = height;
		tvPadding.setLayoutParams(params);

	}

	private boolean checkPermission() {
		int result = ContextCompat.checkSelfPermission(mActivity,
				Manifest.permission.READ_EXTERNAL_STORAGE);
		return result == PackageManager.PERMISSION_GRANTED;
	}

}
