//
//  NetworkBackServiceDelegate.m
//  Unity-iPhone
//
//  Created by 李城宗 on 2018/4/27.
//
//

#import "NetworkBackServiceDelegate.h"
#import "HLMBProgressHUD.h"
#import "UIImage+CompressionImage.h"
#import <StoreKit/StoreKit.h>
//#import "AFNetworking.h"
#define kAPPLEORDERURL @"https://jinshayugang.com/game/pay/applePayOrder%@"//苹果订单
#define kNAPPORDERURL @"https://jinshayugang.com/game/pay/order%@"//支付宝订单
#define KSHAREURL @"https://jinshayugang.com"//分享下载链接
#define kSHARETOBACK @"https://jinshayugang.com/game/userShare/sharePromotion?userId="

#define kAPPLEORDERURLW @"http://192.168.31.214/game/pay/applePayOrder%@"//苹果订单
#define kNAPPORDERURLW @"http://192.168.31.214/game/pay/order%@"//支付宝订单
#define KSHAREURLW @"https://jinshayugang.com"//分享下载链接
#define kSHARETOBACKW @"http://192.168.31.214/game/userShare/sharePromotion?userId="


#define PAYSUCCESS @"SUCCESS"
#define PAYPREFIX @"jinshayugang_"

static NetworkBackServiceDelegate *instance;

@interface NetworkBackServiceDelegate()<SKPaymentTransactionObserver,SKProductsRequestDelegate,UIAlertViewDelegate>
@property(nonatomic,strong)NSString *ApplePayID;
@property(nonatomic,strong)NSString *orderId;
@property(nonatomic,strong)NSString *tokenStr;
//@property (nonatomic, strong) AFHTTPSessionManager *session;
@end

@implementation NetworkBackServiceDelegate

+(instancetype)allocWithZone:(struct _NSZone *)zone{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [super allocWithZone:zone ];
    });
    return instance;
}

+(instancetype)sharedInstance{
    if (instance == nil) {
        instance = [[super alloc] init];
        // 添加观察者
        //        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    }
    return instance;
}

-(id)copyWithZone:(NSZone *)zone{
    return instance;
}

-(id)mutableCopyWithZone:(NSZone *)zone{
    return instance;
}


//结束后一定要销毁
- (void)dealloc
{
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
}

#pragma mark --WXApiDelegate微信代理
-(void)onReq:(BaseReq *)req{
    if ([req isKindOfClass:[GetMessageFromWXReq class]]) {
        
    } else if ([req isKindOfClass:[ShowMessageFromWXReq class]]) {
        
    } else if ([req isKindOfClass:[LaunchFromWXReq class]]) {
        
    }
}
-(void)onResp:(BaseResp *)resp{
    if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        NSString *str = [self getSendMeaageState:resp.errCode];
        UIAlertView *alertView = [[UIAlertView alloc] initWithTitle:@"分享结果" message:str delegate:self cancelButtonTitle:@"确认" otherButtonTitles:nil, nil];
        [alertView show];
        
    } else if ([resp isKindOfClass:[SendAuthResp class]]) {
        SendAuthResp *sendResp = (SendAuthResp *)resp;
        NSString *code = sendResp.code;
        const char* codeC = [code cStringUsingEncoding:NSUTF8StringEncoding];
        //        NSLog(@"code:%@",code);
        
        UnitySendMessage("Wechat", "Access_code", codeC);
        
    } else if ([resp isKindOfClass:[AddCardToWXCardPackageResp class]]) {
        
        
    }else if([resp isKindOfClass:[PayResp class]]){
        //支付返回结果，实际支付结果需要去微信服务器端查询
        NSString *strMsg,*strTitle = [NSString stringWithFormat:@"支付结果"];
        
        switch (resp.errCode) {
            case WXSuccess:
                strMsg = @"支付结果：成功！";
                //                NSLog(@"支付成功－PaySuccess，retcode = %d", resp.errCode);
                break;
                
            default:
                strMsg = [NSString stringWithFormat:@"支付结果：失败！retcode = %d, retstr = %@", resp.errCode,resp.errStr];
                //                NSLog(@"错误，retcode = %d, retstr = %@", resp.errCode,resp.errStr);
                break;
        }
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:strTitle message:strMsg delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
        [alert show];
    }
}

