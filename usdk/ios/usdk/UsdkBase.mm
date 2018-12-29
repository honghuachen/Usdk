#import "UsdkBase.h"

#if defined(__cplusplus)
extern "C" {
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
    extern NSString *CreateNSString (const char* string);
#if defined(__cplusplus)
}
#endif

@implementation UsdkBase
- (void) getConfig:(NSString*)key{}
- (void)sendU3dMessage:(NSString *)messageName param:(NSDictionary *)dict
{
    NSString *param = @"";
    for (NSString *key in dict)
    {
        if ([param length] == 0)
            param = [param stringByAppendingFormat:@"%@=%@", key, [dict valueForKey:key]];
        else
            param = [param stringByAppendingFormat:@"&%@=%@", key, [dict valueForKey:key]];
    }
    
    [self sendU3dStringMessage:messageName param:param];
}

- (void)sendU3dStringMessage:(NSString *)messageName param:(NSString *)str
{
    NSLog(@"SDKCallBack with messagename=%@, param=%@",messageName,str);
    UnitySendMessage("UsdkCallBack", [messageName UTF8String], [str UTF8String]);
}

//通过链接启动app
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation NS_AVAILABLE_IOS(4_2){return YES;}
//App将要启动
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions{return YES;}
//App已经启动
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {return YES;}
 //App将要进入前台
- (void)applicationWillResignActive:(UIApplication *)application {}
 //App已经进入前台
- (void)applicationDidBecomeActive:(UIApplication *)application{}
 //App将要进入后台
- (void)applicationWillEnterForeground:(UIApplication *)application {}
 //App已经进入后台
- (void)applicationDidEnterBackground:(UIApplication *)application {}
 //App将要退出
- (void)applicationWillTerminate:(UIApplication *)application {}
 //App内存警告
- (void)applicationDidReceiveMemoryWarning:(UIApplication *)application{}
 //当应用程序成功的注册一个推送服务
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {}
 //当 APS无法成功的完成向 程序进程推送时
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error{}
 //程序运收远程通知
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {}
- (NSUInteger)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {}
@end

