package com.usdk.plugin;


import java.util.HashMap;
import android.os.Build;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.content.pm.PackageManager;
import android.Manifest;
import android.view.View;
import android.widget.Toast;

import com.chuxin.game.SGGameProxy;
import com.chuxin.game.interf.SGCommonResult;
import com.chuxin.game.interf.SGExitCallbackInf;
import com.chuxin.game.interf.SGPayCallBackInf;
import com.chuxin.game.interf.SGReportDataBackInf;
import com.chuxin.game.interf.SGRoleOptCallBackInf;
import com.chuxin.game.interf.SGUpdateCallBackInf;
import com.chuxin.game.interf.SGUserListenerInf;
import com.chuxin.game.model.SGConst;
import com.chuxin.game.model.SGGameConfig;
import com.chuxin.game.model.SGResult;
import com.chuxin.game.model.UpdateInfo;
import com.chuxin.game.utils.SGResourceUtil;
import com.chuxin.sdk.utils.SGToast;
import  com.chuxin.sdk.utils.ChuXinAppsFlyer;
import com.usdk.platform.adapter.IPlatformCallBack;
import com.usdk.platform.adapter.PlatformCallback;
import com.usdk.platform.adapter.PlatformProxyBase;
import com.usdk.platform.adapter.SdkPayInfo;
import com.usdk.platform.adapter.SdkRoleInfo;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.KeyEvent;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;

public class PlatformProxy extends PlatformProxyBase {

	private String mToken;
	
	private String roleName = "";
	private String roleLevel = "";
	private String roleId = "";
	
	private boolean logined = false;
	private boolean isCreateRole = false;
	
	private String m_productId = "";
	private String m_signKey = "";
	