-(NSString* )getSendMeaageState:(int)code
{
    if (code == 0) {
        
        return @"分享成功";
    }else{
        return @"没有分享";
    }
}

#pragma mark -----CMSDKPayDelegate   峻付通支付回调方法
////获取支付列表成功
- (void)getPayTypeListSuccess:(NSArray *)list{
    //    NSLog(@"支付列表:%@",list);
}
//获取支付列表失败
- (void)getPayTypeListFailure:(NSString *)message{
    //    NSLog(@"失败信息:%@",message);
}
//支付结果0未支付/1支付成功/2支付异常
- (void)cmPayResult:(NSString *)result{
    //    NSLog(@"result:%@",result);
    dispatch_async(dispatch_get_main_queue(), ^{
        [self showHUD:false];
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:[self StateStr:result]];
    });
    
    //     UnitySendMessage("Canvas", "payOfResult", [result UTF8String]);
}

-(NSString*)StateStr:(NSString *)result
{
    NSString *str = NULL;
    if ([result isEqualToString:@"0"]) {
        str = @"未支付";
    }
    else if ([result isEqualToString:@"1"])
    {
        str = @"支付成功";
    }
    else if ([result isEqualToString:@"2"])
    {
        str = @"支付异常";
    }
    else
        str = @"服务器出错";
    
    return str;
}
//打开第三方应用成功的回调
- (void)openAppSuccessed{
    //    NSLog(@"打开第三方应用成功");
    dispatch_async(dispatch_get_main_queue(), ^{
        [self showHUD:false];
    });
}
//打开第三方应用失败的回调
- (void)openAppFailer:(NSString *)failer{
    dispatch_async(dispatch_get_main_queue(), ^{
        [self showHUD:false];
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:failer];
    });
    
}


- ( void ) imageSaved: ( UIImage *) image didFinishSavingWithError:( NSError *)error
          contextInfo: ( void *) contextInfo
{
    NSLog(@"保存结束%@",contextInfo);
    if (error != nil) {
        //        NSLog(@"有错误");
    }
}



#pragma mark --SKProductsRequestDelegate
//接收到产品的返回信息,然后用返回的商品信息进行发起购买请求
-(void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response{
    NSArray *product = response.products;
    
    //如果服务器没有产品
    if([product count] == 0){
        //        NSLog(@"nothing");
        return;
    }else{
        NSLog(@"产品数量为:%lu",(unsigned long)[response.products count]);
    }
    
    SKProduct *requestProduct = nil;
    for (SKProduct *pro in product) {
        
        NSLog(@"%@", [pro description]);
        NSLog(@"%@", [pro localizedTitle]);
        NSLog(@"%@", [pro localizedDescription]);
        NSLog(@"%@", [pro price]);
        NSLog(@"%@", [pro productIdentifier]);
        
        // 11.如果后台消费条目的ID与我这里需要请求的一样（用于确保订单的正确性）
        if([pro.productIdentifier isEqualToString:self.ApplePayID]){
            requestProduct = pro;
        }
    }
    
    // 12.发送购买请求
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    SKPayment *payment = [SKPayment paymentWithProduct:requestProduct];
    [[SKPaymentQueue defaultQueue] addPayment:payment];
    
    
}

//反馈请求的产品结束
-(void)requestDidFinish:(SKRequest *)request
{
    //    NSLog(@"FFFFFFFFFFFFFFFFFFFFFFFF");
}
//请求失败
-(void)request:(SKRequest *)request didFailWithError:(NSError *)error
{
    [self showHUD:false];
    [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"请求错误"];
}

#pragma mark --SKPaymentTransactionObserver

//13.监听购买结果
// Sent when the transaction array has changed (additions or state changes).  Client should check state of transactions and finish as appropriate.
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray<SKPaymentTransaction *> *)transactions
{
    for(SKPaymentTransaction *tran in transactions){
        
        switch (tran.transactionState) {
            case SKPaymentTransactionStatePurchased:
                NSLog(@"交易完成");
                [queue finishTransaction:tran];
                [self completeTransaction:tran];
                break;
            case SKPaymentTransactionStatePurchasing:
                NSLog(@"商品添加进列表");
                break;
            case SKPaymentTransactionStateRestored:
                NSLog(@"已经购买过商品");
                [self showHUD:false];
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];
                break;
            case SKPaymentTransactionStateFailed:
                NSLog(@"交易失败");
                [self showHUD:false];
                [[SKPaymentQueue defaultQueue] finishTransaction:tran];
                break;
            default:
                break;
        }
        
    }
}

