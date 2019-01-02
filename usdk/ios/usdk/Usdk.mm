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
    
    void SDKSetSdkCallBackReceiver(const char* receiver_name_)
    {
        [[Usdk instance] setSdkCallBackReceiver:CreateNSString(receiver_name_)];
    }

    void SDKLogin(const char* args_)
    {
        [[Usdk instance] login:CreateNSString(args_)];
    }

    void SDKLogout(const char* args_)
    {
        [[Usdk instance] logout:CreateNSString(args_)];
    }

    void SDKOpenUserCenter(const char* args_)
    {
        [[Usdk instance] openUserCenter:CreateNSString(args_)];
    }

    void SDKExitGame(const char* args_)
    {
        [[Usdk instance] exitGame:CreateNSString(args_)];
    }

    void SDKPayStart(const char* product_id, int amount)
    {
        [[Usdk instance] payStart:CreateNSString(product_id) amount:amount];
    }

    void SDKPay(const char* pay_info)
    {
        [[Usdk instance] pay:CreateNSString(pay_info)];
    }

    void SDKReleaseSdkResource(const char* args_)
    {
        [[Usdk instance] releaseSdkResource:CreateNSString(args_)];
    }

    void SDKSwitchAccount(const char* args_)
    {
        [[Usdk instance] switchAccount:CreateNSString(args_)];
    }

        
    //apple pay SetProductIdentifiers
    void SDKSetProductIdentifiers(const char* identifers[], int count)
    {
        NSMutableArray* array = [[NSMutableArray alloc] init];
        for (int i = 0; i < count; ++i) {
            [array addObject:CreateNSString(identifers[i])];
        }
        [[Usdk instance] setProductIdentifiers:array];
    }
    
    void SDKOpenAppstoreComment(const char* appid)
    {
        NSString *urlStr = [NSString stringWithFormat:@"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=%@&pageNumber=0&sortOrdering=2&mt=8", CreateNSString(appid)];
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:urlStr]];
    }
    
    void SDKGetConfig(const char* key)
    {
        [[Usdk instance] getConfig:CreateNSString(key)];
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
    
    void SDKCallPlugin(const char* pluginName,const char* method, const char** args)
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

    const char* SDKCallPluginR(const char* pluginName,const char* method, const char** args)
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
@synthesize platformProxy;
@synthesize m_pluginMap;
@synthesize m_application;
@synthesize m_launchOptions;

+(Usdk*) instance
{
    if (!_instance)
    {
        _instance = [[Usdk alloc] init];
        _instance.platformProxy  = (PlatformProxyBase*)[_instance loadPlugin:@"PlatformProxy"];
        _instance.m_pluginMap = [NSMutableDictionary dictionary];
    }
    return _instance;
}

- (void) setSdkCallBackReceiver:(NSString*)receiver_name
{
    [platformProxy setSdkCallBackReceiver:receiver_name];
}

- (void) login:(NSString*)args
{
    [platformProxy login:args];
}

- (void) logout:(NSString*) args
{
    [platformProxy logout: args];
}

- (void) openUserCenter:(NSString*) args
{
    [platformProxy openUserCenter:args];
}

- (void) exitGame:(NSString*) args
{
    [platformProxy exitGame:args];
}

- (void) payStart:(NSString*)product_id amount:(int)amount
{
    [platformProxy payStart:product_id amount:amount];
}

- (void) pay:(NSString*) pay_info
{
    [platformProxy pay: pay_info];
}

- (void) releaseSdkResource:(NSString*)args
{
    [platformProxy releaseSdkResource:args];
}

- (void) switchAccount:(NSString*) args
{
    [platformProxy switchAccount:args];
}

- (void)setProductIdentifiers:(NSArray*)identifers
{
    [platformProxy setProductIdentifiers:identifers];
}

- (NSString*)getConfig:(NSString*)key
{
    return [platformProxy getConfig:key];
}

- (void)callPlugin:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args
{
    UsdkBase *plugin = [self loadPlugin:pluginName];
    if(plugin != nil)
    {
        [plugin performSelector:@selector(methodName) withObject:args];
    }
}

- (NSString*)callPluginR:(NSString*)pluginName methodName:(NSString*)methodName with:(NSArray*) args
{
    UsdkBase *plugin = [self loadPlugin:pluginName];
    if(plugin != nil)
    {
        return [plugin performSelector:@selector(methodName) withObject:args];
    }
    return @"";
}

- (UsdkBase*)loadPlugin:(NSString*)pluginName
{
    //通过KEY找到value
    UsdkBase *object = [m_pluginMap objectForKey:pluginName];
    if (object != nil) 
        return object;
    else
    {
        Class pluginClass = NSClassFromString(pluginName);
        if(pluginClass != nil)
        {
            UsdkBase *plugin = [[pluginClass alloc] init];
            [m_pluginMap setObject:plugin forKeyedSubscript:pluginName];
            [plugin application:m_application didFinishLaunchingWithOptions:m_launchOptions];
            return plugin;
        }
    }
    return nil;
}

//通过链接启动app
- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation NS_AVAILABLE_IOS(4_2)
{
	return YES;
}

//App将要启动
- (BOOL)application:(UIApplication *)application willFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    m_application = application;
    m_launchOptions = launchOptions;
    return YES;
}

//App已经启动
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions 
{
    return YES;
}

 //App将要进入前台
- (void)applicationWillResignActive:(UIApplication *)application {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationWillResignActive:application];
    }
}

 //App已经进入前台
- (void)applicationDidBecomeActive:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationDidBecomeActive:application];
    }
}

 //App将要进入后台
- (void)applicationWillEnterForeground:(UIApplication *)application 
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationWillEnterForeground:application];
    }
}

 //App已经进入后台
- (void)applicationDidEnterBackground:(UIApplication *)application 
{   
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationDidEnterBackground:application];
    }
}

 //App将要退出
- (void)applicationWillTerminate:(UIApplication *)application {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationWillTerminate:application];
    }
}

 //App内存警告
-  (void)applicationDidReceiveMemoryWarning:(UIApplication *)application
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin applicationDidReceiveMemoryWarning:application];
    }
}

 //当应用程序成功的注册一个推送服务
- (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken 
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    }
}

 //当 APS无法成功的完成向 程序进程推送时
- (void)application:(UIApplication*)application didFailToRegisterForRemoteNotificationsWithError:(NSError*)error
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin application:application didFailToRegisterForRemoteNotificationsWithError:error];
    }
}

 //程序运收远程通知
- (void)application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo 
{
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin application:application didReceiveRemoteNotification:userInfo];
    }
}

- (void)application:(UIApplication *)application supportedInterfaceOrientationsForWindow:(UIWindow *)window {
    NSMutableDictionary *tempDic = [NSMutableDictionary dictionaryWithDictionary:m_pluginMap];
    for(NSString *pluginName in tempDic)
    {
        UsdkBase *plugin = [tempDic objectForKey:pluginName];
        if(plugin != nil)
            [plugin application:application supportedInterfaceOrientationsForWindow:window];
    }
}
@end

