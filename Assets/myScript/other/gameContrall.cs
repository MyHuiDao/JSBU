using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CClient;
using System.IO;
public enum fishGame
{
    meiRenYu = 0,
    liKuiPiYu = 1,
    DaNaoTianGong = 2
}
public class gameContrall : MonoBehaviour
{
    /// <summary>
    ///  该类主要用来对游戏大厅的界面按钮进行各种功能实现
    /// </summary>
    public GameObject Loading;
    public GameObject buttonBehind;//bottomBhind对象，便于查找子对象
    public GameObject topParent;//top对象，便于查找子对象
    public GameObject bottomParent;//bottom对象，便于查找子对象
    public GameObject middleParent;
    public static gameContrall instant = null;
    public GameObject chat_head;
    public GameObject chat_kefu;
    private InputField chat;
    private AsyncOperation operation;
    private RectTransform rect;
    private bool test;
    private GameObject heads;
    private GameObject kefu;
    private Transform chat_parent;
    public string aa;
    public bool bb;
    public bool cc;
    public bool ee;
    private int timeDay;
    private void Awake()
    {
        httpConnect.Toggles();
    }
    void Start()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
#if UNITY_IOS
        if(weiXinLoad.instance.AndroidFunction)
        {
            bottomParent.transform.Find("duiHuan").GetComponent<Button>().onClick.AddListener(() => shop("Withdraw"));//兑换
        }else{
            if (weiXinLoad.instance.ServerIdentifyStr.Equals("0")/*weiXinLoad.instance.AndroidFunction*/)
            {
                bottomParent.transform.Find("duiHuan").GetComponent<Button>().onClick.AddListener(() => shop("Withdraw"));//兑换
            }
            else
            {
                bottomParent.transform.Find("duiHuan").GetComponent<Button>().onClick.AddListener(() => jingQingQiDai(null));//兑换
            } 
        }


#elif UNITY_ANDROID
        bottomParent.transform.Find("duiHuan").GetComponent<Button>().onClick.AddListener(() => shop("Withdraw"));//兑换
#endif
        //string time = System.DateTime.Now.ToString().Split(' ')[0];//06/02/2018
        //string[] strTime = time.Split('/');
        //string strTimeDay = strTime[0] + strTime[1];
        //timeDay = int.Parse(strTimeDay);
        instant = this;
        //chat_head = (GameObject)Resources.Load("Rank/chat_user");
        //chat_kefu = (GameObject)Resources.Load("Rank/chat_kefu");
        chat_parent = GameObject.Find("chat_Content").transform;
        chat = GameObject.Find("chat").GetComponent<InputField>();
        rect = GameObject.Find("chat_Content").GetComponent<RectTransform>();
        GameObject.Find("send_web").GetComponent<Button>().onClick.AddListener(send);//发送聊天消息
        GameObject IDTexObj = GameObject.Find("IDtex").gameObject;
//#if UNITY_IOS
//        //没有安装微信 && 安卓端
//        if(UnitySendMessageToiOS.Instante().checkInstallWeChat() != 0 && !weiXinLoad.instance.AndroidFunction)
//        {
//            IDTexObj.transform.localScale = Vector3.zero;
//        }s
//#endif
        GameObject.Find("backGround").GetComponent<Button>().onClick.AddListener(close_);
        IDTexObj.GetComponent<Button>().onClick.AddListener(() => Android_copy("IDtext"));//同上
        GameObject.Find("gold_player_pay").GetComponent<Button>().onClick.AddListener(() => player_chongzhi("gameChongZhis"));
        GameObject.Find("chongzhi").GetComponent<Button>().onClick.AddListener(() => gameChongZhi("gameChongZhis"));//充值
        topParent.transform.Find("guiZe").GetComponent<Button>().onClick.AddListener(() => gameGuiZe("guiZe"));//规则
        topParent.transform.Find("head").GetComponent<Button>().onClick.AddListener(() => head("playerInfo"));//头像
        topParent.transform.Find("set").GetComponent<Button>().onClick.AddListener(() => lamada("set"));//设置
        topParent.transform.Find("customServe").GetComponent<Button>().onClick.AddListener(() => Cus_kefu("customServe"));//客服
        bottomParent.transform.Find("accountBindle").GetComponent<Button>().onClick.AddListener(() => lamada("accountBindle"));//账号绑定
        bottomParent.transform.Find("gameRank").GetComponent<Button>().onClick.AddListener(() => Rank_huoyue("gameRank"));//排行榜
        bottomParent.transform.Find("moneySafe").GetComponent<Button>().onClick.AddListener(() => lamada("moneySafe"));//保险柜
        bottomParent.transform.Find("gameChongZhi").GetComponent<Button>().onClick.AddListener(() => gameChongZhi("gameChongZhis"));//充值
        bottomParent.transform.Find("youJian").GetComponent<Button>().onClick.AddListener(() => email("youJian"));//邮件

