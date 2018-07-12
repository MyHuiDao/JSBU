
//  Created by zhl on 2017/2/8.
//  Copyright © 2017年 HLZ. All rights reserved.
//
#import <Foundation/Foundation.h>
@interface DES : NSObject
+ (NSString *)encodeToPercentEscapeString:(NSString *) input;
+ (NSString *)md532BitUpper:(NSString *)inPutText;
+ (NSString *)md532BitLower:(NSString *)inPutText;
+ (NSString *)encryptWithContent:(NSString *)content type:(int)type key:(NSString*)aKey;
+ (NSString *)replacenormalcharacter:(NSString *)normalcharacterstr;
+ (NSString *)replacespecialcharacter:(NSString *)specialcharacterstr;
@end
