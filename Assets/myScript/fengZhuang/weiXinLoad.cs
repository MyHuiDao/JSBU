﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class weiXinLoad : MonoBehaviour
{
    

#if UNITY_ANDROID
    AndroidJavaClass androidClass;
    public static AndroidJavaObject androidObject;
#endif
    string code = null;//获取微信code
    public static weiXinLoad instance;
    public Text m_text;
    public static bool show = false;
#if UNITY_IPHONE
    private GameObject UserProtocolObj;
    Toggle UserProtocolToggle;
    public bool AndroidFunction;
    private string serverIdentifyStr = "1";
#endif

    void Start()
    {
        instance = this;


#if UNITY_ANDROID
        androidObject.Call("getVersionCode");
#endif

#if UNITY_IPHONE
        UnitySendMessageToiOS.Instante().getVersion();

#endif

       
    }
    WWW _www = null;
    string url;
    int version = 0;
    string downApkUrl;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
#if UNITY_ANDROID
        androidClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        androidObject = androidClass.GetStatic<AndroidJavaObject>("currentActivity");
#endif
#if UNITY_IOS
        StartCoroutine(appleReview("http://jinshayugang.com/ios.json"));
#endif

    }

    void Update()
    {
        //m_text.text = show.ToString();
    }


    void Access_code(string _code)
    {
        code = _code;//获取code
                     //  m_text.text = code;
        if (code == null)
        {
            Debug.Log("获取code失败");
            return;
        }
        // print("codeC#:" + code);
        httpConnect.GET(this, httpConnect.URL + "/user/loginByWeixinId?code=" + code, null, weiXinLand, httpError);

    }
    /// <summary>
    /// 微信登陆
    /// </summary>
    /// <param name="str"></param>
    void weiXinLand(string str)
    {
        Debug.Log("WXLand:" + str);
        JsonData js = JsonMapper.ToObject(str);
        if ((string)js["code"] == "0")
        {
            netConnect.token = (string)js["data"];
            sceneLoad.loadScene();
        }
        else if ((string)js["code"] == "-1")
        {
            Debug.Log(js["data"].ToString());
        }
        Debug.Log("token" + netConnect.token);

    }


    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        Debug.Log(str);
    }

    IEnumerator requestVersionUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            url = httpConnect.URL + "/game/getVersion";
        }
        else
        {
            url = httpConnect.URL + "/game/getIosVersion";
        }
        _www = new WWW(url);
        yield return _www;

        if (_www == null)
        {
            Debug.Log("没有获取到更新信息");
        }
        else
        {
            VersionUpdte(_www.text);
        }

    }

    void VersionUpdte(string str)
    {
        _www = null;
        Debug.Log("VesionData:" + str);
        JsonData js = JsonMapper.ToObject(str);
        if ((string)js["code"] == "0")
        {
            downApkUrl = (string)js["data"]["url"];
            int serverVesion = int.Parse(js["data"]["version"].ToString().Replace(".", ""));

            if (version < serverVesion)
            {
                //显示更新界面
                showVersionUpdate();
            }
            else
            {//如果没有版本更新的话,就考虑显示用户手册界面
#if UNITY_IOS
                if (AndroidFunction == false)//使用安卓端功能
                {
                    //showUserProtocol();
                }
#endif
            }
        }
        else
        {
            Debug.Log("版本是最新的");
        }

    }

    //加载版本更新界面
    void showVersionUpdate()
    {
        
        //GameObject versionObj = (GameObject)Resources.Load("myPrefabs/Fish/LoadRes/VersionUpdateUI");
        GameObject versionObj = Instantiate(ResouseManager.Instance.VESIONUPDATEP, GameObject.Find("Canvas").transform) as GameObject;

        for (int i = 0; i < 2; ++i)
        {
            if (i == 0)
                versionObj.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
                {
                OnBtnClick(0,versionObj);
                });
            else
                versionObj.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate
                {
                OnBtnClick(1,null);
                });
        }

        //UnLoadObj(VersionUpdateP);

    }

    void OnBtnClick(int i,UnityEngine.Object obj)
    {
        //Debug.Log("===i:" + i);

        if (i == 0)
        {
            Destroy(obj);
            //UnLoadObj(obj);
#if UNITY_IOS
            if (AndroidFunction == false)
            {
                //showUserProtocol();
            }
#endif

        }
        else
        {
            Application.OpenURL(downApkUrl);

        }

    }

    public void getAndroidVersion(string str)
    {
        m_text.text = "V" + str;
        version = int.Parse(str.Replace(".", ""));
        if (show == true)
        {
            //UserProtocolObj.transform.localScale = Vector3.zero;
        }
        StartCoroutine(requestVersionUpdate());
    }

#if UNITY_IOS
    //public void showUserProtocol()
    //{
    //    GameObject UserProtocol = (GameObject)Resources.Load("myPrefabs/Fish/LoadRes/UserProtocol");
    //    obj = Instantiate(UserProtocol, GameObject.Find("Canvas").transform) as GameObject;
    //    GameObject toggleObj = obj.transform.GetChild(1).GetChild(0).gameObject;
    //    UserProtocolToggle = toggleObj.transform.GetComponent<Toggle>();

    //    UserProtocolObj = obj.transform.GetChild(0).gameObject;
    //    if (USERPROTOCOL == "true")
    //    {
    //        UserProtocolObj.transform.localScale = Vector3.zero;
    //        UserProtocolObj.SetActive(false);
    //        UserProtocolToggle.isOn = true;
    //    }

    //    GameObject btn0 = obj.transform.GetChild(1).GetChild(1).gameObject;
    //    EventTriggerListener.Get(btn0).onClick = OnUserProtocolBtnClick;

    //    GameObject btn1 = obj.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
    //    GameObject btn2 = obj.transform.GetChild(0).GetChild(0).GetChild(2).gameObject;
    //    EventTriggerListener.Get(btn1).onClick = OnUserProtocolBtnClick;
    //    EventTriggerListener.Get(btn2).onClick = OnUserProtocolBtnClick;
    //}


    void OnUserProtocolBtnClick(GameObject obj)
    {
        if (obj.name.Contains("0"))//用户协议!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            UserProtocolObj.SetActive(true);
            UserProtocolObj.transform.localScale = Vector3.one;
        }
        else if (obj.name.Contains("1"))//同意
        {
            UserProtocolObj.SetActive(false);
            UserProtocolToggle.isOn = true;

        }
        else if (obj.name.Contains("2"))//不同意
        {
            UserProtocolObj.SetActive(false);
            UserProtocolToggle.isOn = false;
        }
    }


    //public bool UserAgreeProtocol()
    //{
    //    if (!UserProtocolToggle.isOn)
    //    {
    //        obj.transform.GetChild(2).gameObject.SetActive(true);
    //        StartCoroutine(hideObj());
    //    }
    //    return UserProtocolToggle.isOn;
    //}
    //IEnumerator hideObj()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    obj.transform.GetChild(2).gameObject.SetActive(false);

    //}

    public string USERPROTOCOL
    {
        get { return PlayerPrefs.GetString("UP"); }
        set { PlayerPrefs.SetString("UP", value); }
    }


    //开启安卓功能0  关闭安卓功能1
    public string ServerIdentifyStr
    {
        get { return serverIdentifyStr; }
    }


    IEnumerator appleReview(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            serverIdentifyStr = "1";
            //Debug.Log("------:" + ServerIdentifyStr);
        }else{//已经上架
            //Debug.Log("wwwerror:"+www.error);
            serverIdentifyStr = "0";

        }
    }
#endif




}
