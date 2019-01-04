//
//  UsdkPayInfo.m
//  Unity-iPhone
//
//  Created by Cassie on 2019/1/2.
//

#import <Foundation/Foundation.h>
#import "UsdkPayInfo.h"

@implementation UsdkPayInfo
+(UsdkPayInfo*)Create:(NSString*)info
{
    UsdkPayInfo *payInfo = [[UsdkPayInfo alloc] init];
    NSMutableDictionary *dic = [[NSMutableDictionary alloc] init];
    NSArray *propertys = [info componentsSeparatedByString:@"&"];
    for(int i = 0;i < [propertys count];i++)
    {
        NSArray *property = [propertys[i] componentsSeparatedByString:@"="];
        if([property count] == 2)
        {
            [dic setObject:property[1] forKey:property[0]];
        }
    }
    
    
    //payInfo.tradeSN = [dic valueForKey:@"tradeSN"] ? [dic valueForKey:@"tradeSN"] : @"";
    //payInfo.tokenId = [dic valueForKey:@"tokenId"] ? [dic valueForKey:@"tokenId"] : @"";
    //payInfo.accountID = [dic valueForKey:@"uid"] ? [dic valueForKey:@"uid"] : @"";
    payInfo.uid = [dic valueForKey:@"uid"] ? [dic valueForKey:@"uid"] : @"";
    payInfo.productId = [dic valueForKey:@"productId"] ? [dic valueForKey:@"productId"] : @"";
    payInfo.productName = [dic valueForKey:@"productName"] ? [dic valueForKey:@"productName"] : @"";
    payInfo.productUnit = [dic valueForKey:@"productUnit"] ? [dic valueForKey:@"productUnit"] : @"";
    payInfo.productQuantity = [dic valueForKey:@"productQuantity"] ? [dic valueForKey:@"productQuantity"] : @"";
    payInfo.productUnitPrice = [dic valueForKey:@"productUnitPrice"] ? [dic valueForKey:@"productUnitPrice"] : @"";
    payInfo.totalAmount = [dic valueForKey:@"totalAmount"] ? [dic valueForKey:@"totalAmount"] : @"";
    payInfo.payAmount = [dic valueForKey:@"payAmount"] ? [dic valueForKey:@"payAmount"] : @"";
    payInfo.productDesc = [dic valueForKey:@"productDesc"] ? [dic valueForKey:@"productDesc"] : @"";
    payInfo.currencyName = [dic valueForKey:@"currencyName"] ? [dic valueForKey:@"currencyName"] : @"";
    payInfo.roleId = [dic valueForKey:@"roleId"] ? [dic valueForKey:@"roleId"] : @"";
    payInfo.roleName = [dic valueForKey:@"roleName"] ? [dic valueForKey:@"roleName"] : @"";
    payInfo.roleLevel = [dic valueForKey:@"roleLevel"] ? [dic valueForKey:@"roleLevel"] : @"";
    payInfo.roleVipLevel = [dic valueForKey:@"roleVipLevel"] ? [dic valueForKey:@"roleVipLevel"] : @"";
    payInfo.serverId = [dic valueForKey:@"serverId"] ? [dic valueForKey:@"serverId"] : @"";
    payInfo.serverName = [dic valueForKey:@"serverName"] ? [dic valueForKey:@"serverName"] : @"";
    payInfo.zoneId = [dic valueForKey:@"zoneId"] ? [dic valueForKey:@"zoneId"] : @"";
    payInfo.partyName = [dic valueForKey:@"partyName"] ? [dic valueForKey:@"partyName"] : @"";
    payInfo.virtualCurrencyBalance = [dic valueForKey:@"virtualCurrencyBalance"] ? [dic valueForKey:@"virtualCurrencyBalance"] : @"";
    payInfo.customInfo = [dic valueForKey:@"customInfo"] ? [dic valueForKey:@"customInfo"] : @"";
    payInfo.gameTradeNo = [dic valueForKey:@"gameTradeNo"] ? [dic valueForKey:@"gameTradeNo"] : @"";
    payInfo.gameCallbackUrl = [dic valueForKey:@"gameCallbackUrl"] ? [dic valueForKey:@"gameCallbackUrl"] : @"";
    payInfo.additionalParams = [dic valueForKey:@"additionalParams"] ? [dic valueForKey:@"additionalParams"] : @"";
    payInfo.transaction_id = [dic valueForKey:@"transaction_id"] ? [dic valueForKey:@"transaction_id"] : @"";
    payInfo.pay_type = [dic valueForKey:@"pay_type"] ? [dic valueForKey:@"pay_type"] : @"";
    return payInfo;
}
@end
