#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#define ERROR_CODE              @"errorCode"
#define MESSAGE                 @"message"
#define CHANNEL_CODE            @"channelCode"
#define ORDER_ID                @"orderID"
#define TRANSACTION_ID          @"transactionid"

//callback method name
#define INIT_SDK_CALLBACK           @"initSDKCallBack"
#define EXIT_GAME_CALLBACK          @"exitGameCallBack"
#define LOGIN_CALLBACK              @"loginCallBack"
#define LOGOUT_CALLBACK             @"logoutCallBack"
#define PAY_CALLBACK                @"payCallBack"
#define APPLEPAY_CALLBACK           @"applePayCallBack"
#define EXCHANGE_GIFTCODE_CALLBACK  @"exchangeGiftCodeCallBack"

typedef NS_ENUM(NSInteger, SdkCallBackErrorCode)
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

#pragma mark - Usdk
@interface Usdk()
@property PlatformProxy *platformProxy;
@property NSMutableDictionary *m_pluginMap;
@property UIApplication *m_application;
@property NSDictionary *m_launchOptions;

+(Usdk*) _instance;
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
- (void)getConfig:(NSString*)key;
- (void)callPlugin:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args;
- (NSString*)callPluginR:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args;
- (UsdkBase*)loadPlugin:(NSString*)pluginName;
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation NS_AVAILABLE_IOS(4_2);
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions ;
- (void)applicationWillResignActive:(UIApplication *)application ;
- (void)applicationDidBecomeActive:(UIApplication *)application;
- (void)applicationWillEnterForeground:(UIApplication *)application ;
- (void)applicationDidEnterBackground:(UIApplication *)application ;
- (void)applicationWillTerminate:(UIApplication *)application ;
-  (void)applicationDidReceiveMemoryWarning:(UIApplication *)application;
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken ;
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo ;
- (NSUInteger)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window;
@end