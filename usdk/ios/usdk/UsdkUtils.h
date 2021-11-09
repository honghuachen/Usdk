// 字符串转换 NSString -> char *
extern const char* MakeStringCopy(NSString * str);

@interface UsdkUtils : NSObject

+ (NSString *)convertDictionaryToJsonString:(NSDictionary *)dictionary;
+ (NSString *)convertArrayToJsonString:(NSArray *)array;
+ (NSString *)stringWithUTF8String:(const char*)charString;

@end
