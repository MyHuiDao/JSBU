﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Text;
using CClient;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using System.Threading;
using WebSocketSharp.Net;

public class destroyBullet
{

    public int _gold;//捕获鱼得到的金币数

    public string _fireID;

    public object _fishList;

    public string sumgold;

}
/// <summary>
/// 此类专门用来处理回调函数线程问题
/// </summary>
public class meiRenYuThreadDeal : MonoBehaviour
{



    public static meiRenYuThreadDeal instant = null;
    bool changeFishMakeTime = false;
    float changeFishMakeTimeNum = 20;//10s后鱼产生开始变多
    float changeFishMakeTimeNumCount = 0;
    public bool is200009 = false;
    public bool is20012 = false;

    public bool is20005 = false;
    public bool is200051 = false;
    public bool is20017 = false;
    public bool is20021 = false;
    public bool is20023 = false;
    public bool is20022 = false;
    public bool is20010 = false;
    public bool is20024 = false;
    public bool is20026 = false;
    public bool is20007 = false;

    public bool bulletShowNotEnough = false;
    float bulletTime = 0;
    bool yuZhenValue = false;
    int target;//设置谁的金币

    //以下参数都是20001方法里面的参数,都必须为静态
    public static int gosWeiZhi;//自己所在的位置
    public static bool is20001 = false;

    //以下参数都是20017方法里面的参数
    public int goldTarget;
    public object oGold;
    public int bingDongTarget;
    public object o1;
    public static List<destroyBullet> destroyBulletList = new List<destroyBullet>();
    public string yuZhenTime = "";
    bool yuZhenJiShi = false;
    float yuzhenjishi = 0;
    GameObject specialFish1Image;
    GameObject specialFish2Image;
    Vector3 yuzhen1Pos;//鱼阵的起始位置
    Vector3 yuzhen2Pos;
    Vector3 yuzhen3Pos;
    void Start()
    {
        instant = this;
        yuzhen1Pos = new Vector3(-25.5f, 0, 0);
        yuzhen2Pos = new Vector3(20, 0, 0);
        yuzhen3Pos = new Vector3(-0.19f, 0, 0);

    }


    void Update()
    {

        if (bulletShowNotEnough)
        {
            bulletTime += Time.deltaTime;
            if (bulletTime >= 1)
            {
                hideBulletImply();
                bulletTime = 0;
                bulletShowNotEnough = false;
            }

        }
        if (is20001)
        {
            deal20001();
            is20001 = false;
        }
        //控制开始时鱼的产生速度
        if (changeFishMakeTime)
        {
            changeFishMakeTimeNumCount += Time.deltaTime;
            if (changeFishMakeTimeNumCount >= changeFishMakeTimeNum)
            {
                FishMaker.instant.fishGenWaitTime = 0.1f;
                changeFishMakeTimeNumCount = 0;
                changeFishMakeTime = false;
            }
        }


        if (destroyBulletList.Count != 0)
        {


            deal20009(destroyBulletList[0]._fireID, destroyBulletList[0]._fishList, destroyBulletList[0]._gold, destroyBulletList[0].sumgold);
            destroyBulletList.RemoveAt(0);
        }

        if (is20007)
        {
            deal20007();
            is20007 = false;
        }
        if (is20012)
        {
            deal20012();
            is20012 = false;
        }


        if (is20005)
        {
            deal20005();
            is20005 = false;
        }
        if (is200051)
        {
            //StartCoroutine(_deal20005());
            deal200051();
            is200051 = false;
        }
        if (is20017)
        {
            deal20017();
            is20017 = false;
        }
        if (is20021)
        {
            deal20021();
            is20021 = false;
        }
        if (is20023)
        {
            deal20023();
            is20023 = false;
        }
        if (is20022)
        {
            deal20022();
            is20022 = false;
        }
        if (is20010)
        {
            deal20010();
            is20010 = false;
        }
        if (is20024)
        {
            deal20024();
            is20024 = false;
        }
        if (is20026)
        {
            deal20026();
            is20026 = false;
        }

        if (fishArrayContral.instant.yuzhenFinish)
        {
            Debug.Log("发送鱼阵");
            if (contrall.instant().isZhuJi)
            {
                //在此发送销毁鱼阵的方法

                Debug.Log("发送销毁鱼阵20027.。。。。。。。。。。。。。。。。。。。。。。。");
                ClientSocket.instant().send("20027");
                
               
            }
            is20010 = true;
            fishArrayContral.instant.yuzhenFinish = false;

        }


        if (yuZhenJiShi)
        {
            yuzhenjishi += Time.deltaTime;
            if (yuzhenjishi >= 5)
            {
                yuzhenjishi = 0;
                yuZhenJiShi = false;
                GameObject.Find("yuZhenWarn").transform.localScale = Vector3.zero;
            }
        }

        if (yuZhenValue)
        {
            if (GameObject.FindGameObjectWithTag("yuzhen").transform.childCount != 0)
            {
                deal200231();
                yuZhenValue = false;

            }
        }




    }
    /// <summary>
    /// 屏幕中子弹过多时。
    /// </summary>
    void hideBulletImply()
    {
        GameObject.Find("bulletMax").transform.localScale = Vector3.zero;
    }