	@Override
	public void OnCreate(Activity activity, Bundle savedInstanceState) {
		super.onCreate(activity, savedInstanceState);

		/**
		 * Android 6.0+ 鐢宠READ_PHONE_STATE鏉冮檺锛堟鏉冮檺蹇呴』鐢宠锛屾湁鐨勬墜鏈洪粯璁ゆ槸鎷掔粷杩欎釜鏉冮檺锛屽彲鑳戒細瀵艰嚧娓告垙鏃犳硶鎵撳紑锛�
		 */
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
			int checkCallPhonePermission = ContextCompat.checkSelfPermission(mActivity, Manifest.permission.READ_PHONE_STATE);
			if (checkCallPhonePermission != PackageManager.PERMISSION_GRANTED) {
				ActivityCompat.requestPermissions(mActivity, new String[]{Manifest.permission.READ_PHONE_STATE}, 1);
			} else {
				initMethod();
			}
		} else {
			initMethod();
		}
	}

	private void initMethod() {
		SGGameConfig config = new SGGameConfig();

		Boolean isDebug = Boolean.parseBoolean( getConfig("isDebug") );
		if(isDebug)
		{
			// 璁剧疆debug妯″紡
			config.setDebugState(true);
			// 鈥渢est鈥濊〃绀烘祴璇曟湇
			config.setLocation("test");
		}
		else
		{
			// 鍙戝竷鏃惰璁剧疆涓篺alse
			config.setDebugState(false);
			// "cn"琛ㄧず姝ｅ紡鏈嶏紝鍙戝竷鏃堕渶璁剧疆涓衡�渃n鈥�
			config.setLocation("cn");
		}

		m_productId = getConfig("cxhd_productId");
		m_signKey = getConfig("cxhd_signKey");
		debug("*** sdk鍙傛暟 productId="+m_productId+";signKey="+m_signKey);
		// 璁剧疆妯珫灞�
		config.setorientation(SGConst.ORIENTATION_LANDSCAPE);
		// 涓嬮潰涓や釜鍙傛暟浠巗andglass骞冲彴鐢宠鍙栧緱
		config.setProductId(m_productId);
		config.setSignKey(m_signKey);

		SGGameProxy.instance().initWithConfig(mActivity, config, new SGCommonResult() {
			@Override
			public void onComplete(SGResult result, Bundle arg1) {
				if(null != result){
					debug("鍒濆鍖栬繑鍥�: code="+result.getCode()+", msg="+result.getMsg());
				}
				if (result != null && result.isOK()) {
					//鍒濆鍖栨垚鍔�
					debug("*** 鍒濆鍖栨垚鍔�");
					platformCallBack.initSDKCallBack(
							IPlatformCallBack.ErrorCode.InitSuccess.ordinal(),
							"");
				}
				else
				{
					debug("*** 鍒濆鍖栧け璐�");
					platformCallBack.initSDKCallBack(
							IPlatformCallBack.ErrorCode.InitFail.ordinal(),
							"");
				}
			}
		});
		SGGameProxy.instance().setUserListener(mActivity, userListener);
		SGGameProxy.instance().applicationInit(mActivity);
		SGGameProxy.instance().onCreate(mActivity);
	}

	private SGUserListenerInf userListener = new SGUserListenerInf() {

		@Override
		public void onLogin(SGResult ret) {
			debug("*** Login Result:" + ret.getMsgLocal());
			if (ret.isOK()) {
				debug("*** Login success!!");
				logined = true;
				//鎷垮埌杩欎釜data鏁版嵁锛坖son鏍煎紡鐨剆tring锛夛紝鍘熷皝涓嶅姩鍙戝線娓告垙鏈嶅姟鍣紝浠庤�岃繘琛岃韩浠芥牎楠岀瓑鎿嶄綔銆�
				String data = ret.getMsg();
				System.err.println("login data = " + data);
				//浼爌roductId鍒版湇鍔″櫒鍖哄垎浜у搧
				mToken = String.format("data@%s&key@%s&signKey@%s", data,m_productId,m_signKey);
				mTokens.clear();
				mTokens.add(data);
				mTokens.add(m_productId);
				mTokens.add(m_signKey);
				debug("mToken: "+mToken);
				
				platformCallBack.loginCallBack(
						IPlatformCallBack.ErrorCode.LoginSuccess.ordinal(),
						"");
			}
			else{
				debug("*** login fail");
				
				platformCallBack.loginCallBack(
						IPlatformCallBack.ErrorCode.LoginFail.ordinal(),
						"");
			}
		}

		@Override
		public void onLogout(SGResult ret) {
			logined = false;
			if (ret.isOK()) {
				debug("*** logout success!");
				platformCallBack.logoutCallBack(
						IPlatformCallBack.ErrorCode.LogoutFinish.ordinal(),
						"");
			} else {
				debug("*** logout failed!");
				platformCallBack.logoutCallBack(
						IPlatformCallBack.ErrorCode.LogoutFinish.ordinal(),
						"");
			}
		}
	};
	
	// 璁剧疆uid, 杩欎釜uid鐢辨父鎴忔湇鍔″櫒杩斿洖
	void setUid(String uid){
		debug("setUid() uid="+uid);
		// 涓嬮潰鐨勮繖娈典唬鐮佽皟鐢ㄦ椂鏈烘槸鍦ㄦ父鎴忔湇杩斿洖鐧诲綍鎴愬姛鍚�
		// 璇id涓轰粠娓告垙鏈嶅姟鍣ㄧ櫥褰曢獙璇佹垚鍔熷悗锛岄簾娓稿钩鍙拌繑鍥炵殑鐢ㄦ埛鍞竴鏍囪瘑
		SGGameProxy.instance().setUid(uid);
		//uname 鏆傛椂鍐欐 鐢╬roductId鏇夸唬
		SGGameProxy.instance().setUName(m_productId);
	}
	
	@Override
	public void login(String custom_params_) {
		super.login(custom_params_);

		debug("*** login()");
		
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				SGGameProxy.instance().login(mActivity, null);
			}
		});
	}
	
	@Override
	public void openUserCenter(String custom_params_) {
		super.switchAccount(custom_params_);
		debug("*** openUserCenter()");
		
		mActivity.runOnUiThread(new Runnable() {
			public void run() {
				if (SGGameProxy.instance().isHasUserCenter()) {
					SGGameProxy.instance().userCenter(mActivity);
				} else {
					SGToast.showMessage(mActivity, "鏆備笉鏀寔鐢ㄦ埛涓績");
				}
			}
		});	
	}
	
	public void logout(String custom_params_) {
		super.logout(custom_params_);
		debug("*** logout()");
		logined = false;
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				SGGameProxy.instance().logout(mActivity, null);
			}
		});
	}
	
	// 鍒囨崲璐﹀彿
	public void switchAccount(String custom_params_)
	{
		super.switchAccount(custom_params_);
		debug("*** switchAccount()");
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				if (SGGameProxy.instance().isSupportChangeAccount(mActivity, null)) {
					SGGameProxy.instance().changeAccount(mActivity, null);
				} else {
					//SGToast.showMessage(MainActivity.this, "鏆備笉鏀寔鍒囨崲璐﹀彿鍔熻兘锛岃璋僱ogout 鎺ュ彛");
					logout("");
				}
			}
		});
	}
	
	@Override
	public void OnBackPressed() {
		super.onBackPressed();
		debug("*** onBackPressed()");
		
		exit("");
	}
	
	  @Override
	  public boolean OnKeyDown(int keyCode, KeyEvent event)  {
	      if (keyCode == KeyEvent.KEYCODE_BACK) {
	    	  debug("*** onKeyDown + onBackPressed()");
	          exit("");
	      return true;
	      }
	      return super.onKeyDown(keyCode, event);
	  }
	
	@Override
	public void exit(String custom_params_) {
		super.exit(custom_params_);
		debug("*** exit()");
		
		mActivity.runOnUiThread(new Runnable(){
			@Override
			public void run() {
				SGGameProxy.instance().exit(mActivity, new SGExitCallbackInf() {

					@Override
					public void onNo3rdExiterProvide() {
						// 娓告垙鑷繁鐨勯��鍑哄脊妗�
						debug("*** exit() + onNo3rdExiterProvide()");
						exitGameDialog();
					}

					@Override
					public void onExit() {
						// 娣诲姞鏃犲脊妗嗛��鍑洪�昏緫
						debug("*** exit() + onExit()");
						if("" != roleName)
							reportOptData(roleName, roleId, roleLevel, "2");
						
						SGGameProxy.instance().hideFloatMenu(mActivity);
						SGGameProxy.instance().onStop(mActivity);
						SGGameProxy.instance().onDestroy(mActivity);
						
						android.os.Process.killProcess(android.os.Process.myPid());
					}
				});
			}
		});
	}
	
	
	//閫�鍑烘父鎴忕‘璁ゅ璇濇
	public void exitGameDialog()
	{
		AlertDialog.Builder builder = new AlertDialog.Builder(mActivity);
		builder.setTitle("鎻愮ず");
		builder.setMessage("鏄惁瑕侀��鍑烘父鎴忥紵");
		builder.setPositiveButton("纭畾", new OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.dismiss();
				
				if("" != roleName)
					reportOptData(roleName, roleId, roleLevel, "2");
				
				SGGameProxy.instance().hideFloatMenu(mActivity);
				SGGameProxy.instance().onStop(mActivity);
				SGGameProxy.instance().onDestroy(mActivity);
				
				android.os.Process.killProcess(android.os.Process.myPid());
			}
		});
		
		builder.setNegativeButton("鍙栨秷", new OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.dismiss();
			}
		});
		
		builder.create().show();
	}
	
	@Override
	public void pay(String pay_info_) {
		super.pay(pay_info_);
		debug("*** pay() + pay_info_ = " + pay_info_);
		
		Boolean isPayFixed = true;//榛樿鍙惎鐢ㄥ畾棰濇敮浠楶ayFixed

		
		if(isPayFixed)
		{
			PayFixed(pay_info_);
		}
		else
		{
			PayUnFixed(pay_info_);
		}
	}
	
	public void PayFixed(String pay_info_) {
		debug("*** linyou PayFixed()");
		final SdkPayInfo payInfo = new SdkPayInfo(pay_info_);
		
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				SGGameProxy.instance().payFixed(mActivity, payInfo.productName, payInfo.productId, payInfo.payAmount * 100, payInfo.productQuantity, payInfo.customInfo,
						new SGPayCallBackInf() {
							@Override
							public void onPay(SGResult result) {
								if (result.isOK()) {
									debug("*** linyou pay successed");
									
									platformCallBack.payCallBack(
											IPlatformCallBack.ErrorCode.PaySuccess.ordinal(),
		     								"");
								} else {
									debug("*** linyou pay fail:" + result.getMsg());
									platformCallBack.payCallBack(
											IPlatformCallBack.ErrorCode.PayFail.ordinal(),
		     								"");
								}
							}
						});
			}
		});
		

	}

	public void PayUnFixed(String pay_info_) {
		debug("*** linyou PayUnFixed()");
		final SdkPayInfo payInfo = new SdkPayInfo(pay_info_);
		
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				SGGameProxy.instance().payUnFixed(mActivity, payInfo.productName, payInfo.productId, payInfo.payAmount * 100, payInfo.productQuantity, payInfo.customInfo,
						new SGPayCallBackInf() {

							@Override
							public void onPay(SGResult result) {
								if (result.isOK()) {
									debug("*** linyou pay successed");
									
									platformCallBack.payCallBack(
											IPlatformCallBack.ErrorCode.PaySuccess.ordinal(),
		     								"");
								} else {
									debug("*** linyou pay fail:" + result.getMsg());
									platformCallBack.payCallBack(
											IPlatformCallBack.ErrorCode.PayFail.ordinal(),
		     								"");
								}
							}
						});
			}
		});
	}

	public void playerLogin(String role_info_) {
		debug("*** playerLogin()");
		
		SdkRoleInfo roleInfo = new SdkRoleInfo(role_info_);
		setUid(roleInfo.uid);
		
		if(isCreateRole)
		{
			createRole(role_info_);
			isCreateRole = false;
		}
		
		roleName = roleInfo.roleName;
		roleLevel = roleInfo.roleLevel;
		roleId = roleInfo.roleId;
		reportOptData(roleInfo.roleName,roleInfo.roleId,roleInfo.roleLevel,"1");
		
		submitExtendData(roleInfo.roleId, roleInfo.roleName, roleInfo.roleLevel, roleInfo.serverId, roleInfo.serverName, roleInfo.roleCreateTime);

	}
	
	public void playerLogout(String role_info_)
	{
		debug("*** playerLogout()");
		logined = false;
		SdkRoleInfo roleInfo = new SdkRoleInfo(role_info_);
		reportOptData(roleInfo.roleName,roleInfo.roleId,roleInfo.roleLevel,"2");
	}

	public void playerCreated(String role_info_) {
		debug("*** playerCreated()");
		
		isCreateRole = true;
	}
	
	void createRole(String role_info_)
	{
		final SdkRoleInfo roleInfo = new SdkRoleInfo(role_info_);
		roleName = roleInfo.roleName;
		roleLevel = roleInfo.roleLevel;
		roleId = roleInfo.roleId;

		reportOptData(roleInfo.roleName,roleInfo.roleId,roleInfo.roleLevel,"4");


		SGGameProxy.instance().createRole(mActivity, roleInfo.roleName, "", "", new SGRoleOptCallBackInf() {
			@Override
			public void onUpgrade(SGResult result) {
			}

			@Override
			public void onCreate(SGResult result) {
				if (result.isOK()) {
					debug("*** playerCreated() + 瑙掕壊鍒涘缓鎴愬姛");
				}
			}
		});
	}
	
	public void playerLevelup (String role_info_) {
		debug("*** playerLevelup()");
		
		final SdkRoleInfo roleInfo = new SdkRoleInfo(role_info_);
		roleName = roleInfo.roleName;
		roleLevel = roleInfo.roleLevel;
		roleId = roleInfo.roleId;
		reportOptData(roleInfo.roleName,roleInfo.roleId,roleInfo.roleLevel,"3");
		
		mActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				SGGameProxy.instance().upgradeRole(mActivity, roleInfo.roleName, roleInfo.roleLevel, "", "",
						new SGRoleOptCallBackInf() {

							@Override
							public void onUpgrade(SGResult result) {
								if (result.isOK()) {
									debug("*** SGGameProxy.instance().upgradeRole 瑙掕壊鍗囩骇鎴愬姛");
								}
							}

							@Override
							public void onCreate(SGResult result) {
							}
				});
			}
		});	
	}
	
	// 瑙掕壊鐧诲綍鎴愬姛鍚庝笂鎶ユ暟鎹�
	void submitExtendData(String roleId, String roleName, String roleLevel, String zoneId, String zoneName, String roleCreateTime)
	{
		debug("submitExtendData() roleId="+roleId+", roleName="+roleName+", roleLevel="+roleLevel+", zoneId="+zoneId+", zoneName="+zoneName+", roleCreateTime="+roleCreateTime);
		this.roleId = roleId;
		this.roleName = roleName;
		this.roleLevel = roleLevel;
		// 娓告垙鐧诲綍鎴愬姛鍚庯紝鎷垮埌瑙掕壊鍜屽尯鏈嶇殑鐩稿叧淇℃伅鏃讹紝瑕佽皟鐢⊿DK 灏嗘淇℃伅涓婃姤锛屽瓧娈电殑key
		// 蹇呴』鎸夌収濡備笅鏍煎紡鏉ワ紝瀛楁鐨勫�间笉鑳戒负绌�
		HashMap<String, String> map = new HashMap<String, String>();
		//瑙掕壊id
		map.put("roleId", roleId);
		//瑙掕壊鍚嶇О
		map.put("roleName", roleName);
		//瑙掕壊绛夌骇
		map.put("roleLevel", roleLevel);
		// 鏈嶅姟鍣↖D锛屼粠s1---s10000(寤鸿鎸変竴瀹氶『搴忛�掑)锛屽繀椤诲拰娓告垙鍖烘湇涓�涓�瀵瑰簲
		map.put("zoneId", zoneId);
		// 鏈嶅姟鍣ㄥ悕绉帮紝濡傛灉涓虹┖鍒欎紶鍏モ�溾�濆瓧绗︿覆(灏介噺涓嶄紶绌哄瓧绗︿覆)
		map.put("zoneName", zoneName);
		map.put("roleCreateTime", roleCreateTime);
		SGGameProxy.instance().submitExtendData(mActivity, map);
	}
	
	// 鏁版嵁涓婃姤
	void reportOptData(String roleName, String roleId, String level, final String type)
	{
		// type绫诲瀷锛�1-鍚姩锛�2-閫�鍑猴紝3-瑙掕壊鍗囩骇锛�4-鍒涘缓瑙掕壊锛�
		SGGameProxy.instance().reportOptData(mActivity, roleName, roleId, level, type, "", "", new SGReportDataBackInf() {

			@Override
			public void onCallBack(SGResult result) {
				if (result.isOK()) {
					debug("*** SGGameProxy.instance().reportOptData 鏁版嵁涓婃姤鎴愬姛");
				}
			}
		});
	}
	
	//desolate 搴熷純
	void update() 
	{
		SGGameProxy.instance().checkVersionUpdate(mActivity, new SGUpdateCallBackInf() {

			@Override
			public void onResult(UpdateInfo info) {
				// 闇�瑕佸崌绾�
				if (info != null && info.isUpdate()) {
					// 涓嬭浇娓告垙浠ュ強瀹夎锛岃繖閲岄渶瑕佹父鎴忔柟鑷澶勭悊
				}
			}
		});
	}
	
	public void OpenVPlus()
    {
    	debug("OpenVPlus");
    	//SGGameProxy.instance().performFeature(mActivity, "vplayer", null);   
    }
	
	@Override
	public void OnActivityResult(int requestCode, int resultCode, Intent data) {
		super.onActivityResult(requestCode, resultCode, data);
		SGGameProxy.instance().onActivityResult(mActivity, requestCode, resultCode, data);
	}

	@Override
	public void OnResume() {
		SGGameProxy.instance().onResume(mActivity);
		super.onResume();
	}

	@Override
	public void OnRestart() {
		SGGameProxy.instance().onRestart(mActivity);
		super.onRestart();
	}

	@Override
	public void OnStart() {
		SGGameProxy.instance().onStart(mActivity);
		super.onStart();
	}

	@Override
	public void OnPause() {
		SGGameProxy.instance().onPause(mActivity);
		super.onPause();
	}

	@Override
	public void OnNewIntent(Intent intent) {
		SGGameProxy.instance().onNewIntent(intent);
		super.onNewIntent(intent);
	}

	@Override
	public void OnStop() {
		SGGameProxy.instance().onStop(mActivity);
		super.onStop();
	}
	
	@Override
	public void OnDestroy() {
		SGGameProxy.instance().onDestroy(mActivity);
		super.onDestroy();
	}
	
	@Override
	public void OnRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
        debug("onRequestPermissionsResult + " + requestCode);
		switch (requestCode) {
			case 1:
				if (grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                    debug("onRequestPermissionsResult + " + "PERMISSION_GRANTED");
					initMethod();
				} else {
					// Permission Denied
					Toast.makeText(mActivity, "鎺堟潈澶辫触", Toast.LENGTH_SHORT).show();
					finish();
				}
				break;
			default:
				super.onRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
