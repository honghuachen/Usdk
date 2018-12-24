package com.zwwx.plugins.weixin;

public interface IWeiXin {
	public abstract void sendWXSceneTimeline();
	public abstract void setWXRoleName(String name);
	public abstract void setWXServerName(String name);
	public abstract void initWXShare(String appID);
}
