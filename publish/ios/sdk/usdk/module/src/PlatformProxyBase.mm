#import "PlatformProxyBase.h"

@implementation PlatformProxyBase
- (void) setSdkCallBackReceiver:(NSString*) receiver_name{}
- (void) login:(NSString*)arg{}
- (void) logout:(NSString*) arg{}
- (void) openUserCenter:(NSString*) arg{}
- (void) exitGame:(NSString*) arg{}
- (void) payStart:(NSString*)product_id amount:(int)amount{}
- (void) pay:(NSString*) pay_info{}
- (void) releaseSdkResource:(NSString*)arg{}
- (void) switchAccount:(NSString*) arg{}
- (void) setProductIdentifiers:(NSArray*)identifers{}

- (UsdkRoleInfo*)CreateRoleInfo:(NSString*)info
{
    return [UsdkRoleInfo Create:info];
}

- (UsdkPayInfo*)CreatePayInfo:(NSString*)info
{   
    return [UsdkPayInfo Create:info];
}
@end

