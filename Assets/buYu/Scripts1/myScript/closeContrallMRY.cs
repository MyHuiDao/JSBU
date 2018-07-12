using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class closeContrallMRY : MonoBehaviour
{


    /// <summary>
    /// 实现所有弹出按钮的关闭和确定
    /// </summary>
    void Start()
    {

        GameObject[] obj = GameObject.FindGameObjectsWithTag("close");//所有关闭按钮的统一

        for (int i = 0; i < obj.Length; i++)
        {

            EventTriggerListener.Get(obj[i]).onClick = close;
        }
    }


    void Update()
    {

    }


    void close(GameObject g)
    {

        g.transform.parent.gameObject.transform.localScale = Vector3.zero;

        //RootCanvas.find("backGround").transform.localScale = Vector3.zero;//把阴影背景缩放为零


    }


}

