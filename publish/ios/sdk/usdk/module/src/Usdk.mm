#import "Usdk.h"

#if defined(__cplusplus)
extern "C" {
#endif
    char* MakeStringCopy( NSString * str)
    {
        const char* string = [str UTF8String];
        if (NULL == string) {
            return NULL;
        }
        char* res = (char*)malloc(strlen(string)+1);
        strcpy(res, string);
        return res;
    }
    
    NSString* CreateNSString (const char* string)
    {
        if (string)
            return [NSString stringWithUTF8String: string];
        else
            return [NSString stringWithUTF8String: ""];
    }
    
//    void SDKSetSdkCallBackReceiver(const char* receiver_name_)
//    {
//        [[Usdk instance] setSdkCallBackReceiver:CreateNSString(receiver_name_)];
//    }
//
//    void SDKLogin(const char* args_)
//    {
//        [[Usdk instance] login:CreateNSString(args_)];
//    }
//
//    void SDKLogout(const char* args_)
//    {
//        [[Usdk instance] logout:CreateNSString(args_)];
//    }
//
//    void SDKOpenUserCenter(const char* args_)
//    {
//        [[Usdk instance] openUserCenter:CreateNSString(args_)];
//    }
//
//    void SDKExitGame(const char* args_)
//    {
//        [[Usdk instance] exitGame:CreateNSString(args_)];
//    }
//
//    void SDKPayStart(const char* product_id, int amount)
//    {
//        [[Usdk instance] payStart:CreateNSString(product_id) amount:amount];
//    }
//
//    void SDKPay(const char* pay_info)
//    {
//        [[Usdk instance] pay:CreateNSString(pay_info)];
//    }
//
//    void SDKReleaseSdkResource(const char* args_)
//    {
//        [[Usdk instance] releaseSdkResource:CreateNSString(args_)];
//    }
//
//    void SDKSwitchAccount(const char* args_)
//    {
//        [[Usdk instance] switchAccount:CreateNSString(args_)];
//    }
//
//
//    //apple pay SetProductIdentifiers
//    void SDKSetProductIdentifiers(const char* identifers[], int count)
//    {
//        NSMutableArray* array = [[NSMutableArray alloc] init];
//        for (int i = 0; i < count; ++i) {
//            [array addObject:CreateNSString(identifers[i])];
//        }
//        [[Usdk instance] setProductIdentifiers:array];
//    }
//
//    void SDKOpenAppstoreComment(const char* appid)
//    {
//        NSString *urlStr = [NSString stringWithFormat:@"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=%@&pageNumber=0&sortOrdering=2&mt=8", CreateNSString(appid)];
//        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:urlStr]];
//    }
//
//    void SDKGetConfig(const char* key)
//    {
//        [[Usdk instance] getConfig:CreateNSString(key)];
//    }
    
    ////sdk C# call OC
    int GetArgsLen(const char** args)
    {
        int len=0;
        const char* tmp = args[0];
        while(tmp)
        {
            len++;
            tmp=args[len];
        }
        printf("length %d\n", len);
        return len;
    }
    
    void __CallPlugin(const char* pluginName,const char* method, const char** args)
    {
        printf("iOSMethod method %s\n", method);
        int length = GetArgsLen(args);
        NSMutableArray *params = [[NSMutableArray alloc] init];
        for(int i=0;i<length;i++)
        {
            printf("arg %d = %s \n",i,args[i]);
            NSString *param = CreateNSString(args[i]);
            [params addObject:param];
        }
        
        NSString * methodName = CreateNSString(method);
        
        [[Usdk instance] callPlugin:CreateNSString(pluginName) methodName:methodName with:params];
    }
    
    const char* __CallPluginR(const char* pluginName,const char* method, const char** args)
    {
        printf("IosMethodReturnInt method %s\n", method);
        int length = GetArgsLen(args);
        NSMutableArray *params = [[NSMutableArray alloc] init];
        for(int i=0;i<length;i++)
        {
            printf("arg %d = %s \n",i,args[i]);
            NSString *param = CreateNSString(args[i]);
            [params addObject:param];
        }
        NSString * methodName = CreateNSString(method);
        NSString * ret = [[Usdk instance] callPluginR:CreateNSString(pluginName) methodName:methodName with:params];
        return MakeStringCopy(ret);
    }
    
#if defined(__cplusplus)
}
#endif

