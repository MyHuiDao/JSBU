using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System.IO;

public class rows
{
    public string title;
    public string content;
    public string createTime;

}
/// <summary>
/// 此类是点击按钮来发送http信息的
/// </summary>
public class httpSend : MonoBehaviour
{
    public static httpSend instant = null;
    Texture2D encoded;
    private Image image_qr_code;
    public static bool isbind = false;//判断当前是否是绑定状态
    private string Path_save;
    public List<rows> row1 = new List<rows>();
    private rows row = new rows();
    private int number = 0;
    private bool isApple = false;
    private List<string> kefus = new List<string>();
    public string[] kefu_wechat;
    private Dictionary<string, string> payDataDic = new Dictionary<string, string>();
    private Dictionary<string, string> rankDic = new Dictionary<string, string>();
    private string rankMessage;
    private Transform shops;
    public  bool Register;
    public bool hide_tx;
    private void Start()
    {
        encoded = new Texture2D(420, 420);

        instant = this;
        //shops = GameObject.Find("shop_Panel").transform;
        image_qr_code = GameObject.FindGameObjectWithTag("QR_code").GetComponent<Image>();
        GameObject.Find("ppusSure").GetComponent<Button>().onClick.AddListener(delegate () { btnSafeMoney(true); });//存入保险柜
        GameObject.Find("ppopSure").GetComponent<Button>().onClick.AddListener(delegate () { btnSafeMoney(false); });//取出保险柜
        GameObject.Find("getYanZhengMa").GetComponent<Button>().onClick.AddListener(btnMobileSendMsg);
        GameObject.Find("gameMoneyFreshen").GetComponent<Button>().onClick.AddListener(btnGameMoneyFreshen);//刷新保险柜
        GameObject.Find("tuiGuangYuan").GetComponent<Button>().onClick.AddListener(birth_QRcode);
        GameObject fenxianghaoyouObj = GameObject.Find("fenxianghaoyou");
        GameObject fenxiangpengyouquanObj = GameObject.Find("fenxiangpengyouquan");
#if UNITY_IOS
        if (weiXinLoad.instance.AndroidFunction == false)
        {
            if (UnitySendMessageToiOS.Instante().checkInstallWeChat() != 0)
            {
                fenxianghaoyouObj.transform.localScale = Vector3.zero;
                fenxiangpengyouquanObj.transform.localScale = Vector3.zero;
            }
        }
#endif
        fenxianghaoyouObj.GetComponent<Button>().onClick.AddListener(() => screenshot(0));
        fenxiangpengyouquanObj.GetComponent<Button>().onClick.AddListener(() => screenshot(1));
        GameObject.Find("fenxianglianjie").GetComponent<Button>().onClick.AddListener(sharepage);
       // GameObject.Find("mobileBind_Close").GetComponent<Button>().onClick.AddListener(clear);
        GameObject.Find("mobileBindleSure").GetComponent<Button>().onClick.AddListener(() => btnMobileBind(false));//发送绑定消息
        GameObject.Find("mobileUnBindleSure").GetComponent<Button>().onClick.AddListener(() => btnMobileBind(true));//发送解绑消息
    }
    void clear()
    {
        GameObject.Find("yanZhengMa_InputField").GetComponent<InputField>().text = null;
    }

