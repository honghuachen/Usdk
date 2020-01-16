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
        
        NSString *methodName = CreateNSString(method);
        
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

        NSString *methodName = CreateNSString(method);
        NSString *ret = [[Usdk instance] callPluginR:CreateNSString(pluginName) methodName:methodName with:params];
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

- (void)callPlugin:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args
{
    UsdkBase *plugin = [self loadPlugin:pluginName];
    if(plugin != nil)
    {
        NSString *selMethodName = args.count>0 ? [NSString stringWithFormat:@"%@:", methodName] : [NSString stringWithFormat:@"%@", methodName];
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
        NSString *selMethodName = args.count>0 ? [NSString stringWithFormat:@"%@:", methodName] : [NSString stringWithFormat:@"%@", methodName];
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
}

- (void)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:self.m_pluginMap];
    [tempDic enumerateKeysAndObjectsUsingBlock:^(id  _Nonnull key, id  _Nonnull obj, BOOL * _Nonnull stop) {
        id<UsdkApplicationDelegate> delegate = obj;
        if ([delegate respondsToSelector:@selector(application:supportedInterfaceOrientationsForWindow:)]) {
            [delegate application:application supportedInterfaceOrientationsForWindow:window];
        }
    }];
}
@end