// Sent when transactions are removed from the queue (via finishTransaction:).
//- (void)paymentQueue:(SKPaymentQueue *)queue removedTransactions:(NSArray<SKPaymentTransaction *> *)transactions{
//    NSLog(@"0000000000000000000000000");
//}
//
//// Sent when an error is encountered while adding transactions from the user's purchase history back to the queue.
//- (void)paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error{
//    NSLog(@"1111111111111111111111111");
//}
//
//// Sent when all transactions from the user's purchase history have successfully been added back to the queue.
//- (void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue NS_AVAILABLE_IOS(3_0){
//    NSLog(@"2222222222222222222222222");
//}
//
//// Sent when the download state has changed.
//- (void)paymentQueue:(SKPaymentQueue *)queue updatedDownloads:(NSArray<SKDownload *> *)downloads {
//    NSLog(@"333333333333333333333333333");
//}


// Sent when a user initiates an IAP buy from the App Store
//- (BOOL)paymentQueue:(SKPaymentQueue *)queue shouldAddStorePayment:(SKPayment *)payment forProduct:(SKProduct *)product{
//    NSLog(@"444444444444444444444444444");
//    return true;
//}

//交易结束
-(void)completeTransaction:(SKPaymentTransaction *)transaction
{
    NSData *receiptData = [NSData dataWithContentsOfURL:[[NSBundle mainBundle] appStoreReceiptURL]];
    if (receiptData == NULL) {
        NSLog(@"验证信息为空");
        return;
    }
    NSString * str=[[NSString alloc]initWithData:/*receiptData*/transaction.transactionReceipt encoding:NSUTF8StringEncoding];
    
    //    NSLog(@"-----kkkkk --------%@",str);
    
    NSString *environment=[self environmentForReceipt:str];
    //    NSLog(@"----- 完成交易调用的方法completeTransaction 1--------%@",environment);
    
    
    // 验证凭据，获取到苹果返回的交易凭据
    // appStoreReceiptURL iOS7.0增加的，购买交易完成后，会将凭据存放在该地址
    //    NSURL *receiptURL = [[NSBundle mainBundle] appStoreReceiptURL];
    // 从沙盒中获取到购买凭据
    //    NSData *receiptData = [NSData dataWithContentsOfURL:receiptURL];
    /**
     20      BASE64 常用的编码方案，通常用于数据传输，以及加密算法的基础算法，传输过程中能够保证数据传输的稳定性
     21      BASE64是可以编码和解码的
     22      */
    NSString *encodeStr = [receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];
    NSString *sendString = [NSString stringWithFormat:@"{\"receipt-data\" : \"%@\"}", encodeStr];
    const char* sendChar = [sendString cStringUsingEncoding:NSUTF8StringEncoding];
    
    if (self.orderId == NULL || environment == NULL || sendString == NULL) {
        [self showHUD:false];
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"交易数据有有误"];
        return;
    }
    //将购买过的持久化
    [self storeOrderID:self.orderId Enviroment:environment PingZ:sendString];
    
    UnitySendMessage("buttonBehind", "applePayBackHJ",[environment isEqualToString:@"environment=Sandbox"]?[@"0" cStringUsingEncoding:NSUTF8StringEncoding] : [@"1" cStringUsingEncoding:NSUTF8StringEncoding]);
    UnitySendMessage("buttonBehind", "applePayBackPZ",sendChar);
    
    //    NSLog(@"sendStr:%@",encodeStr);
    //将sendString 发送服务器
    //    [self httpPostVoucher:sendString OrderId:self.orderId Type:0];
    
    
    //以下是向苹果服务器请求验证过程
    /*
     NSURL *StoreURL=nil;
     if ([environment isEqualToString:@"environment=Sandbox"]) {
     
     StoreURL= [[NSURL alloc] initWithString: @"https://sandbox.itunes.apple.com/verifyReceipt"];
     }
     else{
     
     StoreURL= [[NSURL alloc] initWithString: @"https://buy.itunes.apple.com/verifyReceipt"];
     }
     //这个二进制数据由服务器进行验证；zl
     NSData *postData = [NSData dataWithBytes:[sendString UTF8String] length:[sendString length]];
     
     //    NSLog(@"++++++%@",postData);
     NSMutableURLRequest *connectionRequest = [NSMutableURLRequest requestWithURL:StoreURL];
     
     [connectionRequest setHTTPMethod:@"POST"];
     [connectionRequest setTimeoutInterval:50.0];//120.0---50.0zl
     [connectionRequest setCachePolicy:NSURLRequestUseProtocolCachePolicy];
     [connectionRequest setHTTPBody:postData];
     
     //开始请求
     NSError *error=nil;
     NSData *responseData=[NSURLConnection sendSynchronousRequest:connectionRequest returningResponse:nil error:&error];
     if (error) {
     NSLog(@"验证购买过程中发生错误，错误信息：%@",error.localizedDescription);
     return;
     }
     NSDictionary *dic=[NSJSONSerialization JSONObjectWithData:responseData options:NSJSONReadingAllowFragments error:nil];
     NSLog(@"请求成功后的数据:%@",dic);
     //这里可以等待上面请求的数据完成后并且state = 0 验证凭据成功来判断后进入自己服务器逻辑的判断,也可以直接进行服务器逻辑的判断,验证凭据也就是一个安全的问题。楼主这里没有用state = 0 来判断。
     //  [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
     
     NSString *product = transaction.payment.productIdentifier;
     
     NSLog(@"transaction.payment.productIdentifier++++%@",product);
     
     if ([product length] > 0)
     {
     NSArray *tt = [product componentsSeparatedByString:@"."];
     
     NSString *bookid = [tt lastObject];
     
     if([bookid length] > 0)
     {
     
     NSLog(@"打印bookid%@",bookid);
     //这里可以做操作吧用户对应的虚拟物品通过自己服务器进行下发操作,或者在这里通过判断得到用户将会得到多少虚拟物品,在后面（[self getApplePayDataToServerRequsetWith:transaction];的地方）上传上面自己的服务器。
     }
     }
     //此方法为将这一次操作上传给我本地服务器,记得在上传成功过后一定要记得销毁本次操作。调用[[SKPaymentQueue defaultQueue] finishTransaction: transaction];
     //    [self getApplePayDataToServerRequsetWith:transaction];
     */
}

