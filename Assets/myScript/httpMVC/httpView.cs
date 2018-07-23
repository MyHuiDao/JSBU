using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


/// <summary>
/// 此类用来显示进入到游戏之前的所有信息显示
/// </summary>
public class httpView : MonoBehaviour {
    private Text nameText;
    private Text centenText;
    public static string id;//本机唯一识别符
    //public static Sprite touXiang;
    public static  Sprite head_sprite;
    private Sprite Guize_sprite;
    private GameObject Guize;
    public GameObject gold;
    public GameObject shop;
    public string link;
    public static httpView instant = null;
    public GameObject rank;
    public GameObject email;
    public static  string head_;
    private Sprite[] Pay = new Sprite[8];
    //private Sprite[] Shops = new Sprite[4];
    //private Sprite[] Ranks = new Sprite[3];
    private Dictionary<int, GameObject> generateDic = new Dictionary<int, GameObject>();
  

    void Start() {
        //touXiang = Resources.Load<Sprite>("Rank/cat");
        //if(weiXinLoad.instance.headSprite == null)
        //{
        //    Debug.LogError("=========================");
        //}
        Guize = GameObject.Find("guize_parent");
        instant = this;
        nameText = GameObject.Find("nameText").GetComponent<Text>();
        centenText = GameObject.Find("gunDongGongGao").GetComponent<Text>();
        //for (int i = 0; i < Pay.Length; i++)
        //{
        //    Pay[i] = Resources.Load<Sprite>("Pay_image/bi_" + (i + 1).ToString());
        //}
        //for (int i = 0; i < Shops.Length; i++)
        //{
        //    Shops[i] = Resources.Load<Sprite>("Pay_image/" + ((i + 1) * 100).ToString());
        //}
        //for (int i = 0; i < Ranks.Length; i++)
        //{
        //    Ranks[i] = Resources.Load<Sprite>("Pay_image/rank_" + (i + 1).ToString());
        //}
    }

    // Update is called once per frame
    void Update() {
        GameObject.Find("jinBiMoney1").GetComponent<Text>().text = saveDate.gold.ToString();
        GameObject.Find("gameMoney").GetComponent<Text>().text = saveDate.gold.ToString();
        GameObject.Find("withdraw_Text").GetComponent<Text>().text = saveDate.gold.ToString();
    }

    /// <summary>
    /// 显示玩家信息
    /// </summary>
    /// <param name="_code">id</param>
    /// <param name="_experience">体验币</param>
    /// <param name="_gold">金币</param>
    /// <param name="_heading">头像</param>
    /// <param name="_id">ID主键</param>
    /// <param name="_nickName">昵称</param>
    /// <param name="_userType">玩家类型，免费玩家：free,付费玩家：pay</param>
    /// <param name="_isVip">是否为VIP</param>

    public void getname(string _nickName, string _code, int _experience, int _gold, string _heading, string _id, string _userType, string _isVip, string _isBingd, int _vipLevel)
    {
        nameText.text = _nickName;
        saveDate.nickname = _nickName;
        GameObject.Find("tiYanMoney1").GetComponent<Text>().text = _experience.ToString();
        saveDate.gold = _gold;
        GameObject.Find("IDtext").GetComponent<Text>().text = _code;
        saveDate.code = _code;
        if (_isBingd == "0")
        {
            httpSend.isbind = false;//没有绑定
        }
        else
        {
            httpSend.isbind = true;//已经绑定
        }
        head_ = _heading;
        if (head_.Contains("http"))
        {
            StartCoroutine(image(_heading));
        }
        else
        {
            head_photo("touXiangKuang");
        } 
        id = _id;
        gameContrall.instant.show_player(_code, _nickName, _gold.ToString(), _experience.ToString(), _vipLevel, _isVip);
        GameObject.Find("mobileBindle").transform.GetChild(4).GetChild(1).GetComponent<Text>().text = _code;
    }

