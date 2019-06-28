@protocol UsdkApplicationDelegate <NSObject>
//@optional
@required
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation;
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions ;
- (void)applicationWillResignActive:(UIApplication *)application ;
- (void)applicationDidBecomeActive:(UIApplication *)application;
- (void)applicationWillEnterForeground:(UIApplication *)application ;
- (void)applicationDidEnterBackground:(UIApplication *)application ;
- (void)applicationWillTerminate:(UIApplication *)application ;
- (void)applicationDidReceiveMemoryWarning:(UIApplication *)application;
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken ;
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo ;
- (void)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window;
@end
