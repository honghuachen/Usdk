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
import com.usdk.platform.adapter.IPlatformCallBack;
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
						platformCallBack
								.exitGameCallBack(IPlatformCallBack.ErrorCode.ExitSuccess
										.ordinal());
					}
				});

			}
		});
	}

	@Override
	public void logout(String custom_params_) {
		BDGameSDK.logout();
		uid = "";
		platformCallBack.logoutCallBack(
				IPlatformCallBack.ErrorCode.LogoutFinish.ordinal(), "logout");
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
							platformCallBack.loginCallBack(
									IPlatformCallBack.ErrorCode.LoginSuccess
											.ordinal(), resultDesc);
							break;
						case ResultCode.LOGIN_CANCEL:
							platformCallBack.loginCallBack(
									IPlatformCallBack.ErrorCode.LoginCancel
											.ordinal(), resultDesc);
							break;
						case ResultCode.LOGIN_FAIL:
						default:
							uid = "";
							platformCallBack.loginCallBack(
									IPlatformCallBack.ErrorCode.LoginFail
											.ordinal(), resultDesc);
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
							platformCallBack.payCallBack(
									IPlatformCallBack.ErrorCode.PaySuccess
											.ordinal(), resultDesc);
							break;
						case ResultCode.PAY_CANCEL:
							platformCallBack.payCallBack(
									IPlatformCallBack.ErrorCode.PayCancel
											.ordinal(), resultDesc);
							break;
						case ResultCode.PAY_FAIL: 
							platformCallBack.payCallBack(
									IPlatformCallBack.ErrorCode.PayFail
											.ordinal(), resultDesc);
							break;
						case ResultCode.PAY_SUBMIT_ORDER:
						default:
							platformCallBack.payCallBack(
									IPlatformCallBack.ErrorCode.PayOthers
											.ordinal(), resultDesc);
							break;
						}
					}
				});
	}

	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		super.onCreate(activity, savedInstanceState);

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
					platformCallBack.initSDKCallBack(
							IPlatformCallBack.ErrorCode.InitSuccess.ordinal(),
							resultDesc);
					break;
				case ResultCode.INIT_FAIL:
				default:
					platformCallBack.initSDKCallBack(
							IPlatformCallBack.ErrorCode.InitFail.ordinal(),
							resultDesc);
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
			
					platformCallBack.logoutCallBack(
							IPlatformCallBack.ErrorCode.LogoutFinish.ordinal(),
							resultDesc);
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