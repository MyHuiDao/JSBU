//
//
//  Created by dyj on 16/8/19.
//  Copyright © 2016年 dyj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
NS_ASSUME_NONNULL_BEGIN

@interface CMPayModel : NSObject
@property (nonatomic, strong ) NSString         * key; //**加密向量//
@property (nonatomic, strong ) NSString         * iv; //**加密密钥//
@property (nonatomic, strong ) UIViewController * controler;    //当前显示的页面
@property (nonatomic, strong ) NSString         * weichatAppid;    //可选参数
@property (nonatomic, strong ) NSString         * p1_yingyongnum; //商户应用编号//
@property (nonatomic, strong ) NSString         * p2_ordernumber; //商户订单号//
@property (nonatomic, strong ) NSString         * p3_money; //金额/格式最好是保留两位小数
@property (nonatomic, strong ) NSString         * p6_ordertime;//订单创建时间
@property (nonatomic, strong ) NSString         * p7_productcode;//支付方式代码
@property (nonatomic, strong ) NSString         * p8_sign;//MD5签名参数
@property (nullable, nonatomic ,copy)NSDictionary       * parameterDic;//可选参数 可空
@end
NS_ASSUME_NONNULL_END
