using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class yuerMsgContrall
{



    public static yuerMsgContrall instance = null;
    public static yuerMsgContrall instant()
    {
        if (instance == null) instance = new yuerMsgContrall();
        return instance;
    }

    yuerMsgContrall()
    {


    }
    public void yuErMsgContralAddMethod()
    {
        CallBackFun.shareCallBack.addMethod(this);//把此类所有方法添加到callBack的字典中，方法类型为do1111等
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    public void do10000(object o)
    {
        Debug.Log("收到10000");
        JsonData j = (JsonData)o;



        yuerThreadDeal.instance.roomId = j["roomId"].ToString();
        saveDate.roomID = j["roomId"].ToString();
        yuerThreadDeal.instance.roomPersonNum = j["onlineUsers"].ToString();
        saveDate.roomPeopleNum = int.Parse(j["onlineUsers"].ToString());
        yuerThreadDeal.instance.is10000 = true;
        Debug.Log("倒计时："+ int.Parse(j["countdown"].ToString()));
        startTimeJiShi.instance.daoJiShi = int.Parse(j["countdown"].ToString());
        startTimeJiShi.instance.isStartJiShi = true;

    }
    /// <summary>
    /// 下注信息
    /// </summary>
    public void do10001(object o)
    {
        yuerContrall.instance.touzhuTarget.RemoveAt(0);
        yuerThreadDeal.instance.target = ((JsonData)o)["betsNumber"].ToString();
        yuerThreadDeal.instance.moneyAdd= ((JsonData)o)["betsGold"].ToString();
        yuerThreadDeal.instance.is10001 = true;
        Debug.Log("投注成功");
    }
    /// <summary>
    /// 下注失败
    /// </summary>
    public void do10002(object o)
    {
        yuerThreadDeal.instance.is10002 = true;

        Debug.Log("弹出投注失败");
    }

    /// <summary>
    /// 结算结果
    /// </summary>
    public void do10003(object o)
    {
        JsonData j = (JsonData)o;
        fishMove.aVar = float.Parse(j["aVar"].ToString());
        fishMove.cycleVar = float.Parse(j["cycleVar"].ToString()); 
        fishMove.bSpeedVar= float.Parse(j["bSpeedVar"].ToString());

        string[] fishname = ((JsonData)o)["result"].ToString().Split(',');
        string[] mingci=new  string[4];
        for (int i = 0; i < fishname.Length; i++)
        {
            if (fishname[i] =="1")
            {
                mingci[0] = (i + 1).ToString();
            }
            if (fishname[i] == "2")
            {
                mingci[1] = (i + 1).ToString();
            }
            if (fishname[i] == "3")
            {
                mingci[2] = (i + 1).ToString();
            }
            if (fishname[i] == "4")
            {
                mingci[3] = (i + 1).ToString();
            }
        }
      
       
        saveDate.fishRank = mingci;
        if (mingci.Length != 4)
        {
            Debug.Log("返回参数有问题");
        }
        for (int i = 0; i < mingci.Length; i++)
        {
            if (initialPrepare.nameAndNumdict.ContainsKey("fish" + (i + 1)))
            {
                initialPrepare.nameAndNumdict["fish" + (i + 1)] = int.Parse(mingci[i]);
            }
            else
            {
                initialPrepare.nameAndNumdict.Add("fish" + (i + 1), int.Parse(mingci[i]));
            }


        }
        initialPrepare.do10003Msg = o;//结算内容
        saveDate.jiesuanGetGold = ((JsonData)o)["gold"].ToString();
        Debug.Log("结算金币" + ((JsonData)o)["gold"].ToString());
        startTimeJiShi.instance.isToPrepare = true;




    }

    public void do10007(object o)
    {

    }


    /// <summary>
    /// 自己离开房间
    /// </summary>
    public void do10005(object o)
    {

    }

    /// <summary>
    /// 有人离开房间
    /// </summary>
    public void do10006(object o)
    {

        yuerThreadDeal.instance.leavePersonNum = ((JsonData)o).ToString();
        yuerThreadDeal.instance.is10006 = true;
    }


    /// <summary>
    /// 获取倒计时
    /// </summary>
    /// <param name="o"></param>
    public void do10009(object o)
    {
        Debug.Log("倒计时："+int.Parse(((JsonData)o).ToString()));
        startTimeJiShi.instance.daoJiShi = int.Parse(((JsonData)o).ToString()) ;
        startTimeJiShi.instance.isStartJiShi = true;
    }

    /// <summary>
    /// 获取金币数
    /// </summary>
    /// <param name="o"></param>
    public void do10010(object o)
    {
        yuerThreadDeal.instance.money = ((JsonData)o).ToString();
        yuerThreadDeal.instance.is10010 = true;
    }


    /// <summary>
    /// 获取房间开奖记录
    /// </summary>
    public void do10008(object o)
    {
         yuerContrall.instance.o= o;
        yuerContrall.instance.is10008 = true;
       


    }


    /// <summary>
    /// 有其他人进入房间
    /// </summary>
    public void do10011(object o)
    {
        yuerThreadDeal.instance.roomPeopleNum = int.Parse(((JsonData)o).ToString());
        yuerThreadDeal.instance.is10011 = true;
    }
}
