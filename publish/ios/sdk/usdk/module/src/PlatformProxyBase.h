#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UsdkBase.h"
#import "IUsdkBase.h"
#import "UsdkRoleInfo.h"
#import "UsdkPayInfo.h"

@interface PlatformProxyBase : UsdkBase<IUsdkBase>
- (void) setSdkCallBackReceiver:(NSString*) receiver_name;
- (void) login:(NSString*)arg;
- (void) logout:(NSString*) arg;
- (void) openUserCenter:(NSString*) arg;
- (void) exitGame:(NSString*) arg;
- (void) payStart:(NSString*)product_id amount:(int)amount;
- (void) pay:(NSString*) pay_info;
- (void) releaseSdkResource:(NSString*)arg;
- (void) switchAccount:(NSString*) arg;
- (void) setProductIdentifiers:(NSArray*)identifers;
- (UsdkRoleInfo*)CreateRoleInfo:(NSString*)info;
- (UsdkPayInfo*)CreatePayInfo:(NSString*)info;
@end
