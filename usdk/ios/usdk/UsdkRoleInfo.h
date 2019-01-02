#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

/**
 @breif 玩家信息属性
 **/
@interface UsdkRoleInfo : NSObject
@property (nonatomic, copy) NSString *uid;
@property (nonatomic, copy) NSString *roleId;
@property (nonatomic, copy) NSString *roleType;
@property (nonatomic, copy) NSString *roleLevel;
@property (nonatomic, copy) NSString *roleVipLevel;
@property (nonatomic, copy) NSString *serverId;
@property (nonatomic, copy) NSString *zoneId;
@property (nonatomic, copy) NSString *roleName;
@property (nonatomic, copy) NSString *serverName;
@property (nonatomic, copy) NSString *zoneName;
@property (nonatomic, copy) NSString *partyName;
@property (nonatomic, copy) NSString *gender;
@property (nonatomic, copy) NSString *balance;
@property (nonatomic, copy) NSString *roleCreateTime;

+(UsdkRoleInfo*)Create:(NSString*)info;
@end
