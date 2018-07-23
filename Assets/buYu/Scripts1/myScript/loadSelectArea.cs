using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 第一次加载selectArea预制体
/// </summary>
public class loadSelectArea : MonoBehaviour
{

    public bool is20002 = false;
    public static bool connectNet = false;
    public static loadSelectArea instant = null;
    //public static GameObject areaPrefabs = null;
    
    void Start()
    {
        instant = this;
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        //if (buyugameBiaoShi == getMeiRenYuArea.buyuGame)
        //{


        //areaPrefabs = weiXinLoad.instance.selectAreas[getMeiRenYuArea.buyuGame];// Resources.Load("myPrefabs/meiRenYu/selectArea" + getMeiRenYuArea.buyuGame) as GameObject;
        Instantiate(weiXinLoad.instance.selectAreas[getMeiRenYuArea.buyuGame]/*areaPrefabs*/, GameObject.Find("main").transform);

        //areaPrefabs = Test.obj[ getMeiRenYuArea.buyuGame+4];
        //Instantiate(areaPrefabs, GameObject.Find("main").transform);

    }

    void Update()
    {

        if (connectNet)
        {
            dealConnectNet();
            connectNet = false;
        }
        if (is20002)
        {
            deal20002();
            is20002 = false;
        }
    }
    public void deal20002()
    {
        GameObject.Find("moneyNotEnough").transform.localScale = Vector3.one;
        getMeiRenYuArea.intstant.moneyNoEnough = true;
    }
    /// <summary>
    /// 断线重连
    /// </summary>
    void dealConnectNet()
    {
        if (GameObject.Find("selectArea" + getMeiRenYuArea.buyuGame + "(Clone)") != null)
        {
            Destroy(GameObject.Find("selectArea" + getMeiRenYuArea.buyuGame + "(Clone)").gameObject);

            Instantiate(weiXinLoad.instance.selectAreas[getMeiRenYuArea.buyuGame]/*loadSelectArea.areaPrefabs*/, GameObject.Find("main").transform);
        }
        else
        {
            if (GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)") != null)
            {
                contrall.instant().clearAll();
                Destroy(GameObject.Find("MainScene" + getMeiRenYuArea.buyuGame + "(Clone)").gameObject);

            }
            Instantiate(weiXinLoad.instance.selectAreas[getMeiRenYuArea.buyuGame]/*loadSelectArea.areaPrefabs*/, GameObject.Find("main").transform);
        }
    }
}
