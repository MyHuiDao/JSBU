using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

enum WXSCENE{
WXSceneSession,
WXSceneTimeline,
WXSceneFavorite,

}

public class UnitySendMessageToiOS
{

    public static UnitySendMessageToiOS intance;
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

	private void Awake()
	{
        intance = this;
	}

	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {}


    [DllImport("__Internal")]
    private static extern int _wxLogon();//微信登陆

    [DllImport("__Internal")]
    private static extern void _wxShare(string filePath,int type);//微信分享图片

    [DllImport("__Internal")]
    private static extern void _wxShareLink(string title,string description);

    [DllImport("__Internal")]
    private static extern void _unityRecharge(string pc, string mid, string rmbID,string token,string applePayID);//支付

    [DllImport("__Internal")]
    private static extern void _copyText(string text);

    [DllImport("__Internal")]
    private static extern void _showorHideHud(string str);

    public int wxLogon()//微信登陆
    {
        return _wxLogon();
    }

    public void shareToiOS(string filePath,int type)////微信分享
    {

        if(filePath == null){
            Debug.Log("截屏文件不存在");
            return;
        }
        _wxShare(filePath,type);

    }

    public void wxShareLink()
    {
        Debug.Log("链接分享被点击");
        _wxShareLink(title,description);
    }

    public void rechargeToiOS(string payMethod,string terminalId,string goldId,string token,string applePayID)
    {
        _unityRecharge(payMethod,terminalId,goldId,token,applePayID);
    }

    public void Test()
    {
        Debug.Log("TUI");
    }

    public void copyText(string text)
    {
        _copyText(text);
    }

    public void showorHideHud(string str)
    {
        _showorHideHud(str);
    }

}
