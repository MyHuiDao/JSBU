using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class initialPrepare : MonoBehaviour
{


    public static initialPrepare instance = null;
    public GameObject[] fish;
    public Dictionary<int, saveFishData> fishDataDict = new Dictionary<int, saveFishData>();//名次跟算法
    public Dictionary<int, Vector3> trackDict = new Dictionary<int, Vector3>();//轨道跟位置
    public Dictionary<int, Vector3> trackEndDict = new Dictionary<int, Vector3>();//轨道跟位置
    public static Dictionary<string, int> nameAndNumdict = new Dictionary<string, int>();//从服务器接收到的消息,鱼的排名
    public Dictionary<string, int> trackNum = new Dictionary<string, int>();//取自最近一次鱼的排名
   
    public GameObject[] fArray;
    public GameObject zhongjiangPrefab;
    public GameObject weizhongjiangPrefab;





    //public float a;
    //public float b;

    public static object do10003Msg = null;


    void Start()
    {
        instance = this;
        initialStart.instance.allYinXiao[6].Play();
        //a = 5.6f;
        //b = 0.06f;

        initialUI();

       
        initialTrack();
        //initialData();
        inistateFish();
        initialMingCi();
        initialJieSuan();

        GameObject.Find("returnToStart").GetComponent<Button>().onClick.AddListener(returnToStart);


    }

    // Update is called once per frame
    void Update()
    {


    }



    void initialUI()
    {
        Debug.Log(saveDate.roomID);
   
        GameObject.Find("moneyText1").GetComponent<Text>().text = saveDate.money;
        GameObject.Find("roomText1").GetComponent<Text>().text = saveDate.roomID;
        if (saveDate.roomPeopleNum <= 10)
        {
            GameObject.Find("personNum1").GetComponent<Text>().text = "10";
        }
        else
        {
            GameObject.Find("personNum1").GetComponent<Text>().text = saveDate.roomPeopleNum.ToString();
        }
       
        Debug.Log(GameObject.Find("moneyText1").GetComponent<Text>().text);


       
        GameObject.Find("Camera").transform.position = saveDate.cameraPos;
        GameObject.Find("cameraFllowMe").transform.position=saveDate.cameraFllowMePos;
        Debug.Log("cameraFllowMe" + GameObject.Find("cameraFllowMe").transform.position);

    }


    void inistateFish()//初始化鱼
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject g = Instantiate(fish[i], GameObject.Find("fish").transform);
            g.name = "fish" + (i + 1);
            g.transform.localPosition = trackDict[trackNum[g.name]];
        }
        fishCrown.fishAlreadyHave = true;
    }
    /*
    void initialData()//注意名次跟算法对应.1对应最快的算法
    {

        saveFishData s0 = new saveFishData();
        //s0.A = a - b * 9;
        //s0.B = b * 10;
        //s0.mingciRank = 4;
        fishDataDict.Add(4, s0);



        saveFishData s1 = new saveFishData();
        //s1.A = a - b * 6;
        //s1.B = b * 10;
        //s1.mingciRank = 3;
        fishDataDict.Add(3, s1);


        saveFishData s2 = new saveFishData();
        //s2.A = a - b * 3;
        //s2.B = b * 10;
        //s2.mingciRank = 2;
        fishDataDict.Add(2, s2);


        saveFishData s3 = new saveFishData();
        //s3.A = a - b * 0;
        //s3.B = b * 10;
        //s3.mingciRank = 1;
        fishDataDict.Add(1, s3);
    }
    */

    void initialTrack()
    {
        for (int i = 0; i < 4; i++)
        {
            trackNum.Add("fish" + (i + 1), int.Parse(startTimeJiShi.fishRank[i]));//int.Parse(startTimeJiShi.fishRank[i]));//控制赛道
        }
       
     



        trackDict.Add(1, new Vector3(-97f, 4.54f, 0));//后面可以放个空物体，得到此物体的位置,鱼初始位置
        trackDict.Add(2, new Vector3(-97f, 1.62f, 0));
        trackDict.Add(3, new Vector3(-97f, -1.36f, 0));
        trackDict.Add(4, new Vector3(-97f, -4.28f, 0));


        trackEndDict.Add(1, new Vector3(-23.25f, 5f, 0));//名次初始位置，不用变，调父类即可
        trackEndDict.Add(2, new Vector3(-23.25f, 1.7f, 0));
        trackEndDict.Add(3, new Vector3(-23.25f, -1.42f, 0));
        trackEndDict.Add(4, new Vector3(-23.25f, -4.45f, 0));
    }


    void initialJieSuan()
    {
        GameObject contentParent = GameObject.Find("Content");
        if (!((IDictionary)((JsonData)do10003Msg)).Contains("bets"))
        {
            GameObject g = Instantiate(weizhongjiangPrefab, contentParent.transform);
            g.transform.Find("touzhuKind").transform.localScale = Vector3.zero;       
            g.transform.Find("suanshi").transform.localScale = Vector3.zero;
            return;
        }
       
   
        JsonData j = ((JsonData)do10003Msg)["bets"];
        for (int i = 0; i < j.Count; i++)
        {
            GameObject g;

         
            if (j[i]["getGold"].ToString() == "0")
            {
                
                g = Instantiate(weizhongjiangPrefab, contentParent.transform);
                g.name = "weiZhongJiang" + i;
                Instantiate(fArray[initialStart.touzhuEquit[j[i]["betsNumber"].ToString()] - 1], GameObject.Find("weiZhongJiang" + i).transform.Find("touzhuKind"));
                Transform t = GameObject.Find("weiZhongJiang" + i).transform.Find("suanshi");
                t.Find("touzhuNum").GetComponent<Text>().text = j[i]["betsGold"].ToString();
                t.Find("peilv").GetComponent<Text>().text = j[i]["odds"].ToString();
                t.Find("jieguo").GetComponent<Text>().text = j[i]["getGold"].ToString();

            }
            else
            {
                g = Instantiate(zhongjiangPrefab, contentParent.transform);
                g.name = "zhongJiang" + i;
                Instantiate(fArray[initialStart.touzhuEquit[j[i]["betsNumber"].ToString()] - 1], GameObject.Find("zhongJiang" + i).transform.Find("touzhuKind"));
                Transform t = GameObject.Find("zhongJiang" + i).transform.Find("suanshi");
                t.Find("touzhuNum").GetComponent<Text>().text = j[i]["betsGold"].ToString();
                t.Find("peilv").GetComponent<Text>().text = j[i]["odds"].ToString();
                t.Find("jieguo").GetComponent<Text>().text = j[i]["getGold"].ToString();
                g.transform.SetAsFirstSibling();

            }       
        }
     
        for (int k = 0; k < contentParent.transform.childCount; k++)
        {
            if (contentParent.transform.GetChild(k).name.Contains("weiZhongJiang"))
            {
                
                contentParent.transform.GetChild(k).SetSiblingIndex(20+ k);
            }
        }






    }
    void initialMingCi()
    {
        foreach (string k in nameAndNumdict.Keys)
        {


            GameObject.Find("endnum" + nameAndNumdict[k]).transform.localPosition = trackEndDict[trackNum[k]];



        }

    }


   


    void returnToStart()
    {
        initialStart.instance.allYinXiao[6].Pause();
        Destroy(GameObject.Find("prepareAndjieSuna(Clone)").gameObject);
        Destroy(GameObject.Find("background(Clone)").gameObject);
        Instantiate(m_slider.yuerStartPrefab);
        yuerSendMSg.instant().getCountDown();
       
        //在此处请求倒计时信息
    }
}
