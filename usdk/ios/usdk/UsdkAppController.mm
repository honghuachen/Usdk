#import "UnityAppController.h"
#import "UsdkFactory.h"

@interface UsdkAppController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS (UsdkAppController)
@implementation UsdkAppController
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation NS_AVAILABLE_IOS(4_2){
    [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
	[[Usdk instance] application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
	return YES;
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    [super applicationDidBecomeActive:application];
	[[Usdk instance] applicationDidBecomeActive:application];
}

- (void)applicationDidEnterBackground:(UIApplication *)application {   
    [super applicationDidEnterBackground : application];
	[[Usdk instance] applicationDidEnterBackground : application];
}

- (void)applicationWillEnterForeground:(UIApplication *)application {
    [super applicationWillEnterForeground : application];
	[[Usdk instance] applicationWillEnterForeground : application];
}

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [super application: application didFinishLaunchingWithOptions : launchOptions];
	[[Usdk instance] application: application didFinishLaunchingWithOptions : launchOptions];
    return YES;
}

- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
	[[Usdk instance] application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
}

- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
    [super application:application didFailToRegisterForRemoteNotificationsWithError:error];
	[[Usdk instance] application:application didFailToRegisterForRemoteNotificationsWithError:error];
}

- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
    [super application:application didReceiveRemoteNotification:userInfo];
	[[Usdk instance] application:application didReceiveRemoteNotification:userInfo];
}

- (NSUInteger)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {
	[[Usdk instance] application:application supportedInterfaceOrientationsForWindow:window];
    return [super application:application supportedInterfaceOrientationsForWindow:window];
}

- (void)applicationWillTerminate:(UIApplication *)application {
    [super applicationWillTerminate:application];
	[[Usdk instance] applicationWillTerminate:application];	
}

- (BOOL)application:(UIApplication*)application willFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
	[super application:application willFinishLaunchingWithOptions:launchOptions];	
	[[Usdk instance] application:application willFinishLaunchingWithOptions:launchOptions];	
    return YES;
}

 //App将要进入前台
- (void)applicationWillResignActive:(UIApplication *)application {
	[super applicationWillResignActive:application];	
	[[Usdk instance] applicationWillResignActive:application];	
}

 //App内存警告
-  (void)applicationDidReceiveMemoryWarning:(UIApplication *)application
{
	[super applicationDidReceiveMemoryWarning:application];	
	[[Usdk instance] applicationDidReceiveMemoryWarning:application];
}
@end
