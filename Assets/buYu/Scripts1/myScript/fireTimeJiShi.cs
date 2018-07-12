using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此类如果没收到服务器开火消息，执行
/// </summary>
public class fireTimeJiShi : MonoBehaviour {


    public static  bool isCanFire=false;
    float time = 0;
	
	// Update is called once per frame
	void Update () {
        if (isCanFire)
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                GameController.lastBulletFire = true;
            }
        }

	}
}
