//
//  NetworkBackServiceDelegate.h
//  Unity-iPhone
//
//  Created by 李城宗 on 2018/4/27.
//
//

#import <Foundation/Foundation.h>
#import "WXApi.h"
#import <CMergeSDK/CMergeSDK.h>

@interface NetworkBackServiceDelegate : NSObject<WXApiDelegate,CMSDKPayDelegate>
+(instancetype) sharedInstance;
-(void)showHUD:(BOOL)isShow;
@end