-(NSString * )environmentForReceipt:(NSString * )str
{
    str= [str stringByReplacingOccurrencesOfString:@"\r\n" withString:@""];
    
    str = [str stringByReplacingOccurrencesOfString:@"\n" withString:@""];
    
    str = [str stringByReplacingOccurrencesOfString:@"\t" withString:@""];
    
    str=[str stringByReplacingOccurrencesOfString:@" " withString:@""];
    
    str=[str stringByReplacingOccurrencesOfString:@"\"" withString:@""];
    
    NSArray * arr=[str componentsSeparatedByString:@";"];
    
    //存储收据环境的变量
    NSString * environment=arr[2];
    return environment;
}

-(void)storeOrderID:(NSString*)order Enviroment:(NSString*)enviroment PingZ:(NSString*)pingz
{
    [[NSUserDefaults standardUserDefaults] setObject:pingz  forKey:@"AlreadyPay"];
    [[NSUserDefaults standardUserDefaults] setObject:[enviroment isEqualToString:@"environment=Sandbox"]?@"0":@"1" forKey:@"environment"];
    [[NSUserDefaults standardUserDefaults] setObject:order forKey:@"orderID"];
    [[NSUserDefaults standardUserDefaults] synchronize];
}

-(NSString *)GetStoreMessage
{
    NSString *returnStr = NULL;
    NSString *pz = [[NSUserDefaults standardUserDefaults] objectForKey:@"AlreadyPay"];
    
    if ([pz isEqualToString:PAYSUCCESS] || pz == NULL) {
        returnStr = [[PAYSUCCESS stringByAppendingString:[@";" stringByAppendingString:PAYSUCCESS]] stringByAppendingString:[@";" stringByAppendingString:PAYSUCCESS]];
        NSLog(@"succcccc:%@",returnStr);
        
        
    }else{
        int envir = (int)[[NSUserDefaults standardUserDefaults] integerForKey:@"environment"];
        NSString *od = [[NSUserDefaults standardUserDefaults] objectForKey:@"orderID"];
        returnStr = [[od stringByAppendingString:[@";" stringByAppendingString:pz]] stringByAppendingString:[@";" stringByAppendingString:[NSString stringWithFormat:@"%d",envir]]];
    }
    return returnStr;
}