    public void deal20001()
    {


        GameController.Instance.initialGold();
        if (gosWeiZhi == 0)
        {
            buYuMusicContral.instant.specialFish1Image.transform.rotation = Quaternion.Euler(0, 0, 0);
            buYuMusicContral.instant.specialFish2Image.transform.rotation = Quaternion.Euler(0, 0, 0);
            buYuMusicContral.instant.specialFish3Image.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (gosWeiZhi == 1)
        {
            GameObject.Find("Main Camera").transform.Rotate(new Vector3(0, 0, 180));
            GameObject.Find("UIRotation").transform.Rotate(new Vector3(0, 0, 180));//使界面旋转180                  
            GameObject[] goldtext = GameObject.FindGameObjectsWithTag("goldText");
            for (int i = 0; i < 2; i++)
            {
                goldtext[i].transform.Rotate(new Vector3(0, 0, 180));//让每个金币显示旋转180
            }

            Debug.Log("第二个人进来才能收到。。。。。。。。。。。。。。。。。。");
            //特效旋转180
            buYuMusicContral.instant.specialFish1Image.transform.rotation = Quaternion.Euler(0, 0, 180);
            buYuMusicContral.instant.specialFish2Image.transform.rotation = Quaternion.Euler(0, 0, 180);
            buYuMusicContral.instant.specialFish3Image.transform.rotation = Quaternion.Euler(0, 0, 180);

        }
        if (contrall.instant().userDict.Count == 1)
        {
            FishMaker.instant.fishGenWaitTime = 0.7f;
            changeFishMakeTime = true;
        }
    }
    /// <summary>
    /// 发送20012信息
    /// </summary>
    public void deal20012()
    {

        List<Transform> list = new List<Transform>();//把需要转发所有鱼的信息保存起来
       
        Transform[] fishGroup = GameObject.Find("fishGroup").GetComponentsInChildren<Transform>();
        Debug.Log("zhuanfa" + fishGroup.Length);
        for (int i = 0; i < fishGroup.Length; i++)
        {
            if (fishGroup[i].tag == "allFish")
            {
                list.Add(fishGroup[i]);
            }
        }
        StringBuilder messege = new StringBuilder();
        messege.Append("[");
        //if (list.Count == 0)
        //{
        //    messege.Append("]");
        //    ClientSocket.instant().send("20012", (object)messege);
        //    return;
        //}
        for (int j = 0; j < list.Count; j++)
        {
            messege.Append("{\"id\":\"" + list[j].GetComponent<FishAttr>().id + "\",\"x\":" + (int)list[j].transform.position.x + ",\"y\":" + (int)list[j].transform.position.y + ",\"n\":" + list[j].GetComponent<FishAttr>().nextDian + "},");
        }
        messege.Remove(messege.Length - 1, 1);//删除最后的逗号
        messege.Append("]");
        ClientSocket.instant().send("20012", (object)messege);
    }


    public void deal20009(string fireID, object fishList, int gold, string sumgold)
    {

        int target = -1;//代表是哪一个对象加金币
        for (int i = 0; i < 4; i++)
        {

            if (GameController.Instance.bulletDict[i].ContainsKey(fireID))
            {

                GameController.Instance.bulletDict[i][fireID].GetComponent<BulletAttr>().destroyBullet();
                GameController.Instance.bulletDict[i].Remove(fireID);
                target = i;
                break;
            }
        }


        if (target != -1)
        {

            //开始销毁死去的鱼

            for (int i = 0; i < ((JsonData)fishList)["fishList"].Count; i++)
            {

                string str = ((JsonData)fishList)["fishList"][i].ToString();

                if (contrall.instant().isCanClearFish)//鱼阵
                {
                    Debug.Log("销毁鱼阵里的鱼");
                    if (fishArrayContral.instant.yuZhenTarget.ContainsKey(str))
                    {
                        if (fishArrayContral.instant.yuZhenTarget[str] != null)
                        {
                            fishArrayContral.instant.yuZhenTarget[str].GetComponent<FishAttr>().TakeDamage(target, int.Parse(((JsonData)fishList)["fishGold"][str].ToString()));
                        }
                    }

                    if (fishArrayContral.listGroupYuzhen.Count != 0)
                    {
                        if (fishArrayContral.listGroupYuzhen[0].Contains(((JsonData)fishList)["fishList"][i].ToString()))
                        {
                            fishArrayContral.listGroupYuzhen[0].Remove(((JsonData)fishList)["fishList"][i].ToString());
                        }
                    }

                }
                else
                {

                    if (FishMaker.fishTarget.ContainsKey(str))
                    {
                        if (FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType == "20" || FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType == "19")
                        {

                            screenDouDong.instance.Shake();
                            buYuMusicContral.instant.allYinXiao[5].Play();
                            specialFish1Image = Instantiate(buYuMusicContral.instant.specialFish1Image, GameObject.Find("fishGroup").transform);
                            specialFish1Image.transform.position = FishMaker.fishTarget[str].transform.position;

                        }
                        else if (FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType == "18")
                        {
                            buYuMusicContral.instant.allYinXiao[6].Play();
                            specialFish2Image = Instantiate(buYuMusicContral.instant.specialFish2Image, GameObject.Find("fishGroup").transform);
                            specialFish2Image.transform.position = FishMaker.fishTarget[str].transform.position;

                        }
                        else if (FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType == "17")
                        {
                            buYuMusicContral.instant.allYinXiao[7].Play();
                            specialFish2Image = Instantiate(buYuMusicContral.instant.specialFish3Image, GameObject.Find("fishGroup").transform);
                            specialFish2Image.transform.position = FishMaker.fishTarget[str].transform.position;
                        }
                        else if (getMeiRenYuArea.buyuGame == 0 && int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType) > 5)
                        {

                            buYuMusicContral.instant.allYinXiao[24 - int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType)].Play();
                        }
                        else if (getMeiRenYuArea.buyuGame == 1 && int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType) > 5)
                        {

                            buYuMusicContral.instant.allYinXiao[26 - int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType)].Play();
                        }
                        else if (getMeiRenYuArea.buyuGame == 2 && int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType) > 5)
                        {

                            buYuMusicContral.instant.allYinXiao[28 - int.Parse(FishMaker.fishTarget[str].GetComponent<FishAttr>().fishType)].Play();
                        }



                        FishMaker.fishTarget[str].GetComponent<FishAttr>().TakeDamage(target, int.Parse(((JsonData)fishList)["fishGold"][str].ToString()));
                    }
                }

            }

            if (FishMaker.SaveNet.ContainsKey(fireID))
            {

                FishMaker.SaveNet.Remove(fireID);//删除字典中的网
            }


            // Debug.Log("-><color=#FF4040>" + "打死鱼得到的金币" + "</color>"+ target+"对象：打死鱼所加金币："+gold);
            //Debug.Log("总金币数"+sumgold);

            GameController.Instance.setGoldText(target, sumgold);
            //GameController.Instance.goldText[target].text = (int.Parse(GameController.Instance.goldText[target].text) + gold).ToString();//此处显示抓到鱼后的金币
            //GameController.Instance.setGoldText(target,gold);


        }




    }



    /// <summary>
    /// 离开房间
    /// </summary>
    public void exitRoom()
    {


        is20005 = true;

    }
    IEnumerator _deal20005()
    {
        GameObject _obj = Instantiate(ResouseManager.Instance.LOADP/*load*/, GameObject.Find("Order90Canvas").transform) as GameObject;
        yield return _obj;
        otherContral.instant.returnGameScene = true;

        contrall.instant().clearAll();
        if (GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)") != null)
        {
            Destroy(GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)").gameObject);

        }
        yield return new WaitForEndOfFrame();
        Destroy(_obj);
        yield return new WaitForEndOfFrame();
        GameObject _obj1 = Instantiate(ResouseManager.Instance.SELECTAREA/*loadSelectArea.areaPrefabs*/, GameObject.Find("main").transform);
        yield return _obj1;
        ClientSocket.instant().ws.Close();//断开网络连接
        Debug.Log("断开");
    }
    public void deal20005()
    {
        Debug.Log("退出");
        otherContral.instant.returnGameScene = true;
        Debug.Log("断开");
        //ClientSocket.instant().ws.Close();//断开网络连接
        //Thread th = new Thread(new ParameterizedThreadStart(disConetc));
        //th.Start(ClientSocket.instant().ws);
        contrall.instant().clearAll();

        if (GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)") != null)
        {
            Destroy(GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)").gameObject);

        }
        Instantiate(ResouseManager.Instance.SELECTAREA/*loadSelectArea.areaPrefabs*/, GameObject.Find("main").transform);
    }

    void disConetc(System.Object _ws)
    {
        Debug.LogError("cccccccccccccc");
        WebSocketSharp.WebSocket _socket = (WebSocketSharp.WebSocket)_ws;
        Debug.Log("断开");
        _socket.Close();
    }
    /// <summary>
    /// 设置金币显示
    /// </summary>
    /// <param name="target"></param>
    public void setGoldText(int _target)
    {
        target = _target;
        is200051 = true;
    }
    public void deal200051()
    {
        GameController.Instance.goldText[target].text = "等待玩家加入";
        Transform g = GameObject.Find("GunPanel" + target).transform.Find("GunPosGroup");
        if (target == 1)
        {
            g.transform.Find("1Gun").rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            g.transform.Find("2Gun").rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            g.transform.Find("3Gun").rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else
        {
            g.transform.Find("1Gun").rotation = Quaternion.Euler(Vector3.zero);
            g.transform.Find("2Gun").rotation = Quaternion.Euler(Vector3.zero);
            g.transform.Find("3Gun").rotation = Quaternion.Euler(Vector3.zero);
        }


    }
    public void deal20017()
    {
        GameObject.Find("GunPanel" + goldTarget).transform.Find("GosClassButton").Find("ButtonP").GetComponent<Image>().sprite = GameController.Instance.ButtonP;
        GameObject.Find("GunPanel" + goldTarget).transform.Find("GosClassButton").Find("ButtonM").GetComponent<Image>().sprite = GameController.Instance.ButtonM;
        GameController.Instance.goldText[goldTarget].text = ((JsonData)oGold)["gold"].ToString();
    }

    public GameObject bgImage;
    public Transform bgImageParent;
    public Transform bgBeforeParent;
    public int currentBg = 0;
    public Sprite[] bgSprites;
    public GameObject yuzhenWave;
    public bool yuzhenOpenFire = true;
    GameObject bg = null;
    /// <summary>
    /// 鱼阵开始
    /// </summary>
    public void deal20021()
    {
        yuzhenOpenFire = false;
        bg = Instantiate(bgImage, bgBeforeParent);
        bg.transform.SetSiblingIndex(1);
        if (currentBg >= 3) { currentBg = -1; }
        bg.GetComponent<Image>().sprite = bgSprites[currentBg + 1];
        currentBg++;
        bg.transform.localPosition = new Vector3(-3117.88f, 0, 0);
        yuzhenWave.GetComponent<Animator>().enabled = true;
        yuzhenWave.transform.DOLocalMoveX(1504f, 8);
        bg.transform.DOLocalMoveX(0, 8);
        bg.transform.SetAsFirstSibling();

        Invoke("deleBg", 8.2f);
    }
    void deleBg()
    {


        yuzhenOpenFire = true;
        bg.transform.parent = bgImageParent;
        yuzhenWave.transform.localPosition = new Vector3(-1613f, 0, 0);
        yuzhenWave.GetComponent<Animator>().enabled = false;
        Destroy(bgImageParent.transform.GetChild(0).gameObject);
        bgImageParent.transform.Find("BgReplace(Clone)").name = "Bg";
        //在此处把所有鱼删除掉
        meiRenYuThreadDeal.destroyBulletList.Clear();
        FishMaker.do20004Order.Clear();
        FishMaker.deadFish.Clear();
        FishMaker.moveFish.Clear();
        FishMaker.instant.listGroupOrder = 0;
        FishMaker.fish.Clear();
        FishMaker.fishTarget.Clear();
        FishMaker.SaveNet.Clear();
        FishMaker.listGroup.Clear();
        FishMaker.instant.fishGroupNum = 1;
        FishMaker.instant.fishGroupIsOver = true;

        for (int i = 0; i < GameObject.Find("fishGroup").transform.childCount; i++)
        {
            Destroy(GameObject.Find("fishGroup").transform.GetChild(i).gameObject);
        }

        fishArrayContral.instant.yuzhenStart = true;

    }





    /// <summary>
    /// 鱼阵生成
    /// </summary>
    public void deal20023()
    {
        Debug.Log("生成鱼阵");
        fishArrayContral.instant.yuzhenCard = contrall.instant().yuZhenGroupCard;//代表鱼阵几
        //生成鱼的种类
        fishArrayContral.instant.yuzhenPrefab = Instantiate(fishArrayContral.instant.yuzhen[fishArrayContral.instant.yuzhenCard - 1], GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)").transform);
        if (fishArrayContral.instant.yuzhenCard == 1)
        {
            fishArrayContral.instant.yuzhenPrefab.transform.localPosition = yuzhen1Pos;
        }
        else if (fishArrayContral.instant.yuzhenCard == 2)
        {
            fishArrayContral.instant.yuzhenPrefab.transform.localPosition = yuzhen2Pos;
        }
        else if (fishArrayContral.instant.yuzhenCard == 4)
        {
            fishArrayContral.instant.yuzhenPrefab.transform.localPosition = Vector3.zero;
        }

        yuZhenValue = true;

    }
    public void deal200231()
    {
        for (int i = 0; i < fishArrayContral.listGroupYuzhen[0].Count; i++)
        {
            fishArrayContral.instant.MakeFishes(i, FishMaker.fish[fishArrayContral.listGroupYuzhen[0][i]].id);
        }

    }
    /// <summary>
    /// 鱼阵开始游动
    /// </summary>
    public void deal20022()
    {
        if (fishArrayContral.instant.yuzhenCard == 1)//超右边方向游动
        {
            fishArrayContral.instant.yuzhenPrefab.transform.DOLocalMoveX(30, 150);//鱼阵开始游动 
        }
        else if (fishArrayContral.instant.yuzhenCard == 2)//超左边方向游动
        {
            fishArrayContral.instant.yuzhenPrefab.transform.DOLocalMoveX(-30, 150);//鱼阵开始游动 
        }
        else if (fishArrayContral.instant.yuzhenCard == 3)
        {
            yuzhenThreeMove.isStartMove = true;
        }
        else if (fishArrayContral.instant.yuzhenCard == 4)
        {
            fishArrayFor1.isStartMove = false;
            //if (getMeiRenYuArea.buyuGame == 0)
            //{
            //    GameObject.Find("yuzhenNew4(Clone)").transform.Find("fish18_1").gameObject.SetActive(true);
            //}
            //else if (getMeiRenYuArea.buyuGame == 1)
            //{
            //    GameObject.Find("yuzhenNew4(Clone)").transform.Find("fish21_1").gameObject.SetActive(true);
            //}
            //else if (getMeiRenYuArea.buyuGame == 2)
            //{
            //    GameObject.Find("yuzhenNew4(Clone)").transform.Find("龙").gameObject.SetActive(true);
            //}
            //if (fishArrayFor1.instance == null)
            //{
            //    while (true) { if (fishArrayFor1.instance != null) { break; } }
            //}
            GameObject.Find("yuzhenNew4(Clone)").transform.Find("龙").gameObject.SetActive(true);
            fishArrayFor1.isStartMove = true;

        }


    }
    public void deal20010()
    {

        //Invoke("destroyYuZhen", 5);
        FishMaker.instant.fishGenWaitTime = 0.7f;
        changeFishMakeTime = true;
        fishArrayContral.listGroupYuzhen.Clear();
        fishArrayContral.instant.yuZhenTarget.Clear();
       
        Debug.Log("销毁鱼阵");
        Destroy(GameObject.FindGameObjectWithTag("yuzhen").gameObject);
    }
    //void destroyYuZhen()
    //{
    //    fishArrayContral.listGroupYuzhen.Clear();
    //    fishArrayContral.instant.yuZhenTarget.Clear();
    //    fishArrayContral.instant.yuzhenFinish = false;
    //    Debug.Log("销毁鱼阵");
    //    Destroy(GameObject.FindGameObjectWithTag("yuzhen").gameObject);
    //}


    public void deal20024()
    {
        GameController.Instance.goldText[bingDongTarget].text = (int.Parse(GameController.Instance.goldText[bingDongTarget].text) - int.Parse((((JsonData)o1)["gold"].ToString()))).ToString();
    }

    public void deal20026()
    {
        //GameObject.Find("yuZhenTimeTxt").GetComponent<Text>().text = "1";// (float.Parse(yuZhenTime)/60).ToString();
        GameObject.Find("yuZhenWarn").transform.localScale = Vector3.one;

        yuZhenJiShi = true;
    }

    /// <summary>
    /// 金币不足
    /// </summary>
    public void deal20007()
    {
        GameObject.Find("moneyNotEnough").transform.localScale = Vector3.one;
        //Invoke("show20007",2);

    }
    void show20007()
    {
        GameObject.Find("moneyNotEnough").transform.localScale = Vector3.zero;
    }

}

