//
//  HLMBProgressHUD.h
//  istaymall
//
//  Created by dyj on 16/3/26.
//  Copyright © 2016年 Tangshan Jun-ho Technology Co.Ltd. All rights reserved.
//

#import "MBProgressHUD.h"

@interface HLMBProgressHUD : MBProgressHUD
+(void)addMBProgressHUDinView:(UIView *)view hudMode:(MBProgressHUDMode)mode hideDelay:(NSTimeInterval)timeInterval hudDetailText:(NSString *)text;
@end
