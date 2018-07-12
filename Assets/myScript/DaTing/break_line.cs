using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CClient;

public class break_line : MonoBehaviour {
    public static bool hall_line;
    //private GameObject scene;
    public float timer = 0f;
    byte[] b = new byte[0];

    // Use this for initialization
    private void Start()
    {
        //Instantiate(jiazaiScene.scene, Vector3.zero, Quaternion.identity);
        //Instantiate(Test.obj[0], Vector3.zero, Quaternion.identity);
    }
    void Update()
    {
        if (hall_line)
        {
            dealConnectNet();
            hall_line = false;
        }
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            if (Application.isPlaying)
            {
                //if(ClientSocket.instant().ws != null){

                //}
                ClientSocket.instant().ws.Send(b);
                timer = 0;
            }
        }
}
    /// <summary>
    /// 断线重连
    /// </summary>
    void dealConnectNet()
    { 
        if (GameObject.Find("Hall(Clone)") != null)
        {
            Destroy(GameObject.Find("Hall(Clone)").gameObject);
           GameObject hall= Instantiate(jiazaiScene.scene, Vector3.zero, Quaternion.identity);
           //GameObject hall = Instantiate(Test.obj[0], Vector3.zero, Quaternion.identity);
            hall.name = "Hall";
        }
    }
}
