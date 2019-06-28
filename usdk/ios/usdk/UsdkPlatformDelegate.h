@protocol UsdkPlatformDelegate <NSObject>
//@optional
@required
- (void) setSdkCallBackReceiver:(NSString*)receiverName;
- (void) login;
- (void) logout;
- (void) openUserCenter;
- (void) exitGame;
- (void) pay;
- (void) releaseSdkResource;
- (void) switchAccount;
- (void) setProductIdentifiers:(NSArray*)identifers;
@end
