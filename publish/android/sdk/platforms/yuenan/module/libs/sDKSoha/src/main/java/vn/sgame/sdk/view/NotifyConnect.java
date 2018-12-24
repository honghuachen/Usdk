package vn.sgame.sdk.view;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.dialogs.DialogLoginForConnectNew;
import vn.soha.game.sdk.dialogs.DialogNotice;
import vn.soha.game.sdk.dialogs.DialogNotice.OnFuckListener;
import vn.soha.game.sdk.dialogs.DialogUpdate;
import vn.soha.game.sdk.server.API;
import vn.soha.game.sdk.utils.AppUtils;
import vn.soha.game.sdk.utils.DialogUtils;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.MyThread;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.content.Context;
import android.graphics.PixelFormat;
import android.graphics.Point;
import android.graphics.Rect;
import android.os.Build;
import android.os.Handler;
import android.text.Html;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Display;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.JavascriptInterface;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.RelativeLayout;
import android.widget.RelativeLayout.LayoutParams;
import android.widget.TextView;

import com.facebook.AccessToken;
import com.facebook.FacebookCallback;
import com.facebook.FacebookException;
import com.facebook.login.LoginManager;
import com.facebook.login.LoginResult;

public class NotifyConnect {
	Activity mActivity;
	View supportView;





	public boolean isShowing = true;

	boolean isShowingPopup = false;
	int mStatusBarHeight;
	public void getStatusBarHeight() {
		Rect rectangle= new Rect();
		Window window= mActivity.getWindow();
		window.getDecorView().getWindowVisibleDisplayFrame(rectangle);
		int statusBarHeight= rectangle.top;
		mStatusBarHeight = statusBarHeight;
	}

	public  int dpToPx(Context context, int dp) {
		DisplayMetrics displayMetrics = context.getResources()
				.getDisplayMetrics();
		int px = Math.round(dp
				* (displayMetrics.xdpi / DisplayMetrics.DENSITY_DEFAULT));
		return px;
	}


	TextView tvMessage,tvAction;
	ImageView imgClose;



	int screenWidth =0;
	LayoutInflater infalter;
	PopupWindow popupWindow ,supportPopup;
	LinearLayout lnLoginFaceBook,lnLoginMobile;
	View popUpView;
	RelativeLayout rlWarningContainer;

	Boolean isPause;


