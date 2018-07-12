using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

struct PayBtn
{
    public bool isSelect;
    public GameObject btnObj;
}

public class PayGame : MonoBehaviour
{
    public static PayGame instance;
    string goldID;
    string terminalId;//终端id
    public string payMethod = "0";
    string token;
    Transform gameChongZhiTopTrans;
    Dictionary<double, int> applePayDic = new Dictionary<double, int>();
    private bool isApplePay = false;
    private string appleID;
    private string oriderID;
    private string pingZeng;
    private int environment;
    PayBtn[] _PayBtn = new PayBtn[2];
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {

        AddOnClick();
        terminalId = SystemInfo.deviceUniqueIdentifier;
        token = netConnect.token;
        GameObject.Find("Ali").GetComponent<Button>().onClick.AddListener(()=>OnPay("ZFB"));
        //GameObject.Find("Wx").GetComponent<Button>().onClick.AddListener(() => OnPay("WX"));
        GameObject.Find("Vip").GetComponent<Button>().onClick.AddListener(Pay_Vip_service);
        transform.GetChild(2).GetChild(6).GetChild(5).GetComponent<Button>().onClick.AddListener(fresh);
        transform.GetChild(2).GetChild(7).GetChild(3).GetComponent<Button>().onClick.AddListener(() => common(7, false, 0));
    }
    void Update(){ }

    void AddOnClick()
    {
        gameChongZhiTopTrans = gameObject.transform.GetChild(2).GetChild(5);
        for (int i = 0; i < gameChongZhiTopTrans.childCount; ++i)
        {
            EventTriggerListener.Get(gameChongZhiTopTrans.GetChild(i).gameObject).onClick = OnGoldClick;
        }
    }

    void fresh()
    {
        common(6, false, 0);
        hallHttp.instance.message();
    }

    void OnPayMethodClick(GameObject go)
    {
#if UNITY_ANDROID
        
            payMethod = "0";
            isApplePay = false;
#elif UNITY_IOS

        if (go.name.Equals("_applePay")){
            payMethod = "1";
            isApplePay = true;
        }
#endif
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="go"></param>
    void OnGoldClick(GameObject go)
    {
        Debug.Log("NNNNNNN:" + go.name);
        if (payMethod == null || token == null) return;
        goldID = go.transform.GetChild(4).GetComponent<Text>().text;

#if UNITY_IPHONE
        appleID = go.transform.GetChild(1).GetComponent<Text>().text.Replace("¥","");
        if(weiXinLoad.instance.ServerIdentifyStr.Contains("0"))
        {
            common(6, true, 1); 
        }else{
            OnPay("IAP");
        }
#endif

#if UNITY_ANDROID
        common(6, true, 1); 
#endif

    }
    /// <summary>
    /// 支付方法
    /// </summary>
    /// <param name="type"></param>
    void OnPay(string type)
    {
#if UNITY_ANDROID
        weiXinLoad.androidObject.Call("singlePay", goldID, type, terminalId, token);
#elif UNITY_IOS
        UnitySendMessageToiOS.Instante().rechargeToiOS(type, terminalId, goldID, token, appleID);
#endif

    }
    /// <summary>
    /// 
    /// </summary>
    ///
   void Pay_Vip_service()
    {     
        common(7,true,1);
        common(6, false, 0);
    }
    void common(int child,bool Rays,int alpha )
    {
        RootCanvas.canvas_group(transform.GetChild(2).GetChild(child).GetComponent<CanvasGroup>(), Rays, alpha);
    }


#region  iOS回调
    //支付环境
    public void applePayBackHJ(string str)
    {
        if (str == null)
        {
            if (str.Equals("environment=Sandbox"))
            {
                environment = 0;

            }
            else
            {
                environment = 1;
            }
            Debug.Log("environment:" + environment);
        }

    }
    //订单ID
    public void applePayBackOrderID(string orederID)
    {
        if(orederID!=null)
            this.oriderID = orederID;
        
        //Debug.Log("oriderID:" + orederID);
    }

    //将支付凭证发送到服务器进行验证
    public void applePayBackPZ(string pz)
    {
        if(pz!=null){
            pingZeng = pz;
            //Debug.Log("pingZeng:" + pz);
            StartCoroutine(HttpPostForYZ()); 
        }else{
            Debug.Log("支付凭证为空");
        }

    }
#endregion

    IEnumerator HttpPostForYZ()
    {
        //通知iOS
        if(pingZeng!=null && oriderID!= null)
        {
            UnitySendMessageToiOS.Instante().showorHideHud("0");
            string url = httpConnect.URL + "/pay/applePaycallback";
            WWWForm form = new WWWForm();
            form.AddField("voucher", pingZeng);
            form.AddField("orderId", oriderID);
            form.AddField("type", environment);
            byte[] byteArr = form.data;
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", netConnect.token);
            WWW wWW = new WWW(url, byteArr, header);
            yield return wWW;

            if (wWW == null)
            {
                //Debug.Log("支付验证没有成功");
                postError(wWW.text);

            }
            else
            {
                postBack(wWW.text);
            }
        }

    }

    void postBack(string str)
    {
        Debug.Log("PostBackStr:" + str);
        JsonData js = JsonMapper.ToObject(str);
        if ((string)js["code"] == "0")
        {
            //向ios传递参数关闭界面
            UnitySendMessageToiOS.Instante().showorHideHud("购买成功:"+(string)js["data"]+"金币");
        }
        else
        {
            //Debug.Log("没有查询到结果code:"+(string)js["code"]);
            UnitySendMessageToiOS.Instante().showorHideHud("购买不成功");
        }
    }
    void postError(string str)
    {
        //Debug.Log("PostBackErrorStr:" + str);
    }

    public void addClick(GameObject obj)
    {
        if (obj.name.Contains("gold_1"))
        {
            EventTriggerListener.Get(obj).onClick = OnGoldClick;
        }

    }
}
