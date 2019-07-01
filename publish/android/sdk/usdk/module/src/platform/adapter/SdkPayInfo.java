package com.usdk.platform.adapter;

import java.util.HashMap;
import java.util.Map;

import android.util.Log;

public class SdkPayInfo {
	public String uid;
	public String productId;
	public String productName;
	public String productDesc;
	public String productUnit;
	public int productUnitPrice;
	public int productQuantity;
	public int totalAmount;
	public int payAmount;
	public String currencyName;
	public String roleId;
	public String roleName;
	public String roleLevel;
	public String roleVipLevel;
	public String serverId;
	public String zoneId;
	public String partyName;
	public String virtualCurrencyBalance;
	public String customInfo;
	public String gameTradeNo;
	public String gameCallBackURL;
	public String additionalParams;

	public SdkPayInfo(String pay_info_) {
		Map<String, String> pay_infos = ParseString(pay_info_);

		try {
			uid = pay_infos.get("uid");
			productId = pay_infos.get("productId");
			productName = pay_infos.get("productName");
			productDesc = pay_infos.get("productDesc");
			productUnit = pay_infos.get("productUnit");
			productUnitPrice = Integer.parseInt(pay_infos
					.get("productUnitPrice"));
			productQuantity = Integer.parseInt(pay_infos
					.get("productQuantity"));
			totalAmount = Integer.parseInt(pay_infos.get("totalAmount"));
			payAmount = Integer.parseInt(pay_infos.get("payAmount"));
			currencyName = pay_infos.get("currencyName");
			roleId = pay_infos.get("roleId");
			roleName = pay_infos.get("roleName");
			roleLevel = pay_infos.get("roleLevel");
			roleVipLevel = pay_infos.get("roleVipLevel");
			serverId = pay_infos.get("serverId");
			zoneId = pay_infos.get("zoneId");
			partyName = pay_infos.get("partyName");
			virtualCurrencyBalance = pay_infos
					.get("virtualCurrencyBalance");
			customInfo = pay_infos.get("customInfo");
			gameTradeNo = pay_infos.get("gameTradeNo");
			gameCallBackURL = pay_infos.get("gameCallBackURL");
			additionalParams = pay_infos.get("additionalParams");
		} catch (Exception e) {
			Log.e("zwwxsdk", e.getMessage());
		}
	}
	
	public Map<String, String> ParseString(String pay_info_) {
		Map<String, String> String_infos = new HashMap<String, String>();
		try {
			String[] params = pay_info_.split("&");
			for (int i = 0; i < params.length; i++) {
				String[] infos = params[i].split("=");
				if(infos.length == 2)
					String_infos.put(infos[0], infos[1]);
				else if(infos.length == 1)
					String_infos.put(infos[0], "");
				else
					Log.e("zwwxsdk", "'pay_info_' is error String");
			}
		} catch (Exception e) {
			Log.e("zwwxsdk", e.getMessage());
		}

		return String_infos;
	}
}