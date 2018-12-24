package vn.soha.game.sdk;

import org.json.JSONObject;

import vn.soha.game.sdk.utils.NameSpace;
import vn.soha.game.sdk.utils.ServiceHelper;
import vn.soha.game.sdk.utils.Utils;
import android.app.Activity;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class SohaActivity extends Activity {
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		
		Bundle extras = getIntent().getExtras();
		if (extras != null) {
			Log.e("stk", "SohaActivity extra=" + extras);
			if (extras.containsKey("token")) {
				String token = extras.getString("token");
				Log.e("stk", "token=" + token);
				exchangeSOAPAccessToken(token);
			}
		}
	}
	
	public void exchangeSOAPAccessToken(final String token) {
		Log.e("stk", "SohaActivity.exchangeSOAPAccessToken()");
		new Thread(new Runnable() {
			@Override
			public void run() {
				// TODO Auto-generated method stub
				// access_token=ACCESS_TOKEN&app_id=APP_ID"
				String api = NameSpace.API_EXCHANGE_TOKEN + "access_token=" + token + "&app_id=" + Utils.getAppId(SohaActivity.this);
				final String response = ServiceHelper.get(api);

				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						if (response == null) {
							Utils.openAlertCheckInternet(SohaActivity.this, new DialogInterface.OnClickListener() {
								@Override
								public void onClick(DialogInterface dialog, int which) {
									// TODO Auto-generated method stub
									exchangeSOAPAccessToken(token);
								}
							});
						} else {
							try {
								JSONObject responseJSON = new JSONObject(response);
								String status = responseJSON.getString("status");
								if (status.equals("success")) {
									Log.e("stk", "SohaActivity: exchangeSOAPAccessToken successful");
									String newToken = responseJSON.getString("token");
									Utils.saveSoapAccessToken(SohaActivity.this, newToken);
								} else if (status.equals("fail")) {
									Log.e("stk", "SohaActivity: exchangeSOAPAccessToken failed");
								}
							} catch (Exception e) {
								// TODO: handle exception
								e.printStackTrace();
							} finally {
								// start main activity
								Intent intent = getPackageManager().getLaunchIntentForPackage(getPackageName());
								startActivity(intent);
								finish();
							}
						}
					}
				});
			}
		}).start();
	}
}
