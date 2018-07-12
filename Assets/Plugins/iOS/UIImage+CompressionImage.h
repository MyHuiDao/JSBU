//
//  UIImage+CompressionImage.h
//  Unity-iPhone
//
//  Created by chx huihuang on 2018/5/3.
//

#import <UIKit/UIKit.h>

@interface UIImage (CompressionImage)
//将图片压缩到指定大小
+(UIImage*) ScaleImage:(UIImage*) image toKB:(NSUInteger)kb;
+(UIImage *)compressOriginalImage:(UIImage *)image toSize:(CGSize)size;
+ (NSData *)compressOriginalImage:(UIImage *)image toMaxDataSizeKBytes:(CGFloat)size;
+(NSString *)UIImageToBase64Str:(UIImage *) image maxDataLength:(CGFloat) len;
+(float)ImageKb:(UIImage*) image;
@end
