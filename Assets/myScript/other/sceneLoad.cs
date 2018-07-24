using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*主要问题是加载网络和加载场景的先后问题*/
/// <summary>
/// 此类用来对所有场景的加载
/// </summary>
public class sceneLoad : MonoBehaviour
{
    public static sceneLoad instance = null;
    Button main_image;
    private AudioSource login_effect;
    //void Awake()
    //{
    //    if (weiXinLoad.show == true)
    //    {
    //        GameObject.Find("Canvas_laod").SetActive(false);
    //    }
    //}
    void Start()
    {
        ResouseManager.Instance.Start();
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        instance = this;
        login_effect = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        Music_Control.music_effect(login_effect);
        main_image = GameObject.Find("main").GetComponent<Button>();
        main_image.enabled = false;
        GameObject.Find("gongGao").GetComponent<Button>().onClick.AddListener(() => method("Notice", Vector3.one, true));
        GameObject touObj = GameObject.Find("tourist");
        GameObject weChatObj = GameObject.Find("weChat");
#if UNITY_IOS
        if (weiXinLoad.instance.AndroidFunction == false)
        {
            if (UnitySendMessageToiOS.Instante().checkInstallWeChat() != 0)
            {
                weChatObj.transform.localScale = Vector3.zero;
                touObj.transform.localPosition = Vector3.Lerp(touObj.transform.localPosition, weChatObj.transform.localPosition, 0.5f);
            }
        }
#elif UNITY_ANDROID
        {
            weChatObj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
#endif
        touObj.GetComponent<Button>().onClick.AddListener(delegate () { loadGameScene(false, true, false); });//游客登陆
        weChatObj.GetComponent<Button>().onClick.AddListener(delegate () { loadGameScene(false, false, true); });//微信登陆
        main_image.onClick.AddListener(close_Account);
    }
    public void loadGameScene(bool isAccountLand, bool isYouKe, bool isWeiXin)
    {
#if UNITY_ANDROID
        netConnect.instance.connectnet(isAccountLand, isYouKe, isWeiXin);//游客或账号登陆 
#endif
#if UNITY_IOS
        if(weiXinLoad.instance.AndroidFunction == true)
        {
            netConnect.instance.connectnet(isAccountLand, isYouKe, isWeiXin);//游客或账号登陆 
        }
        else{
            //if(weiXinLoad.instance.UserAgreeProtocol())
            //{
            //    weiXinLoad.instance.USERPROTOCOL = "true";
            //    netConnect.instance.connectnet(isAccountLand, isYouKe, isWeiXin);//游客或账号登陆 
            //}
        }
#endif

    }
    /// <summary>
    /// 调用此方法来加载场景
    /// </summary>
    public static void loadScene()
    {
        Globe.nextSceneName = "gameScene";//保存需要加载的目标场景
        SceneManager.LoadScene("jiaZaiScene");//切换场景影响到了www的加载。
    }
    void httpError(string str)
    {
        Debug.Log(str);
    }
    //public void qq_load()
    //{
    //    SceneManager.LoadScene("Test_qq");//切换场景影响到了www的加载。
    //}


    
    void rigisterKuang1( string name)
    {
        method(name, Vector3.zero, true);
    }
    /// <summary>
    /// 点击屏幕
    /// </summary>
    void close_Account()
    {
        GameObject.Find("Notice").transform.localScale =Vector3.zero;
        main_image.enabled = false;
    }
    /// <summary>
    /// 缩放处理事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="vec"></param>
    void method(string name,Vector3 vec,bool value)
    {
        GameObject.Find(name).transform.localScale = vec;
        main_image.enabled = value;
    }
    /// <summary>
    /// 关闭按钮
    /// </summary>
    /// <param name="button"></param>
    public void close_button(Button button)
    {
        button.enabled = false;

    }

}