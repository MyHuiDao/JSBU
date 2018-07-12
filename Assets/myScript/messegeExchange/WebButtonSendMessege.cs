using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CClient;
using System.Text;

/// <summary>
/// 此类专门用来点击按钮发送信息
/// </summary>
public class WebButtonSendMessege
{




    static WebButtonSendMessege instance;
    public static WebButtonSendMessege instant()
    {

        if (instance == null) instance = new WebButtonSendMessege();
        return instance;

    }
    /// <summary>
    /// 选择游戏进入分区
    /// </summary>
    /// <param name="_code">说明</param>
    /// <param name="_data">分区ID</param>
    public void selectGameArea(string _id)
    {
        //Debug.Log("发送进入房间信息20001"+_id);
        ClientSocket.instant().send("20001", _id);//跟获取游戏分区结合


    }

    /// <summary>
    /// 捕鱼开炮
    /// </summary>
    public void openFire(string _code, string _angle)
    {

        if (GameObject.Find("BulletHolder").transform.childCount >= saveDate.bullutMaxNum)
        {
            GameObject.Find("bulletMax").transform.localScale = Vector3.one;
            meiRenYuThreadDeal.instant.bulletShowNotEnough = true;

            return;
        }
        ClientSocket.instant().send(_code, _angle);

    }

    /// <summary>
    /// 退出房间
    /// </summary>
    public void exitRoom()
    {

        ClientSocket.instant().send("20005");

    }
    /// <summary>
    /// 鱼消失
    /// </summary>
    public void fishDead(string _id)
    {
        if (FishMaker.fishTarget.Count == 0)
        {
            return;
        }
        iTween.Pause(FishMaker.fishTarget[_id]);
        FishAttr f = FishMaker.fishTarget[_id].GetComponent<FishAttr>();
        f.nextDian = 0;
        FishMaker.fishTarget[_id].transform.localPosition = new Vector3(guiJi.instant().guiJiDict[f.runPoint][0].x, guiJi.instant().guiJiDict[f.runPoint][0].y, 90);//赋予出生点,不设置有一个闪的过程
       


        f.judgeSpot = 0;
        f.initialX = 0;
        f.initialY = 0;
        f.Send20010One = true;

        fishStartMove(_id);//发送给服务器征求鱼的游动}
        //FishMaker.fishTarget[_id].GetComponent<FishAttr>().isSend = true;
       // ClientSocket.instant().send("20010", _id);

    }
    /// <summary>
    /// 子弹消失
    /// </summary>
    public void bulletDie(string _code, string bulletID, string bulletX, string bulletY, List<string> fishGroupID)
    {

        StringBuilder fishID = new StringBuilder();
        for (int i = 0; i < fishGroupID.Count; i++)
        {

            fishID.Append(fishGroupID[i] + ",");
        }
        if (fishGroupID.Count == 0)//如果网没补到鱼
        {
            for (int i = 0; i < 4; i++)
            {
                if (GameController.Instance.bulletDict[i].ContainsKey(bulletID))
                {
                    Debug.Log("发送20009" + bulletID);
                    ClientSocket.instant().send("20009", (object)("{\"fireId\":\"" + bulletID + "\",\"x\":" + bulletX + ",\"y\":" + bulletY + ",\"fishList\":\"\"}"));
                    //GameController.Instance.bulletDict[i][bulletID].GetComponent<BulletAttr>().destroyBullet();
                    //GameController.Instance.bulletDict[i].Remove(bulletID);

                    break;
                }
            }
            return;
        }
        fishID.Remove(fishID.Length - 1, 1);//删除，号
        Debug.Log("发送20009"+bulletID);
        ClientSocket.instant().send("20009", (object)("{\"fireId\":\"" + bulletID + "\",\"x\":" + bulletX + ",\"y\":" + bulletY + ",\"fishList\":\"" + fishID + "\"}"));

    }
    /// <summary>
    /// 鱼开始游动
    /// </summary>
    public void fishStartMove(string _id)
    {

        ClientSocket.instant().send("20011", _id);

    }
    /// <summary>
    /// 鱼阵开始游动
    /// </summary>
    /// <param name="_id"></param>
    public void fishArrayStartMove(int yuzhen)
    {
        ClientSocket.instant().send("20022", yuzhen.ToString());
    }
    /// <summary>
    /// 转发主机信息
    /// </summary>
    public void giveCurrentRoomMsg()
    {
        meiRenYuThreadDeal.instant.is20012 = true;

    }

    /// <summary>
    /// 加炮等级
    /// </summary>
    public void addButtleClass()
    {
        ClientSocket.instant().send("20013");

    }
    /// <summary>
    /// 减炮等级
    /// </summary>
    public void reduceButtleClass()
    {
        ClientSocket.instant().send("20015");

    }

    /// <summary>
    /// 发送释放的技能
    /// </summary>
    public void releaseSkill(string _skillName, int _useSkillTarget = 0, string _lockFishID = "")
    {

        ClientSocket.instant().send("20019", (object)("{\"skillName\":\"" + _skillName + "\",\"useSkillTarget\":" + _useSkillTarget + ",\"lockFishID\":\"" + _lockFishID + "\"}"));
    }

    public void skillOver(string _skillName, int _useSkillTarget = 0)
    {

        ClientSocket.instant().send("20020", (object)("{\"skillName\":\"" + _skillName + "\",\"useSkillTarget\":" + _useSkillTarget + "}"));

    }

    public void yuZhenTime()
    {
        ClientSocket.instant().send("20026");
    }



}
