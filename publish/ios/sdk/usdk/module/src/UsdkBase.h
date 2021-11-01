#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "UsdkApplicationDelegate.h"
#import "Usdk.h"

@interface UsdkBase : NSObject
- (void)sendCallBack2Unity:(NSString *)eventName Param:(NSString *)ret;
@end
