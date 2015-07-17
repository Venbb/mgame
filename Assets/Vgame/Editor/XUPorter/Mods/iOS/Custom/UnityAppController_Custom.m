//
//  UnityAppController_Custom.m
//  Unity-iPhone
//
//  Created by niko on 15-7-10.
//
//

#import <Foundation/Foundation.h>
#import "UnityAppController_Custom.h"

#if defined (__cplusplus)
extern "C"
{
#endif
    bool isWXAppInstalled()
    {
        return [WXApi isWXAppInstalled];
    }
    bool isWXAppSupportApi()
    {
        return [WXApi isWXAppSupportApi];
    }
    // 给Unity3d调用的方法
    void weixinLoginByIos()
    {
        // 登录
        [[NSNotificationCenter defaultCenter] postNotificationName:ksendAuthRequestNotification object:nil];
    }
    void ShareByIos(const char* title,const char*desc,const char*url)
    {
        NSString *titleStr=[NSString stringWithUTF8String:title];
        NSString *descStr=[NSString stringWithUTF8String:desc];//0416aa28b5d2ed1f3199083b3806c6bl
        NSString *urlStr=[NSString stringWithUTF8String:url];
        NSLog(@"ShareByIos titleStr:%@",titleStr);
        NSLog(@"ShareByIos descStr:%@",descStr);
        NSLog(@"ShareByIos urlStr:%@",urlStr);
        
        NSDictionary *dic=[[NSBundle mainBundle] infoDictionary];
        NSLog(@"dic:%@",dic);
        NSArray *arr=[[[dic valueForKey:@"CFBundleIcons"] valueForKey:@"CFBundlePrimaryIcon"]valueForKey:@"CFBundleIconFiles"];
        NSLog(@"arr:%@",arr);
        NSString *iconName=arr[0];
        NSLog(@"iconName:%@",iconName);
        // 分享
        WXMediaMessage *message = [WXMediaMessage message];
        message.title = titleStr;//@"专访张小龙：产品之上的世界观";
        message.description = descStr;//@"微信的平台化发展方向是否真的会让这个原本简洁的产品变得臃肿？在国际化发展方向上，微信面临的问题真的是文化差异壁垒吗？腾讯高级副总裁、微信产品负责人张小龙给出了自己的回复。";
        [message setThumbImage:[UIImage imageNamed:iconName]];
        //        [message setThumbImage:[UIImage imageNamed:@"AppIcon72x72"]];
        
        WXWebpageObject *ext = [WXWebpageObject object];
        ext.webpageUrl = urlStr;//@"http://tech.qq.com/zt2012/tmtdecode/252.htm";
        
        message.mediaObject = ext;
        message.mediaTagName = @"WECHAT_TAG_SHARE";
        
        SendMessageToWXReq* req = [[[SendMessageToWXReq alloc] init]autorelease];
        req.bText = NO;
        req.message = message;
        req.scene = WXSceneTimeline;
        [WXApi sendReq:req];
    }
#if defined (__cplusplus)
}
#endif

@implementation UnityAppController_Custom

-(BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(sendAuthRequest) name:ksendAuthRequestNotification object:nil]; // 微信
    //向微信注册
    [WXApi registerApp:WeiXinID];
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}
-(BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    return [WXApi handleOpenURL:url delegate:self];
}
#pragma mark - WXApiDelegate

- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    return [WXApi handleOpenURL:url delegate:self];
}

- (void)onReq:(BaseReq *)req // 微信向第三方程序发起请求,要求第三方程序响应
{
    
}

- (void)onResp:(BaseResp *)resp // 第三方程序向微信发送了sendReq的请求,那么onResp会被回调
{
    if([resp isKindOfClass:[SendAuthResp class]]) // 登录授权
    {
        SendAuthResp *temp = (SendAuthResp*)resp;
        if(temp.code!=nil)UnitySendMessage(GameObjectName, MethodName, [temp.code cStringUsingEncoding:NSUTF8StringEncoding]);
        
        //        [self getAccessToken:temp.code];
    }
    else if([resp isKindOfClass:[SendMessageToWXResp class]])
    {
        // 分享
        if(resp.errCode==0)
        {
            NSString *code = [NSString stringWithFormat:@"%d",resp.errCode]; // 0是成功 -2是取消
            NSLog(@"SendMessageToWXResp:%@",code);
            UnitySendMessage(GameObjectName, ShareMethod, [code cStringUsingEncoding:NSUTF8StringEncoding]);
        }
    }
}
#pragma mark - Private

- (void)sendAuthRequest

{
    
    SendAuthReq* req = [[[SendAuthReq alloc] init] autorelease];
    
    req.scope = @"snsapi_userinfo";
    
    req.state = @"only123";
    
    
    
    [WXApi sendAuthReq:req viewController:_rootController delegate:self];
    
}



- (void)getAccessToken:(NSString *)code

{
    
    NSString *path = [NSString stringWithFormat:@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=%@&secret=%@&code=%@&grant_type=authorization_code",WeiXinID,WeiXinSecret,code];
    
    NSURLRequest *request = [[NSURLRequest alloc] initWithURL:[NSURL URLWithString:path] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0];
    
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    
    [NSURLConnection sendAsynchronousRequest:request queue:queue completionHandler:
     
     ^(NSURLResponse *response,NSData *data,NSError *connectionError)
     
     {
         
         if (connectionError != NULL)
             
         {
             
             
             
         }
         
         else
             
         {
             
             if (data != NULL)
                 
             {
                 
                 NSError *jsonParseError;
                 
                 NSDictionary *responseData = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&jsonParseError];
                 
                 NSLog(@"#####responseData = %@",responseData);
                 
                 if (jsonParseError != NULL)
                     
                 {
                     
                     //                    NSLog(@"#####responseData = %@",jsonParseError);
                     
                 }
                 
                 NSString *accessToken = [responseData valueForKey:@"access_token"];
                 
                 NSString *openid = [responseData valueForKey:@"openid"];
                 
                 [self getUserInfo:accessToken withOpenID:openid];
                 
             }
             
         }
         
     }];
    
}



- (void)getUserInfo:(NSString *)accessToken withOpenID: (NSString *)openid

{
    
    NSString *path = [NSString stringWithFormat:@"https://api.weixin.qq.com/sns/userinfo?access_token=%@&openid=%@",accessToken,openid];
    
    NSURLRequest *request = [[NSURLRequest alloc] initWithURL:[NSURL URLWithString:path] cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0];
    
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    
    [NSURLConnection sendAsynchronousRequest:request queue:queue completionHandler:
     
     ^(NSURLResponse *response,NSData *data,NSError *connectionError) {
         
         if (connectionError != NULL) {
             
             
             
         } else {
             
             if (data != NULL) {
                 
                 NSError *jsonError;
                 
                 NSString *responseData = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&jsonError];
                 
                 NSLog(@"#####responseData = %@",responseData);
                 
                 NSString *jsonData = [NSString stringWithFormat:@"%@",responseData];
                 
                 UnitySendMessage(GameObjectName, MethodName, [jsonData cStringUsingEncoding:NSUTF8StringEncoding]);
                 
                 if (jsonError != NULL) {
                     
                     //                     NSLog(@"#####responseData = %@",jsonError);
                     
                 }
                 
             }
             
         }
         
     }];
    
}

#pragma mark -

@end