static Usdk* _instance = nil;
@implementation Usdk

+(Usdk*) instance
{
    if (!_instance)
    {
        _instance = [[Usdk alloc] init];
        _instance.m_pluginMap = [NSMutableDictionary dictionary];
//        [_instance loadPlugin:PLATFORM_NAME];
    }
    return _instance;
}

//- (void) setSdkCallBackReceiver:(NSString*)receiver_name
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(setSdkCallBackReceiver:)]) {
//            [delegate setSdkCallBackReceiver:receiver_name];
//        }
//    }
//}
//
//- (void) login:(NSString*)args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(login:)]) {
//            [delegate login:args];
//        }
//    }
//}
//
//- (void) logout:(NSString*) args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(logout:)]) {
//            [delegate logout:args];
//        }
//    }
//}
//
//- (void) openUserCenter:(NSString*) args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(openUserCenter:)]) {
//            [delegate openUserCenter:args];
//        }
//    }
//}
//
//- (void) exitGame:(NSString*) args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(exitGame:)]) {
//            [delegate exitGame:args];
//        }
//    }
//}
//
//- (void) payStart:(NSString*)product_id amount:(int)amount
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(payStart:amount:)]) {
//            [delegate payStart:product_id amount:amount];
//        }
//    }
//}
//
//- (void) pay:(NSString*) pay_info
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(pay:)]) {
//            [delegate pay:pay_info];
//        }
//    }
//}
//
//- (void) releaseSdkResource:(NSString*)args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(releaseSdkResource:)]) {
//            [delegate releaseSdkResource:args];
//        }
//    }
//}
//
//- (void) switchAccount:(NSString*) args
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(switchAccount:)]) {
//            [delegate switchAccount:args];
//        }
//    }
//}
//
//- (void)setProductIdentifiers:(NSArray*)identifers
//{
//    id<UsdkPlatformDelegate> delegate = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(delegate != nil){
//        if ([delegate respondsToSelector:@selector(setProductIdentifiers:)]) {
//            [delegate setProductIdentifiers:identifers];
//        }
//    }
//}
//
//- (NSString*)getConfig:(NSString*)key
//{
//    UsdkBase *plugin = [self.m_pluginMap objectForKey:PLATFORM_NAME];
//    if(plugin != nil){
//        return [plugin getConfig:key];
//    }
//    return nil;
//}

- (void)callPlugin:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args
{
    UsdkBase *plugin = [self loadPlugin:pluginName];
    if(plugin != nil)
    {
        NSString *selMethodName = [NSString stringWithFormat:@"%@:", methodName];
        SEL sel = NSSelectorFromString(selMethodName);
        if ([plugin respondsToSelector:sel])
            [plugin performSelector:sel withObject:args];
    }
}

- (NSString*)callPluginR:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args
{
    NSString *ret = @"";
    UsdkBase *plugin = [self loadPlugin:pluginName];
    if(plugin != nil)
    {
        NSString *selMethodName = [NSString stringWithFormat:@"%@:", methodName];
        SEL sel = NSSelectorFromString(selMethodName);
        if ([plugin respondsToSelector:sel])
            ret = [plugin performSelector:sel withObject:args];
    }
    return ret;
}

- (UsdkBase*)loadPlugin:(NSString*)pluginName
{
    //通过KEY找到value
    UsdkBase *object = [self.m_pluginMap objectForKey:pluginName];
    if (object != nil)
        return object;
    else
    {
        Class pluginClass = NSClassFromString(pluginName);
        if(pluginClass != nil)
        {
            UsdkBase *plugin = [[pluginClass alloc] init];
            if(plugin != nil){
                [self.m_pluginMap setObject:plugin forKey:pluginName];
                id<UsdkApplicationDelegate> delegate = [self.m_pluginMap objectForKey:pluginName];
                if ([delegate respondsToSelector:@selector(application:didFinishLaunchingWithOptions:)]) {
                    [delegate application:self.m_application didFinishLaunchingWithOptions:self.m_launchOptions];
                }
            }
            return plugin;
        }
    }
    return nil;
}

//通过链接启动app
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:openURL:sourceApplication:annotation:)]) {
            [delegate application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(application:openURL:sourceApplication:annotation:)]) {
    //                [delegate application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    //            }
    //        }
    //    }
    return YES;
}

