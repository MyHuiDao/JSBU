using System;
using System.Collections.Generic;
using UnityEngine;
using Tween;
using UnityEngine.UI;
using CClient;

/**
 * 鱼的类
 * 挂在鱼的预制体上
 * 
 * 做的事情：撞到border消失； takeDamage(调用+gold,+exp; 播放鱼死亡特效；产生金币飞向左下角特效； 鱼对象销毁)
 * 鱼移动的事情交给了Ef_AutoMove脚本
 * */



public class FishAttr : MonoBehaviour
{

    public float Speed;    //生成鱼的移动速度最大值
    public int nextDian = 0;//下一个点
    public string fishType;//鱼类型
    public string id;//鱼ID
    public string runPoint;//鱼轨迹
    public bool isBelongYuZhen = false;
    public GameObject diePrefab;
    public GameObject goldPrefab;
    public GameObject goldPrefab1;
    public GameObject goldPrefab2;
    public GameObject goldPrefab3;//大金币
    public GameObject goldPrefab4;//元宝
    public GameObject goldPrefab5;//大转盘
    public GameObject goldArt;//金币显示
    GameObject goldArtNum;
    public bool Send20010One = true;
    GameObject fishDeadGroup;
    GameObject goldGroup;
    public int judgeSpot = 0;
    public float initialX = 0;
    public float initialY = 0;
    bool yuzhenOne = true;


    public bool isSend = false;
    private void Start()
    {
         goldArtNum= GameObject.Find("goldArtNum");
        //InvokeRepeating("timeLongTakeDamage", 60,1);
        Invoke("timeLongTakeDamage", 200);//如果30内鱼没死亡，则自动让其消失
        fishDeadGroup = GameObject.Find("fishDead");
        goldGroup = GameObject.Find("Gold");

    }
    private void Update()
    {
        if (isBelongYuZhen)//属于鱼阵
        {
            if (fishArrayContral.instant.yuzhenFinish && yuzhenOne)
            {
               

                if (contrall.instance.isZhuJi)
                {
                    Debug.Log("鱼阵里的鱼发送死亡" + this.id);
                    ClientSocket.instant().send("20010", id);
                }
              

                //WebButtonSendMessege.instant().fishDead(this.id);//鱼自然死亡发送消息给服务器
                yuzhenOne = false;

            }

        }
        else
        {


            if (Send20010One)
            {
                nextDian = nextItem(guiJi.instant().guiJiDict[runPoint], this.transform.localPosition.x, this.transform.localPosition.y);

            }

            if (nextDian == guiJi.instant().guiJiDict[runPoint].Count - 1)//9是终点，且8也在屏幕外
            {


                if (!contrall.instant().isCanClearFish)
                {
                    Send20010One = false;
                    nextDian = 100;//只发一次
                    if (contrall.instant().isZhuJi)//如果是主机则请求
                    {
                        Debug.Log(3);
                        WebButtonSendMessege.instant().fishDead(this.id);//鱼自然死亡发送消息给服务器
                    }

                }
            }

        }
    }

