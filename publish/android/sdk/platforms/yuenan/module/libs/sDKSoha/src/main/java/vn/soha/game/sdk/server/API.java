package vn.soha.game.sdk.server;

import vn.soha.game.sdk.utils.NameSpace;
import vn.soha.game.sdk.utils.TextUtils;
import vn.soha.game.sdk.utils.Utils;
import android.content.Context;
import android.util.Base64;

import com.facebook.AccessToken;

/**
 * @since January 27, 2015
 * @author Sohagame SDK Team
 *
 */
public class API {


	public static String paymentIAPCreate = "https://" + NameSpace.DOMAIN + "/api/POST/pay/create";//"http://" + NameSpace.DOMAIN + "/api/POST/pay/create";
	public static String paymentIAPVerify = "https://" + NameSpace.DOMAIN + "/api/POST/pay/appstore";//"http://" + NameSpace.DOMAIN + "/api/POST/pay/appstore";

	//ducnm start
//	public static String logGCM = "https://" + NameSpace.DOMAIN + "/api/POST/Push/RegisterDevice";//"http://" + NameSpace.DOMAIN + "/api/post/push/RegisterDevice";
//	public static String getAppInfo = "http://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";
//	public static String login = "https://" + NameSpace.DOMAIN + "/dialog/webview/login?";//"https://" + NameSpace.DOMAIN + "/webapp_client/login?"
//	public static String loginBig4 ="https://" + NameSpace.DOMAIN + "/api/GET/Auth/LoginBig4";//"http://" + NameSpace.DOMAIN + "/api/GET/auth/loginbig4
//	public static String getUserInfo ="http://" + NameSpace.DOMAIN + "/api/GET/Me/Userinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/me/userinfo"
//	public static String paymentUrl = "https://" + NameSpace.DOMAIN + "/dialog/webview";
//	public static String getDashboardConfig ="https://" + NameSpace.DOMAIN + "/api/POST/App/Dashboard";
//	public static String logPlayer = "https://" + NameSpace.DOMAIN + "/api/GET/Mobile/LogPlayUser";
	
	public static String logGCM = "https://" + NameSpace.DOMAIN + "/api/POST/Push/RegisterDevice";//"http://" + NameSpace.DOMAIN + "/api/post/push/RegisterDevice";
	public static String getAppInfo = "https://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";
	public static String login = "https://" + NameSpace.DOMAIN + "/dialog/webview/login?";//"https://" + NameSpace.DOMAIN + "/webapp_client/login?"
	public static String loginBig4 ="https://" + NameSpace.DOMAIN + "/api/GET/Auth/LoginBig4";//"http://" + NameSpace.DOMAIN + "/api/GET/auth/loginbig4
	public static String getUserInfo ="https://" + NameSpace.DOMAIN + "/api/GET/Me/Userinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/me/userinfo"
	public static String paymentUrl = "https://" + NameSpace.DOMAIN + "/dialog/webview";
	public static String getDashboardConfig ="https://" + NameSpace.DOMAIN +"/api/POST/App/DB"; //"/api/POST/App/Dashboard";
	public static String logPlayer = "https://" + NameSpace.DOMAIN + "/api/GET/Mobile/LogPlayUser";
	public static String linkGuidePermission = "https://"+ NameSpace.DOMAIN +"/dialog/webview/DashboardGuide";
	//https://soap.soha.vn/api/POST/App/DB
	//ducnm end



	// load web
    public static void refreshDomain()
    {

        paymentIAPCreate = "https://" + NameSpace.DOMAIN + "/api/POST/pay/create";//"http://" + NameSpace.DOMAIN + "/api/POST/pay/create";
        paymentIAPVerify = "https://" + NameSpace.DOMAIN + "/api/POST/pay/appstore";
    	logGCM = "https://" + NameSpace.DOMAIN + "/api/POST/Push/RegisterDevice";//"http://" + NameSpace.DOMAIN + "/api/post/push/RegisterDevice";
    	 getAppInfo = "https://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/App/Oinfo";
    	 login = "https://" + NameSpace.DOMAIN + "/dialog/webview/login?";//"https://" + NameSpace.DOMAIN + "/webapp_client/login?"
    	loginBig4 ="https://" + NameSpace.DOMAIN + "/api/GET/Auth/LoginBig4";//"http://" + NameSpace.DOMAIN + "/api/GET/auth/loginbig4
    	 getUserInfo ="https://" + NameSpace.DOMAIN + "/api/GET/Me/Userinfo";//"http://" + NameSpace.DOMAIN + "/api/GET/me/userinfo"
    	 paymentUrl = "https://" + NameSpace.DOMAIN + "/dialog/webview";
    	getDashboardConfig ="https://" + NameSpace.DOMAIN +"/api/POST/App/DB"; //"/api/POST/App/Dashboard";
    	logPlayer = "https://" + NameSpace.DOMAIN + "/api/GET/Mobile/LogPlayUser";
    	
    }

	// --

}
