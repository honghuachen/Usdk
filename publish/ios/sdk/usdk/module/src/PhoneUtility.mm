#import <sys/sysctl.h>
#import <sys/socket.h>
#import <net/if.h>
#import <net/if_dl.h>
#import <AdSupport/ASIdentifierManager.h>
#import <CommonCrypto/CommonDigest.h>
#import <AudioToolbox/AudioServices.h>
#import "UICKeyChainStore.h"
#import <AdSupport/AdSupport.h>

extern "C"
{
    
    char* makeStringCopy(const char* string)
    {
        if (NULL == string) {
            return NULL;
        }
        char* res = (char*)malloc(strlen(string)+1);
        strcpy(res, string);
        return res;
    }

    const char* _getIDFA()
    {
        NSString *str = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
        NSLog(@"advertisingIdentifier IDFA=%@", str );
        return makeStringCopy([str UTF8String]);
    }

    const char* _getCountry(){
        NSString *country = [[NSLocale currentLocale] objectForKey:NSLocaleCountryCode];
        return makeStringCopy([country UTF8String]);
    }
    
    const char* _getLanguage(){
        NSString *language = [[NSLocale currentLocale] objectForKey:NSLocaleLanguageCode];
        return makeStringCopy([language UTF8String]);
    }
    
    const char* _deviceUDID()
    {
        // NSString *uuid = [UICKeyChainStore stringForKey:@"kKeychainTPS01Identifier" service:@"kKeychainTapEnjoyService"];
        // if(!uuid){
        //     CFUUIDRef uuidRef = CFUUIDCreate(kCFAllocatorDefault);
        //     CFStringRef cfstring = CFUUIDCreateString(kCFAllocatorDefault, uuidRef);
        //     const char *cStr = CFStringGetCStringPtr(cfstring,CFStringGetFastestEncoding(cfstring));
        //     uuid = [NSString stringWithUTF8String:cStr];
        //     CFRelease(uuidRef);
        //     CFRelease(cfstring);
            
        //     UICKeyChainStore *store = [UICKeyChainStore keyChainStoreWithService:@"kKeychainTapEnjoyService"];
        //     [store setString:uuid forKey:@"kKeychainTPS01Identifier"];
        //     [store synchronize];
        // }
        // NSLog(@"getUUIDFromKeychain open_udid=%@", uuid );

        NSString *uuid;
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory,NSUserDomainMask, YES);
        NSString *path = [paths.firstObject stringByAppendingPathComponent:@"_profile_.id"];
        NSError *error;
        NSString *str = [NSString stringWithContentsOfFile:path encoding:NSUTF8StringEncoding error:&error];
        if (error == nil) {
            NSLog(@"本地文件读取成功:%@", str);
            uuid = str;
            UICKeyChainStore *keychainstore = [UICKeyChainStore keyChainStoreWithService:@"kKeychainTapEnjoyService"];
            [keychainstore setString:str forKey:@"kKeychainTPS01Identifier"];
        } else {
            NSLog(@"本地文件读取失败:%@", error);
        }
        if (!uuid) {
            uuid = [UICKeyChainStore stringForKey:@"kKeychainTPS01Identifier" service:@"kKeychainTapEnjoyService"];
        }
        if(!uuid){
            CFUUIDRef uuidRef = CFUUIDCreate(kCFAllocatorDefault);
            CFStringRef cfstring = CFUUIDCreateString(kCFAllocatorDefault, uuidRef);
            const char *cStr = CFStringGetCStringPtr(cfstring,CFStringGetFastestEncoding(cfstring));
            uuid = [NSString stringWithUTF8String:cStr];
            CFRelease(uuidRef);
            CFRelease(cfstring);
            
            UICKeyChainStore *store = [UICKeyChainStore keyChainStoreWithService:@"kKeychainTapEnjoyService"];
            [store setString:uuid forKey:@"kKeychainTPS01Identifier"];
        }
        NSLog(@"getUUIDFromKeychain open_udid=%@", uuid );

        if( [uuid writeToFile:path atomically:YES encoding:NSUTF8StringEncoding error:&error] ) {
            NSLog(@"写入成功");
        } else {
            NSLog(@"写入失败:%@", error);
        }
        return makeStringCopy([uuid UTF8String]);
    }
    
    void _copyToSystem(const char* text)
    {
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string = [NSString stringWithUTF8String:text];
    }
    
    void _nslog(const char* logMessage)
    {
        NSLog(@"[TPS] %@", [NSString stringWithUTF8String:logMessage]);
    }

    bool _hasNotch()
    {
        if (@available(iOS 11.0, *)) {
            UIEdgeInsets edge = [[[UIApplication sharedApplication] keyWindow] safeAreaInsets];
            if (edge.left == UIEdgeInsetsZero.left && edge.right == UIEdgeInsetsZero.right && edge.top == UIEdgeInsetsZero.top && edge.bottom == UIEdgeInsetsZero.bottom) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }

    int _getNotchHeight()
    {
        if (@available(iOS 11.0, *)) {
            UIEdgeInsets edge = [[[UIApplication sharedApplication] keyWindow] safeAreaInsets];
            return edge.top;
        } else {
            return 0;
        }
    }
	
}
