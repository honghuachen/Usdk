#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

/**
 @breif 购买信息属性
 **/

@interface UsdkPayInfo : NSObject
//@property (nonatomic, copy) NSString *tradeSN;
//@property (nonatomic, copy) NSString *tokenId;
@property (nonatomic, copy) NSString *uid;
@property (nonatomic, copy) NSString *productId;
@property (nonatomic, copy) NSString *productName;
@property (nonatomic, copy) NSString *productDesc;
@property (nonatomic, copy) NSString *productUnit;
@property (nonatomic, copy) NSString *productUnitPrice;
@property (nonatomic, copy) NSString *productQuantity;
@property (nonatomic, copy) NSString *totalAmount;
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

+(UsdkPayInfo*)Create:(NSString*)info;
@end
