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

- (void)exitGame{
    NSLog(@"[PlatformProxy14]exitGame");
}

- (void)login{
    NSLog(@"[PlatformProxy]login");
}

- (void)logout {
    NSLog(@"[PlatformProxy]logout");
}

- (void)openUserCenter {
    NSLog(@"[PlatformProxy]openUserCenter");
}

- (void)pay {
    NSLog(@"[PlatformProxy]pay");
}

- (void)releaseSdkResource {
    NSLog(@"[PlatformProxy]releaseSdkResource");
}

- (void)setProductIdentifiers:(NSArray *)identifers {
    NSLog(@"[PlatformProxy]setProductIdentifiers");
}

- (void)setSdkCallBackReceiver:(NSArray *)receiverName {
    NSLog(@"[PlatformProxy]setSdkCallBackReceiver receiverName=%@",receiverName[0]);
}

- (void)switchAccount {
    NSLog(@"[PlatformProxy]switchAccount");
}

- (void)callPlugin:(NSArray *)arg{
    for(id obj in arg){
        NSLog(@"[PlatformProxy]callPlugin obj=%@",obj);
    }
}

- (NSString*)callPluginRInt:(NSArray *)arg{
    for(id obj in arg){
        NSLog(@"[PlatformProxy]callPluginRInt obj=%@",obj);
    }
    return @"2";
}

- (NSString*)callPluginRString:(NSArray *)arg{
    for(id obj in arg){
        NSLog(@"[PlatformProxy]callPluginRString obj=%@",obj);
    }
    return @"iOS return";
}

- (NSString*)callPluginRBool:(NSArray *)arg{
    for(id obj in arg){
        NSLog(@"[PlatformProxy]callPluginRBool obj=%@",obj);
    }
    return @"true";
}

- (NSString*)callPluginRFloat:(NSArray *)arg{
    for(id obj in arg){
        NSLog(@"[PlatformProxy]callPluginRFloat obj=%@",obj);
    }
    return @"3.1415926";
}
@end
