//

//
//  Created by dyj on 16/2/26.
//  Copyright © 2016年 HLZ. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
@class CMPayModel;
@protocol CMSDKPayDelegate <NSObject>
@optional
- (void)getPayTypeListSuccess:(NSArray *)list;//获取支付列表成功
- (void)getPayTypeListFailure:(NSString *)message;//获取支付列表失败
- (void)cmPayResult:(NSString *)result;//支付结果0未支付/1支付成功/2支付异常
- (void)openAppSuccessed;//打开第三方应用成功的回调
- (void)openAppFailer:(NSString *)failer;//打开第三方应用失败的回调
@end
@interface CMPay : NSObject
/**
 @param applicationNumber 应用号
 @param key 应用AES加密的key
 @param iv 应用AES加密的向量
 @param delegate 获取成功的回调
 */
+ (void)getPayTypeListWithApplicationNumber:(NSString*)applicationNumber aesKey:(NSString*)key andAesIv:(NSString*)iv returnDelegate:(id<CMSDKPayDelegate>)delegate;


/**
 @param payModel 参数模型
 @param delegate 指定代理对象
 */
+ (void)payByCMPayModel:(CMPayModel *)payModel delegate:(id<CMSDKPayDelegate>)delegate;

//以下为简化版与完整版的共同方法

+ (void)applicationWillEnterForeground;

/**
 *  Log 输出开关 (默认关闭)
 *
 *  @param flag 是否开启
 */
+ (void)setLogEnable:(BOOL)flag;


@end