    /// <summary>
    /// 获得推广链接
    /// </summary>
    /// <param name="_url">推广链接</param>
    public void getRecommendUrlView(string _url)
    {
        link = _url;
        
    }
    /// <summary>
    /// 获得游戏大厅公告
    /// </summary>
    /// <param name="_data">公告</param>
    public void  getAnnocementView(string _data)
    {    
        centenText.text = _data;
    }
    /// <summary>
    /// 获得商品列表
    /// </summary>
    /// <param name="_desc">商品详情</param>
    /// <param name="_id">主键</param>
    /// <param name="_img">图片资源</param>
    /// <param name="_name">商品名称</param>
    /// <param name="price">商品价格</param>
    /// <param name="_seq">排序</param>
    /// <param name="_stock">库存</param>
    public void getGoodsView(string _desc, string _id, string _img, string _name, int price, int _seq, int _stock)
    {
        GameObject obj = Instantiate(shop, GameObject.Find("shop_Panel").transform) as GameObject;
        obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _name;
        obj.transform.GetChild(2).GetChild(1).GetComponent<Text>().text = price.ToString();
        obj.transform.GetChild(3).GetComponent<Image>().sprite = weiXinLoad.instance.huafeiSprites[_seq - 1];//Shops[_seq - 1];
    }
    /// <summary>
    /// 获得人民币与金币的转化
    /// </summary>
    /// <param name="_goldNum">金币数量</param>
    /// <param name="_goldId">主键</param>
    /// <param name="_rmb">人民币</param>
    /// <param name="_req">排序</param>
    /// <param name="_gold">金币</param>
    public void rmbToMoneyView(int _gold, string _goldId, double _rmb, int _req, string _goldNum, bool isApple)
    {
        if(isApple && _rmb <= 6.0)//苹果支付首充
        {
            if(generateDic.ContainsKey(_req))
            {
                Destroy(generateDic[_req]);
                generateDic.Remove(_req);
            }
            return;
        }
        #region 获取信息
        if(!generateDic.ContainsKey(_req)){
            GameObject obj  = Instantiate(gold, GameObject.Find("gameChongZhiTop").transform) as GameObject;
            generateDic.Add(_req, obj);
            //处理苹果界面
            if(_rmb <= 6.0 && gameContrall.instant.buttonBehind.GetComponent<PayGame>() != null)
            {
                PayGame.instance.addClick(obj); 
            }

            setGoldInfo(obj.transform, _gold, _goldId, _rmb,  _req,  _goldNum,  isApple);
        }else{
            GameObject objExsist = generateDic[_req];
            setGoldInfo(objExsist.transform, _gold, _goldId, _rmb, _req, _goldNum, isApple);
        }
        #endregion
    }
    /// <summary>
    /// 获取微信头像
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator image(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(www.texture);
        head_sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width,www.texture.height), Vector2.zero);
        GameObject[] portrait =GameObject.FindGameObjectsWithTag("head");
        for(int i = 0; i < portrait.Length; i++)
        {
            portrait[i].GetComponent<Image>().sprite = head_sprite;
        }

    }
    public void head_photo(string name)
    {
        GameObject.Find(name).GetComponent<Image>().sprite = weiXinLoad.instance.headSprite;//touXiang;
    }
  
    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator download_image(string url,GameObject ob,string  name)
    {
        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(www.texture);
        Guize_sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
        ob.transform.Find(name).GetComponent<Image>().sprite = Guize_sprite;
    }
    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="name"></param>
    /// <param name="number"></param>
    public void Rank_huoyue(string head,string name,string number,int a)
    {
        #region 排行榜
        if (a<20)//限制上榜名单
        {
            GameObject obj = Instantiate(rank, GameObject.Find("HuoYueParent").transform) as GameObject;
           // Debug.Log(name.Length);
            if (name.Length > 3)
            {
                string name_three = name.Substring(0, 3);
                obj.transform.GetChild(3).GetComponent<Text>().text = name_three;
            }
            else
            {
                obj.transform.GetChild(3).GetComponent<Text>().text = name;
            }

            // obj.transform.GetChild(3).GetComponent<Text>().text = name;
            //if (GameObject.Find("Toggle_huoyue").GetComponent<Toggle>().isOn == true)//区分活跃榜与排行榜
            //{
                if (Time_Rank(number).ToString().Length > 3)
                {
                  if(Time_Rank(number)<10)
                {
                    if (int.Parse(Time_Rank(number).ToString().Substring(3, 1)) > 5)//五入
                    {
                        string timess = (double.Parse(Time_Rank(number).ToString()) + (10 - int.Parse(Time_Rank(number).ToString().Substring(3, 1))) * 0.01).ToString();
                        if (timess.Length < 4)
                        {
                            obj.transform.GetChild(5).GetComponent<Text>().text = timess + "小时";
                        }
                        else
                        {
                            if (float.Parse(timess) < 10)
                            {
                                string timesss = timess.Remove(3);
                                obj.transform.GetChild(5).GetComponent<Text>().text = timesss + "小时";
                            }
                            else
                            {
                                string timesss = timess.Remove(2);
                                obj.transform.GetChild(5).GetComponent<Text>().text = timesss + "小时";
                            }

                        }

                    }
                    else//四舍
                    {
                        string times = Time_Rank(number).ToString().Remove(3);
                        obj.transform.GetChild(5).GetComponent<Text>().text = times + "小时";
                    }

                }
                else//大于10
                {
                    if (int.Parse(Time_Rank(number).ToString().Substring(4, 1)) > 5)//五入
                    {
                        string timess = (double.Parse(Time_Rank(number).ToString()) + (10 - int.Parse(Time_Rank(number).ToString().Substring(4, 1))) * 0.01).ToString();
                        if (timess.Length < 5)
                        {
                            obj.transform.GetChild(5).GetComponent<Text>().text = timess + "小时";
                        }
                        else
                        {
                            string timesss = timess.Remove(4);
                            obj.transform.GetChild(5).GetComponent<Text>().text = timesss + "小时";
                        }

                    }
                    else//四舍
                    {
                        string times = Time_Rank(number).ToString().Remove(4);
                        obj.transform.GetChild(5).GetComponent<Text>().text = times + "小时";
                    }

                }

            }
            else
                {
                    obj.transform.GetChild(5).GetComponent<Text>().text = Time_Rank(number).ToString() + "小时";//60的整数倍
                }
         
           // }
           //else//土豪排行榜
           //{
           //    obj.transform.GetChild(5).GetComponent<Text>().text = number;
           //}

                if (a < 3)
            {
                obj.transform.GetChild(0).GetComponent<Image>().sprite = weiXinLoad.instance.rankSprites[a];//Ranks[a];
                obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            }
            else
            {
                obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (a + 1).ToString();
            }
            if (head.Contains("http"))//头像
            {
                StartCoroutine(download_image(head, obj, "Rank_head"));
            }
            else
            {
                obj.transform.GetChild(4).GetComponent<Image>().sprite = weiXinLoad.instance.headSprite;//touXiang;

            }
        }
        else
        {
            return;
        }
        #endregion
    }
    
   

    /// <summary>
    /// 游戏规则
    /// </summary>
    /// <param name="url"></param>
    public  void zhixing_guizeImage(string url)
    {
        StartCoroutine(download_image(url, Guize,"guizeImage"));
    }
    /// <summary>
    /// 邮箱
    /// </summary>
    /// <param name="_page"></param>
    /// <param name="_records"></param>
    /// <param name="_size"></param>
    /// <param name="_total"></param>
    public void email_game(string _title, string _content, string _creatTime)
    {
        GameObject obj = Instantiate(email, GameObject.Find("email_Content").transform) as GameObject;
        obj.transform.GetChild(0).GetComponent<Text>().text = _title;
        obj.transform.GetChild(1).GetComponent<Text>().text = _creatTime;
        obj.transform.GetChild(2).GetComponent<Text>().text = _content;
    }
    /// <summary>
    /// 金额转化
    /// </summary>
    /// <param name="rmb"></param>
    /// <returns></returns>
    int RmbDoubleToInt(double rmb)
    {
        int i = (int)(rmb + (10 - rmb % 10));
        return i;
    }
    /// <summary>
    /// 时间转化
    /// </summary>
    /// <param name="rmb"></param>
    /// <returns></returns>
    float Time_Rank(string rmb)
    {
        float i = float.Parse(rmb)/3600;
        return i;
    }

    void setGoldInfo(Transform trans,int _gold, string _goldId, double _rmb, int _req, string _goldNum, bool isApple)
    {
        if (isApple)
        {
            trans.GetChild(1).GetComponent<Text>().text = "¥" + _rmb.ToString();
        }
        else
        {
            trans.GetChild(1).GetComponent<Text>().text = "¥" + RmbDoubleToInt(_rmb).ToString();
        }
        trans.GetChild(5).GetComponent<Text>().text = _gold.ToString() + "金币";
        trans.GetChild(4).GetComponent<Text>().text = _goldId;
        trans.GetChild(3).GetComponent<Image>().sprite =weiXinLoad.instance.jinbiSprites[_req];//Pay[_req];
   
    }
}
