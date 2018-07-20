using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 专门控制鱼阵
/// </summary>
public class fishArrayContral : MonoBehaviour
{
    public bool yuzhenStart = false;
    public bool yuzhenFinish = false;
    public GameObject[] yuzhen = new GameObject[4];
    public static fishArrayContral instant = null;
    public static List<List<string>> listGroupYuzhen = new List<List<string>>();//保存每个鱼阵
    public int yuzhenCard;
    public Dictionary<string, GameObject> yuZhenTarget = new Dictionary<string, GameObject>();//保存鱼阵里面的鱼

    public GameObject yuzhenPrefab = null;
    public bool is20019 = false;
    public bool everyMakeOn = false;
    void Start()
    {
        everyMakeOn = false;
        instant = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (yuzhenPrefab != null && everyMakeOn)
        {
            //鱼阵结束yuzhenNew2(Clone)
            if ((yuzhenPrefab.name == "yuzhenNew1(Clone)" && yuzhenPrefab.transform.localPosition.x >= 6) || (yuzhenPrefab.name == "yuzhenNew2(Clone)" && yuzhenPrefab.transform.localPosition.x <= -12) || (yuzhenPrefab.name == "yuzhenNew3(Clone)" && (yuzhenPrefab.transform.Find("pos").position.x) >= -20f) || (yuzhenPrefab.name == "yuzhenNew4(Clone)" && (yuzhenPrefab.transform.Find("pos").position.x) >= 40f))//这儿注意数字
            {
                Debug.Log("进来了。。。。。。。。。。。");
                yuzhenFinish = true;
                contrall.instant().isCanClearFish = false;
                everyMakeOn = false;
                //yuzhenPrefab = null;
            }
            if (yuzhenStart)
            {
                //鱼阵开始

                if (contrall.instant().isZhuJi)//如果是主机，则要发送征求信息
                {

                    WebButtonSendMessege.instant().fishArrayStartMove(yuzhenCard);//发送给服务器征求鱼阵的游动
                }
                yuzhenStart = false;
            }
        }
        if (is20019)
        {
            deal20019();
            is20019 = false;
        }

    }
    void deal20019()
    {
        mainMeiRenYuContrall.instant.fishPauseMask.transform.localScale = Vector3.one;//显示倒计时
    }

    public void MakeFishesYuZhen()
    {
        meiRenYuThreadDeal.instant.is20023 = true;



    }
    /// <summary>
    /// 鱼阵鱼的生成
    /// </summary>
    /// <param name="_id">鱼的ID</param>
    public void MakeFishes(int fishNum, string _id)
    {

        Transform tra = yuzhenPrefab.transform.GetChild(fishNum);
        tra.GetComponent<FishAttr>().id = _id;
        tra.GetComponent<FishAttr>().isBelongYuZhen = true;
        yuZhenTarget.Add(_id, yuzhenPrefab.transform.GetChild(fishNum).gameObject);
    }

}
