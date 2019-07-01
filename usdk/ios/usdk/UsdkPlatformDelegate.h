@protocol UsdkPlatformDelegate <NSObject>
//@optional
@required
- (void) login;
- (void) logout;
- (void) openUserCenter;
- (void) exitGame;
- (void) pay;
- (void) releaseSdkResource;
- (void) switchAccount;
@end
