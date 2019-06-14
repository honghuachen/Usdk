#import "BuglyExtension.h"

@implementation BuglyExtension

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {
    [Bugly startWithAppId:nil];
    return YES;
}

//设置用户标识
- (void)setUserIdentifier:(nonnull NSString *)userId{
    [Bugly setUserIdentifier:userId];
}

//更新应用版本信息
- (void)updateAppVersion:(NSString *)version{
    [Bugly updateAppVersion:version];
}

//设置关键数据，随崩溃信息上报
- (void)setUserValue:(nonnull NSString *)value
              forKey:(nonnull NSString *)key{
    [Bugly setUserValue:value forKey:key];
}

//设置标签
- (void)setTag:(NSUInteger)tag{
    [Bugly setTag:tag];
}

//上报自定义异常
- (void)reportException:(nonnull NSException *)exception{
    [Bugly reportException:exception];
}

//上报错误
- (void)reportError:(NSError *)error{
    [Bugly reportError:error];
}
@end

