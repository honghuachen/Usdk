//
//  LGiOSUtilsHelper.mm
//  Unity-iPhone
//
//  Created by bytedance on 2021/3/28.
//

#import "UsdkUtils.h"

const char* MakeStringCopy(NSString * str)
{
    const char* string = [str UTF8String];
    if (NULL == string) {
        return NULL;
    }
    char* res = (char*)malloc(strlen(string)+1);
    strcpy(res, string);
    return res;
}

@implementation UsdkUtils

+ (NSString *)convertDictionaryToJsonString:(NSDictionary *)dictionary {
    if (dictionary == nil) return @"";
    
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary 
                                                       options:NSJSONWritingFragmentsAllowed
                                                         error:&error];
    NSString *jsonString = @"";
    if(!error) {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    } 
    return jsonString;
}

+ (NSString *)convertArrayToJsonString:(NSArray *)array {
    if (array == nil) return @"";
    
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:array
                                                       options:NSJSONWritingFragmentsAllowed
                                                         error:&error];
    NSString *jsonString = @"";
    if(!error) {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
    return jsonString;
}

+ (NSString *)stringWithUTF8String:(const char*)charString {
    if (charString) {
        return [NSString stringWithUTF8String:charString];
    }
    return @"";
}

@end


