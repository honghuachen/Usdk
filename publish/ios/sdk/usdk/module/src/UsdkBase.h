#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UsdkRoleInfo.h"
#import "UsdkPayInfo.h"
#import "UsdkPlatformDelegate.h"
#import "UsdkApplicationDelegate.h"

@interface UsdkBase : NSObject
- (void)sendDict2U3d:(NSString *)method param:(NSDictionary *)dict;
- (void)sendString2U3d:(NSString *)method param:(NSString *)str;
- (void)callBack2U3d:(NSString *)method Code:(NSInteger *)code Result:(NSString *)ret;
- (NSString*)getConfig:(NSString *)key;
- (UsdkRoleInfo*)createRoleInfo:(NSString *)info;
- (UsdkPayInfo*)createPayInfo:(NSString *)info;
@end