-(void)getRequestAppleProduct:(NSString *) str
{
    //    NSLog(@"str=======:%@",str);
    //购买之前先查询有没有购买完成的
    //    NSArray* transactions = [SKPaymentQueue defaultQueue].transactions;
    //    if (transactions.count > 0) {
    //        //检测是否有未完成的交易
    //        for (SKPaymentTransaction *transaction in transactions) {
    //            if (transaction.transactionState == SKPaymentTransactionStatePurchased) {
    //                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
    //            }
    //        }
    //    }
    
    NSArray *product = [[NSArray alloc] initWithObjects:str,nil];
    
    NSSet *nsset = [NSSet setWithArray:product];
    
    // 8.初始化请求
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:nsset];
    /*
     - (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
     - (void)requestDidFinish:(SKRequest *)request
     - (void)request:(SKRequest *)request didFailWithError:(NSError *)error
     */
    request.delegate = self;//在代理方法里获取所有可卖商品  SKProductsRequestDelegate
    
    // 9.开始请求
    [request start];
    
}


//向我方后台请求订单号
-(NSMutableURLRequest *)Reqest:(NSString*)urlStr Token:(NSString*)tokenStr
{
    //    NSLog(@"urlStr==============:%@",urlStr);
    NSURL *url = [NSURL URLWithString:urlStr];
    NSMutableURLRequest *mutaleUrlRequest = [NSMutableURLRequest requestWithURL:url cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:40];
    [mutaleUrlRequest addValue:tokenStr forHTTPHeaderField:@"token"];
    return mutaleUrlRequest;
}

#pragma mark UIAlertViewDelegate 代理
-(void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    //    NSLog(@"clickButtonAtIndex:%ld",(long)buttonIndex);
    UnitySendMessage("wechat_Share", "share_wechat", [kSHARETOBACK cStringUsingEncoding:NSUTF8StringEncoding]);
}


-(void)laterExecute
{
    [self showHUD:false];
}

