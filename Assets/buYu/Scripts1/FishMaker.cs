using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading;


/**
 * 自动生成鱼的脚本
 * 挂在ScriptsHolder这个GameObj上
 * 随机在某个指定位置随机生成某种鱼
 **/

public class FishMaker : MonoBehaviour
{


    private static Tweener t;
    public Transform fishGroup;        //生成鱼的容器
    //static bool everyFishMove = false;
    public GameObject[] fishPrefabs;	//生成鱼的prefab
    public float fishGenWaitTime = 0.05f;//每条鱼生成时间间隔
    public float waveGenWaitTime = 0.3f;
    public static bool is20004Scencod = false;//专门用于判断已经接收到了20004,但是场景还没加载，先挂起该方法
    public static bool is20012Scencod = false;//专门用于判断已经接收到了20004,但是场景还没加载，先挂起该方法
    public static bool is20001Scencod = false;
    public static bool is20010Scencod = false;
    public static bool is20011Scencod = false;
    public static Dictionary<string, List<string>> SaveNet = new Dictionary<string, List<string>>();//保存每个网


    public static List<List<string>> listGroup = new List<List<string>>();//保存每个鱼群
    public static Dictionary<string, FishAttr> fish = new Dictionary<string, FishAttr>();//保存所有生成鱼的信息，跟实际存在的鱼有区别，这两个只是有参数关系的联系

    public static Dictionary<string, GameObject> fishTarget = new Dictionary<string, GameObject>();

    public bool fishGroupIsOver = true;//检测一波鱼是否完全生成
    public static FishMaker instant = null;
    Dictionary<string, GameObject> fishPrefabsDict = new Dictionary<string, GameObject>();//把名字跟预制体对应起来
    public static bool isNeedFishDestroy = false;
    //public static string fishDeadID;
    public bool issecondTimeStart = false;
    public static bool isHalfGoGame = false;
    public bool isPause = false;
    public int fishGroupNum = 1;//鱼群个数,此处不能乱改

    public static List<string> deadFish = new List<string>();//先保存要销毁的鱼
    public static List<string> moveFish = new List<string>();//保存需要游动的鱼
    public static List<int> do20004Order = new List<int>();//保存需要游动的鱼
    public int listGroupOrder = 0;

    int fishOrderLay;
    void Start()
    {
        fishPrefabsDict.Add("1", fishPrefabs[0]);
        fishPrefabsDict.Add("2", fishPrefabs[1]);
        fishPrefabsDict.Add("3", fishPrefabs[2]);
        fishPrefabsDict.Add("4", fishPrefabs[3]);
        fishPrefabsDict.Add("5", fishPrefabs[4]);
        fishPrefabsDict.Add("6", fishPrefabs[5]);
        fishPrefabsDict.Add("7", fishPrefabs[6]);
        fishPrefabsDict.Add("8", fishPrefabs[7]);
        fishPrefabsDict.Add("9", fishPrefabs[8]);
        fishPrefabsDict.Add("10", fishPrefabs[9]);
        fishPrefabsDict.Add("11", fishPrefabs[10]);
        fishPrefabsDict.Add("12", fishPrefabs[11]);
        fishPrefabsDict.Add("13", fishPrefabs[12]);
        fishPrefabsDict.Add("14", fishPrefabs[13]);
        fishPrefabsDict.Add("15", fishPrefabs[14]);
        fishPrefabsDict.Add("16", fishPrefabs[15]);
        fishPrefabsDict.Add("17", fishPrefabs[16]);
        fishPrefabsDict.Add("18", fishPrefabs[17]);
        fishPrefabsDict.Add("19", fishPrefabs[18]);
        fishPrefabsDict.Add("20", fishPrefabs[19]);

        instant = this;


        is20004Scencod = true;
        is20012Scencod = true;
        is20010Scencod = true;
        is20011Scencod = true;
        fishOrderLay = 1;
    }

