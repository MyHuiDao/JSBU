using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CClient;

/// <summary>
/// 此类用webSocket对服务器进行连接，放在游戏里面进行调用。
/// </summary>
public class gameConnect : MonoBehaviour {


    string token;//用来连接服务器的网址的一部分
   
	// Use this for initialization
	void Start () {

        CallBackFun.shareCallBack.methods.Clear();
        contrall.instant().contrallAddMethod();//先把方法都添加到字典里面
        token = netConnect.token;
        //contrall.instant().isLoadSceneOne =  
        Debug.Log("连接捕鱼");

        if (ClientSocket.instant().connectWhichNet != 2)
        {
            ClientSocket.instant().ws.Close();
            ClientSocket.instant().clientSocket(httpConnect.Web_URL + "/fishing/v1/game/", token);
            ClientSocket.instant().connectWhichNet = 2;
        }
       
        // ClientSocket.instant().clientSocket("ws://192.168.31.238:8081/fishing/v1/game/", token);
        //ClientSocket.instant().clientSocket("ws://jinshayugang.com/fishing/v1/game/", token);//连接到服务器


    }

    // Update is called once per frame
    void Update () {
		
	}
}
