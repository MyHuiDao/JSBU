
using UnityEngine;
using LitJson;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.EventSystems;


public enum login_state
{
    wechat,
    visitor,
    none
}
/// <summary>
/// http通信，发送登陆请求
/// </summary>
public class netConnect : MonoBehaviour
{
    public login_state m_state = login_state.none;
    public static string token = null;
    public static netConnect instance = null;

    //public static GameObject buyu0;
    //public static GameObject buyu1;
    //public static GameObject buyu2;

    void Awake()
    {
        httpConnect.notice();
        instance = this;
        //buyu0 = Resources.Load("myPrefabs/meiRenYu/MainScene0") as GameObject;//取决于点击哪款游戏
        //buyu1 = Resources.Load("myPrefabs/meiRenYu/MainScene1") as GameObject;//取决于点击哪款游戏
        //buyu2 = Resources.Load("myPrefabs/meiRenYu/MainScene2") as GameObject;//取决于点击哪款游戏
      
    }
    private void Start()
    {
       
    }

    public void connectnet(bool isAccountLand, bool isYouKe, bool isWeiXin)
    {
        httpConnect.isLand = true;//登陆则改为true
        if (isYouKe)
        {
            //httpConnect.GET(this, httpConnect.URL + "/user/loginByDeviceId?deviceId=8888", null, userLand, httpError);//实现游客登陆，得到token号，连接到服务器 + SystemInfo.deviceUniqueIdentifier
            httpConnect.GET(this, httpConnect.URL + "/user/loginByDeviceId?deviceId=" + "000000000"/*SystemInfo.deviceUniqueIdentifier*/, null, userLand, httpError);//  实现游客登陆，得到token号，连接到服务器 + SystemInfo.deviceUniqueIdentifier
            m_state = login_state.visitor;
        }
        if (isWeiXin)
        {
#if UNITY_ANDROID
            weiXinLoad.androidObject.Call("LoginWeiXin");
            httpConnect.isLand = true;//登陆则改为true
#endif
#if UNITY_IPHONE
            if(UnitySendMessageToiOS.Instante().wxLogon() == 0){
                print("登陆成功");
            }else{
                print("请安装或更微信");
            }
#endif
            m_state = login_state.wechat;
        }
    }

    /// <summary>
    /// 游客登录
    /// </summary>
    /// <param name="str"></param>
    void userLand(string str)
    {
        Debug.Log(str);
        JsonData js = JsonMapper.ToObject(str);
        //Debug.Log((string)js["code"]);
        if ((string)js["code"] == "0")
        {
            token = (string)js["data"];
            sceneLoad.loadScene();
        }
        else if ((string)js["code"] == "-1")
        {
            if(((string)js["data"]== "账户被禁用"))
            Ani(0);
        }
    }
    public static void Ani(int code)
    {
        GameObject _notice = GameObject.Find("Notices");
        _notice.transform.GetChild(0).GetComponent<Text>().text = httpConnect.net[code].show;
        //_notice.GetComponent<RectTransform>().sizeDelta = _notice.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        _notice.GetComponent<RectTransform>().sizeDelta=new Vector2(_notice.transform.GetChild(0).GetComponent<Text>().text.Length * 75.1f, 169.5f);
        // _notice.GetComponent<CanvasGroup>().alpha = 1;
        //_notice.transform.localScale = Vector3.one;
        //_notice.transform.GetChild(0).localScale = Vector3.one;
        _notice.GetComponent<Animation>().Play("webtishi");
   
     
     
    }

    //public void Anii(int code)
    //{
    //    GameObject.Find("Noticess").GetComponent<Animation>().Play("webtishi");
    //    GameObject.Find("Noticess").transform.GetChild(0).GetComponent<Text>().text = httpConnect.net[code].show;
    //}
    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        Debug.Log(str);
    }



}