    private void Update()
    {
        //if (issecondTimeStart)
        //{

        //    if (fishGroupIsOver)//前一波产生完了
        //    {
        //        if (listGroup.Count >= fishGroupNum)
        //        {
        //            StartCoroutine(ie(fishGroupNum - 1));
        //            issecondTimeStart = false;
        //        }

        //    }
        //}

        if (do20004Order.Count != 0)//不考虑20004和20012的先后顺序，概率几乎为零
        {
            
            if (fishGroupIsOver)//前一波产生完了
            {

                StartCoroutine(ie(listGroupOrder));


                listGroupOrder++;
                do20004Order.RemoveAt(0);



            }
        }

        if (moveFish.Count != 0)
        {


            int j = moveFish.Count;
            for (int i = 0; i < j; i++)
            {
                if (!contrall.instant().isZhuJi)
                {
                    iTween.Pause(fishTarget[moveFish[i]]);
                    FishAttr f = fishTarget[moveFish[i]].GetComponent<FishAttr>();
                    f.nextDian = 0;
                    fishTarget[moveFish[i]].transform.localPosition = new Vector3(guiJi.instant().guiJiDict[f.runPoint][0].x, guiJi.instant().guiJiDict[f.runPoint][0].y, 90);//赋予出生点,不设置有一个闪的过程
                                                                                                                                                                              // Debug.Log("重赋予的值" + FishMaker.fishTarget[moveFish[i]].transform.localPosition);
                    f.judgeSpot = 0;
                    f.initialX = 0;
                    f.initialY = 0;
                    f.Send20010One = true;
                }


                GameObject transformID = fishTarget[moveFish[i]];

                guiJi.instant().startMove(transformID.GetComponent<FishAttr>().runPoint, transformID.GetComponent<FishAttr>().nextDian);//传入所需要的轨迹  
                                                                                                                                        //速度改变        

                iTween.MoveTo(transformID.gameObject, iTween.Hash("path", guiJi.instant().waypoints, "speed", transformID.GetComponent<FishAttr>().Speed, "movetopath", false, "orienttopath", true, "looktime", 0.6, "easetype", "linear"));




            }
            for (int k = 0; k < j; k++)
            {
                moveFish.RemoveAt(0);
            }
        }

        if (deadFish.Count != 0)
        {
            int j = deadFish.Count;
            for (int i = 0; i < j; i++)
            {
                if (FishMaker.fishTarget.ContainsKey(deadFish[i]))
                {

                    Destroy(FishMaker.fishTarget[deadFish[i]].gameObject);


                    contrall.instant().fishTargetRemove(deadFish[i]);

                }



            }
            for (int k = 0; k < j; k++)
            {
                deadFish.RemoveAt(0);
            }



        }





    }

    /// <summary>
    /// 第二次后面产生鱼群执行此方法
    /// </summary>
    public void secondTimeStart()
    {

        issecondTimeStart = true;

    }


