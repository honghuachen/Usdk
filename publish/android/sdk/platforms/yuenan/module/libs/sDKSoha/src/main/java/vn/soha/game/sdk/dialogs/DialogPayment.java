package vn.soha.game.sdk.dialogs;

import java.io.ByteArrayOutputStream;
import java.security.KeyFactory;
import java.security.PublicKey;
import java.security.spec.X509EncodedKeySpec;
import java.util.ArrayList;
import java.util.List;

import javax.crypto.Cipher;

import org.apache.commons.io.IOUtils;
import org.apache.http.NameValuePair;
import org.apache.http.message.BasicNameValuePair;
import org.json.JSONException;
import org.json.JSONObject;

import vn.sgame.sdk.R;
import vn.soha.game.sdk.SohaApplication;
import vn.soha.game.sdk.SohaSDK;
import vn.soha.game.sdk.server.API;
import vn.soha.game.sdk.utils.AnimationUtils;
import vn.soha.game.sdk.utils.DialogUtils;
import vn.soha.game.sdk.utils.JsonParser;
import vn.soha.game.sdk.utils.Logger;
import vn.soha.game.sdk.utils.MQTTUtils;
import vn.soha.game.sdk.utils.NameSpace;
import vn.soha.game.sdk.utils.NetworkUtils;
import vn.soha.game.sdk.utils.ToastUtils;
import vn.soha.game.sdk.utils.Utils;
import android.annotation.SuppressLint;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.DialogInterface.OnDismissListener;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.os.Bundle;
import android.os.Handler;
import android.util.Base64;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.Window;
import android.view.WindowManager;
import android.webkit.JavascriptInterface;
import android.webkit.JsResult;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

/**
 * @since April 08, 2015
 * @author Sohagame SDK Team
 * 
 */
@SuppressLint("SetJavaScriptEnabled")
public class DialogPayment implements OnClickListener {

	private Activity mActivity;
	public static Dialog mDialog;
	private WebView wv;
	public static OnPaymentListener mOnPaymentListener;

	
	//private String javascriptImplementation2 = "javascript:function AppSDKexecute('iappay', { package_id : [PACKAGE_ID_ID] })";

	private PaymentJavaScriptInterface mPaymentJavaScriptInterface;
	private static final String PAYMENT_INTERFACE = "PaymentInterface";
	private static final int MAX_DECRYPT_BLOCK = 128;
	private IntentFilter mIntentFilter = new IntentFilter("android.net.conn.CONNECTIVITY_CHANGE");
	String mPageErrorHtml;
	String mFailingUrl;
	private boolean isShowedDialodErr = false;
	private ImageView imgBack;