//微信或支付宝支付
-(void)payRequest:(NSString*)RmbId andPc:(NSString*)Pc andMd:(NSString*)Md andToken:(NSString*)tekenStr
{
    [CMPay setLogEnable:YES];
    NSString *str = [[[[[@"?goldId=" stringByAppendingString:RmbId] stringByAppendingString:@"&productcode="]
                       stringByAppendingString:Pc] stringByAppendingString:@"&terminal="] stringByAppendingString:Md];
    NSString *urlStr = [NSString stringWithFormat:kNAPPORDERURL,str];
    NSMutableURLRequest *mutaleUrlRequest = [[NetworkBackServiceDelegate sharedInstance] Reqest:urlStr Token:tekenStr];//[NSMutableURLRequest
    NSURLSession *session = [NSURLSession sharedSession];
    NSURLSessionDataTask *task = [session dataTaskWithRequest:mutaleUrlRequest completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {
        
        NSHTTPURLResponse *ponse = (NSHTTPURLResponse*)response;
        if (ponse.statusCode == 200) {
            NSError*jsonError;
            NSDictionary *mingwenDic=[NSJSONSerialization
                                      JSONObjectWithData:data
                                      options:NSJSONReadingMutableContainers
                                      error:&jsonError];
            //            NSLog(@"mingwenDid:%@",mingwenDic);
            
            dispatch_async(dispatch_get_main_queue(), ^{
                
                CMPayModel *payModel = [CMPayModel new];
                payModel.key = @"d2574defee7b9b6437ea0061bd21aff2";
                payModel.iv  = @"a527d3a3175cfd35";
                payModel.controler = [UIApplication sharedApplication].keyWindow.rootViewController;
                payModel.p1_yingyongnum = mingwenDic[@"data"][@"p1_yingyongnum"];
                payModel.p2_ordernumber=mingwenDic[@"data"][@"p2_ordernumber"];
                payModel.p3_money = mingwenDic[@"data"][@"p3_money"];
                payModel.p6_ordertime = mingwenDic[@"data"][@"p6_ordertime"];
                payModel.p7_productcode = mingwenDic[@"data"][@"p7_productcode"];
                payModel.p8_sign = mingwenDic[@"data"][@"p8_sign"];
                payModel.parameterDic =@{@"p10_bank_card_code":@"",@"p11_cardtype":@"",@"p12_channel":@"",@"p13_orderfailertime":@"",@"p14_customname":@"",@"p15_customcontact ":@"",@"p16_customip":@"",@"p17_product":@"",@"p18_productcat":@"",@"p19_productnum":@"",@"p20_pdesc":@"",@"p21_version":@"",@"p23_charset":@"",@"p24_remark":@""};
                
                [CMPay payByCMPayModel:payModel delegate:[NetworkBackServiceDelegate sharedInstance]];
            });
        }else{
            [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
        }
    }];
    [task resume];
}

-(void)showHUD:(BOOL)isShow
{
    if (isShow) {
        [MBProgressHUD showHUDAddedTo:[UIApplication sharedApplication].keyWindow animated:YES];
    }else{
        [MBProgressHUD hideHUDForView:[UIApplication sharedApplication].keyWindow animated:YES];
    }
}


@end


extern "C" int _wxLogon()//微信登陆
{
    
    if (!WXApi.isWXAppInstalled) {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:3 hudDetailText:@"请安装微信或游客登陆"];
        return 1;
    }
    else if (!WXApi.isWXAppSupportApi){
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"该手机不支付微信"];
        return 1;
    }
    
    SendAuthReq *sendReq = [[SendAuthReq alloc] init];
    sendReq.scope = @"snsapi_userinfo";
    sendReq.state = @"wxLogon";
    [WXApi sendReq:sendReq];
    return 0;
}



//微信分享forUnity
extern "C" void _wxShare(const char *readAddr,const int state)//分享朋友圈
{
    if (!WXApi.isWXAppInstalled) {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:3 hudDetailText:@"请安装微信再操作"];
        return;
    }
    else if (!WXApi.isWXAppSupportApi){
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"该手机不支付微信"];
        return;
    }
    
    //使用ios截屏
    //        UIGraphicsBeginImageContextWithOptions([[[UIApplication sharedApplication] keyWindow] bounds].size, NO, 0.0);
    //        [[[UIApplication sharedApplication] keyWindow].layer renderInContext:UIGraphicsGetCurrentContext()];
    //        UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
    //        UIGraphicsEndImageContext();
    /////////////////////////////////////
    
    if (readAddr == NULL) {
        //        NSLog(@"路径为空%s:,State:%d",readAddr,state);
        return;
    }
    
    NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
    NSLog(@"strreadAddr:%@",strReadAddr);
    UIImage *strReadAddrImage = [UIImage imageWithContentsOfFile:strReadAddr];//需要分享的图片
    
    
    //    保存到相册
    //    UIImageWriteToSavedPhotosAlbum(strReadAddrImage, [NetworkBackServiceDelegate sharedInstance],@selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
    //    UIImage *shareImage = [UIImage imageNamed:@"share.png"];//分享图片测试
    
    
    //将文件压缩到指定大小
    UIImage *compressionImage = [UIImage compressOriginalImage:strReadAddrImage toSize:CGSizeMake(800, 640)];
    WXMediaMessage *message = [WXMediaMessage message];
    [message setThumbImage:compressionImage];
    
    //缩略图
    WXImageObject *imageObject = [WXImageObject object];
    NSData *data =[NSData dataWithContentsOfFile:strReadAddr] ;//[UIImage compressOriginalImage:compressionImage toMaxDataSizeKBytes:30];
    imageObject.imageData = data;
    message.mediaObject = imageObject;
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    if (state == 0) {req.scene = WXSceneSession;}
    else if(state == 1){req.scene = WXSceneTimeline;}
    else{req.scene = WXSceneFavorite;}
    
    [WXApi sendReq:req];
    
}


