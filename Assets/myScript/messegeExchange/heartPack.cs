using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CClient;
/// <summary>
/// 心跳包发送
/// </summary>
public class  heartPack : MonoBehaviour {
    public float timer = 0f;
    byte[] b = new byte[0];
    
    void Update()
    {
            timer += Time.deltaTime;
            if (timer >= 2)
            {
            if (Application.isPlaying)
            {
                ClientSocket.instant().ws.Send(b);
                timer = 0;
            }
         }      
    }
}
