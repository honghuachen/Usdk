package com.usdk.plugin;

import android.app.Activity;
import android.os.Bundle;
import com.baidu.gamesdk.BDGameSDK;
import com.baidu.gamesdk.BDGameSDKSetting;
import com.baidu.gamesdk.BDGameSDKSetting.Domain;
import com.baidu.gamesdk.BDGameSDKSetting.Orientation;
import com.baidu.gamesdk.IResponse;
import com.baidu.gamesdk.OnGameExitListener;
import com.baidu.gamesdk.ResultCode;
import com.baidu.platformsdk.PayOrderInfo;
import com.usdk.platform.adapter.PlatformProxyBase;
import com.usdk.platform.adapter.SdkPayInfo;

public class PlatformProxy extends PlatformProxyBase {
	private String mtoken;
	private String uid;

	public String getToken() {
		return mtoken;
	}

	@Override
	public void exit(String custom_params_) {
		this.mActivity.runOnUiThread(new Runnable() {
			public void run() {
				BDGameSDK.gameExit(mActivity, new OnGameExitListener() {

					@Override
					public void onGameExit() {
						sendCallBack2Unity(UsdkCallBackErrorCode.ExitSuccess);
					}
				});

			}
		});		
	}

	@Override
	public void logout(String custom_params_) {
		BDGameSDK.logout();
		uid = "";
		this.sendCallBack2Unity(UsdkCallBackErrorCode.LogoutFinish,"logout");
	}

	@Override
	public void login(String custom_params_) {
		this.mActivity.runOnUiThread(new Runnable() {
			public void run() {
				BDGameSDK.login(new IResponse<Void>() {
					@Override
					public void onResponse(int resultCode, String resultDesc,
							Void extraData) {
						switch (resultCode) {
						case ResultCode.LOGIN_SUCCESS:
							uid = BDGameSDK.getLoginUid();
							String arg0 = "arg0@"
									+ BDGameSDK.getLoginAccessToken();
							String arg1 = "arg1@" + BDGameSDK.getLoginUid();
							mtoken = arg0 + "&" + arg1;

							BDGameSDK.showFloatView(mActivity);
							sendCallBack2Unity(UsdkCallBackErrorCode.LoginSuccess,resultDesc);
							break;
						case ResultCode.LOGIN_CANCEL:
							sendCallBack2Unity(UsdkCallBackErrorCode.LoginCancel,resultDesc);
							break;
						case ResultCode.LOGIN_FAIL:
						default:
							uid = "";
							sendCallBack2Unity(UsdkCallBackErrorCode.LoginFail,resultDesc);
							break;
						}
					}
				});
			}
		});
	}

	@Override
	public void pay(String pay_info_) {
		SdkPayInfo payInfo = new SdkPayInfo(pay_info_);

		if (!BDGameSDK.isLogined())
			return;

		PayOrderInfo payOrderInfo = new PayOrderInfo();
		payOrderInfo.setCooperatorOrderSerial(payInfo.gameTradeNo);
		payOrderInfo.setProductName(payInfo.productName);
		payOrderInfo.setTotalPriceCent(payInfo.payAmount * 100); 
		payOrderInfo.setRatio(payInfo.productUnitPrice*100);
		payOrderInfo.setExtInfo(payInfo.productId); 
		payOrderInfo.setCpUid(uid); 

		BDGameSDK.pay(payOrderInfo, payInfo.gameCallBackURL,
				new IResponse<PayOrderInfo>() {

					@Override
					public void onResponse(int resultCode, String resultDesc,
							PayOrderInfo extraData) {
						switch (resultCode) {
						case ResultCode.PAY_SUCCESS:
							sendCallBack2Unity(UsdkCallBackErrorCode.PaySuccess,resultDesc);
							break;
						case ResultCode.PAY_CANCEL:
							sendCallBack2Unity(UsdkCallBackErrorCode.PayCancel,resultDesc);
							break;
						case ResultCode.PAY_FAIL: 
							sendCallBack2Unity(UsdkCallBackErrorCode.PayFail,resultDesc);
							break;
						case ResultCode.PAY_SUBMIT_ORDER:
						default:
							sendCallBack2Unity(UsdkCallBackErrorCode.PayOthers,resultDesc);
							break;
						}
					}
				});
	}

	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.OnCreate(activity, savedInstanceState);
		
		BDGameSDKSetting mBDGameSDKSetting = new BDGameSDKSetting();
		mBDGameSDKSetting.setAppID(Integer.parseInt(this.getConfig("AppID"))); 
		mBDGameSDKSetting.setAppKey(this.getConfig("AppKey"));
		mBDGameSDKSetting.setDomain(Domain.RELEASE); 
		mBDGameSDKSetting.setOrientation(Orientation.LANDSCAPE);
		BDGameSDK.init(activity, mBDGameSDKSetting, new IResponse<Void>() {
			@Override
			public void onResponse(int resultCode, String resultDesc,
					Void extraData) {
				switch (resultCode) {
				case ResultCode.INIT_SUCCESS:
					sendCallBack2Unity(UsdkCallBackErrorCode.InitSuccess,resultDesc);
					break;
				case ResultCode.INIT_FAIL:
				default:
					sendCallBack2Unity(UsdkCallBackErrorCode.InitFail,resultDesc);
					break;
				}
			}
		});

		BDGameSDK.getAnnouncementInfo(activity);

		BDGameSDK.setSessionInvalidListener(new IResponse<Void>() {

			@Override
			public void onResponse(int resultCode, String resultDesc,
					Void extraData) {
				if (resultCode == ResultCode.SESSION_INVALID) {
					login("");
				}
			}
		});

		BDGameSDK.setSuspendWindowChangeAccountListener(new IResponse<Void>() {

			@Override
			public void onResponse(int resultCode, String resultDesc,
					Void extraData) {
				switch (resultCode) {
				case ResultCode.LOGIN_SUCCESS:
					uid = BDGameSDK.getLoginUid();
					String arg0 = "arg0@" + BDGameSDK.getLoginAccessToken();
					String arg1 = "arg1@" + BDGameSDK.getLoginUid();
					mtoken = arg0 + "&" + arg1;
			
					sendCallBack2Unity(UsdkCallBackErrorCode.LogoutFinish,resultDesc);
					break;
				case ResultCode.LOGIN_CANCEL:
					break;
				case ResultCode.LOGIN_FAIL:
				default:
					break;
				}
			}
		});
	}
	
	@Override
	public void OnFinish() {
		BDGameSDK.closeFloatView(mActivity);
	}

	@SuppressWarnings("deprecation")
	@Override
	public void OnDestroy() {
		BDGameSDK.destroy();
	}

	@Override
	public void OnPause() {
		BDGameSDK.onPause(mActivity);
	}

	@Override
	public void OnResume() {
		BDGameSDK.onResume(mActivity);
	}
}