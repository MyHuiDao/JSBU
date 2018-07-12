using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



/*
 注意：如果为get请求，则传入的参数byte【】  b直接传入null，如果为post请求，则要用系统WWWForm类，实例化此类，对象.addFile();加入要传的参数，对象.data赋值给byte【】  传入byte【】 参数
 */
/// <summary>
/// 用httpConnect来登陆账号
/// </summary>
public class httpConnect
{
    public static bool isPost = false;//判断是否是post
    public static bool isLand = false;//是否登陆
    public static bool isRegister = false;//是否注册

    //public static string URL = "https://jinshayugang.com/game";
    public static string URL = "http://hd.com/game";//内网
    //public static string URL = "http://192.168.31.214/game";//手机上测试地址

    public static string Web_URL = "ws://hd.com";
    //public static string Web_URL = "ws://jinshayugang.com";
    //public static string Web_URL = "ws://192.168.31.214";//手机上测试地址

    private static string versionCheckURL = "";
    Hashtable ht = null;
    static int i = 0;//加载场景只执行一遍
    string url = null;
    Action<string> callBack = null;//用系统的委托，带一个参数
    Action<string> errCallBack = null;
    static MonoBehaviour node;
    Dictionary<string, string> header = new Dictionary<string, string>();
    static byte[] b = null;//为空为get，不为空则为post。
    WWW www = null;
    public static List<net_data> net = new List<net_data>();
    public static List<json_toggle> toggle_list = new List<json_toggle>();
    public static string data;
    public static string show;
    public static string names;
    public static string keys;
    public static List<CanvasGroup> toggle_group = new List<CanvasGroup>();
    private GameObject jiazai;

    //UnityWebRequest www;

    /// <summary>
    /// 提示信息
    /// </summary>
    public static void notice()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("notice");
        string itemsJson = textAsset.text;
        JsonData jj = JsonMapper.ToObject(itemsJson);
        for (int j = 0; j < jj.Count; j++)
        {
            data = jj[j]["data"].ToString();
            show = jj[j]["show"].ToString();
            net_data item = new net_data(data, show);
            net.Add(item);
        }
    }
    public static void Toggles()
    {
        toggle_list.Clear();
        toggle_group.Clear();
        TextAsset textAsset = Resources.Load<TextAsset>("toggle");//解析 Toggle
        string itemsJson = textAsset.text;
        JsonData jj = JsonMapper.ToObject(itemsJson);
        for (int i = 0; i < jj.Count; i++)
        {
            names = (jj[i]["name"].ToString());
            keys = (jj[i]["key"].ToString());
            json_toggle item = new json_toggle(names, keys);
            toggle_list.Add(item);
            toggle_group.Add(GameObject.Find(keys).GetComponent<CanvasGroup>());

        }
    }

    private httpConnect()
    {

    }
    /// <summary>
    /// GET
    /// 方便外部调用 ，所以这里用的静态的方法，其实方法内是创建了一个对象，如果有多个线程同时发起httpConnect连接的时候，
    /// 如果全部用单列去实现，那么需要等当前连接完成，才能去执行后面的连接，所以在方法内部还是创建了对象，这样就算
    /// 有多个线程同时发起httpConnect连接，那么也没有问题，10个不同的对象同进去连接
    /// </summary>
    /// <param name="_node">当前用来起动协程的对象，因为当前类没有继承MonoBehavi....</param>
    /// <param name="_url">连接的地址</param>
    /// <param name="_callBack">连接成功的回调方法，带string参数</param>
    /// <param name="_errcallBack">连接失败的回调方法，带string参数</param>
    public static void GET(MonoBehaviour _node, string _url, byte[] _b, Action<string> _callBack = null, Action<string> _errcallBack = null)
    {
        // Debug.Log("url:" + _url);
        b = _b;
        //用来保存起动协程的对象，只需要有一个对象就可以了，就算有10个线程，也可以用一个对象去启动，所以它是static的。
        node = _node;
        //创建一个httpConnect连接对象 ，
        httpConnect httpConnect = new httpConnect();
        //调用对象的方法。
        httpConnect.doGet(_url, _callBack, _errcallBack);

    }


    private void doGet(string _url, Action<string> _callBack, Action<string> _errcallBack = null)
    {

        callBack = _callBack;
        errCallBack = _errcallBack;
        url = _url;
        node.StartCoroutine(GoTo());

        //  netConnect.instance.m_text.text = "登录中";

    }

    IEnumerator GoTo()
    {

        JsonData data = new JsonData();
        if (i != 0 && !isLand)
        {
            //www = UnityWebRequest.Get(url);
            //www.SetRequestHeader("token", netConnect.token);
            header.Add("token", netConnect.token);
            www = new WWW(url, b, header);
        }
        if (isRegister || isLand)
        {
            //www = UnityWebRequest.Get(url);
            www = new WWW(url);
            GameObject load = (GameObject)Resources.Load("Rank/load");
            jiazai = UnityEngine.Object.Instantiate(load, GameObject.Find("Canvas").transform) as GameObject;
        }

        //  Debug.Log("url0:" + url);
        //Debug.Log(2);
        yield return www;
        //Debug.Log(1);
        UnityEngine.Object.Destroy(jiazai);
        //yield return www.SendWebRequest();
        //Debug.Log("url1:" + url);
        if (www == null)
        {

            // GameObject.Find("httpLoadFail").transform.localScale = Vector3.one;//加载失败显示，也可能返回错误，现在不清楚

        }
        if (www != null && isLand)//先返回www再加载场景
        {
            isLand = false;
            i++;
        }


        if (www.error == null || www.error.Length == 0)
        {
            if (callBack != null) callBack(www.text);
        }
        else
        {
            if (errCallBack != null)
            {
                errCallBack(www.error);
                GameObject fail = (GameObject)Resources.Load("Rank/loadFail");
                GameObject jiazais = UnityEngine.Object.Instantiate(fail, GameObject.Find("Canvas").transform) as GameObject;

            }
        }
        JsonData js = JsonMapper.ToObject(www.text);
        if ((string)js["code"] == "-1")
        {
            Debug.Log("返回-1");
            //GameObject fail = (GameObject)Resources.Load("Rank/loadFail");
            //GameObject jiazais = UnityEngine.Object.Instantiate(fail, GameObject.Find("Canvas").transform) as GameObject;
            //jiazais.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
            //{
            //    SceneManager.LoadScene("landScene");
            //});
        }




        www.Dispose();
    }

    //检查最新版本
    public void VersionCheck()
    {
        //从我方服务器版本
        //获取app版本
        //比较版本,确定要不要跳转浏览器下载
    }

}





