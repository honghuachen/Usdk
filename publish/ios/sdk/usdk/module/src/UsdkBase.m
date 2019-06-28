#import "UsdkBase.h"

@implementation UsdkBase
- (NSString*) getConfig:(NSString*)key{return @"";}
- (void)sendDict2U3d:(NSString *)method param:(NSDictionary *)dict
{
    NSString *param = @"";
    for (NSString *key in dict)
    {
        if ([param length] == 0)
            param = [param stringByAppendingFormat:@"%@=%@", key, [dict valueForKey:key]];
        else
            param = [param stringByAppendingFormat:@"&%@=%@", key, [dict valueForKey:key]];
    }
    
    [self sendString2U3d:method param:param];
}

- (void)sendString2U3d:(NSString *)method param:(NSString *)str
{
    NSLog(@"SDKCallBack with method=%@, param=%@",method,str);
    UnitySendMessage("UsdkCallBack", [method UTF8String], [str UTF8String]);
}

- (UsdkRoleInfo*)createRoleInfo:(NSString *)info
{
    return [UsdkRoleInfo Create:info];
}

- (UsdkPayInfo*)createPayInfo:(NSString *)info
{
    return [UsdkPayInfo Create:info];
}

- (void)callBack2U3d:(NSString *)method Code:(NSInteger *)code Result:(NSString *)ret
{
    NSString *param =  [NSString stringWithFormat:@"code=%d&ret=%@", (int)code,ret];
    [self sendString2U3d:method param:param];
}
@end

