#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "PlatformProxyBase.h"
#import "IUsdkBase.h"
#import "UsdkBase.h"

#define ERROR_CODE              @"errorCode"
#define MESSAGE                 @"message"
#define CHANNEL_CODE            @"channelCode"
#define ORDER_ID                @"orderID"
#define TRANSACTION_ID          @"transactionid"

//callback method name
#define INIT_SDK_CALLBACK           @"initSdkCallBack"
#define EXIT_GAME_CALLBACK          @"exitGameCallBack"
#define LOGIN_CALLBACK              @"loginCallBack"
#define LOGOUT_CALLBACK             @"logoutCallBack"
#define PAY_CALLBACK                @"payCallBack"
#define APPLEPAY_CALLBACK           @"applePayCallBack"

typedef NS_ENUM(NSInteger, UsdkCallBackErrorCode)
{
    InitSuccess = 0,
    InitFail,
    LoginSuccess,
    LoginCancel,
    LoginFail,
    LogoutFinish,
    ExitNoChannelExiter,
    ExitSuccess,
    PaySuccess,
    PayCancel,
    PayFail,
    PayProgress,
    PayOthers,
};

@interface Usdk :NSObject<IUsdkBase>
+(Usdk*) instance;
@property PlatformProxyBase *platformProxy;
@property NSMutableDictionary *m_pluginMap;
@property UIApplication *m_application;
@property NSDictionary *m_launchOptions;

- (void) setSdkCallBackReceiver:(NSString*) receiver_name;
- (void) login:(NSString*)args;
- (void) logout:(NSString*) args;
- (void) openUserCenter:(NSString*) args;
- (void) exitGame:(NSString*) args;
- (void) payStart:(NSString*)product_id amount:(int)amount;
- (void) pay:(NSString*) pay_info;
- (void) releaseSdkResource:(NSString*)args;
- (void) switchAccount:(NSString*) args;
- (void)setProductIdentifiers:(NSArray*)identifers;
- (void)callPlugin:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args;
- (NSString*)callPluginR:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args;
- (UsdkBase*)loadPlugin:(NSString*)pluginName;
- (NSString*)getConfig:(NSString*)key;
@end
