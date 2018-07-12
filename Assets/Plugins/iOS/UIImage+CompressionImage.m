//
//  UIImage+CompressionImage.m
//  Unity-iPhone
//
//  Created by chx huihuang on 2018/5/3.
//

#import "UIImage+CompressionImage.h"

@implementation UIImage (CompressionImage)
+(UIImage *)ScaleImage:(UIImage *)image toKB:(NSUInteger)kb
{
    if (!image) {
        return image;
    }
    if (kb < 1) {
        return image;
    }
    
    kb*=1024;
    
    
    
    CGFloat compression = 0.8f;
    CGFloat maxCompression = 0.07f;
    NSData *imageData = UIImageJPEGRepresentation(image, compression);
    while ([imageData length] > kb && compression > maxCompression) {
        compression -= 0.07f;
        imageData = UIImageJPEGRepresentation(image, compression);
    }
    NSLog(@"当前大小:%fkb",(float)[imageData length]/1024.0f);
    UIImage *compressedImage = [UIImage imageWithData:imageData];
    return compressedImage;

}

+(float)ImageKb:(UIImage*) image
{
    NSData *imageData = UIImageJPEGRepresentation(image, 0.5);
    float f = ([imageData length] / 1024);
    return f;
}

//压缩图片到指定尺寸大小
+(UIImage *)compressOriginalImage:(UIImage *)image toSize:(CGSize)NewSize{
    
    CGSize imageSize = image.size;
    CGFloat width = imageSize.width;
    CGFloat height = imageSize.height;
    
    if (width <= NewSize.width && height <= NewSize.height){
        return image;
    }
    
    if (width == 0 || height == 0){
        return image;
    }
    
    CGFloat widthFactor = NewSize.width / width;
    CGFloat heightFactor = NewSize.height / height;
    CGFloat scaleFactor = (widthFactor<heightFactor?widthFactor:heightFactor);
    
    CGFloat scaledWidth = width * scaleFactor;
    CGFloat scaledHeight = height * scaleFactor;
    CGSize targetSize = CGSizeMake(scaledWidth,scaledHeight);
    
    UIGraphicsBeginImageContext(targetSize);
    [image drawInRect:CGRectMake(0,0,scaledWidth,scaledHeight)];
    UIImage* newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return newImage;
}

/**
 *  压缩图片到指定文件大小
 *
 *  @param image 目标图片
 *  @param size  目标大小（最大值）
 *
 *  @return 返回的图片文件
 */
+ (NSData *)compressOriginalImage:(UIImage *)image toMaxDataSizeKBytes:(CGFloat)size{
    NSData * data = UIImageJPEGRepresentation(image, 1.0);
    CGFloat dataKBytes = data.length/1024.0;
    CGFloat maxQuality = 0.9f;
    CGFloat lastData = dataKBytes;
    while (dataKBytes > size && maxQuality > 0.1f) {
        maxQuality = maxQuality - 0.1f;
        data = UIImageJPEGRepresentation(image, maxQuality);
        dataKBytes = data.length / 1024.0;
        if (lastData == dataKBytes) {
            break;
        }else{
            lastData = dataKBytes;
        }
    }
    return data;
}


//base64压缩
+(NSString *)UIImageToBase64Str:(UIImage *) image maxDataLength:(CGFloat) len
{
    CGFloat quality = 0.5;
    NSData *data = UIImageJPEGRepresentation(image, quality);
    while ((data.length/1024) > len && quality > 0.01f) {
        quality = quality - 0.05;
        data = UIImageJPEGRepresentation(image, quality);
    }
    NSString *encodedImageStr = [data base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
    return encodedImageStr;
}
@end
