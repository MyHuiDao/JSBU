using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using Mosframe;
using UnityEngine.UI;
using CClient;
using UnityEngine.SceneManagement;

public class getMeiRenYuArea : MonoBehaviour {


  
    public int fenQuNum=0;//代表几个分区
    private AsyncOperation jiazai2;
    public List<string> listID = new List<string>();//可以代表传下来的顺序，里面包含的是ID
    public List<int> listImg = new List<int>();
    public List<string> listText = new List<string>();

    public  static   getMeiRenYuArea intstant;


    public static int buyuGame;//判断进入的是哪一款游戏
    public bool moneyNoEnough = false;
    float moneyNoEnoughTime = 0;
    private void Awake()
    {
        intstant = this;
    }
    // Use this for initialization
    void Start () {
        btnGetGameArea(buyuGame);
	}
	
	// Update is called once per frame
	void Update () {
        if (moneyNoEnough)
        {
            moneyNoEnoughTime += Time.deltaTime;
            if (moneyNoEnoughTime >= 2)
            {
                GameObject.Find("moneyNotEnough").transform.localScale = Vector3.zero;
                moneyNoEnough = false;
                moneyNoEnoughTime = 0;
            }
          
        }

	}
    /// <summary>
    /// 获得游戏分区
    /// </summary>
    void btnGetGameArea(int _buyuGame)
    {
        //GameObject.Find("loadling").transform.localScale = Vector3.one;
        if (_buyuGame == 0)
        {
            httpConnect.GET(this, httpConnect.URL + "/game/area/fishing", null, getGameArea, httpError);
        }else if (_buyuGame == 1)
        {
            httpConnect.GET(this, httpConnect.URL + "/game/area/likui_fishing", null, getGameArea, httpError);
        }
        else if(_buyuGame==2)
        {
            httpConnect.GET(this, httpConnect.URL + "/game/area/danaotiangong", null, getGameArea, httpError);
        }
      

    }
    void getGameArea(string str)
    {
        //Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            JsonData js = jso["data"];
            for (int i = 0; i < js.Count; i++)
            {              
                getGameAreaView(int.Parse(js[i]["img"].ToString()),js[i]["name"].ToString(), js[i]["gameType"].ToString(), js[i]["id"].ToString(), js[i]["type"].ToString(), (int)js[i]["minNum"], (int)js[i]["seq"], js[i]["status"].ToString());
            }
         
           

            DynamicScrollView.instant.startInstant();//开始产生游戏分区
         
        }

    }
    /// <summary>
    /// 获取游戏分区
    /// </summary>
    /// <param name="_name">场名称</param>
    /// <param name="_gameType">游戏类型</param>
    /// <param name="_id">主键</param>
    /// <param name="_type">体验币或金币</param>
    /// <param name="_minNum">进入游戏区的最小金币</param>
    /// <param name="_seq">排序</param>
    /// <param name="_status">状态（0：正常，1：未开放，2：不显示）</param>
     void getGameAreaView(int image,string _name, string _gameType, string _id, string _type, int _minNum, int _seq, string _status)
    {
        
        fenQuNum++;
        listImg.Add(image);
        listID.Add(_id);
        listText.Add(_minNum.ToString());

    }


  
  


    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        //GameObject.Find("loadFail").transform.localScale = Vector3.one;
        Debug.Log(str);


    }
}
