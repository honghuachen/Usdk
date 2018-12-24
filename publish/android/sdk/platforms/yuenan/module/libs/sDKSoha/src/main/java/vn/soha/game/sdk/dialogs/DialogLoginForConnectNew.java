package vn.soha.game.sdk.dialogs;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.apache.commons.io.IOUtils;
import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import vn.sgame.sdk.view.KeyboardHeightObserver;
import vn.sgame.sdk.view.KeyboardHeightProvider;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.dialogs.DialogNotice.OnFuckListener;
import vn.soha.game.sdk.server.API;
import vn.soha.game.sdk.utils.AppUtils;
import vn.soha.game.sdk.utils.DialogUtils;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.Dialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnDismissListener;
import android.content.res.Configuration;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Build;
import android.os.Handler;
import android.provider.Settings;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.view.Window;
import android.view.WindowManager.LayoutParams;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.AnimationUtils;
import android.webkit.JavascriptInterface;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.login.LoginManager;
import com.facebook.login.LoginResult;

/**
 * @since January 27, 2015
 * @author Hoang Cao Dev
 *
 */
@SuppressLint({ "JavascriptInterface", "SetJavaScriptEnabled", "InflateParams" }) public class DialogLoginForConnectNew implements KeyboardHeightObserver{

	private static Activity mActivity;
	public static Dialog mDialog;

	private KeyboardHeightProvider keyboardHeightProvider;
	private TextView tvPadding;
	private WebView wv;
	LinearLayout lnErr;
	private IntentFilter mIntentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
	String mPageErrorHtml;
	String mFailingUrl;
ImageView imgBackPayment;

     public interface ConnectCallBack {
    	 void onConnectSuccess();
    	 
     }
     