//微信分享链接
extern "C" void _wxShareLink(const char* title,const char* description)
{
    if (!WXApi.isWXAppInstalled) {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:3 hudDetailText:@"请安装微信再操作"];
        return;
    }
    else if (!WXApi.isWXAppSupportApi){
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"该手机不支付微信"];
        return;
    }
    
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = [NSString stringWithUTF8String:title];
    message.description = [NSString stringWithUTF8String:description];
    
    //    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"dd" ofType:@"png"];
    //    UIImage *image = [UIImage imageWithContentsOfFile:filePath];
    [message setThumbImage:[UIImage imageNamed:@"res2.png"]];
    
    WXWebpageObject *webpageObject = [WXWebpageObject object];
    webpageObject.webpageUrl = KSHAREURL;
    
    message.mediaObject = webpageObject;
    
    SendMessageToWXReq* rep = [[SendMessageToWXReq alloc] init];
    rep.bText = NO;
    rep.message = message;
    rep.scene = WXSceneTimeline;
    [WXApi sendReq:rep];
}

extern "C" void _unityRecharge(const char* pc,const char* md,const char* rmbId,const char *tekon,const char *applePayID)
{
    [[NetworkBackServiceDelegate sharedInstance] showHUD:true];
    NSString *Pc = [NSString stringWithUTF8String:pc];//@"ZFB";
    NSString *Md = [NSString stringWithUTF8String:md];
    NSString *RmbId = [NSString stringWithUTF8String:rmbId];
    NSString *tekenStr = [NSString stringWithUTF8String:tekon];
    [NetworkBackServiceDelegate sharedInstance].tokenStr = tekenStr;
    
    if (strcmp(pc, "ZFB") == 0) {
        //判断能不能打开支付宝
        if (![[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"alipay://"]]) {
            [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"请安装支付宝"];
            //            [MBProgressHUD hideHUDForView:[UIApplication sharedApplication].keyWindow animated:YES];
            [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
            return;
        }else{
            [[NetworkBackServiceDelegate sharedInstance] payRequest:RmbId andPc:Pc andMd:Md andToken:tekenStr];
        }
    }
    else if (strcmp(pc, "WX") == 0){
        //判断能不能打开支付宝
        if (![[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"weixin://"]]) {
            [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"请安装微信"];
            //            [MBProgressHUD hideHUDForView:[UIApplication sharedApplication].keyWindow animated:YES];
            [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
            return;
        }else{
            [[NetworkBackServiceDelegate sharedInstance] payRequest:RmbId andPc:Pc andMd:Md andToken:tekenStr];
        }
    }
    else
    {
        //苹果内购
        
        //先验证丢单情况  订单  凭证  环境
        NSString *storeMessage =  [[NetworkBackServiceDelegate sharedInstance] GetStoreMessage];
        NSLog(@"StoreMessage:%@",storeMessage);
        if (storeMessage != NULL) {
            NSArray *storeMessageArr = [storeMessage componentsSeparatedByString:@";"];
            if (![storeMessageArr[1] isEqualToString:PAYSUCCESS] ) {
                UnitySendMessage("buttonBehind", "applePayBackOrderID",[storeMessageArr[0] cStringUsingEncoding:NSUTF8StringEncoding]);
                
                UnitySendMessage("buttonBehind", "applePayBackHJ",[storeMessageArr[2] cStringUsingEncoding:NSUTF8StringEncoding]);
                UnitySendMessage("buttonBehind", "applePayBackPZ",[storeMessageArr[1] cStringUsingEncoding:NSUTF8StringEncoding]);
                return;
            }
        }
        
        if (applePayID != NULL) {
            NSString *str = [NSString stringWithCString:applePayID encoding:NSUTF8StringEncoding];
            [NetworkBackServiceDelegate sharedInstance].ApplePayID = [PAYPREFIX stringByAppendingString:str];
            NSLog(@"ppppppppppp:%@",[NetworkBackServiceDelegate sharedInstance].ApplePayID);
        }
        
        if ([SKPaymentQueue canMakePayments] ) {
            //向我方服务器请求订单
            NSString *str = [NSString stringWithFormat:@"?goldId=%@",[NSString stringWithUTF8String:rmbId]];
            NSString *urlStr = [NSString stringWithFormat:kAPPLEORDERURL,str];
            //            NSLog(@"urlStr:%@",urlStr);
            NSMutableURLRequest *mutableRequest = [[NetworkBackServiceDelegate sharedInstance] Reqest:urlStr Token:tekenStr];
            NSURLSession *session = [NSURLSession sharedSession];
            NSURLSessionDataTask *tast = [session dataTaskWithRequest:mutableRequest completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {
                NSHTTPURLResponse *resp = (NSHTTPURLResponse*)response;
                if (resp.statusCode == 200) {
                    //                                        NSLog(@"请求成功");
                    NSError*jsonError;
                    NSDictionary *mingwenDic=[NSJSONSerialization
                                              JSONObjectWithData:data
                                              options:NSJSONReadingMutableContainers
                                              error:&jsonError];
                    NSLog(@"mingwenDic:%@",mingwenDic);
                    dispatch_async(dispatch_get_main_queue(), ^{
                        
                        if ([mingwenDic[@"code"] isEqualToString:@"0"]) {
                            //                            NSLog(@"我后台订单获取成功");
                            //将订单id保存
                            [NetworkBackServiceDelegate sharedInstance].orderId = mingwenDic[@"data"];
                            const char* orderChar = [mingwenDic[@"data"] cStringUsingEncoding:NSUTF8StringEncoding];
                            UnitySendMessage("buttonBehind", "applePayBackOrderID",orderChar);
                            
                            //向苹果请求支付
                            [[NetworkBackServiceDelegate sharedInstance] getRequestAppleProduct:[NetworkBackServiceDelegate sharedInstance].ApplePayID];
                        }else{
                            [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
                        }
                        
                    });
                }
            }];
            [tast resume];
        }else{
            dispatch_async(dispatch_get_main_queue(), ^{
                [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
                [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"不支持苹果支付"];
            });
        }
    }
}

extern "C" void _copyText(const char* text,const char* text1)
{
    NSString *textStr = [NSString stringWithUTF8String:text];
    NSString *textStr1 = [NSString stringWithUTF8String:text1];
    if ([textStr1 containsString:@"复制成功!"]) {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:3 hudDetailText:@"复制成功!"];
        return;
    }
    if (!WXApi.isWXAppInstalled) {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:3 hudDetailText:@"请安装微信再操作"];
        return;
    }
    else if (!WXApi.isWXAppSupportApi){
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"该手机不支付微信"];
        return;
    }
    NSURL *url = [NSURL URLWithString:@"weixin://"];
    BOOL canOpen = [[UIApplication sharedApplication] canOpenURL:url];
    if (canOpen) {
        //打开微信
        [[UIApplication sharedApplication] openURL:url];
    }else
    {
        [HLMBProgressHUD addMBProgressHUDinView:[UIApplication sharedApplication].keyWindow hudMode:MBProgressHUDModeText hideDelay:2 hudDetailText:@"请安装微信再操作"];
        return;
    }
    
    
    if (textStr == NULL) {
        return;
    }
    UIPasteboard *board = [UIPasteboard generalPasteboard];
    board.string = textStr;
    
    
}

extern "C" void _showorHideHud(const char* state)
{
    NSString *str = [NSString stringWithUTF8String:state];
    if ([str isEqualToString:@"1"] ||[str isEqualToString:@"-1"]) {
        [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
    }
    else if ([str isEqualToString:@"0"]){
        [[NetworkBackServiceDelegate sharedInstance] storeOrderID:PAYSUCCESS Enviroment:PAYSUCCESS PingZ:PAYSUCCESS];
        [[NetworkBackServiceDelegate sharedInstance] showHUD:false];
    }
    
}

//获取系统版本号
extern "C"  void _getVersion()
{
    //获取本地软件的版本号
    NSString *localVersion = [[[NSBundle mainBundle]infoDictionary] objectForKey:@"CFBundleVersion"];
    UnitySendMessage("Wechat", "getAndroidVersion", [localVersion cStringUsingEncoding:NSUTF8StringEncoding]);
    
}

//判断客户端有没有安装微信 返回0代表安装了,其它的代表没有安装
extern "C" int _checkInstallWeChat()
{
    if (!WXApi.isWXAppInstalled) {
        return 1;
    }
    else if (!WXApi.isWXAppSupportApi){
        return 1;
    }
    return  0;
}