    /// <summary>
    /// 返回当前点的下一个点
    /// </summary>
    /// <param name="items">轨迹中的所有点</param>
    /// <param name="abcd">当前点</param>
    /// <returns></returns>
    public int nextItem(List<Item> items, float x, float y)
    {
        for (int i = judgeSpot; i < items.Count - 1; i++)
        {

            Item item1 = items[i];
            Item item2 = items[i + 1];
            if (Mathf.Abs(item1.y - item2.y) < 5 && item1.x < item2.x)
            {
                if (x >= item1.x && x <= item2.x)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (Mathf.Abs(item1.y - item2.y) < 5 && item1.x > item2.x)
            {
                if (x <= item1.x && x >= item2.x)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (Mathf.Abs(item1.x - item2.x) < 5 && item1.y < item2.y)
            {
                if (y >= item1.y && y <= item2.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (Mathf.Abs(item1.x - item2.x) < 5 && item1.y > item2.y)
            {
                if (y <= item1.y && y >= item2.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (item1.x > item2.x && item1.y < item2.y)
            {

                if (x >= item2.x && x <= item1.x && y <= item2.y && y >= item1.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (item1.x < item2.x && item1.y > item2.y)
            {
                if (x >= item1.x && x <= item2.x && y <= item1.y && y >= item2.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (item1.x < item2.x && item1.y < item2.y)
            {
                if (x <= item2.x && x >= item1.x && y >= item1.y && y <= item2.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }
            else if (item1.x > item2.x && item1.y > item2.y)
            {
                if (x >= item2.x && x <= item1.x && y >= item2.y && y <= item1.y)
                {
                    judgeSpot = i;
                    return i + 1;
                }

            }



        }

        return 0;
    }

    /// <summary>
    /// 返回可以死亡的鱼消息后才可以死亡
    /// </summary>
    public void TakeDamage(int target, int _gold)
    {
        GameObject goldArtColne= Instantiate(goldArt,goldArtNum.transform);
        goldArtColne.transform.position = this.transform.position;
        goldArtColne.transform.Find("goldShow").GetComponent<Text>().text = _gold.ToString();
        //2.播放鱼死亡效果,有死亡效果则播放，没有则不播放
        if (diePrefab != null)
        {
            GameObject die = Instantiate(diePrefab);    //鱼死亡的预制体身上有Ef_DestroySelf脚本，1s后会自动销毁
            die.transform.SetParent(fishDeadGroup.transform, false);
            die.transform.position = transform.position;
        }
        //3.鱼死亡后会有获得金币的效果

        //int goldCount = _gold / ((int.Parse(GameController.Instance.oneShootCostText[target].text.ToString())/ int.Parse(contrall.instance.addFileGoldNum) * ((int.Parse(contrall.instance.addFileGoldNum)/10)) * 10));
        int goldCount = _gold / (int.Parse(GameController.Instance.oneShootCostText[target].text.ToString()));
        GameObject goldGo = null;
        if (goldCount <= 0)
        {


            goldGo = Instantiate(goldPrefab);
            goldGo.transform.Find("gold0").gameObject.SetActive(true);
            goldGo.transform.SetParent(goldGroup.transform, false);
            goldGo.transform.position = this.transform.position;

        }
        else if (goldCount < 10)//小金币
        {
            goldGo = Instantiate(goldPrefab);
            for (int i = 0; i < goldCount; i++)
            {
                GameObject g = goldGo.transform.Find("gold" + i).gameObject;
                g.SetActive(true);
                g.GetComponent<Ef_MoveTo>().target = target;


            }
            goldGo.transform.SetParent(goldGroup.transform, false);
            goldGo.transform.position = this.transform.position;
            if (goldGo.transform.position.y < -1)
            {
                Vector3 v1 = goldGo.transform.position;
                v1.y = 1;
                goldGo.transform.position = v1;
            }
        }
        else if (goldCount <= 20)//大金币
        {
            for (int i = 0; i < goldCount / 2; i++)
            {
                GameObject g;
                g = Instantiate(goldPrefab3);
                g.transform.SetParent(goldGroup.transform, false);
                g.transform.position = this.transform.position;
                if (g.transform.position.y < -1)
                {
                    Vector3 v1 = g.transform.position;
                    v1.y = 1;
                    g.transform.position = v1;
                }
                 Vector3 v = g.transform.position;
                v.x -= 1.1f * i;
                v.y -= 1.1f * i;
                g.transform.position = v;
                if (g.GetComponent<Ef_MoveTo>() != null)
                {
                    g.GetComponent<Ef_MoveTo>().target = target;//确定金币飞到哪个对象
                }
            }


        }
        else if (goldCount < 25)
        {
            GameObject g;
            g = Instantiate(goldPrefab4);       
            g.transform.SetParent(GameObject.Find("specialEffect").transform, false);
            g.transform.position = new Vector3(0, 0, 90);
            //if (g.transform.position.y < -1)
            //{

            //    g.transform.position = GameObject.Find("GunPanel" + target).transform.Find("goldPosition").transform.position;
            //}

        }
        else if (goldCount < 30)
        {
            GameObject g;
            g = Instantiate(goldPrefab5);
            g.transform.SetParent(GameObject.Find("specialEffect").transform, false);
            g.transform.position = new Vector3(0, 0, 90);
            //if (g.transform.position.y < -1)
            //{

            //    g.transform.position = GameObject.Find("GunPanel" + target).transform.Find("goldPosition").transform.position;
            //}
        }
        else if (goldCount < 36)//转盘
        {
            GameObject g;
            g = Instantiate(goldPrefab1);
            g.transform.Find("goldFly2Text").GetComponent<Text>().text = _gold.ToString();
            g.transform.SetParent(GameObject.Find("specialEffect").transform, false);
            g.transform.position = this.transform.position;
            if (g.transform.position.y < -1)
            {
               
                g.transform.position = GameObject.Find("GunPanel" + target).transform.Find("goldPosition").transform.position;
            }         
           
        }
        else//最大特效
        {
            goldGo = Instantiate(goldPrefab2);
            goldGo.transform.Find("goldFly3Text").GetComponent<Text>().text = _gold.ToString();
            goldGo.transform.SetParent(GameObject.Find("specialEffect").transform, false);
            goldGo.transform.position = new Vector3(0, 0, 90);
        }

        if (goldGo != null && goldGo.GetComponent<Ef_MoveTo>() != null)
        {

            goldGo.GetComponent<Ef_MoveTo>().target = target;//确定金币飞到哪个对象
        }
        for (int i = 0; i < FishMaker.listGroup.Count; i++)
        {
            if (FishMaker.listGroup[i].Contains(this.id))
            {
                FishMaker.listGroup[i].Remove(id);
                break;
            }
        }
        FishMaker.fish.Remove(id);//删除fish类
        FishMaker.fishTarget.Remove(id);
        destroy();


    }
    /// <summary>
    /// 如果鱼存在时间过长，就是有问题了，直接删掉
    /// </summary>
    void timeLongTakeDamage()
    {
        if (!isBelongYuZhen)
        {

            Debug.Log("鱼一直没死");
            if (contrall.instant().isZhuJi)
            {
                if (isSend)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log(1);
                    FishMaker.fishTarget[id].GetComponent<FishAttr>().isSend = true;
                    ClientSocket.instant().send("20010",id);
                }



            }
        }


    }
    public void destroy()
    {

        Destroy(gameObject);


    }
    //private void OnDestroy()
    //{
    //    buYuMusicContral.instant.allYinXiao[2].Play();//播放得分音效
    //}

}
