using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class copy : MonoBehaviour
{
    public static copy instance;

    void Start()
    {
        instance = this;
      
        for (int i = 0; i < hallHttp.instance.kefu_wechat.Count; i++)
        {
            int a = i;
            GameObject.Find("weixinkefu").transform.GetChild(a + 2).GetChild(1).GetComponent<Button>().onClick.AddListener(() => platform(hallHttp.instance.kefu_wechat[a]));
            GameObject.Find("KeFu_Pay").transform.GetChild(a).GetChild(2).GetComponent<Button>().onClick.AddListener(() => platform(hallHttp.instance.kefu_wechat[a]));
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hallHttp.instance.kefu_wechat.Count; i++)
        {
            GameObject.Find("weixinkefu").transform.GetChild(i + 2).GetChild(0).GetChild(0).GetComponent<Text>().text = hallHttp.instance.kefu_wechat[i];
            GameObject.Find("KeFu_Pay").transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = hallHttp.instance.kefu_wechat[i];
        }
    }
    /// <summary>
    /// 复制并打开微信
    /// </summary>
    /// <param name="number"></param>
    void platform(string number)
    {
#if UNITY_ANDROID
        weiXinLoad.androidObject.Call("openwx", number);
#endif
#if UNITY_IPHONE
        UnitySendMessageToiOS.Instante().copyText(number,"");
#endif
    }
}
