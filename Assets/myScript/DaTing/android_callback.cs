using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class android_callback : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void share_wechat(string web)
    {
        httpConnect.GET(this, web +httpView.id, null, sharewechat, httpError);
    }

    void sharewechat(string str)
    { 
        JsonData js = JsonMapper.ToObject(str);
        if ((string)js["code"] == "0")
        {
            if(((IDictionary)js).Contains("data"))
            {
                hallHttp.instance.message();
                netConnect.Ani(14);
            }
        }
    }
    /// <summary>
    /// 出现连接错误执行该方法
    /// </summary>
    /// <param name="str"></param>
    void httpError(string str)
    {
        Debug.Log(str);
    }
}
