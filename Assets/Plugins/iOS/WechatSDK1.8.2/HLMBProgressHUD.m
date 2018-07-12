//
//  HLMBProgressHUD.m
//  istaymall
//
//  Created by dyj on 16/3/26.
//  Copyright © 2016年 Tangshan Jun-ho Technology Co.Ltd. All rights reserved.
//

#import "HLMBProgressHUD.h"

@implementation HLMBProgressHUD
+(void)addMBProgressHUDinView:(UIView *)view hudMode:(MBProgressHUDMode)mode hideDelay:(NSTimeInterval)timeInterval hudDetailText:(NSString *)text{
    MBProgressHUD *hud=[[MBProgressHUD alloc]initWithView:view];
    hud.mode=mode;
    hud.detailsLabelText =text;
    hud.animationType =2;
    [[UIApplication sharedApplication].keyWindow addSubview:hud];
    [hud show:YES];
    [hud hide:YES afterDelay:timeInterval];
    
}
@end
