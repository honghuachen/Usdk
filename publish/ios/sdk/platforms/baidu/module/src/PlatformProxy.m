#import "UsdkBase.h"
#import "UsdkApplicationDelegate.h"
#import "UsdkPlatformDelegate.h"

@interface PlatformProxy : UsdkBase<UsdkApplicationDelegate,UsdkPlatformDelegate>
@end

@implementation PlatformProxy
- (void)application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error {
    NSLog(@"[PlatformProxy1]application:(UIApplication *)application didFailToRegisterForRemoteNotificationsWithError:(NSError *)error");
}

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    NSLog(@"[PlatformProxy2]application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions");
    return YES;
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
    NSLog(@"[PlatformProxy3]application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo");
}

- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    NSLog(@"[PlatformProxy4]application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken");
}

- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
    NSLog(@"[PlatformProxy5]application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation");
    return YES;
}

- (void)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {
    NSLog(@"[PlatformProxy6]application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window");
}

- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    NSLog(@"[PlatformProxy7]application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions");
    return YES;
}

- (void)applicationDidBecomeActive:(UIApplication *)application {
    NSLog(@"[PlatformProxy8]applicationDidBecomeActive:(UIApplication *)application");
}

- (void)applicationDidEnterBackground:(UIApplication *)application {
    NSLog(@"[PlatformProxy9]applicationDidEnterBackground:(UIApplication *)application");
}

- (void)applicationDidReceiveMemoryWarning:(UIApplication *)application {
    NSLog(@"[PlatformProxy10]applicationDidReceiveMemoryWarning:(UIApplication *)application");
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
     NSLog(@"[PlatformProxy11]applicationWillEnterForeground:(UIApplication *)application");
}

- (void)applicationWillResignActive:(UIApplication *)application {
     NSLog(@"[PlatformProxy12]applicationWillResignActive:(UIApplication *)application");
}

- (void)applicationWillTerminate:(UIApplication *)application {
     NSLog(@"[PlatformProxy13]applicationWillTerminate:(UIApplication *)application");
}

- (void)exitGame:(NSString *)arg {
    NSLog(@"[PlatformProxy14]exitGame");
}

- (void)login:(NSString *)arg {
    NSLog(@"[PlatformProxy]login");
}

- (void)logout:(NSString *)arg {
    NSLog(@"[PlatformProxy]logout");
}

- (void)openUserCenter:(NSString *)arg {
    NSLog(@"[PlatformProxy]openUserCenter");
}

- (void)pay:(NSString *)pay_info {
    NSLog(@"[PlatformProxy]pay");
}

- (void)payStart:(NSString *)product_id amount:(int)amount {
    NSLog(@"[PlatformProxy]payStart");
}

- (void)releaseSdkResource:(NSString *)arg {
    NSLog(@"[PlatformProxy]releaseSdkResource");
}

- (void)setProductIdentifiers:(NSArray *)identifers {
    NSLog(@"[PlatformProxy]setProductIdentifiers");
}

- (void)setSdkCallBackReceiver:(NSString *)receiver_name {
    NSLog(@"[PlatformProxy]setSdkCallBackReceiver");
}

- (void)switchAccount:(NSString *)arg {
    NSLog(@"[PlatformProxy]switchAccount");
}

- (void)callPlugin:(NSArray *)arg {
    NSLog(@"[PlatformProxy]callPlugin arg=%@",arg);
    for (id obj in arg){
        NSLog(@"[PlatformProxy]obj=%@",obj);
    }
}

- (NSString*)callPluginR:(NSArray *)arg {
    NSLog(@"[PlatformProxy]callPluginR arg=%@",arg);
    for (id obj in arg){
        NSLog(@"[PlatformProxy]obj=%@",obj);
    }
    return @"iOS call R";
}

- (NSString*)callPluginR2:(NSArray *)arg {
    NSLog(@"[PlatformProxy]callPluginR2 arg=%@",arg);
    for (id obj in arg){
        NSLog(@"[PlatformProxy]obj=%@",obj);
    }
    return @"iOS call R2";
}

@end