	public DialogPayment(Activity activity, OnPaymentListener onPaymentListener) {
		// TODO Auto-generated constructor stub
		
		Log.e("payment ", "Pay_ init pay dialog");
		mActivity = activity;
		mActivity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		mOnPaymentListener = onPaymentListener;
		SohaSDK.hideDashboard();
		SohaSDK.vInitGoogleBillingStart(mActivity);
		vInit();
		SohaSDK.sendLog(MQTTUtils.ACTION_PAYMENT_OPEN, "");
		SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_OPEN, "");
		SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_OPEN, "");
		mActivity.registerReceiver(mBroadcastReceiver, mIntentFilter);
		isShowedDialodErr = false;
		
		SohaApplication.getInstance().trackScreenView("form_payment");
		Log.e("payment ", "Pay_ init call end of contructor");
	}

	private BroadcastReceiver mBroadcastReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			ConnectivityManager connectivityManager = (ConnectivityManager) context
					.getSystemService(Context.CONNECTIVITY_SERVICE);
			NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
			if (activeNetworkInfo == null) {
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
			} else {
				// mDialog.findViewById(R.id.rlCheckInternet).setVisibility(View.GONE);
				//wv.reload();
			}
		}
	};

	Boolean isShowDashboardAgain =true;
	public void vInit() {
		mDialog = new Dialog(mActivity, android.R.style.Theme_Translucent_NoTitleBar_Fullscreen);
		mDialog.getWindow().getAttributes().windowAnimations = R.style.dialogAnim;
		mDialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
		mDialog.setContentView(R.layout.dialog_webview);
		mDialog.setCancelable(true);
		mDialog.show();
		Log.e("payment ", "Pay_ call show payDialog");
		mDialog.setOnKeyListener(new Dialog.OnKeyListener() {

			@Override
			public boolean onKey(DialogInterface arg0, int keyCode,
					KeyEvent event) {
				// TODO Auto-generated method stub
				switch (keyCode) {
				case KeyEvent.KEYCODE_BACK:
					if(event.getAction() ==KeyEvent.ACTION_UP)
					{
						/*SohaSDK.sendLog(
								MQTTUtils.ACTION_PAYMENT_CLOSE, "");
						SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_CLOSE, "");
						SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_CLOSE, "");*/
						mDialog.dismiss();
					}	
						
					return true;
				}
				return false;
			}
		});

		mDialog.findViewById(R.id.header).setVisibility(View.GONE);

		mDialog.setOnDismissListener(new OnDismissListener() {

			@Override
			public void onDismiss(DialogInterface dialog) {
				// TODO Auto-generated method stub
				mOnPaymentListener.onDismiss();
				if(isShowDashboardAgain)
				{
					SohaSDK.showDashboard(mActivity);
				}
				
				SohaSDK.vInitGoogleBillingStop(mActivity);
				mActivity.getWindow().clearFlags(android.view.WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
				SohaSDK.sendLog(
						MQTTUtils.ACTION_PAYMENT_CLOSE, "");
				SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_CLOSE, "");
				SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_CLOSE, "");
			
				try {
					SohaSDK.unregistBroadcast(mBroadcastReceiver);
					//mActivity.unregisterReceiver(mBroadcastReceiver);
				} catch (Exception e) {
					Log.e("unregisterReceiver", e.getMessage());
				}
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

		TextView tvHeaderTitle = (TextView) mDialog.findViewById(R.id.tvHeaderTitle);
		tvHeaderTitle.setText(mActivity.getString(R.string.textviewPayment));
		
		imgBack = (ImageView) mDialog.findViewById(R.id.imgBackPayment);
		//imgBack.setVisibility(View.VISIBLE);
		imgBack.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				SohaSDK.sendLog(MQTTUtils.ACTION_PAYMENT_CLOSE, "");
				SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_CLOSE, "");
				SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_CLOSE, "");
				mDialog.dismiss();
			}
		});

		wv = (WebView) mDialog.findViewById(R.id.wv);
		wv.getSettings().setJavaScriptEnabled(true);
		wv.setHorizontalScrollBarEnabled(false);
		wv.setVerticalScrollBarEnabled(false);
		mPaymentJavaScriptInterface = new PaymentJavaScriptInterface();
		wv.addJavascriptInterface(mPaymentJavaScriptInterface, "PaymentInterface");
		
		
		
		
		wv.getSettings().setLoadsImagesAutomatically(true);
		wv.setBackgroundColor(0x00000000);

		//		if (Build.VERSION.SDK_INT >= 21) {
		//			wv.getSettings().setMixedContentMode(0);
		//		}
		wv.setWebViewClient(new WebViewClient() {			
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
				if (NetworkUtils.isInternetConnected(mActivity)) {
					mDialog.findViewById(R.id.rl).setVisibility(View.VISIBLE);
				}
				// mDialog.findViewById(R.id.rl).startAnimation(AnimationUtils.loadAnimation(mActivity, android.R.anim.fade_in));
				Log.e(DialogPayment.class.getSimpleName(), url);
				if (url.contains("uri_payment") && url.contains("status")) {
					Uri uri = Uri.parse(url);
					if (uri.getQueryParameter("status").equals("success")) {
						mOnPaymentListener.onSuccessful(uri.getQueryParameter("order_id"));
						JSONObject jsonObject = new JSONObject();
						try {
							jsonObject.put("trans_id", uri.getQueryParameter("order_id"));
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						SohaSDK.sendLog(
								MQTTUtils.ACTION_PAYMENT_FINISH, Base64
										.encodeToString(jsonObject
												.toString().getBytes(),
												Base64.DEFAULT));
						SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_FINISH, Base64.encodeToString(jsonObject.toString().getBytes(), Base64.DEFAULT));
						SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_FINISH, "");
						mDialog.dismiss();
					} else {
						JSONObject jsonObject = new JSONObject();
						try {
							jsonObject.put("message", uri.getQueryParameter("message"));
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						SohaSDK.sendLog(
								MQTTUtils.ACTION_PAYMENT_FINISH, Base64
										.encodeToString(jsonObject
												.toString().getBytes(),
												Base64.DEFAULT));
						SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_PAYMENT_FINISH, "");
						SohaSDK.fuckLog(MQTTUtils.ACTION_PAYMENT_FINISH, Base64.encodeToString(jsonObject.toString().getBytes(), Base64.DEFAULT));
						mDialog.dismiss();
						if (!isShowedDialodErr) {
							isShowedDialodErr = true;
							new AlertDialog.Builder(mActivity)
							// .setTitle(R.string.title_dialog_confirm)
							.setMessage(uri.getQueryParameter("message"))
							.setPositiveButton(android.R.string.ok,
									new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog, int which) {

								}
							}).create().show();
						}
					}
				} else if (url.contains("http://close")) {
					mDialog.dismiss();
				}
			}
			@Override
			public void onPageFinished(WebView view, String url) {
				// TODO Auto-generated method stub
				super.onPageFinished(view, url);
			
				mDialog.findViewById(R.id.rl).setVisibility(View.GONE);
				wv.getSettings().setCacheMode(WebSettings.LOAD_DEFAULT);

				wv.loadUrl("javascript:function AppSDKexecute(method, orderInfo) {var orderText = JSON.stringify(orderInfo); PaymentInterface.interactPay(method,orderText);}");
			//	wv.loadUrl(	"javascript:function AppSDKexecute(method) {alert(\"Hehe\"+method);}");
				//wv.loadUrl(	"javascript:function AppSDKexecute(method, orderInfo) {alert(\"Hihi\"+method);}");
				//wv.loadUrl("javascript:function AppSDKexecute(method) {PaymentInterface.interactPay(method);}");

			}

			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				view.loadUrl(url);
				return true;
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
		
		wv.setWebChromeClient(new
				WebChromeClient() {            
			@Override
			public boolean onJsConfirm(WebView view, String url, final String message, final JsResult result) {
				AlertDialog.Builder adb = new AlertDialog.Builder(mActivity)
				// .setTitle(R.string.title_dialog_confirm)
				.setMessage(message)
				.setPositiveButton(android.R.string.ok,
						new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int which) {
						result.confirm();
					}
				}).setNegativeButton(android.R.string.cancel, 
						new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int which) {
						result.cancel();
					}
				});
				adb.setCancelable(false);
				adb.setOnCancelListener(new OnCancelListener() {

					@Override
					public void onCancel(DialogInterface arg0) {
						// TODO Auto-generated method stub
						result.cancel();
					}
				});
				
				adb.create().show();
				return true;
			}
		});

		if (Utils.isUpdateWebview(mActivity)) {
			wv.getSettings().setCacheMode(WebSettings.LOAD_DEFAULT);
		} else {
			wv.getSettings().setCacheMode(WebSettings.LOAD_CACHE_ELSE_NETWORK);
		}
		try {
			wv.loadUrl(API.paymentUrl +"?"+ Utils.getParamsPayment(mActivity));
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

	private void getOrderInfoGooglePay(String orderJSON) {
		try {
			String orderInfo = new JSONObject(orderJSON).getString("package_id");
			if (NetworkUtils.isInternetConnected(mActivity)) {
				if (isEnableButtonPay) {
					vPaymentCreate(orderInfo);
				} else {
					ToastUtils.vToastErrorTryAgain(mActivity);
				}
			} else {
				ToastUtils.vToastErrorNetwork(mActivity);
			}
			Logger.e("orderInfo = " + orderInfo);
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
			ToastUtils.vToastErrorTryAgain(mActivity);
		}
	}
	
	//String demo = "{'status' : 'fail','error_code' : 1002,'message' : 'phien dang nhap het han vui long dang nhap lai!','data' : ''}";
	private boolean isEnableButtonPay = true;
	 JSONObject json;
	private void vPaymentCreate(final String orderInfo) {
		isEnableButtonPay = false;
		DialogUtils.vDialogLoadingShowProcessing(mActivity, true);
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				List<NameValuePair> params = new ArrayList<NameValuePair>();
				params.add(new BasicNameValuePair("signed_request", Utils.postCreateIAP(mActivity, orderInfo)));	
				Log.e("paramCreateIAP", Utils.postCreateIAP(mActivity, orderInfo));
				final JSONObject json = JsonParser.getJSONFromPostUrl(API.paymentIAPCreate, params);
				/* try {
					json = new JSONObject(demo);
				} catch (JSONException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}*/
				mActivity.runOnUiThread(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						DialogUtils.vDialogLoadingDismiss();
						Log.e("final JSON", json + "");
						if (json != null) {
							try {
								String status = json.getString("status");
								JSONObject obj = new JSONObject();
								if (status.equals("success")) {
									String orderId = json.getString("data");
									obj.put("trans_id", orderId);
									obj.put("item_id", orderInfo);
									SohaApplication.getInstance().trackEvent("sdk", MQTTUtils.ACTION_IAP_START, "");
									SohaSDK.fuckLog(MQTTUtils.ACTION_IAP_START, Base64.encodeToString(obj.toString().getBytes(), Base64.DEFAULT));
									SohaSDK.sendLog(MQTTUtils.ACTION_IAP_START, Base64.encodeToString(obj.toString().getBytes(), Base64.DEFAULT));
									vGooglePay(orderInfo, orderId);
									Logger.e("orderId = " + orderId);
									// ToastUtils.vToastShort(mActivity, orderInfo);
								} else {
									
									if(json.getInt("error_code")==1002)
									{
										isShowDashboardAgain =false;
										mDialog.dismiss();
										new DialogLogoutTokenExpried(mActivity, json.getString("message"));
									}
									else
									{
										ToastUtils.vToastLong(mActivity, json.getString("message"));
									}
									
								}
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
								ToastUtils.vToastErrorTryAgain(mActivity);
							}
						} else {
							ToastUtils.vToastErrorTryAgain(mActivity);
						}
						new Handler().postDelayed(new Runnable() {

							@Override
							public void run() {
								// TODO Auto-generated method stub
								isEnableButtonPay = true;
							}
						}, 500);
					}
				});
				
