#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "IUsdkBase.h"
@interface UsdkBase :NSObject<IUsdkBase>
- (NSString*)getConfig:(NSString*)key;
@end