     ConnectCallBack callback ;
	public DialogLoginForConnectNew(Activity activity) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		SohaSDK.isPendingShowConnectAcc =false;
		SohaSDK.hideDashboard();
		vInitUI();
		vLogin();
		SohaSDK.registerBroadCast(mBroadcastReceiver, mIntentFilter);
		//mActivity.registerReceiver(mBroadcastReceiver, mIntentFilter);
	}
	
	public DialogLoginForConnectNew(Activity activity,ConnectCallBack callback) {
		// TODO Auto-generated constructor stub
		this.callback =callback;
		mActivity = activity;
		SohaSDK.isPendingShowConnectAcc =false;
		SohaSDK.hideDashboard();
		vInitUI();
		vLogin();
		SohaSDK.registerBroadCast(mBroadcastReceiver, mIntentFilter);
		//mActivity.registerReceiver(mBroadcastReceiver, mIntentFilter);
	}

	private BroadcastReceiver mBroadcastReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			ConnectivityManager connectivityManager = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
			NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
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


	private void vLogin() {
		
			mDialog.show();
			if(SohaSDK.connectAccountView!=null)
			{
				SohaSDK.connectAccountView.hide();
			}
		
	}

	public void vInitUI() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.getWindow().setSoftInputMode(LayoutParams.SOFT_INPUT_STATE_HIDDEN);
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_webview);
		mDialog.setCancelable(false);
		imgBackPayment = (ImageView)mDialog.findViewById(R.id.imgBackPayment);
		
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
		
		//imgBackPayment.setVisibility(View.VISIBLE);
		imgBackPayment.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if (Build.VERSION.SDK_INT>=23) {
					if (Settings.canDrawOverlays(mActivity)) {
						SohaSDK.showWarningButton(mActivity);
					}
				} else{
					SohaSDK.showWarningButton(mActivity);
				}
				mDialog.dismiss();
			}
		});
		// load page error
		try {
			mPageErrorHtml = IOUtils.toString(mActivity.getAssets().open("page-error.html"));
			// mPageErrorHtml = mPageErrorHtml.replace("[BUTTONSTYLE]", "btnTryAgainGreen");
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
		lnErr = (LinearLayout)mDialog.findViewById(R.id.lnErr);
		wv = (WebView) mDialog.findViewById(R.id.wv);
		wv.setBackgroundColor(0x00000000);
		mDialog.setOnKeyListener(new Dialog.OnKeyListener() {

			@Override
			public boolean onKey(DialogInterface arg0, int keyCode,
					KeyEvent event) {
				// TODO Auto-generated method stub
				switch (keyCode) {
				case KeyEvent.KEYCODE_BACK:
					if(event.getAction() ==KeyEvent.ACTION_UP&&!isLoginFaceBookClick)
					{
						wv.loadUrl("javascript:onclick_back()");
					}
		
					return true;
				}
				return false;
			}
		});
		
		
		mDialog.setOnDismissListener(new OnDismissListener() {

			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				SohaSDK.isPendingShowConnectAcc =true;
				SohaSDK.showDashboard(mActivity);
				try {
					SohaSDK.unregistBroadcast(mBroadcastReceiver);
				
				} catch (Exception e) {
					Log.e("unregisterReceiver", e.getMessage());
				}
			}
		});
		mDialog.findViewById(R.id.header).setVisibility(View.GONE);

		wv.getSettings().setJavaScriptEnabled(true);
		wv.setHorizontalScrollBarEnabled(false);
		wv.setVerticalScrollBarEnabled(false);
		wv.addJavascriptInterface(new LoginJavaScriptInterface(), "LoginInterface");

		wv.setWebViewClient(new WebViewClient() {			
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
				Log.e("URL", url);
				if (NetworkUtils.isInternetConnected(mActivity)) {
					mDialog.findViewById(R.id.rl).setVisibility(View.VISIBLE);
				}

				if (url.contains("access_token")) {
					wv.setVisibility(View.GONE);
					Uri uri = Uri.parse(url);
					Utils.saveSoapAccessToken(mActivity, uri.getQueryParameter("access_token"));
					if(callback!=null)
					{
						callback.onConnectSuccess();
					}
					showMergeSuccessfulDialog();
					vGetUserInfo();
				}

			}

			@Override
			public void onPageFinished(WebView view, String url) {
				// TODO Auto-generated method stub
				super.onPageFinished(view, url);
				// mDialog.findViewById(R.id.rl).startAnimation(AnimationUtils.loadAnimation(mActivity, android.R.anim.fade_out));
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
				wv.getSettings().setCacheMode(WebSettings.LOAD_DEFAULT);
				
				//wv.loadUrl("javascript:function AppSDKexecute(method) {LoginInterface.sohalogin(method);}");
				wv.loadUrl("javascript:function AppSDKexecute(method,value) {LoginInterface.loginInteract(method,value);}");
			}

			@Override
			public void onReceivedError(WebView view, int errorCode, String description, String failingUrl) {
				// TODO Auto-generated method stub
				super.onReceivedError(view, errorCode, description, failingUrl);
				mFailingUrl = failingUrl;
				view.loadDataWithBaseURL(null, mPageErrorHtml.replace("[RELOADLINK]", failingUrl), "text/html", "utf-8", null);
				mFailingUrl = failingUrl;
			}

		});
		
		wv.loadUrl(API.login + Utils.getDefaultParams(mActivity)+"&connect_account=1");
	}

	public interface OnLoginListener {
		public void onLoginSuccessful(String userId, String accessToken);
		public void onLoginFailed(String reason);
	}

	public interface OnLogoutListener {
		public void onLogoutSuccessful();
	}

	/** ---------------- FACEBOOK LOGIN ---------------- */
	public void loginFacebook() {
		
			LoginManager.getInstance().registerCallback(SohaSDK.callbackManager, facebookLoginResultCallback);
			LoginManager.getInstance().logInWithReadPermissions(mActivity, Arrays.asList("email", "public_profile", "user_friends"));
	
	}

	private final FacebookCallback<LoginResult> facebookLoginResultCallback = new FacebookCallback<LoginResult>() {
		@Override public void onSuccess(LoginResult loginResult) {
			try {
				vLoginBig4(loginResult.getAccessToken().getToken(), "2");
				Log.e("FB Login", "success");
			} catch (Exception e) {
				// TODO: handle exception
				e.printStackTrace();
				ToastUtils.vToastShort(mActivity, "can't not connect facebook, please check your network and try again");
			}
		
		}	

		@Override public void onCancel() {
			Log.e("FB Login", "cancel");
			if(isLoginFaceBookClick) {
				if (isWaiting) {
					if (AppUtils.isAppInstalled(mActivity, "com.facebook.katana")) {
						new Handler().postDelayed(new Runnable() {

							@Override
							public void run() {
								// TODO
								loginFacebook(); 
							}
						}, 1000);
					}
					isWaiting =false;
				} else {
				}
				new Handler().postDelayed(new Runnable() {

					@Override
					public void run() {
						// TODO
						isLoginFaceBookClick = false;
					}
				}, 400);
			}
		}

		@Override public void onError(FacebookException e) {
			Log.e("FB Login", "error");
		}
	};

	private Handler hander = new Handler();
	static Boolean isWaiting = false;
	static Boolean isLoginFaceBookClick = false;
	// --

	private class LoginJavaScriptInterface {
	/*	@JavascriptInterface
		public void sohalogin(final String method) {
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					if (method.equals("ConnectLoginFB")) {
						isRatedClick=true;
						isWaiting = false;
						loginFacebook();
						hander.postDelayed(new Runnable() {

							@Override
							public void run() {
								// TODO Auto-generated method stub
								isWaiting = true;
							}
						}, 6000);
					} else if (method.equals("close_popup")) {
						mDialog.dismiss();
						//ToastUtils.vToastShort(mActivity, "PlayNow");
					} 
				}
			});
		}*/
		
		@JavascriptInterface
		public void loginInteract(final String method,final String value) {
			
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					if (method.equals("ConnectLoginFB")) {
						isLoginFaceBookClick=true;
						isWaiting = false;
						loginFacebook();
						hander.postDelayed(new Runnable() {

							@Override
							public void run() {
								// TODO Auto-generated method stub
								isWaiting = true;
							}
						}, 6000);
					} else if (method.equals("close_popup")) {
						mDialog.dismiss();
						//ToastUtils.vToastShort(mActivity, "PlayNow");
					} 
					
					if(method.equals("onclick_back"))
					{
						int result = Integer.parseInt(value);
						if(result==0)
						{
							mDialog.dismiss();
						}
						Log.e("value ", "__"+value);
					}
				}
			});
					
		}
	}

	public Boolean isLoginBig4 =false;
	// login Big 4
	private void vLoginBig4(final String big4_access_token, final String big4_type) {
		DialogUtils.vDialogLoadingShowProcessing(mActivity, true);
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils.getParamsLoginBig4ConnectAcc(mActivity, big4_access_token, big4_type)));	
				
				final JSONObject json = JsonParser.getJSONFromPostUrl(API.loginBig4, params);
				
				mActivity.runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						
						
						DialogUtils.vDialogLoadingDismiss();
						
						try {
							Log.e("final JSONN", json + "");
							/*String jsonDemo = json.getString("signed_request");//"{\"status\":\"success\",\"error_code\":0,\"type\":\"\",\"data\":{\"access_token\":\"c29hcHRva2VuMC4xNDg4NzcwMCAxNDk1MjUwMTIwKzEzODIxNTUyNzY=\",\"access_token_expired\":1497842960},\"message\":\"\"}";
							JSONObject jsonObject = new JSONObject(jsonDemo);*/
							if (json.getString("status").equals("success")) {
								String accessToken = json.getJSONObject("data").getString("access_token");
								Utils.saveSoapAccessToken(mActivity, accessToken);
								if(callback!=null)
								{
									callback.onConnectSuccess();
								}
								showMergeSuccessfulDialog();
								isLoginBig4 =true;
								vGetUserInfo();
							} else {
								if(json.getString("status").equals("confirm_otp"))
								{
									String otpToken = json.getString("otp_token");
									String message =json.getString("message");  
									String syntax =json.getString("syntax");  
									String phoneNumber =json.getString("phone_number");  
									wv.loadUrl(API.login + Utils.getParamsLoginOtp(mActivity,otpToken,message,syntax,phoneNumber));
									
								}
								else
								{
									ToastUtils.vToastShort(mActivity, json.getString("message"));
								}
							}
						} catch (Exception e) {
							// TODO Auto-generated catch block
							ToastUtils.vToastErrorTryAgain(mActivity);
							e.printStackTrace();
						}
					}
				});
			}
		}).start();

	}
	
	// get account info
		private void vGetUserInfo() {
			DialogUtils.vDialogLoadingShowProcessing(mActivity, true);
			new Thread(new Runnable() {

				@Override
				public void run() {
					// TODO Auto-generated method stub
					List<NameValuePair> params = new ArrayList<NameValuePair>();
					params.add(new BasicNameValuePair("signed_request", Utils.getDefaultParamsPost(mActivity)));	
					Log.e("param", Utils.getDefaultParams(mActivity));
					final JSONObject json = JsonParser.getJSONFromPostUrl(API.getUserInfo, params);
					mActivity.runOnUiThread(new Runnable() {

						@Override
						public void run() {
							// TODO Auto-generated method stub
							DialogUtils.vDialogLoadingDismiss();
							if(json==null)
							{
								ToastUtils.vToastShort(mActivity,mActivity.getResources().getString(R.string.toastErrorTryAgain1) );
							}else
							{
								try {
									
									
									//String jsonDemo = "{\"status\":\"success\",\"error_code\":0,\"user_info\":{\"id\":\"834939798\",\"puid\":\"7437413\",\"email\":\"hoangcaomobile@gmail.com\",\"username\":\"hoangcaomobile\",\"avatar\":\"http://avatar.my.soha.vn/80/hoangcaomobile.png\",\"new_user\":0,\"type_user\":\"play_now\"}}";
								//	JSONObject jsonObject = new JSONObject(jsonDemo);
									if (json.getString("status").equals("success")) {
									/*	if(!isLoginBig4)
										{
											ToastUtils.vToastShort(mActivity,mActivity.getResources().getString(R.string.connectSucess) );
										}*/
										
										JSONObject mUserInfo = json.getJSONObject("user_info");
										String userId = mUserInfo.getString("id");
										String puId = mUserInfo.getString("puid");
										String username = mUserInfo.getString("username");
										String email = mUserInfo.getString("email");
										String userType = mUserInfo.getString("type_user");

										Utils.saveUserId(mActivity, userId);
										Utils.savePUserId(mActivity, puId);
										Utils.saveUserName(mActivity, username);
										Utils.saveUserEmail(mActivity, email);
										Utils.saveUserType(mActivity, userType);
										if(userType.equalsIgnoreCase("play_now"))
										{
											//SohaSDK.startConnectAccountThread();
											Utils.setIsShowConnectAccount(mActivity, true);
											
										}
										else
										{
											SohaSDK.destroyConnectView();
											Utils.setIsShowConnectAccount(mActivity, false);
										}

										String avatar = mUserInfo.getString("avatar");
										Utils.saveUserAvatar(mActivity, avatar);


										if (Build.VERSION.SDK_INT>=23) {
											if (Settings.canDrawOverlays(mActivity)) {
												SohaSDK.showWarningButton(mActivity);
											}
										} else {
											SohaSDK.showWarningButton(mActivity);
										}
										mDialog.dismiss();
										
										
										

										// UPDATE
										// update {status: 0 title: "" message: "" link: "" force: 0}
										if (json.toString().contains("update")) {
											JSONObject jo = json.getJSONObject("update");
											if (jo.getString("status").equals("1")
													&& jo.getString("force").equals("0")) {
												// need to update (not required)
												new DialogUpdate(mActivity, false, jo.getString("link"));
											} else if (jo.getString("status").equals("1")) {
												// need to update (required)
												new DialogUpdate(mActivity, true, jo.getString("link"));
											}
										}
									} else {
										
										if (json.getString("status").equals("notice")) {
											
											mDialog.dismiss();
											new DialogNotice(mActivity, json, new OnFuckListener() {
												
												@Override
												public void onFuck() {
													// TODO Auto-generated method stub
													vGetUserInfo();
												}
											});
										}
										else
										{
											ToastUtils.vToastErrorTryAgain(mActivity);
										}
										
										
									}
								} catch (Exception e) {
									// TODO Auto-generated catch block
									e.printStackTrace();
									ToastUtils.vToastShort(mActivity,mActivity.getResources().getString(R.string.toastErrorTryAgain2) );
								}
							}
							
						}
					});
				}
			}).start();

		}
	// --

	// --
		
		private static void showMergeSuccessfulDialog() {
			// if (isShowingHello) {
			// return;
			// }

			final ViewGroup rootView = (ViewGroup) mActivity
					.findViewById(android.R.id.content);
			LayoutInflater inflater = LayoutInflater.from(mActivity);
			final View helloView = inflater.inflate(R.layout.layout_hello, null);
			((TextView) helloView.findViewById(R.id.tv_hello)).setText(mActivity
					.getString(R.string.textviewConectSuccess));

			Animation animation = AnimationUtils.loadAnimation(mActivity,
					R.anim.top_in_then_out);
			animation.setAnimationListener(new AnimationListener() {
				@Override
				public void onAnimationStart(Animation animation) {
					// isShowingHello = true;
				}

				@Override
				public void onAnimationRepeat(Animation animation) {
				}

				@Override
				public void onAnimationEnd(Animation animation) {
					// TODO Auto-generated method stub
					// isShowingHello = false;
					helloView.setVisibility(View.GONE);
				}
			});

			helloView.startAnimation(animation);
			rootView.addView(helloView);
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
}