//App将要启动
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    self.m_application = application;
    self.m_launchOptions = launchOptions;
    return YES;
}

//App已经启动
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    self.m_application = application;
    self.m_launchOptions = launchOptions;
    return YES;
}

//App将要进入前台
- (void)applicationWillResignActive:(UIApplication *)application {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationWillResignActive:)]) {
            [delegate applicationWillResignActive:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationWillResignActive:)]) {
    //                [delegate applicationWillResignActive:application];
    //            }
    //        }
    //    }
}

//App已经进入前台
- (void)applicationDidBecomeActive:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationDidBecomeActive:)]) {
            [delegate applicationDidBecomeActive:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationDidBecomeActive:)]) {
    //                [delegate applicationDidBecomeActive:application];
    //            }
    //        }
    //    }
}

//App将要进入后台
- (void)applicationWillEnterForeground:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationWillEnterForeground:)]) {
            [delegate applicationWillEnterForeground:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationWillEnterForeground:)]) {
    //                [delegate applicationWillEnterForeground:application];
    //            }
    //        }
    //    }
}

//App已经进入后台
- (void)applicationDidEnterBackground:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationDidEnterBackground:)]) {
            [delegate applicationDidEnterBackground:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationDidEnterBackground:)]) {
    //                [delegate applicationDidEnterBackground:application];
    //            }
    //        }
    //    }
}

//App将要退出
- (void)applicationWillTerminate:(UIApplication *)application {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationWillTerminate:)]) {
            [delegate applicationWillTerminate:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationWillTerminate:)]) {
    //                [delegate applicationWillTerminate:application];
    //            }
    //        }
    //    }
}

//App内存警告
-  (void)applicationDidReceiveMemoryWarning:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(applicationDidReceiveMemoryWarning:)]) {
            [delegate applicationDidReceiveMemoryWarning:application];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(applicationDidReceiveMemoryWarning:)]) {
    //                [delegate applicationDidReceiveMemoryWarning:application];
    //            }
    //        }
    //    }
}

//当应用程序成功的注册一个推送服务
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:didRegisterForRemoteNotificationsWithDeviceToken:)]) {
            [delegate application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(application:didRegisterForRemoteNotificationsWithDeviceToken:)]) {
    //                [delegate application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    //            }
    //        }
    //    }
}

//当 APS无法成功的完成向 程序进程推送时
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:didFailToRegisterForRemoteNotificationsWithError:)]) {
            [delegate application:application didFailToRegisterForRemoteNotificationsWithError:error];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(application:didFailToRegisterForRemoteNotificationsWithError:)]) {
    //                [delegate application:application didFailToRegisterForRemoteNotificationsWithError:error];
    //            }
    //        }
    //    }
}

//程序运收远程通知
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:didReceiveRemoteNotification:)]) {
            [delegate application:application didReceiveRemoteNotification:userInfo];
        }
    }];
    
    //    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(application:didReceiveRemoteNotification:)]) {
    //                [delegate application:application didReceiveRemoteNotification:userInfo];
    //            }
    //        }
    //    }
}

- (void)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:supportedInterfaceOrientationsForWindow:)]) {
            [delegate application:application supportedInterfaceOrientationsForWindow:window];
        }
    }];
    //    for(NSString *pluginName in tempDic)
    //    {
    //        id<UsdkApplicationDelegate> delegate = [tempDic objectForKey:pluginName];
    //        if(delegate != nil){
    //            if ([delegate respondsToSelector:@selector(application:supportedInterfaceOrientationsForWindow:)]) {
    //                [delegate application:application supportedInterfaceOrientationsForWindow:window];
    //            }
    //        }
    //    }
}
@end
