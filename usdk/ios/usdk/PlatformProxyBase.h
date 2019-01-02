#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UsdkBase.h"
#import "IUsdkBase.h"

/**
 @breif 玩家信息属性
 **/
@interface SDKRoleInfo : NSObject
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
@end

/**
 @breif 购买信息属性
 **/

@interface SDKPayInfo : NSObject
//@property (nonatomic, copy) NSString *tradeSN;
//@property (nonatomic, copy) NSString *tokenId;
@property (nonatomic, copy) NSString *uid;
@property (nonatomic, copy) NSString *productId;
@property (nonatomic, copy) NSString *productName;
@property (nonatomic, copy) NSString *productDesc;
@property (nonatomic, copy) NSString *productUnit;
@property (nonatomic, copy) NSString *productUnitPrice;
@property (nonatomic, copy) NSString *productQuantity;
@property (nonatomic, copy) NSString *totalAmount;//需与payAmount一致
@property (nonatomic, copy) NSString *payAmount;
@property (nonatomic, copy) NSString *currencyName;
@property (nonatomic, copy) NSString *roleId;
@property (nonatomic, copy) NSString *roleName;
@property (nonatomic, copy) NSString *roleLevel;
@property (nonatomic, copy) NSString *roleVipLevel;
@property (nonatomic, copy) NSString *serverId;
@property (nonatomic, copy) NSString *serverName;
@property (nonatomic, copy) NSString *zoneId;
@property (nonatomic, copy) NSString *partyName;
@property (nonatomic, copy) NSString *virtualCurrencyBalance;
@property (nonatomic, copy) NSString *customInfo;
@property (nonatomic, copy) NSString *gameTradeNo;
@property (nonatomic, copy) NSString *gameCallbackUrl;
@property (nonatomic, copy) NSString *additionalParams;
@property (nonatomic, copy) NSString *transaction_id;
@property (nonatomic, copy) NSString *pay_type;
@end

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
-(SDKRoleInfo*)CreateRoleInfo:(NSString*)info;
-(SDKPayInfo*)CreatePayInfo:(NSString*)info;
@end
