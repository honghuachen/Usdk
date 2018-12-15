package com.usdk.platform.adapter;

import java.util.HashMap;
import java.util.Map;

import android.util.Log;

public class SdkPayInfo {
	public String uid;// �û�ID,��Ϸ����ʹ�õ�¼ʱ���Ϸ��������ص�uid
	public String productId;// ��ƷID
	public String productName;// ��Ʒ����
	public String productDesc;// ��Ʒ����
	public String productUnit;// ��Ʒ��λ������Ԫ��
	public int productUnitPrice;// ��Ʒ����,��λ�֣����贫���Ժ�汾��ȥ����?
	public int productQuantity;// ��Ʒ���������繺��60Ԫ����60
	public int totalAmount;// ��Ʒ�ܽ��?,��λ��
	public int payAmount;// ʵ��֧���ܶ�,��λ��
	public String currencyName;// ���������ش���ʵ��֧���Ĺ��ʱ�׼���Ҵ��룬����CNY(�����?)/USD(��Ԫ)
	public String roleId;// ��ɫID
	public String roleName;// ��ɫ����
	public String roleLevel;// ��ɫ�ȼ�
	public String roleVipLevel;// ��ɫvip�ȼ�
	public String serverId;// ��ID������Ϊ�����֣��Ҳ��ܳ���2147483647��Ӧ�ñ�����Ҫ��
	public String zoneId;// ��ID
	public String partyName;// �������?
	public String virtualCurrencyBalance;// �����������Ҫ���ֶε��������췢��С�ס�VIVO
	public String customInfo;// ��չ�ֶΣ�����֧���ɹ���͸������Ϸ
	public String gameTradeNo;// ��Ϸ����ID��֧���ɹ���͸������Ϸ
	public String gameCallBackURL;// ֧���ص���ַ�����Ϊ�գ����̨���õĻص���ַ
	public String additionalParams;// ��չ����

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