using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using CClient;
/// <summary>
/// 大厅的信息发送和接受
/// </summary>
public class hallHttp : MonoBehaviour
{
    private string token;
    public static hallHttp instance;
    private Canvas can;
    private List<string> kefus = new List<string>();
    public List<string> kefu_wechat = new List<string>();
    public bool mobilenumber;
    // Use this for initialization
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GameObject.Find("Canvas_button").GetComponent<Canvas>().worldCamera = Camera.main;
        instance = this;
        message();
        httpConnect.GET(this, httpConnect.URL + "/game/gameNotice", null, getAnnocement, httpError);//获得游戏大厅公告
        httpConnect.GET(this, httpConnect.URL + "/user/userSafeGet", null, getSafeMoney, httpError);//获得保险柜信息
        httpConnect.GET(this, httpConnect.URL + "/user/getRecommendUrl", null, getRecommendUrl, httpError);//获取推广链接
        httpConnect.GET(this, httpConnect.URL + "/game/getCustmoterWeixin", null, json_wechat_custom, httpError);//微信客服

        CallBackFun.shareCallBack.methods.Clear();
        contrall.instant().contrallAddMethod();//websocket

        token = netConnect.token;
        ClientSocket.instant().clientSocket(httpConnect.Web_URL+"/game/game/hall/1/", token);//连接到服务器

    }
    // Update is called once per frame
    public void message()
    {
        httpConnect.GET(this, httpConnect.URL + "/user/getCurrentUser", null, getUserMessege, httpError);//获取用户信息
    }
    /// <summary>
    /// 推广链接
    /// </summary>
    void getRecommendUrl(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            httpView.instant.getRecommendUrlView((string)jso["data"]);
        }
    }
    /// <summary>
    /// 获取保险柜信息
    /// </summary>
    void getSafeMoney(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if (jso["code"].ToString() == "0")
        {
            GameObject.Find("baoXianGui").GetComponent<Text>().text = jso["data"].ToString();
        }
        if (jso["code"].ToString() == "-1")
        {
            Debug.Log(jso["msg"].ToString());
        }
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="str"></param>
    void getUserMessege(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            if (js.Keys.Contains("mobile"))//如果手机已经绑定了，传回手机信息，否则不传
            {
                mobilenumber = true;
                RootCanvas.canvas_group(GameObject.Find("number_Image").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobileUnBindleSure").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobile_InputField").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobileBindleSure").GetComponent<CanvasGroup>(), false, 0);
                GameObject.Find("mobile_number").GetComponent<Text>().text = js["mobile"].ToString();
            }
            else
            {
                mobilenumber = false;
                RootCanvas.canvas_group(GameObject.Find("number_Image").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobileUnBindleSure").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobile_InputField").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobileBindleSure").GetComponent<CanvasGroup>(), true, 1);
            }
            httpView.instant.getname(js["nickName"].ToString(), js["code"].ToString(), int.Parse(js["experience"].ToString()), int.Parse(js["gold"].ToString()), js["headImg"].ToString(), js["id"].ToString(), js["userType"].ToString(), js["isVip"].ToString(), js["isBind"].ToString(), int.Parse(js["vipLevel"].ToString()));
        }
    }
    /// <summary>
    /// 获得游戏大厅公告
    /// </summary>
    /// <param name="str"></param>
    void getAnnocement(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            for (int i = 0; i < js.Count; i++)
            {
                httpView.instant.getAnnocementView(js[i]["centen"].ToString());
            }
        }
    }
    /// <summary>
    /// 微信客服
    /// </summary>
    /// <param name="str"></param>
    void json_wechat_custom(string str)
    {
        //Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = JsonMapper.ToObject(jso["data"].ToJson());
            for (int i = 0; i < js.Count; i++)
            {
                kefus.Add(js[i].ToString());
            }
        }
        for (int i = 0; i < kefus.Count; i++)
        {
            if (kefus[i].Contains(":"))
            {
                int bb = kefus[i].Length;
                int ab = kefus[i].IndexOf(":") + 1;
                kefu_wechat.Add(kefus[i].Substring(ab, bb - ab));
            }
        }
    }

    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        Debug.Log(str);
    }
}