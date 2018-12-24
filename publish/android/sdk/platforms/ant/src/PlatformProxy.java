package com.zwwx.platform;

import com.zwwx.adapter.PlatformCallBack;
import com.zwwx.adapter.ProxyBase;

public final class PlatformProxy extends ProxyBase {
	@Override
	public void pay(String pay_info_) {
		platformCallBackWrapper.payCallBack(
				PlatformCallBack.ErrorCode.PaySuccess.ordinal(), "success");
	}

	@Override
	public void exit(String arg0) {
		// TODO Auto-generated method stub
		super.exit(arg0);
		
		platformCallBackWrapper.exitGameCallBack(PlatformCallBack.ErrorCode.ExitNoChannelExiter.ordinal());
	}
}