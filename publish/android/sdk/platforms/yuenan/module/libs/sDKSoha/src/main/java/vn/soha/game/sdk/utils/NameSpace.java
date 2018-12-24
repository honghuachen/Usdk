package vn.soha.game.sdk.utils;

public class NameSpace {
	// DOMAIN
	//public static final String DOMAIN = ""+DOMAIN+"";
	//public static final String DOMAIN = "maoristudio.net";
	//public static final String DOMAIN = "channel.connectg.info";
	//
	
	public static String DOMAIN = "soap.soha.vn";

	// API MYSOHA
	public static final String API_LOG_GAME_STATE = "https://sdk2mysoha.sohacorp.vn/tracking";
	public static final String API_GET_NOTIFICATION = "https://sdk2mysoha.sohacorp.vn/public/getNotifications?";
	public static final String API_GET_NOTIFICATION_DEV = "https://dev.sdk.mysoha.sohacorp.vn/public/getNotifications?";
	public static final String API_GET_CONFIG = "https://sdk2mysoha.sohacorp.vn/public/getMysohaScheme?device_type=android";
	public static final String API_GET_CONFIG_DEV = "https://dev.sdk.mysoha.sohacorp.vn/public/getMysohaScheme?device_type=android";
	public static final String API_GET_NOTIFICATION_DETAILS = "https://sdk2mysoha.sohacorp.vn/public/getNotification?";

	// API SOAP
/*	public static String API_GET_SESSION = "https://"+DOMAIN+"/api/a/GET/me/session?";
	public static String API_CONFIRM_ORDER = "https://"+DOMAIN+"/api/a/GET/order/mobile?check=%s&order_id=%s";*/
	// log thong tin nhan vat
	/*public static String API_LOG_USER = "https://" + DOMAIN + "/api/a/GET/mobile/LogPlayUser?";
	public static String API_LOG_USER_POST = "http://" + DOMAIN + "/api/get/mobile/logplayuser";*/
	// ------------
	public static String API_LISTGAME = "https://game.soha.vn/apiv1/game/getMobileGameData?channel_id=12&platform=android";
	public static String API_APP_INFO = "https://"+DOMAIN+"/api/a/iGET/app/oinfo?app_key=";

	public static String API_LOG_DEVICE_INFO = "https://"+DOMAIN+"/api/a/POST/mobile/LogInstall?type=0";
	public static String API_GCM_REGISTER = "https://"+DOMAIN+"/api/a/POST/Push/RegisterDevice";
	public static String API_EXCHANGE_TOKEN = "https://"+DOMAIN+"/api/a/GET/me/exchangetoken?";
	
	/*public static String API_APP_INFO = "https://"+DOMAIN+"/api/a/iGET/app/oinfo?app_key=";
	public static String API_LISTGAME = "http://game.soha.vn/apiv1/game/getMobileGameData?channel_id=12&platform=android";
	public static String API_LOG_DEVICE_INFO = "https://"+DOMAIN+"/api/a/POST/mobile/LogInstall?type=0";
	public static String API_GCM_REGISTER = "https://"+DOMAIN+"/api/a/POST/Push/RegisterDevice";
	public static String API_EXCHANGE_TOKEN = "https://"+DOMAIN+"/api/a/GET/me/exchangetoken?";*/
	public static void refreshDomain()
	{
	 API_APP_INFO = "https://"+DOMAIN+"/api/a/iGET/app/oinfo?app_key=";
	 API_LOG_DEVICE_INFO = "https://"+DOMAIN+"/api/a/POST/mobile/LogInstall?type=0";
	 API_GCM_REGISTER = "https://"+DOMAIN+"/api/a/POST/Push/RegisterDevice";
	 API_EXCHANGE_TOKEN = "https://"+DOMAIN+"/api/a/GET/me/exchangetoken?";
	}
	// shared preference
	public static String SHARED_PREF_NAME = "sp_sohagame";
	public static String SHARED_PREF_SOAP_ACCESS_TOKEN = "soap_access_token";
	public static final String SHARED_PREF_MYSOHA_ACCESS_TOKEN = "mysoha_access_token";
	public static final String SHARED_PREF_CLIENT_NAME = "client_name";
	public static final String SHARED_PREF_APP_ID = "app_id";
	public static final String SHARED_PREF_APP_ID_FACEBOOK = "app_id_facebook";
	public static final String SHARED_PREF_GAME_VERSION = "game_version";
	public static final String SHARED_PREF_SDK_VERSION = "sdk_version";
	public static String SHARED_PREF_AREAID = "areaid";
	public static String SHARED_PREF_ROLEID = "roleid";
	public static String SHARED_PREF_ROLENAME = "rolename";
	public static String SHARED_PREF_ROLELEVEL = "rolelevel";
	public static final String SHARED_PREF_NOTIFICATON_NUMBER = "notification_number";
	public static final String SHARED_PREF_PENDING_LOGIN_MYSOHA = "pending_login_mysoha";
	public static  String SHARED_PREF_USER_ID = "user_id";
	public static final String SHARED_PREF_USER_EMAIL = "user_email";
	public static final String SHARED_PREF_USER_NAME = "user_name";
	// MYSOHA LOG
	public static final String SHARED_PREF_LOG_OPEN_APP = "log_open_app";
	public static final String SHARED_PREF_LOG_INSTALL_APP = "log_install_app";
	public static final String SHARED_PREF_LOG_UPDATE_CONFIG = "log_update_config";

	// MYSOHA CONFIG
	public static final String PACKAGE_MYSOHA = "vcc.com.sohagame.dev";
	public static final String CLASS_LOGIN_MYSOHA = "vcc.com.sohagame.LoginActivity";
	public static final String DOWNLOAD_PAGE = "http://sohacorp.vn/mysoha";
	
	public static String STORAGE_PATH;
// MQTT
	public static final String SHARED_PREF_IS_OPENED_APP= "APP_IS_OPENED";
	public static String SHARED_PREF_IS_INSTALLED_APP= "APP_IS_INSTALLED";
	public static final String SHARED_PREF_IS_INSTALLED_MQTT= "APP_IS_INSTALLED_MQTT";
	public static final String SHARED_PREF_CLIENT_CODE_MQTT = "client_code_mqtt";
	public static final String SHARED_PREF_GA_CODE = "SHARED_PREF_GA_CODE";

}