//				String a = "NQf3HwazT7XlDTiojczWrvMIGzu7hkRWmtygazHPSqhwP5HaI70fk3UPk+FiaouwTPgJ3Rhca2XXmm7ht3rSNSneLBuvrvchoVSlcBSfL5NbA4xkSAu4Wq1G/x56ZzvPU4oLDSQzRntsZXXgDWMH1VUBq33rw83+DXP6/fcB/x4=";
//				try {
//					decryptByPublicKey1(a.getBytes(), Utils.publicKey);
//					EncryptorEngine.decryptRSA(a, Utils.publicKey);
//				} catch (Exception e) {
//					// TODO Auto-generated catch block
//					e.printStackTrace();
//				}
//				
				
//				try{
//					Cipher asymmetricCipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");
//
//					// asume, that publicKeyBytes contains a byte array representing
//					// your public key
//					X509EncodedKeySpec publicKeySpec = new X509EncodedKeySpec(Utils.publicKey.getBytes());
//
//					KeyFactory keyFactory;
//					keyFactory = KeyFactory.getInstance(publicKeySpec.getFormat());
//					Key key = keyFactory.generatePublic(publicKeySpec);
//					
//					String a = "PlErJgI0Ql4cH/Ax7yOJIUQMtnZZuiVTur5Y4MtgTWc+QqXFaV29dxx3SXBfCZyjQcKsQMNioGgiOwPNlVG0fMpfsYnkQkip2ySLKkaEZ3gZPOb8yNSFD6k/qmcFE6jnj9c9qSVL4qP+uViKy6oNYU4EbV/oXDi6J8SrptRd1Bw=";
//					
//					// initialize your cipher
//					asymmetricCipher.init(Cipher.DECRYPT_MODE, key);
//					// asuming, cipherText is a byte array containing your encrypted message
//					byte[] plainText = asymmetricCipher.doFinal(a.getBytes());
//					String a1 = new String(plainText);
//					Log.e("__________________sjdh", a1);
//				}catch(Exception e){
//					e.printStackTrace();
//				}
				
			}
		}).start();
		
	}

	private void vGooglePay(String productId, String devPayload) {
		if (SohaSDK.isInAppBillingConnected && SohaSDK.mService != null) {
			try {
				Bundle buyIntentBundle = SohaSDK.mService.getBuyIntent(3, mActivity.getPackageName(), productId, "inapp", devPayload);
				PendingIntent pendingIntent = buyIntentBundle.getParcelable("BUY_INTENT");
				mActivity.startIntentSenderForResult(pendingIntent.getIntentSender(),
						1001, new Intent(), Integer.valueOf(0), Integer.valueOf(0),
						Integer.valueOf(0));
				Logger.e("called Google Play, product = " + productId + ", " + devPayload);
			} catch (Exception e) {
				// TODO Auto-generated catch block
				//				if (TextUtils.isStringNull(TextUtils.getEmail(mActivity))) {
				//					ToastUtils.vToastLong(mActivity, mActivity.getString(R.string.toastErrorNotLoginGoogle));
				//				} else {
				//					ToastUtils.vToastErrorTryAgain(mActivity);
				//				}
				ToastUtils.vToastErrorTryAgain(mActivity);
				e.printStackTrace();
			}
		} else {
			ToastUtils.vToastErrorTryAgain(mActivity);
		}
	}

	public interface OnPaymentListener {
		public void onSuccessful(String orderId);
		public void onDismiss();
	}

	Handler h =new Handler();
	// click vao item
	private class PaymentJavaScriptInterface {
	/*	@JavascriptInterface
		public void sohapay(final String orderText) {
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					getOrderInfoGooglePay(orderText);
				}
			});
		}*/
		@JavascriptInterface
		public void interactPay(final String action,final String orderText) {
			mActivity.runOnUiThread(new Runnable() {
				@Override
				public void run() {
					// TODO Auto-generated method stub
					
					if(action.equalsIgnoreCase("logout"))
					{
						h.post(new Runnable() {
							
							@Override
							public void run() {
								// TODO Auto-generated method stub
								mDialog.dismiss();
								isShowDashboardAgain =false;
								SohaSDK.logoutFromServer();
							}
						});
						
					}
					if(action.equalsIgnoreCase("connect_account"))
					{
						DialogLoginForConnectNew connect = new DialogLoginForConnectNew(mActivity);
					}
					//Toast.makeText(mActivity,action , 1000).show();
					if(action.equalsIgnoreCase("iappay"))
					{
						getOrderInfoGooglePay(orderText);
						//Toast.makeText(mActivity, orderText, 1000).show();
					}
					if(action.equalsIgnoreCase("close_popup"))
					{
						
						mDialog.dismiss();
					}
					if(action.equalsIgnoreCase("onclick_back"))
					{
						int result = Integer.parseInt(orderText);
						if(result==0)
						{
							mDialog.dismiss();
						}
					}
					
				}
			});
		}
	
	}

	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
		AnimationUtils.vAnimationClick(v);

	}
	
	//------------------- decrypt RSA ------------------------------
	public static byte[] decryptBASE64(String key) throws Exception {
        return Base64.decode(key, Base64.DEFAULT);
    }
	
	public static byte[] decryptByPublicKey1(byte[] data, String key)
            throws Exception {
 
        byte[] keyBytes = key.getBytes();  //decryptBASE64(key);
 
        X509EncodedKeySpec x509KeySpec = new X509EncodedKeySpec(keyBytes);
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        PublicKey publicKey = keyFactory.generatePublic(x509KeySpec);
 
        Cipher cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding");
        cipher.init(Cipher.DECRYPT_MODE, publicKey);
        Log.e("____________________aaa", ""+new String (cipher.doFinal(data)));
        return cipher.doFinal(data);
    }
	
	//----------------------
	public static byte[] decryptByPublicKey(byte[] encryptedData, String publicKey)  
            throws Exception {  
        byte[] keyBytes = decryptBASE64(publicKey);
        X509EncodedKeySpec x509KeySpec = new X509EncodedKeySpec(keyBytes);  
        KeyFactory keyFactory = KeyFactory.getInstance("RSA");  
        PublicKey publicK = keyFactory.generatePublic(x509KeySpec);  
//        Cipher cipher = Cipher.getInstance(keyFactory.getAlgorithm()); 
        Cipher cipher = Cipher.getInstance("RSA/ECB/PKCS1Padding"); //RSA/ECB/PKCS1Padding
        cipher.init(Cipher.DECRYPT_MODE, publicK);  
        int inputLen = encryptedData.length;  
        ByteArrayOutputStream out = new ByteArrayOutputStream();  
        int offSet = 0;  
        byte[] cache;  
        int i = 0;  
        while (inputLen - offSet > 0) {  
            if (inputLen - offSet > MAX_DECRYPT_BLOCK) {  
                cache = cipher.doFinal(encryptedData, offSet, MAX_DECRYPT_BLOCK);  
            } else {  
                cache = cipher.doFinal(encryptedData, offSet, inputLen - offSet);  
            }  
            out.write(cache, 0, cache.length);  
            i++;  
            offSet = i * MAX_DECRYPT_BLOCK;  
        }  
        byte[] decryptedData = out.toByteArray();  
        out.close();  
        Log.e("Diep_________ML", "= "+ new String(decryptedData));
        return cipher.doFinal(decryptedData);  
    }  
	public static String bcd2Str(byte[] bytes) {
	    char temp[] = new char[bytes.length * 2], val;

	    for (int i = 0; i < bytes.length; i++) {
	        val = (char) (((bytes[i] & 0xf0) >> 4) & 0x0f);
	        temp[i * 2] = (char) (val > 9 ? val + 'A' - 10 : val + '0');

	        val = (char) (bytes[i] & 0x0f);
	        temp[i * 2 + 1] = (char) (val > 9 ? val + 'A' - 10 : val + '0');
	    }
	    return new String(temp);
	}
	
}