    /// <summary>
    /// 商品列表
    /// </summary>
    //public void btnGetGoods()
    //{
    //    httpConnect.GET(this, httpConnect.URL + "/shop/getGoods?classId=", null, getGoods, httpError);
    //}
    //void getGoods(string str)
    //{
    //    JsonData jso = JsonMapper.ToObject(str);
    //    if ((string)jso["code"] == "0")
    //    {
    //        JsonData js = jso["data"];
    //        for (int i = 0; i < js.Count; i++)
    //        {
    //            httpView.instant.getGoodsView(js[i]["desc"].ToString(), js[i]["id"].ToString(), js[i]["img"].ToString(), js[i]["name"].ToString(), (int)js[i]["price"], (int)js[i]["seq"], (int)js[i]["stock"]);
    //        }
    //        gameContrall.instant.lamada("duiHuan");
    //        for (int i = 0; i < shops.childCount; i++)
    //        {
    //            shops.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
    //            {
    //                netConnect.instance.Anii(11);
    //            });
    //        }
    //    }
    //}
    /// <summary>
    /// 金币与人民币对应关系
    /// </summary>
    public void btnRmbToMoney(string code, bool _isApple)
    {
        this.isApple = _isApple;
        if (payDataDic.ContainsKey(code))
        {
            rmbToMoney(payDataDic[code]);
            return;
        }
        httpConnect.GET(this, httpConnect.URL + "/gold/getExChangeGoldList?type=" + code, null, rmbToMoney, httpError);
    }
    void rmbToMoney(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            if (isApple && !payDataDic.ContainsKey("1"))
            {
                payDataDic.Add("1", str);
            }
            else if (!isApple && !payDataDic.ContainsKey("0"))
            {
                payDataDic.Add("0", str);
            }
            //
            for (int i = 0; i < js.Count; i++)
            {
                httpView.instant.rmbToMoneyView((int)js[i]["gold"], (string)js[i]["id"], (double)js[i]["rmb"], i, (string)js[i]["img"], this.isApple);
            }
            gameChongZhis();
        }
    }

    /// <summary>
    /// 保险柜的存取功能
    /// </summary>
    void btnSafeMoney(bool isPush)
    {
        if (isPush)//存入
        {
            WWWForm form = new WWWForm();
            int gold = int.Parse(GameObject.Find("pushNumTextPush").GetComponent<Text>().text);
            form.AddField("gold", gold);
            byte[] b = form.data;
            httpConnect.GET(this, httpConnect.URL + "/user/userSafe", b, safeMoney1, httpError);
        }
        else
        {
            WWWForm form = new WWWForm();
            int gold = int.Parse(GameObject.Find("pushNumTextPop").GetComponent<Text>().text);
            form.AddField("gold", gold);
            byte[] b = form.data;
            httpConnect.GET(this, httpConnect.URL + "/user/userTakeOut", b, safeMoney2, httpError);
        }

    }
    void safeMoney1(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            int num = int.Parse(GameObject.Find("baoXianGui").GetComponent<Text>().text) + int.Parse(GameObject.Find("pushNumTextPush").GetComponent<Text>().text);
            GameObject.Find("baoXianGui").GetComponent<Text>().text = num.ToString();//显示保险柜的数量
            hallHttp.instance.message();
        }
        else if ((string)jso["code"] == "-1")
        {
            //netConnect.instance.Ani(4);
        }

    }
    void safeMoney2(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            int num = int.Parse(GameObject.Find("baoXianGui").GetComponent<Text>().text) - int.Parse(GameObject.Find("pushNumTextPop").GetComponent<Text>().text);
            GameObject.Find("baoXianGui").GetComponent<Text>().text = num.ToString();//显示保险柜的数量
            hallHttp.instance.message();
        }
        else if ((string)jso["code"] == "-1")
        {
            //netConnect.instance.Ani(3);
        }
    }
    /// <summary>
    /// 刷新保险柜
    /// </summary>
    void btnGameMoneyFreshen()
    {
        httpConnect.GET(this, httpConnect.URL + "/user/userSafeGet", null, getSafeMoney, httpError);//获得保险柜信息
        httpConnect.GET(this, httpConnect.URL + "/user/getCurrentUser", null, getUserMessege, httpError);//获取大厅的金币
    }
    void getSafeMoney(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if (jso["code"].ToString() == "0")
        {
            GameObject.Find("baoXianGui").GetComponent<Text>().text = jso["data"].ToString();
            netConnect.Ani(1);
        }
        if (jso["code"].ToString() == "-1")
        {
        }
    }
    void getUserMessege(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            GameObject.Find("gameMoney").GetComponent<Text>().text = js["gold"].ToString();
            GameObject.Find("jinBiMoney1").GetComponent<Text>().text = js["gold"].ToString();
        }
    }

    /// <summary>
    /// 手机号码发送短信
    /// </summary>
    void btnMobileSendMsg()
    {
        if (isbind == false)
        {
            httpConnect.GET(this, httpConnect.URL + "/user/userMobileSendMsg?mobile=" + GameObject.Find("mobileNumText").GetComponent<Text>().text, null, mobileSendMsg, httpError);
        }
        else
        {
            httpConnect.GET(this, httpConnect.URL + "/user/userMobileSendMsg?mobile=" + GameObject.Find("mobile_number").GetComponent<Text>().text, null, mobileSendMsg, httpError);
        }
    }

    void mobileSendMsg(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            //netConnect.instance.Ani(5);
        }
        else if ((string)jso["code"] == "-1")
        {
            if ((string)jso["msg"] == httpConnect.net[8].data)
            {
                //netConnect.instance.Ani(8);
            }
            else if ((string)jso["msg"] == httpConnect.net[9].data)
            {
               // netConnect.instance.Ani(9);
            }
        }
    }


    /// <summary>
    /// 用户绑定或者解绑手机号
    /// </summary>
    void btnMobileBind(bool isBind)
    {
        if (isBind == false)//绑定
        {
            httpConnect.GET(this, httpConnect.URL + "/user/userMobileBind?mobile=" + GameObject.Find("mobileNumText").GetComponent<Text>().text + "&code=" + GameObject.Find("yanZhengMaText").GetComponent<Text>().text, null, mobileBind, httpError);//绑定
        }
        else//解绑
        {
            httpConnect.GET(this, httpConnect.URL + "/user/userMobileUnBind?mobile=" + GameObject.Find("mobile_number").GetComponent<Text>().text + "&code=" + GameObject.Find("yanZhengMaText").GetComponent<Text>().text, null, mobileBind, httpError);//解绑
        }
    }
    /// <summary>
    /// 手机绑定
    /// </summary>
    /// <param name="str"></param>
    void mobileBind(string str)
    {
        Debug.Log(str + "");
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            if (isbind == false)//绑定
            {
                RootCanvas.canvas_group(GameObject.Find("number_Image").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobileUnBindleSure").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobile_InputField").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobileBindleSure").GetComponent<CanvasGroup>(), false, 0);
                isbind = true;
                //netConnect.instance.Ani(7);
                GameObject.Find("mobile_number").GetComponent<Text>().text = GameObject.Find("mobile_InputField").GetComponent<InputField>().text;
                hallHttp.instance.mobilenumber = true;
            }
            else//解绑
            {
                RootCanvas.canvas_group(GameObject.Find("number_Image").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobileUnBindleSure").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("mobile_InputField").GetComponent<CanvasGroup>(), true, 1);
                RootCanvas.canvas_group(GameObject.Find("mobileBindleSure").GetComponent<CanvasGroup>(), true, 1);
                isbind = false;
                //netConnect.instance.Ani(6);
                hallHttp.instance.mobilenumber = false;
            }
        }
        else if ((string)jso["code"] == "-1")
        {
            if ((string)jso["msg"] == httpConnect.net[10].data)
            {
                //netConnect.instance.Ani(10);
            }
            if ((string)jso["msg"] == "参数不能为空")
            {
                //netConnect.instance.Ani(8);
            }
        }
    }

    /// <summary>
    /// 生成二维码
    /// </summary>
    /// <param name="textForEncoding"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    private static Color32[] Encode2(string textForEncoding, int width, int height)
    {
        ZXing.Common.BitMatrix matrix = new MultiFormatWriter().encode(textForEncoding,
                BarcodeFormat.QR_CODE, width, height);
        Color32[] pixels = new Color32[width * height];
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 black = new Color32(0, 0, 0, 255);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (matrix[x, y])
                {
                    pixels[y * width + x] = black;// 0xff000000;
                }
                else
                {
                    pixels[y * width +x] = white;// 0xffffffff;
                }
            }
        }
        return pixels;
    }


    /// <summary>
    /// 按钮生成二维码
    /// </summary>
    public void birth_QRcode()
    {
        string Lastresult = httpView.instant.link;
        //string Lastresult = "http://baidu.com?recommendId=d3eef655-4c88-4517-8b16-d04b79de6fa6";
        var textForEncoding = Lastresult;
        if (textForEncoding != null)
        {
            //二维码写入图片  
            var color = Encode2(textForEncoding, encoded.height, encoded.width);
            encoded.SetPixels32(color);
            encoded.Apply();
            byte[] arr = encoded.EncodeToPNG();
            Texture2D texture2D = new Texture2D(encoded.width, encoded.height);
            texture2D.LoadImage(arr);
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.height, texture2D.width), new Vector2(0.5f, 0.5f));
            image_qr_code.sprite = sprite;
         
        }
    }
    /// <summary>
    /// 截图按钮方法
    /// </summary>
    public void screenshot(int code)
    {
        StartCoroutine(getTexture2d(code));
    }
    public void sharepage()
    {

        if (netConnect.instance.m_state == login_state.visitor)
        {
           // netConnect.instance.Ani(25);
        }
        else
        {
            httpConnect.GET(this, httpConnect.URL + "/user/getRegisterLink", null, STS, httpError);
        }
        //#if UNITY_ANDROID
        //        weiXinLoad.androidObject.Call("sharepage");
        //#endif

        //#if UNITY_IPHONE
        //        if (UnitySendMessageToiOS.Instante().checkInstallWeChat() == 0)
        //        {
        //            UnitySendMessageToiOS.Instante().wxShareLink();
        //        }
        //#endif
    }
    /// <summary>
    /// 截图
    /// </summary>
    /// <returns></returns>
    IEnumerator getTexture2d(int type)
    {
        string filename = "wxshare.png";
        string destination = isAndroidPlatform()?"/sdcard/DCIM/Camera":Application.persistentDataPath;//"/sdcard/DCIM";
        if (!Directory.Exists(destination))   //判断目录是否存在，不存在则会创建目录 
        {
            Directory.CreateDirectory(destination);
        }
        Path_save = destination + "/" + filename;  //文件路径
        if (!File.Exists(Path_save))
        {
            Camera.main.cullingMask = 1 << 5;
            yield return new WaitForEndOfFrame();
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)//判断是否为Android平台 
            {
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); //截取屏幕 
                texture.Apply();
                byte[] bytes = texture.EncodeToPNG();//转为字节数组 
                System.IO.File.WriteAllBytes(Path_save, bytes);//存图片
            }
            Camera.main.cullingMask = -1;
        }

