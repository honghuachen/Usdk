#import "UsdkBase.h"

@implementation UsdkBase
- (NSString*) getConfig:(NSString*)key{return @"";}
- (void)sendDict2Unity:(NSString *)method param:(NSDictionary *)dict
{
    NSString *param = @"";
    for (NSString *key in dict)
    {
        if ([param length] == 0)
            param = [param stringByAppendingFormat:@"%@=%@", key, [dict valueForKey:key]];
        else
            param = [param stringByAppendingFormat:@"&%@=%@", key, [dict valueForKey:key]];
    }
    
    [self sendString2Unity:method param:param];
}

- (void)sendString2Unity:(NSString *)method param:(NSString *)str
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

- (void)sendCallBack2Unity:(UsdkCallBackErrorCode)code Param:(NSString *)str
{
    NSString *param = @"";
    if(str == nil || !str.length)
        param = [NSString stringWithFormat:@"errorCode=%d", (int)code];
    else
        param = [NSString stringWithFormat:@"errorCode=%d&ret=%@", (int)code,str];
    [self sendString2Unity:@"CallBack" param:param];
}
@end
