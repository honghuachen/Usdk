#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UsdkRoleInfo.h"
#import "UsdkPayInfo.h"
#import "UsdkPlatformDelegate.h"
#import "UsdkApplicationDelegate.h"

typedef NS_ENUM(NSInteger, UsdkCallBackErrorCode)
{
    InitSuccess = 0,
    InitFail,
    LoginSuccess,
    LoginCancel,
    LoginFail,
    LogoutFinish,
    ExitNoChannelExiter,
    ExitSuccess,
    PaySuccess,
    PayCancel,
    PayFail,
    PayProgress,
    PayOthers,
};

@interface UsdkBase : NSObject
- (void)sendDict2Unity:(NSString *)method param:(NSDictionary *)dict;
- (void)sendString2Unity:(NSString *)method param:(NSString *)str;
- (void)sendCallBack2Unity:(UsdkCallBackErrorCode)code Param:(NSString *)str;
- (NSString*)getConfig:(NSString *)key;
- (UsdkRoleInfo*)createRoleInfo:(NSString *)info;
- (UsdkPayInfo*)createPayInfo:(NSString *)info;
@end