#if UNITY_ANDROID
        weiXinLoad.androidObject.Call("sharepic", new object[] { Path_save, type });
#endif

#if UNITY_IPHONE
        UnitySendMessageToiOS.Instante().shareToiOS(Path_save, type);
#endif
    }
    /// <summary>
    /// 规则
    /// </summary>
    public void download_guize()
    {
        httpConnect.GET(this, httpConnect.URL + "/game/getFishingGameRule", null, json_data_guize, httpError);
    }
    void json_data_guize(string str)
    {
        //Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            httpView.instant.zhixing_guizeImage(jso["data"].ToString());//此处得解析多个大括号
        }
        gameContrall.instant.lamada("guiZe");
    }

    /// <summary>
    ///排行榜
    /// </summary>
    public void rank(string code)
    {
        //if (GameObject.Find("Toggle_huoyue").GetComponent<Toggle>().isOn == true)
        //{
            rankMessage = "huoyueT" + code;
            if (rankDic.ContainsKey(rankMessage))
            {
                json_data_rank(rankDic[rankMessage]);
            }
            else
            {
                httpConnect.GET(this, httpConnect.URL + "/rank/findRankActiveByDate?dateType=" + code, null, json_data_rank, httpError);
                //Debug.Log("------------------");
            }

        //}
        //else if (GameObject.Find("Toggle_huoyue").GetComponent<Toggle>().isOn == false)
        //{
        //    rankMessage = "huoyueF" + code;
        //    if (rankDic.ContainsKey(rankMessage))
        //    {
        //        json_data_rank(rankDic[rankMessage]);
        //    }
        //    else
        //    {
        //        httpConnect.GET(this, httpConnect.URL + "/rank/findRankPayByDate?dateType=" + code, null, json_data_rank, httpError);
        //        // Debug.Log("+++++++++++++++");
        //    }
        //}
    }
    /// <summary>
    /// 活跃
    /// </summary>
    /// <param name="str"></param>
    void json_data_rank(string str)
    {
        //Debug.Log(str);
        if (!rankDic.ContainsKey(rankMessage))
        {
            rankDic.Add(rankMessage, str);
        }
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            for (int i = 0; i < js.Count; i++)
            {
                httpView.instant.Rank_huoyue((string)js[i]["head"], (string)js[i]["nickName"], js[i]["num"].ToString(), i);
            }
        }

        gameContrall.instant.Rank_huoyue("gameRank");
    }

    /// <summary>
    /// 邮箱
    /// </summary>
    public void email()
    {
        WWWForm form = new WWWForm();
        form.AddField("page", 1);
        form.AddField("size", 10);
        byte[] b = form.data;
        httpConnect.GET(this, httpConnect.URL + "/user/getUserGameEmail", b, json_email, httpError);
    }
    void json_email(string str)
    {
        Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            JsonData jsa = js["rows"];
            if (jsa.Count != 0)
            {
                for (int j = 0; j < jsa.Count; j++)
                {
                    int b = j;
                    row.title = jsa[j]["title"].ToString();
                    row.content = jsa[j]["content"].ToString();
                    row.createTime = jsa[j]["createTime"].ToString();
                    row1.Add(row);
                    httpView.instant.email_game(row1[b].title, row1[b].content, row1[b].createTime);
                }
            }
            else
            {
                Debug.Log("没有邮件");
            }
            GameObject.Find("email_Content").AddComponent<Email>();
        }
        gameContrall.instant.lamada("youJian");
    }
    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        //GameObject load = (GameObject)Resources.Load("Rank/loadFail");
        GameObject jiazai = Instantiate(weiXinLoad.instance.loadFailP/*load*/, GameObject.Find("Canvas").transform) as GameObject;
        Debug.Log(str);
    }

    //打开游戏充值界面
    void gameChongZhis()
    {
        gameContrall.instant.lamada("gameChongZhis");//打开游戏充值界面
        PayGame payGame = gameContrall.instant.buttonBehind.GetComponent<PayGame>();
        if (payGame == null)
        {
            gameContrall.instant.buttonBehind.AddComponent<PayGame>();
        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// 隐藏提现
    /// </summary>
    public void money_st()
    {
        httpConnect.GET(this, httpConnect.URL + "/game/getSwitch", null, TX, httpError);

    }
    void STS(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);   
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            Application.OpenURL(js.ToString());
            Register=true;
        }
        if ((string)jso["code"] == "-1")
        {
            if ((string)jso["msg"] == httpConnect.net[26].data)
            {
                //netConnect.instance.Ani(26);
            }
            Register = false;
        }
    }

    void TX(string str)
    {
        Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            gameContrall.instant.lamada("Withdraw");
        }
        else
        {
            netConnect.Ani(11);
        }
    }
        bool isAndroidPlatform()
    {
#if UNITY_IOS
        return false;//Application.persistentDataPath;
#elif UNITY_ANDROID
        return true;
#endif
    }
}