	public NotifyConnect(Activity activity,Boolean isPause) {
		// TODO Auto-generated constructor stub
		mActivity = activity;
		this.isPause = isPause;
		getStatusBarHeight();
		supportView = LayoutInflater.from(mActivity).inflate(R.layout.notify_connect_new, null);
		rlWarningContainer = (RelativeLayout) supportView.findViewById(R.id.rlWarningContainer);
		if(isPause)
		{
			supportView.setVisibility(View.GONE);
		}
        tvMessage = (TextView)supportView.findViewById(R.id.tvMessage);
        tvAction  = (TextView)supportView.findViewById(R.id.tvAction);
        infalter = (LayoutInflater) mActivity.getSystemService(Activity.LAYOUT_INFLATER_SERVICE);
        popUpView = infalter.inflate(R.layout.popup_connect_account, rlWarningContainer,false);
		lnLoginFaceBook = (LinearLayout) popUpView.findViewById(R.id.lnLoginfaceBook);
		lnLoginMobile = (LinearLayout) popUpView.findViewById(R.id.lnLoginMobile);

		imgClose = (ImageView)supportView.findViewById(R.id.imgClose);
		imgClose.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				onDestroy();

			}
		});

        tvAction.setOnClickListener(new OnClickListener() {

    		@Override
    		public void onClick(View v) {
    			// TODO Auto-generated method stub
    			onDestroy();
    			DialogLoginForConnectNew dialog = new DialogLoginForConnectNew(mActivity);
    		}
    	});

        lnLoginFaceBook.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				if (NetworkUtils.isInternetConnected(mActivity)) {
					onDestroy();
					popupWindow.dismiss();
					loginFacebook();
				} else {
					ToastUtils.vToastErrorNetwork(mActivity);
				}

			}
		});

        lnLoginMobile.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				if (NetworkUtils.isInternetConnected(mActivity)) {
					onDestroy();
					popupWindow.dismiss();

					if (DialogLoginForConnectNew.mDialog != null && DialogLoginForConnectNew.mDialog.isShowing()) {
						DialogLoginForConnectNew.mDialog.dismiss();
					}

					new DialogLoginForConnectNew(mActivity);
				} else {
					ToastUtils.vToastErrorNetwork(mActivity);
				}

			}
		});
        tvAction.setText(Html.fromHtml("<u>"+mActivity.getResources().getString(R.string.connectNow2)+"</u>"));
       // initTextNotify(tvMessage,tvAction,mActivity.getResources().getString(R.string.connectMessage));
		attachView();
		startCloseThread();
	}



	public  int getScreenWidth() {
		int screenWidth = 0;
		//		int screenHeight = 0;
		Point size = new Point();
		WindowManager w = mActivity.getWindowManager();

		if(Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB)    {
			w.getDefaultDisplay().getSize(size);
			screenWidth = size.x;
			//			screenHeight = size.y;
		} else {
			Display d = w.getDefaultDisplay();
			screenWidth = d.getWidth();
			//			screenHeight = d.getHeight();
		}
		return screenWidth;

	}


	int padding=5;
	int buttonSize=20;
	void initTextNotify(TextView tvMessage,TextView tvAction,String message)
	{
		message+=" ";
		screenWidth = getScreenWidth()-dpToPx(mActivity, padding*2)-dpToPx(mActivity,buttonSize);
		int oldWidth=0;
		int oldHeight=0;
		int secondLineSize=0;
		String currentLine="";
        int i =0;
        tvAction.setText(Html.fromHtml("<u>"+mActivity.getResources().getString(R.string.connectNow)+"</u>"));
        for( i =0;i<message.length();i++)
        {
        	currentLine+=message.charAt(i);
        	tvMessage.setText(currentLine);

        	tvMessage.measure(0, 0);
        	if(oldWidth>0 &&tvMessage.getMeasuredWidth()+dpToPx(mActivity, 10)>=screenWidth )
        	{
        		break;
        	}
        	else
        	{
        		oldWidth = tvMessage.getMeasuredWidth();
        		oldHeight = tvMessage.getMeasuredHeight();
        	}
        }

        if(i<message.length())
        {
        	if(i<(message.length()-1)&&message.charAt(i)!=' '&&message.charAt(i+1)!=' ')
        	{

        		for(int j=i;j>=0;j--)
        		{
        			String charTem = message.charAt(j)+"";
        			if(charTem.equals(" "))
        			{
        				i=j;
        				break;
        			}
        			Log.e("Char ", message.charAt(j)+"");

        		}
        	}
        	currentLine ="";
        	  for( ;i<message.length();i++)
              {
              	currentLine+=message.charAt(i);

              }


        	  tvMessage.setText(currentLine);
        	  tvMessage.measure(0, 0);
        	  secondLineSize = tvMessage.getMeasuredWidth();

        	  LayoutParams param  = (LayoutParams) tvAction.getLayoutParams();
        	  param.leftMargin = secondLineSize;

        	  tvMessage.setText(message);
        	  tvMessage.measure(0, 0);
        	  param.topMargin = oldHeight-10;//tvMessage.getMeasuredHeight()/2;//-oldHeight;

        }
        else
        {
        	tvAction.measure(0, 0);
        	int textActionSize = tvAction.getMeasuredWidth();
        	if(screenWidth> tvMessage.getMeasuredWidth()+textActionSize+dpToPx(mActivity, padding))
        	{
        		 LayoutParams param  = (LayoutParams) tvAction.getLayoutParams();
              	  param.leftMargin = tvMessage.getMeasuredWidth();
              //	 param.topMargin = oldHeight-10;//tvMessage.getMeasuredHeight()/2;//-oldHeight;
        	}
        	else
        	{
        		 LayoutParams param  = (LayoutParams) tvAction.getLayoutParams();
              	  param.leftMargin = 0;
              	 param.topMargin = oldHeight+dpToPx(mActivity, 10);//tvMessage.getMeasuredHeight()/2;//-oldHeight;
        	}

        }
	}

	public void updateForShowPopUp()
	{
		if (Build.VERSION.SDK_INT < 19
				|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.TYPE_PHONE,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.updateViewLayout(supportView, params);

		} else if (Build.VERSION.SDK_INT >= 19
				&& Build.VERSION.SDK_INT < 23) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.TYPE_TOAST,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;

			windowManager.updateViewLayout(supportView, params);

		} else {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;

			windowManager.updateViewLayout(supportView, params);
		}
	}

	public void updateForHidePopUp()
	{
		if (Build.VERSION.SDK_INT < 19
				|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_PHONE,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);

		} else if (Build.VERSION.SDK_INT >= 19
				&& Build.VERSION.SDK_INT < 23) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_TOAST,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);
		} else {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);
		}
	}
	public void attachView() {
		if (Build.VERSION.SDK_INT < 19
				|| (Build.VERSION.SDK_INT >= 23 && Build.VERSION.SDK_INT <= 25)) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_PHONE,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);

		} else if (Build.VERSION.SDK_INT >= 19
				&& Build.VERSION.SDK_INT < 23) {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_TOAST,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);
		} else {
			WindowManager windowManager = (WindowManager) mActivity
					.getApplicationContext().getSystemService(
							Activity.WINDOW_SERVICE);
			WindowManager.LayoutParams params = new WindowManager.LayoutParams(
					WindowManager.LayoutParams.MATCH_PARENT,
					WindowManager.LayoutParams.WRAP_CONTENT,
					WindowManager.LayoutParams.TYPE_APPLICATION_OVERLAY,
					WindowManager.LayoutParams.FLAG_NOT_FOCUSABLE
							| WindowManager.LayoutParams.FLAG_LAYOUT_IN_SCREEN,
					PixelFormat.TRANSPARENT);
			params.gravity = Gravity.LEFT | Gravity.TOP;
			windowManager.addView(supportView, params);
		}
	}

	MyThread t;
	Handler h = new Handler();
	void startCloseThread()
	{
		t = new MyThread(new Runnable() {

			@Override
			public void run() {
				// TODO Auto-generated method stub
				while(t!=null &&t.isRuning())
				{
					try {
						Thread.sleep(15000);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
					if(!isShowingPopup)
					{
						h.post(new Runnable() {

							@Override
							public void run() {
								// TODO Auto-generated method stub
								onDestroy();
							}
						});
						if(t!=null)
						{
							t.setRuning(false);
						}

					}


				}

			}
		});
		t.start();
	}

	public void hide() {
		supportView.setVisibility(View.GONE);
	}

	public void show() {
		isPause =false;

		if(isShowing&&Utils.isShowConnectAccount(mActivity))
		{
			supportView.setVisibility(View.VISIBLE);
		}

	}


	public void onDestroy() {
		if(t!=null)
		{
			t.setRuning(false);
			t=null;
		}
		isShowing = false;
		supportView.setVisibility(View.GONE);
	}


	/** ---------------- FACEBOOK LOGIN ---------------- */
	public void loginFacebook() {
		if (AccessToken.getCurrentAccessToken() != null && !AccessToken.getCurrentAccessToken().isExpired()) {
			vLoginBig4(AccessToken.getCurrentAccessToken().getToken(), "2");
			Log.e("FB Login", "success");
			// wv.get
		} else {
			LoginManager.getInstance().registerCallback(SohaSDK.callbackManager, facebookLoginResultCallback);
			LoginManager.getInstance().logInWithReadPermissions(mActivity, Arrays.asList("email", "public_profile", "user_friends"));
		}
	}

	private final FacebookCallback<LoginResult> facebookLoginResultCallback = new FacebookCallback<LoginResult>() {
		@Override public void onSuccess(LoginResult loginResult) {
			vLoginBig4(loginResult.getAccessToken().getToken(), "2");
			Log.e("FB Login", "success");
		}

		@Override public void onCancel() {
			Log.e("FB Login", "cancel");
			if(isRatedClick) {
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
				isRatedClick = false;
			}
		}

		@Override public void onError(FacebookException e) {
			Log.e("FB Login", "error");
		}
	};

	private Handler hander = new Handler();
	static Boolean isWaiting = false;
	static Boolean isRatedClick = false;
	// --

	private class LoginJavaScriptInterface {
		@JavascriptInterface
		public void sohalogin(final String method) {
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					if (method.equals("LoginFB")) {
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
					} else if (method.equals("PlayNow")) {
						ToastUtils.vToastShort(mActivity, "PlayNow");
					} else if (method.equals("ConnectAccount")) {
						ToastUtils.vToastShort(mActivity, "ConnectAccount");
					}
				}
			});
		}
	}

	// login Big 4
	private void vLoginBig4(final String big4_access_token, final String big4_type) {
		DialogUtils.vDialogLoadingShowProcessing(mActivity, true);
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils.getParamsLoginBig4ConnectAcc(mActivity, big4_access_token, big4_type)));
				Log.e("param", Utils.getParamsLoginBig4(mActivity, big4_access_token, big4_type));
				final JSONObject json = JsonParser.getJSONFromPostUrl(API.loginBig4, params);
				mActivity.runOnUiThread(new Runnable() {

					@Override
					public void run() {
						// TODO Auto-generated method stub
						DialogUtils.vDialogLoadingDismiss();
						Log.e("final JSONN", json + "");
						try {
							//String jsonDemo = "{\"status\":\"success\",\"error_code\":0,\"type\":\"\",\"data\":{\"access_token\":\"c29hcHRva2VuMC4xNDg4NzcwMCAxNDk1MjUwMTIwKzEzODIxNTUyNzY=\",\"access_token_expired\":1497842960},\"message\":\"\"}";
							//JSONObject jsonObject = new JSONObject(jsonDemo);
							if (json.getString("status").equals("success")) {

								//Utils.setIsShowConnectAccount(mActivity, false);
								String accessToken = json.getJSONObject("data").getString("access_token");
								Utils.saveSoapAccessToken(mActivity, accessToken);
								vGetUserInfo();
							} else {
								String message = json.getString("message");
								ToastUtils.vToastShort(mActivity,message);
							}
						} catch (JSONException e) {
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
							Log.e("final JSONN","user_info__"+ json + "");
							try {
							//	String jsonDemo = "{\"status\":\"success\",\"error_code\":0,\"user_info\":{\"id\":\"834939798\",\"puid\":\"7437413\",\"email\":\"hoangcaomobile@gmail.com\",\"username\":\"hoangcaomobile\",\"avatar\":\"http://avatar.my.soha.vn/80/hoangcaomobile.png\",\"new_user\":0,\"type_user\":\"play_now\"}}";
								//JSONObject jsonObject = new JSONObject(jsonDemo);
								if (json.getString("status").equals("success")) {
									ToastUtils.vToastShort(mActivity,mActivity.getResources().getString(R.string.connectSucess) );
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
										Utils.setIsShowConnectAccount(mActivity, true);

									}
									else
									{
										Utils.setIsShowConnectAccount(mActivity, false);

										SohaSDK.destroyConnectView();

									}

									String avatar = mUserInfo.getString("avatar");
									Utils.saveUserAvatar(mActivity, avatar);

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
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
					});
				}
			}).start();
		}



	// --
}
