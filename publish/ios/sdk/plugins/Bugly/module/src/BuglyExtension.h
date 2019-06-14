#import <Bugly/Bugly.h>
#import <UsdkBase.h>

@interface BuglyExtension : UsdkBase
//设置用户标识
- (void)setUserIdentifier:(nonnull NSString *)userId;
//更新应用版本信息
- (void)updateAppVersion:(NSString *_Nonnull)version;
//设置关键数据，随崩溃信息上报
- (void)setUserValue:(nonnull NSString *)value
              forKey:(nonnull NSString *)key;
//设置标签
- (void)setTag:(NSUInteger)tag;
//上报自定义异常
- (void)reportException:(nonnull NSException *)exception;
//上报错误
- (void)reportError:(NSError *_Nonnull)error;
@end