    /// <summary>
    /// 注意：非主机，直接把鱼全部产生出来，等待服务器的游动通知
    /// </summary>
    /// <returns></returns>
    IEnumerator ie(int length)
    {

        fishGroupNum++;
        fishGroupIsOver = false;
        bool _isHalfWayGoGame = isHalfGoGame;

        int fishGroupLength = listGroup[length].Count;//正在实例化鱼的数量记录起来     

        if (_isHalfWayGoGame)//中途进来直接全部产生
        {

            for (int i = 0; i < listGroup[length].Count; i++)
            {

                //if (listGroup[length].Count != fishGroupLength)//如果正在产生的鱼群鱼个数发生了变化，要及时调整参数
                //{
                //    fishGroupLength = listGroup[length].Count;
                //}
                //if (i == fishGroupLength)
                //{
                //    Debug.Log("中途进来直接产生，跳出");
                //    break;
                //}

                MakeFishes(fish[listGroup[length][i]].fishType, fish[listGroup[length][i]].id, fish[listGroup[length][i]].runPoint, fish[listGroup[length][i]].nextDian, fish[listGroup[length][i]].Speed, _isHalfWayGoGame);
            }

            yield return null;
        }
        else if (!contrall.instant().isZhuJi)//不是主机直接产生
        {
            Debug.Log("非主机生产鱼");
            for (int i = 0; i < listGroup[length].Count; i++)
            {
                //Debug.Log(listGroup[length].Count);
                //Debug.Log(fishGroupLength);
                if (listGroup[length].Count != fishGroupLength)//如果正在产生的鱼群鱼个数发生了变化，要及时调整参数
                {
                    i = i - (fishGroupLength - listGroup[length].Count);
                    fishGroupLength = listGroup[length].Count;
                }
                if (i < 0)
                {
                    break;
                }

                MakeFishes(fish[listGroup[length][i]].fishType, fish[listGroup[length][i]].id, fish[listGroup[length][i]].runPoint, fish[listGroup[length][i]].nextDian, fish[listGroup[length][i]].Speed, _isHalfWayGoGame);
            }

            yield return null;
        }
        else//主机产生
        {

            if (listGroup.Count > length)
            {
                for (int i = 0; i < listGroup[length].Count; i++)
                {
                    yield return new WaitForSeconds(fishGenWaitTime);


                    try
                    {
                        if (listGroup[length].Count != fishGroupLength)//如果正在产生的鱼群鱼个数发生了变化，要及时调整参数
                        {
                            i = i - (fishGroupLength - listGroup[length].Count);
                            fishGroupLength = listGroup[length].Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }

                    if (i < 0)
                    {
                        break;
                    }

                    try
                    {
                        MakeFishes(fish[listGroup[length][i]].fishType, fish[listGroup[length][i]].id, fish[listGroup[length][i]].runPoint, fish[listGroup[length][i]].nextDian, fish[listGroup[length][i]].Speed, _isHalfWayGoGame);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }



                }
            }



        }

        isHalfGoGame = false;//中途进来只有一次
        fishGroupIsOver = true;

    }
    /// <summary>
    /// 鱼的生成
    /// </summary>
    /// <param name="_fishType">鱼类型</param>
    /// <param name="_id">鱼的ID</param>
    /// <param name="runPoint">鱼的轨迹</param>
    void MakeFishes(string _fishType, string _id, string _runPoint, int _nextDian, float _speed, bool _isHalfGoGame)
    {

        if (contrall.instant().isCanClearFish)//鱼阵开始后后面的鱼不再游动
        {

            return;
        }
        //生成鱼的种类
        GameObject fishObj = Instantiate(fishPrefabsDict[_fishType]);
        //if (meiRenYuThreadDeal.gosWeiZhi == 1)
        //{
        //    fishObj.transform.rotation = Quaternion.Euler(0,0,180);

        //}
        fishObj.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = fishOrderLay;
        fishOrderLay++;
        if (fishOrderLay >= 90)
        {
            fishOrderLay = 0;
        }

        if (_isHalfGoGame)//如果是中途进入房间的
        {
            fishObj.transform.localPosition = new Vector3(fish[_id].initialX, fish[_id].initialY, 90);

        }
        else
        {
            fishObj.transform.localPosition = new Vector3(guiJi.instant().guiJiDict[_runPoint][0].x, guiJi.instant().guiJiDict[_runPoint][0].y, 90);//赋予出生点,不设置有一个闪的过程
        }


        //添加鱼对象

        fishObj.GetComponent<FishAttr>().id = _id;
        fishObj.GetComponent<FishAttr>().fishType = _fishType;
        fishObj.GetComponent<FishAttr>().runPoint = _runPoint;
        fishObj.GetComponent<FishAttr>().nextDian = _nextDian;
        fishObj.GetComponent<FishAttr>().Speed = _speed;

        fishTarget.Add(_id, fishObj);//把鱼本身添加到字典中

        fishObj.transform.SetParent(fishGroup, false);
        if (contrall.instant().isZhuJi)
        {
            if (isPause)
            {

                Debug.Log(4);
                FishMaker.fishTarget[_id].GetComponent<FishAttr>().isSend = true;
                CClient.ClientSocket.instant().send("20010", _id);
                //WebButtonSendMessege.instant().fishDead(_id);

                return;//如果冰冻了，产生直接消失


            }
        }
        if (contrall.instant().isZhuJi)//如果是主机，则要发送征求信息
        {

            WebButtonSendMessege.instant().fishStartMove(_id);//发送给服务器征求鱼的游动}


        } 
        if (_isHalfGoGame)//只有中途进的那个瞬间才会直接游动
        {

            if (contrall.instant().judgeIsMove.Contains(_id))
            {

                guiJi.instant().startMove(fishTarget[_id].GetComponent<FishAttr>().runPoint, fishTarget[_id].GetComponent<FishAttr>().nextDian);//传入所需要的轨迹  
                                                                                                                                                //速度改变        
                iTween.MoveTo(fishTarget[_id].gameObject, iTween.Hash("path", guiJi.instant().waypoints, "speed", fishTarget[_id].gameObject.GetComponent<FishAttr>().Speed, "movetopath", false, "orienttopath", true, "looktime", 0.6, "easetype", "linear"));
            }

        }





    }


    public void fishMove(string _id)
    {
        //Debug.Log("游" + _id);
        if (!fishTarget.ContainsKey(_id))
        {
            Debug.Log("字典里面没有包含此ID,测试，后面没问题取消。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。" + _id);
            return;
            //fishTarget.Add(_id,);
        }
        moveFish.Add(_id);

        //transformID = fishTarget[_id];
        //Debug.Log("游2");
        //everyFishMove = true;

    }

}
