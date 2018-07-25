﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class UnitySendMessageToiOS
{

    private static UnitySendMessageToiOS intance;
    static string[] scenes = { "sharefriend", "sharecircle" };
    static string title = "最新版《金沙渔港》重磅来袭！轻松赢话费。边玩游戏边赚钱，好玩又刺激。走过路过不要错过哦！";
    static string description = "下载游戏吧";


    static public  UnitySendMessageToiOS Instante()
    {
        if(intance == null){
            intance = new UnitySendMessageToiOS();
        }
        return intance;
    }

    [DllImport("__Internal")]
    private static extern int _wxLogon();//微信登陆

    [DllImport("__Internal")]
    private static extern void _wxShare(string filePath,int type);//微信分享图片

    [DllImport("__Internal")]
    private static extern void _wxShareLink(string title,string description);//分享链接

    [DllImport("__Internal")]
    private static extern void _unityRecharge(string pc, string mid, string rmbID,string token,string applePayID);//支付

    [DllImport("__Internal")]
    private static extern void _copyText(string text,string text1);//粘贴板

    [DllImport("__Internal")]
    private static extern void _showorHideHud(string state);//显示或关闭按钮

    [DllImport("__Internal")]
    private static extern void _getVersion();

    [DllImport("__Internal")]
    private static extern int _checkInstallWeChat();

    public int wxLogon()//微信登陆
    {
        try{
            return _wxLogon();
        }catch(Exception ex)
        {
            Debug.Log("wxLogonError:" + ex);
            return int.MinValue;
        }

    }

    public void shareToiOS(string filePath,int type)////微信分享
    {
        try{
            if (filePath == null)
            {
                //Debug.Log("截屏文件不存在");
                return;
            }
            _wxShare(filePath, type);
        }catch(Exception ex){
            Debug.Log("shareToiOSError:" + ex);
        }
       

    }

    public void wxShareLink()
    {
        try{
            //Debug.Log("链接分享被点击");
            _wxShareLink(title, description);
        }catch(Exception ex)
        {
            Debug.Log("wxShareLinkError:" + ex);
        }
      
    }
    /// <summary>
    /// Recharges the toi os.
    /// </summary>
    /// <param name="payMethod">支付方式</param>
    /// <param name="terminalId">终端Id</param>
    /// <param name="goldId">服务器金币Id号</param>
    /// <param name="token">Token.</param>
    /// <param name="applePayID">苹果商店对应的支付等级ID</param>
    public void rechargeToiOS(string payMethod,string terminalId,string goldId,string token,string applePayID)
    {
        Debug.Log("payMethod:"+payMethod + "::goldId:" + goldId + "::terminalId:" + terminalId + "::token:" + token + "::applePayID:" + applePayID);
        try{
            _unityRecharge(payMethod, terminalId, goldId, token, applePayID);   
        }catch(Exception ex){
            Debug.Log("rechargeToiOSError:" + ex);
        }

    }


    public void copyText(string text,string text1)
    {
        try
        {
            if (text1.Contains("复制成功"))
            { 
                _copyText(text, text1);
            }
            else{
                if (checkInstallWeChat() == 0)//返回为0代表安装了微信
                {
                    _copyText(text, text1);
                }
            }
            
        }catch(Exception ex)
        {
            Debug.Log("copyTextError:" + ex);
        }
       
    }

    public void showorHideHud(string state)
    {
        try{
            _showorHideHud(state);
        }catch(Exception ex)
        {
            Debug.Log("showorHideHudError:" + ex);
        }

    }

    //获取客户端版本号
    public void getVersion()
    {
        try{
            _getVersion();
        }catch(Exception ex)
        {
            weiXinLoad.instance.getAndroidVersion("1.0.0");
            Debug.Log("_getVersionError:" + ex);
        }
         
    }

    //检查客户端有没安装微信
    public int checkInstallWeChat()
    {
        try{
            return _checkInstallWeChat();
        }catch(Exception ex)
        {
            Debug.Log("checkInstallWeChatError:" + ex);
            return 1;
        }

      
    }

}
