package com.baidu.bdgamesdk.demo.activity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.baidu.bdgamesdk.demo.R;
import com.baidu.gamesdk.BDGameSDK;
import com.baidu.gamesdk.IResponse;
import com.baidu.gamesdk.ResultCode;

/**
 * 游戏主界面 
 * @author cgp
 * @date 2014-10-22 下午8:37:13
 *
 */
public class MainActivity extends BaseActivity implements View.OnClickListener {

    private Button loginBtn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        initView();
        BDGameSDK.getAnnouncementInfo(this);
    }

    private void initView() {
        loginBtn = (Button) findViewById(R.id.login_btn);
        loginBtn.setOnClickListener(this);

    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.login_btn:
                Log.d("click", "login");
                login();
                break;
            default:
                break;
        }

    }

    private void login() { // 登录
        loginBtn.setEnabled(false);
        BDGameSDK.login(new IResponse<Void>() {

            @Override
            public void onResponse(int resultCode, String resultDesc, Void extraData) {
                Log.d("login", "this resultCode is " + resultCode);
                String hint = "";
                switch (resultCode) {
                    case ResultCode.LOGIN_SUCCESS:
                        hint = "登录成功";
                        GameActivity.uid = BDGameSDK.getLoginUid(); // TODO 保存登陆后获取的uid到调用支付API时使用
                        Intent intent = new Intent(MainActivity.this, GameActivity.class);
                        startActivity(intent);
                        finish();
                        break;
                    case ResultCode.LOGIN_CANCEL:
                        loginBtn.setEnabled(true);
                        hint = "取消登录";
                        break;
                    case ResultCode.LOGIN_FAIL:
                    default:
                        loginBtn.setEnabled(true);
                        hint = "登录失败:" + resultDesc;
                }
                Toast.makeText(getApplicationContext(), hint, Toast.LENGTH_LONG).show();
            }
        });
    }

}
