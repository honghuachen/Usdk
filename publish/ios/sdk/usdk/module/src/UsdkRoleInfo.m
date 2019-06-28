//
//  UsdkRoleInfo.m
//  Unity-iPhone
//
//  Created by Cassie on 2019/1/2.
//

#import <Foundation/Foundation.h>
#import "UsdkRoleInfo.h"

@implementation UsdkRoleInfo
+(UsdkRoleInfo*)Create:(NSString*)info
{
    UsdkRoleInfo *roleInfo = [[UsdkRoleInfo alloc] init];
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
    
    roleInfo.uid = [dic valueForKey:@"uid"] ? [dic valueForKey:@"uid"] : @"";
    roleInfo.roleId = [dic valueForKey:@"roleId"] ? [dic valueForKey:@"roleId"] : @"";
    roleInfo.roleType = [dic valueForKey:@"roleType"] ? [dic valueForKey:@"roleType"] : @"";
    roleInfo.roleLevel = [dic valueForKey:@"roleLevel"] ? [dic valueForKey:@"roleLevel"] : @"";
    roleInfo.roleVipLevel = [dic valueForKey:@"roleVipLevel"] ? [dic valueForKey:@"roleVipLevel"] : @"";
    roleInfo.serverId = [dic valueForKey:@"serverId"] ? [dic valueForKey:@"serverId"] : @"";
    roleInfo.zoneId = [dic valueForKey:@"zoneId"] ? [dic valueForKey:@"zoneId"] : @"";
    roleInfo.roleName = [dic valueForKey:@"roleName"] ? [dic valueForKey:@"roleName"] : @"";
    roleInfo.serverName = [dic valueForKey:@"serverName"] ? [dic valueForKey:@"serverName"] : @"";
    roleInfo.zoneName = [dic valueForKey:@"zoneName"] ? [dic valueForKey:@"zoneName"] : @"";
    roleInfo.partyName = [dic valueForKey:@"partyName"] ? [dic valueForKey:@"partyName"] : @"";
    roleInfo.gender = [dic valueForKey:@"gender"] ? [dic valueForKey:@"gender"] : @"";
    roleInfo.balance = [dic valueForKey:@"balance"] ? [dic valueForKey:@"balance"] : @"";
    roleInfo.roleCreateTime = [dic valueForKey:@"roleCreateTime"] ? [dic valueForKey:@"roleCreateTime"] : @"";
    
    return roleInfo;
}
@end
