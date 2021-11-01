#import "UsdkBase.h"

@implementation UsdkBase
- (NSString*) getConfig:(NSString*)key{return @"";}

- (void)sendCallBack2Unity:(NSString *)eventName Param:(NSString *)ret
{ 
    if([Usdk instance].usdkCallback)
        [Usdk instance].usdkCallback(MakeStringCopy(eventName),MakeStringCopy(ret));
}
@end