        //GameObject.Find("serve_button").GetComponent<Button>().onClick.AddListener(() => customServe1("duiHuan"));
        GameObject freeMoneyObj = topParent.transform.Find("freeMoney").gameObject;
#if UNITY_IOS
        if(weiXinLoad.instance.AndroidFunction)
        {
            freeMoneyObj.transform.localScale = Vector3.one; 
        }else{
            if (weiXinLoad.instance.ServerIdentifyStr.Equals("1"))
            {
                freeMoneyObj.transform.localScale = Vector3.zero;
            }else{
                freeMoneyObj.transform.localScale = Vector3.one; 
            }
        }

#endif
        freeMoneyObj.GetComponent<Button>().onClick.AddListener(freemoney); //分享
        GameObject.Find("lianXiKeFu").GetComponent<Button>().onClick.AddListener(() => customServe1("tuiGuangyuan"));
        EventTriggerListener.Get(topParent.transform.Find("quitGame").gameObject).onClick = quitGame;//退出游戏
        EventTriggerListener.Get(bottomParent.transform.Find("tuiGuangYuan").gameObject).onClick = tuiGuangYuan;//推广员
        GameObject.Find("email").transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            RootCanvas.canvas_group(GameObject.Find("email").GetComponent<CanvasGroup>(), false, 0);
            RootCanvas.canvas_group(GameObject.Find("email_view").GetComponent<CanvasGroup>(), true, 1);
        });
        //=========================================================================================================    
        GameObject meiRenYuObj = middleParent.transform.Find("meiRenYu").gameObject;
        GameObject liKuiPiYuObj = middleParent.transform.Find("liKuiPiYu").gameObject;
        GameObject DaNaoTianGongObj = middleParent.transform.Find("DaNaoTianGong").gameObject;
        GameObject yuErKuaiPaoObj = middleParent.transform.Find("yuErKuaiPao").gameObject;
        meiRenYuObj.GetComponent<Button>().onClick.AddListener(delegate () { catchFish((int)fishGame.meiRenYu,meiRenYuObj); });
        liKuiPiYuObj.gameObject.GetComponent<Button>().onClick.AddListener(delegate () { catchFish((int)fishGame.liKuiPiYu,liKuiPiYuObj); });
        DaNaoTianGongObj.GetComponent<Button>().onClick.AddListener(delegate () { catchFish((int)fishGame.DaNaoTianGong,DaNaoTianGongObj); });
        yuErKuaiPaoObj.GetComponent<Button>().onClick.AddListener(delegate () { catchYuErMatch(yuErKuaiPaoObj); });
        GameObject.Find("shouJiBangDing").GetComponent<Button>().onClick.AddListener(phoneBindle);
        for (int i = 0; i < 4; i++)
        {
            int a = i;
            if (i % 2 == 0)
            {
                GameObject.Find(httpConnect.toggle_list[i].name).GetComponent<Toggle>().onValueChanged.AddListener((bool value) => Change_Toggle(a, a + 1));
            }
            else
            {
                GameObject.Find(httpConnect.toggle_list[i].name).GetComponent<Toggle>().onValueChanged.AddListener((bool value) => Change_Toggle(a, a - 1));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bb == true)
        {
            receive();
            bb = false;
        }
        if (cc == true)
        {
            tishi();
            cc = false;
        }
        if (ee == true)
        {
            netConnect.instance.Ani(16);
            ee = false;
        }
        Rotate_light("free_light");
    }
    void Cus_kefu(string name)
    {
        if (GameObject.Find("cus_right").GetComponent<copy>() == null)
        {
            GameObject.Find("cus_right").AddComponent<copy>();
        }
        lamada(name);
    }
    /// <summary>
    /// 免费金币
    /// </summary>
    public void freemoney()
    {
        buttonBehind.transform.Find("freemoney").localScale = Vector3.one;
    }
    void tishi()
    {
        netConnect.instance.Ani(12);
    }
    /// <summary>
    /// 旋转特效
    /// </summary>
    public void Rotate_light(string name)
    {
        GameObject.Find(name).transform.Rotate(0, 0, 1);
    }
    /// <summary>
    /// 从推广员跳到客服
    /// </summary>
    void customServe1(string name)
    {
        Cus_kefu("customServe");
        buttonBehind.transform.Find(name).localScale = Vector3.zero;
        RootCanvas.canvas_group(GameObject.Find("Canvas_button").GetComponent<CanvasGroup>(), false, 0);
    }
    /// <summary>
    /// 聊天
    /// </summary>
    void send()
    {
        if (chat.text.Trim() != "")
        {
            heads = Instantiate(chat_head, chat_parent) as GameObject;
            heads.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = chat.text;
            ClientSocket.instant().send("10000", chat.text);
            if (GameObject.Find("chat_Content").GetComponent<RectTransform>().sizeDelta.y > 880)
            {
                touxiang.instance.add = true;
            }
        }
        else
        {
            netConnect.instance.Ani(13);
            return;
        }
        chat.text = null;
    }
    /// <summary>
    /// 接收客服消息
    /// </summary>
    public void receive()
    {
        kefu = Instantiate(chat_kefu, chat_parent) as GameObject;
        kefu.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = aa;
        if (GameObject.Find("chat_Content").GetComponent<RectTransform>().sizeDelta.y > 880)
        {
            touxiang.instance.add = true;
        }
    }
    //========================================================================
    /// <summary>
    /// 敬请期待
    /// </summary>
    void jingQingQiDai(GameObject obj)
    {
        GameObject middleLastObj = middleParent.transform.GetChild(middleParent.transform.childCount - 1).gameObject;
        if (middleLastObj == obj)
        {
            GameObject.Find("jingQingQiDai").GetComponent<Animation>().Play();
            Invoke("jingQingQiDai1", 1);
        }
        if(obj == null)
        {
            GameObject.Find("jingQingQiDai").GetComponent<Animation>().Play();
            Invoke("jingQingQiDai1", 1); 
        }
    }
    //=============================================================================
    void jingQingQiDai1()
    {
        GameObject.Find("jingQingQiDai").transform.localScale = Vector3.zero;
    }
    //=============================================================================
    /// <summary>
    /// 玩家信息
    /// </summary>
    /// <param name="g"></param>
    void head(string name)
    {
        lamada(name);
    }
    public void show_player(string id, string nickName, string gold, string tiYan, int lv, string _isVip)
    {
        GameObject.Find("player_id").transform.GetComponentInChildren<Text>().text = id;
        GameObject.Find("player_IDS").transform.GetComponentInChildren<Text>().text = id;
        GameObject.Find("player_nickname").transform.GetComponentInChildren<Text>().text = nickName;
        GameObject.Find("player_gold").transform.GetChild(1).GetComponent<Text>().text = gold;
        GameObject.Find("player_tiyan").transform.GetChild(0).GetComponent<Text>().text = tiYan;
        GameObject.Find("player_lv").transform.GetComponentInChildren<Text>().text = lv.ToString();
        if (netConnect.instance.m_state == login_state.visitor)
        {
            GameObject.Find("player_logintype").transform.GetComponentInChildren<Text>().text = "游客登录";

            GameObject.Find("player_head").GetComponent<Image>().sprite = GameObject.Find("touXiangKuang").GetComponent<Image>().sprite;
        }
        if (netConnect.instance.m_state == login_state.wechat)
        {
            GameObject.Find("player_logintype").transform.GetComponentInChildren<Text>().text = "微信登录";
        }
        if (_isVip == "0")
        {
            GameObject.Find("player_vip").transform.localScale = Vector3.zero;
        }
        else
        {
            GameObject.Find("player_vip").transform.localScale = Vector3.one;
        }
    }
    bool wechat_sever;
    /// <summary>
    /// 退出游戏
    /// </summary>
    void quitGame(GameObject g)
    {
        buttonBehind.transform.Find("quitGame").localScale = Vector3.one;
    }
    //======================================================================
    /// <summary>
    /// 手机绑定
    /// </summary>
    /// <param name="g"></param>
    void phoneBindle()
    {
        if (netConnect.instance.m_state == login_state.wechat)
        {
            buttonBehind.transform.Find("mobileBindle").localScale = Vector3.one;
            buttonBehind.transform.Find("accountBindle").localScale = Vector3.zero;
            RootCanvas.find("backGround").transform.localScale = Vector3.one;//显示阴影背景
        }
        else
        {
            netConnect.instance.Ani(15);
        }
    }
    /// <summary>
    /// 推广员
    /// </summary>
    void tuiGuangYuan(GameObject g)
    {
        buttonBehind.transform.Find("tuiGuangyuan").localScale = Vector3.one;
        RootCanvas.canvas_group(GameObject.Find("Canvas_button").GetComponent<CanvasGroup>(), true, 1);
#if UNITY_IOS
        if(weiXinLoad.instance.AndroidFunction)
        {
            buttonBehind.transform.Find("tuiGuangyuan").GetChild(6).localScale = Vector3.one;
            buttonBehind.transform.Find("tuiGuangyuan").GetChild(7).localScale = Vector3.one;
        }else{
            if (weiXinLoad.instance.ServerIdentifyStr.Equals("1"))
            {
                buttonBehind.transform.Find("tuiGuangyuan").GetChild(6).localScale = Vector3.zero;
                buttonBehind.transform.Find("tuiGuangyuan").GetChild(7).localScale = Vector3.zero;
            }else{
                buttonBehind.transform.Find("tuiGuangyuan").GetChild(6).localScale = Vector3.one;
                buttonBehind.transform.Find("tuiGuangyuan").GetChild(7).localScale = Vector3.one;
            }
        }
 
#endif
        string filename = "/huidao.png";
        string destination = Application.persistentDataPath;
        string path = destination + filename;
        if (Directory.Exists(path))   //判断目录是否存在，不存在则会创建目录 
        {
            Directory.Delete(path);
        }
        else
        {
            return;
        }
    }
    bool isOpenChongZhiNum = false;
    /// <summary>
    /// 游戏充值,点击之后发送信息显示
    /// </summary>
    public void gameChongZhi(string name)
    {
        if (GameObject.Find("Pay_WxKeFu").GetComponent<copy>() == null)
        {
            GameObject.Find("Pay_WxKeFu").AddComponent<copy>();
        }
        if (!isOpenChongZhiNum)
        {

#if UNITY_IPHONE
            if(weiXinLoad.instance.AndroidFunction)
            {
                httpSend.instant.btnRmbToMoney("0", false);
            }else{
                if (weiXinLoad.instance.ServerIdentifyStr.Equals("0") /*&& weiXinLoad.instance.AndroidFunction*/)//苹果审核通过后为0
                {
                    httpSend.instant.btnRmbToMoney("0", false);
                }
                else
                {
                    httpSend.instant.btnRmbToMoney("1", true);
                }
            }

#endif
#if UNITY_ANDROID
            httpSend.instant.btnRmbToMoney("0", false);
#endif
            isOpenChongZhiNum = true;
        }
        else
        {
            lamada(name);
        }
    }
    void player_chongzhi(string name)
    {
        GameObject.Find("playerInfo").transform.localScale = Vector3.zero;
        gameChongZhi(name);
    }
    bool load = false;
    /// <summary>
    /// 游戏规则
    /// </summary>
    /// <param name="name"></param>
    void gameGuiZe(string name)
    {
        if (!load)
        {
            httpSend.instant.download_guize();
            load = true;
        }
        else
        {
            lamada(name);
        }
    }
    bool open = false;
    /// <summary>
    /// 商城
    /// </summary>
    /// <param name="name"></param>
    void shop(string name)
    {
       

        //if (!open)
        //{
        //    httpSend.instant.btnGetGoods();
        //    open = true;
        //}
        //else
        //{
        //Transform transf = buttonBehind.transform.Find(name);
        //transf.localScale = Vector3.one;
        //RootCanvas.find("backGround").transform.localScale = Vector3.one;//显示阴影背景
        //}
        if (netConnect.instance.m_state == login_state.wechat)
        {
            lamada(name);
            httpSend.instant.money_st();
        }
        if (netConnect.instance.m_state == login_state.visitor)
        {
            netConnect.instance.Ani(18);
        }
    }
    bool rank;
    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    public void Rank_huoyue(string name)
    {
        GameObject.Find("HuoYueParent").transform.position = Vector3.zero;
        if (!rank)
        {
            rankGrag.instance.delete();
            httpSend.instant.rank("0");
            rank = true;
        }
        else
        {
            lamada(name);
           
        }
    }
    bool refresh = false;
    /// <summary>
    /// 邮箱
    /// </summary>
    /// <param name="name"></param>
    void email(string name)
    {
        if (!refresh)
        {
            httpSend.instant.email();
            refresh = true;
        }
        else
        {
            lamada(name);
        }
    }
    /// <summary>
    /// 匿名方法
    /// </summary>
    /// <param name="name"></param>
    public void lamada(string name)
    {
        Transform transf = buttonBehind.transform.Find(name);
        transf.localScale = Vector3.one;
#if UNITY_IOS
        if (transf.name.Contains("customServe"))
        {
            if (UnitySendMessageToiOS.Instante().checkInstallWeChat() != 0 && weiXinLoad.instance.AndroidFunction == false)
            {
                Transform transf2 = transf.GetChild(3).GetChild(1);
                for (int i = 2; i < 6; ++i)
                {
                    transf2.GetChild(i).GetChild(1).localScale = Vector3.zero;
                }

            }
        }
#endif
        RootCanvas.find("backGround").transform.localScale = Vector3.one;//显示阴影背景

    }
    //===========================================================================
    /// <summary>
    /// 进入捕鱼游戏
    /// </summary> 
    public bool return_scene;
    void catchFish(int buyuGanme,GameObject obj)
    {
       
        GameObject middleLastObj = middleParent.transform.GetChild(middleParent.transform.childCount - 1).gameObject;
        if (middleLastObj == obj)
        {
            GameObject load = Instantiate(Loading, GameObject.Find("Canvas").transform) as GameObject;
            getMeiRenYuArea.buyuGame = buyuGanme;
            return_scene = true;
            obj.GetComponent<Button>().enabled = true;
            operation = SceneManager.LoadSceneAsync("MainMeiRenYu");
            if (operation != null)
            {
                ClientSocket.instance.ws.Close();
                m_slider.instance.GetScene(operation, true, false);

            }
            else
            {
                Debug.Log("跳转场景为空");
            }
        }
      
    }

    /// <summary>
    /// 进入鱼儿赛跑
    /// </summary>
    void catchYuErMatch(GameObject obj)
    {
        GameObject middleLastObj = middleParent.transform.GetChild(middleParent.transform.childCount - 1).gameObject;
        if (middleLastObj == obj)
        {
            obj.GetComponent<Button>().enabled = true;
            return_scene = true;
            operation = SceneManager.LoadSceneAsync("yuerScene");
            //Debug.Log("实例化了");
            GameObject load = Instantiate(Loading, GameObject.Find("Canvas").transform) as GameObject;
            if (operation != null)
            {
                ClientSocket.instance.ws.Close();
                m_slider.instance.GetScene(operation, false, true);
            }
            else
            {
                Debug.Log("跳转场景为空");
            }

        }

    }
    /// <summary>
    /// 复制文字
    /// </summary>
    /// <param name="word"></param>
    public void Android_copy(string word)
    {
        string text = GameObject.Find(word).GetComponent<Text>().text;
#if UNITY_ANDROID
        weiXinLoad.androidObject.Call("Android_copy", text);
#endif

#if UNITY_IPHONE
        UnitySendMessageToiOS.Instante().copyText(text,"复制成功!");
#endif
    }
    
    private void Change_Toggle(int group1, int group2)
    {
        httpConnect.toggle_group[group1].alpha = 1;
        httpConnect.toggle_group[group1].blocksRaycasts = true;
        httpConnect.toggle_group[group2].alpha = 0;
        httpConnect.toggle_group[group2].blocksRaycasts = false;
    }
    void close_()
    {
      
        if (WithDraw_Sp.instance.return_blind == true)
        {
        
           if(GameObject.Find("mobileBindle").transform.localScale == Vector3.one)
            {
                GameObject.Find("Withdraw").transform.localScale = Vector3.one;
            }
            else
            {
                GameObject.Find("Withdraw").transform.localScale = Vector3.zero;
                RootCanvas.find("backGround").transform.localScale = Vector3.zero;//显示阴影背景
            }
            GameObject.Find("mobileBindle").transform.localScale = Vector3.zero;
            WithDraw_Sp.instance.return_blind = false;
            GameObject.Find("yanZhengMa_InputField").GetComponent<InputField>().text = null;
        }
        else
        {
            for (int i = 0; i < buttonBehind.transform.childCount; ++i)
            {
                buttonBehind.transform.GetChild(i).localScale = Vector3.zero;
            }
            RootCanvas.find("backGround").transform.localScale = Vector3.zero;//显示阴影背景
        }   
    }
   

}