package com.usdk.platform.adapter;

import java.util.HashMap;
import java.util.Map;

import android.util.Log;

public class SdkRoleInfo {
	public String uid;// �û�ID,��Ϸ����ʹ�õ�¼ʱ���Ϸ��������ص�uid
	public String roleId;// ��ID������Ϊ�����֣��Ҳ��ܳ���2147483647��Ӧ�ñ�����Ҫ��
	public String roleType;// ��Ϸ���봫����ʵ����Ϸ������,��������
	public String roleLevel;// 角色等级
	public String roleVipLevel;// 角色Vip等级
	public String serverId;// 服Id
	public String zoneId;// 区ID
	public String roleName;// 角色ID
	public String serverName;// 服名�??
	public String zoneName;// 区名�??
	public String partyName;// 帮会名称
	public String gender;// 性别
	public String balance;
	public String roleCreateTime;// 角色创建时间，时间戳,秒级

	public SdkRoleInfo(String role_info_) {
		Map<String, String> role_infos = ParseString(role_info_);

		try {
			uid = role_infos.get("uid");
			roleId = role_infos.get("roleId");
			roleType = role_infos.get("roleType");
			roleLevel = role_infos.get("roleLevel");
			roleVipLevel = role_infos.get("roleVipLevel");
			serverId = role_infos.get("serverId");
			zoneId = role_infos.get("zoneId");
			roleName = role_infos.get("roleName");
			serverName = role_infos.get("serverName");
			zoneName = role_infos.get("zoneName");
			partyName = role_infos.get("partyName");
			gender = role_infos.get("gender");
			balance = role_infos.get("balance");
			roleCreateTime = role_infos.get("roleCreateTime");
		} catch (Exception e) {
			Log.e("zwwxsdk", e.getMessage());
		}
	}
	
	public Map<String, String> ParseString(String role_info_) {
		Map<String, String> String_infos = new HashMap<String, String>();
		try {
			String[] params = role_info_.split("&");
			for (int i = 0; i < params.length; i++) {
				String[] infos = params[i].split("=");
				if(infos.length == 2)
					String_infos.put(infos[0], infos[1]);
				else if(infos.length == 1)
					String_infos.put(infos[0], "");
				else
					Log.e("zwwxsdk", "'role_info_' is error String");
			}
		} catch (Exception e) {
			Log.e("zwwxsdk", e.getMessage());
		}

		return String_infos;
	}
}
