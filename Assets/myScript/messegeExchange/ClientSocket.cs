using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using WebSocketSharp.Net;
using LitJson;
using System.Reflection;
using System.Net;
using System.Threading;

public class messegeQueuePara
{

    public MethodInfo m1 = null;
    public object[] obj = null;
}
namespace CClient
{
    /// <summary>
    /// 创建客户端，连接网络
    /// </summary>
    public class ClientSocket
    {
        public bool isYuerMatch = false;
        public List<messegeQueuePara> messegeQueue = new List<messegeQueuePara>();
        public WebSocket ws;//webSocket客户端对象

        jsonSend js = null;
        public string Message;


        public static ClientSocket instance = null;

        public static ClientSocket instant()
        {

            if (instance == null) { instance = new ClientSocket(); }

            return instance;


        }

        public void clientSocket(string url, string token)
        {
            //cilentConnectServe("ws://192.168.31.238:8081/fishing/v1/game/" + token);//连接到服务器
            cilentConnectServe(url + token);//连接到服务器http://jinshayugang.com/fishing  

        }

        void cilentConnectServe(string path)
        {
            js = new jsonSend();
            ws = new WebSocket(path);
            ws.OnMessage += Ws_OnMessage;//添加委托，收到消息后，自动执行。
            ws.OnClose += Ws_OnClose;//连接关闭时触发
            ws.OnError += Ws_OnError;//出现异常触发
            ws.OnOpen += Ws_OnOpen;//一旦服务器响应了WebSocket连接请求，open事件触发并建立一个连接。
            ws.Connect();
            //ws.Send(js.jsonMessege(1000));//实际上还是发的字符串，只是该字符串格式要像json格式的
            //可以随处调用该方法，随处发送消息
            //System.Net.Sockets.Worker:Receive()

        }

        //发送消息给服务器
        public void send(string code)
        {

            ws.Send(js.jsonMessege(code));


        }
        public void send(string code, string data)
        {

            ws.Send(js.jsonMessege(code, data));

        }
        public void send(string code, object data)
        {

            ws.Send(js.jsonMessege(code, data));

        }

        //接收服务器的消息
        void receive(string receiveMessege)
        {


            jsonJieXi(receiveMessege);

        }

        /// <summary>
        /// 接收到的服务器消息进行json解析
        /// </summary>
        void jsonJieXi(string receiveMessege)
        {

            JsonData js = JsonMapper.ToObject(receiveMessege);


            MethodInfo m1 = CallBackFun.shareCallBack.methods[int.Parse(js["code"].ToString())];//把该方法取出来 
            if (isYuerMatch)
            {
                CallBackFun.shareCallBack.pars[0] = (object)js["data"];
            }
            else
            {
                if (js["code"].ToString() == "20016" || js["code"].ToString() == "20014" || js["code"].ToString() == "20018" || js["code"].ToString() == "10002" || js["code"].ToString() == "20021" || js["code"].ToString() == "20025")//这些方法不返回data
                {
                    CallBackFun.shareCallBack.pars[0] = null;
                }
                else
                {
                    CallBackFun.shareCallBack.pars[0] = (object)js["data"];
                }
            }
            m1.Invoke(CallBackFun.shareCallBack.obj, CallBackFun.shareCallBack.pars);//自动执行方法\          
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log(e.Data);



            if (e.Data != "")
            {

                Message = e.Data;
                receive(e.Data);
            }
        }
        private void Ws_OnOpen(object sender, EventArgs e)
        {
            Debug.Log("服务器连接成功");
        }

        private void Ws_OnError(object sender, ErrorEventArgs e)
        {
            Debug.Log("webSocket Error:" + e.Message);


        }

        private void Ws_OnClose(object sender, CloseEventArgs e)
        {
            reConnectUI.isShowUi = true;
            
            Debug.Log("连接已关闭：" + e);
            Debug.Log(e.Code);
            Debug.Log(e.Reason);
            if (e.WasClean)
            {
                Debug.Log("服务器主动关闭连接");
            }
            else
            {

                Debug.Log("发生异常，网络中断");
            }

            Thread.Sleep(3000); //停3秒
            Debug.Log("后面进行断线重连");
            //进行断线重连
            if (!otherContral.instant.returnGameScene)
            {
                Debug.Log("断线重连了");
                ws.Close();
                loadSelectArea.connectNet = true;
            }

            if (!gameContrall.instant.return_scene)
            {


                break_line.hall_line = true;

            }
        }

       
    }
}
