using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CClient;

public class yuerGameConnect : MonoBehaviour {

    
    public bool isreconnectNet = false;
    string token;//用来连接服务器的网址的一部分
                 
    void Start () {
       
        CallBackFun.shareCallBack.methods.Clear();
        yuerMsgContrall.instant().yuErMsgContralAddMethod();//先把方法都添加到字典里面
         
        token = netConnect.token;
        Debug.Log("连接鱼儿快跑");
        //ClientSocket.instant().clientSocket("ws://jinshayugang.com/match-fish/v1/game/", token);
      //  ClientSocket.instant().clientSocket("ws://hd.com/match-fish/v1/game/", token);
        ClientSocket.instant().clientSocket(httpConnect.Web_URL+"/match-fish/v1/game/",token);
        ClientSocket.instant().isYuerMatch = true;
        yuerSendMSg.instant().joinYuerRoom();
        //yuerSendMSg.instant().getCombatGain(1,1);//获取战绩
       
    }

    // Update is called once per frame
    void Update () {
		
	}
}
