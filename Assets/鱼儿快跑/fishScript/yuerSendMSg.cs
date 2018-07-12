using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CClient;

public class yuerSendMSg {

    static yuerSendMSg instance;
    public static yuerSendMSg instant()
    {

        if (instance == null) instance = new yuerSendMSg();
        return instance;

    }

    /// <summary>
    /// 进入房间，
    /// </summary>
    public void joinYuerRoom()
    {
        Debug.Log("发送进入房间信息10000");
        ClientSocket.instant().send("10000");
    }

    /// <summary>
    /// 下注
    /// </summary>
    public void chipIn(string _betNum,string _betGold)
    {
        Debug.Log("发送下注信息");
        ClientSocket.instant().send("10001", (object)("{\"betsNumber\":\"" + _betNum + "\",\"betsGold\":\"" + _betGold +  "\"}"));
    }

    /// <summary>
    /// 获取倒计时
    /// </summary>
    public void getCountDown()
    {
        ClientSocket.instant().send("10009");
        ClientSocket.instant().send("10010");
    }

    /// <summary>
    /// 获取战绩
    /// </summary>
    public void getCombatGain(int _page,int _sizeNum)
    {
        Debug.Log("发送获取战绩的信息");
        ClientSocket.instant().send("10008", (object)("{\"page\":\"" + _page + "\",\"size\":\"" + _sizeNum + "\"}"));
    }
}
