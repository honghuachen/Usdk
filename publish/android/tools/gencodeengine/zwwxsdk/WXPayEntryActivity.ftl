package ${entity.javaPackage}.wxapi;

import com.tencent.mm.opensdk.modelbase.BaseReq;
import com.tencent.mm.opensdk.modelbase.BaseResp;
import com.tencent.mm.opensdk.openapi.IWXAPI;
import com.tencent.mm.opensdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.opensdk.openapi.WXAPIFactory;
import com.zwwx.sdk.ZhangWoGameSDKStatusCode;
import com.zwwx.sdk.utils.ZhangWoLog;
import com.zwwx.sdk.view.activity.ZhangWoPayActivity;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class WXPayEntryActivity extends Activity implements IWXAPIEventHandler{
    private IWXAPI api;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //setContentView(R.layout.pay_result);
        
    	api = WXAPIFactory.createWXAPI(this, "wx925e48daf9c98c51");
        api.handleIntent(getIntent(), this);
    }

	@Override
	protected void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		setIntent(intent);
        api.handleIntent(intent, this);
	}

	@Override
	public void onReq(BaseReq req) {
	}

	@Override
	public void onResp(BaseResp resp) {
		ZhangWoLog.d("weixin PayFinish, errCode = " + resp.errCode);
		
		if(resp.errCode == 0)
		{
			//pay success		
			if(ZhangWoPayActivity.callBack != null)
				ZhangWoPayActivity.callBack.callBack(ZhangWoGameSDKStatusCode.CODE_PAY_SUCC,"支付成功");
		}
		else if(resp.errCode == -1)
		{
			//pay fail
			if(ZhangWoPayActivity.callBack != null)
				ZhangWoPayActivity.callBack.callBack(ZhangWoGameSDKStatusCode.CODE_PAY_ERR,"支付失败");
		}
		else if(resp.errCode == -2)
		{
			//pay cancel
			if(ZhangWoPayActivity.callBack != null)
				ZhangWoPayActivity.callBack.callBack(ZhangWoGameSDKStatusCode.CODE_PAY_ERR,"支付取消");
		}
		else
		{
			//pay unknown
			if(ZhangWoPayActivity.callBack != null)
				ZhangWoPayActivity.callBack.callBack(ZhangWoGameSDKStatusCode.CODE_PAY_ERR,"支付失败（未知错误）");
		}
		this.finish();
	}
}