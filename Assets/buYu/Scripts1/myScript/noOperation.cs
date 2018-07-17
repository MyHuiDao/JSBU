using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 多长时间不操作，退出
/// </summary>
public class noOperation : MonoBehaviour
{

    float noOperationLongtime = 90;
    float noOperationImplyTime = 60;
    float timeAdd = 0;

    bool tipImage = false;
    float exsitbeTimeAdd = 0;
    float exsitbeTime = 3;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.isAutoFire)
        {
            if (Input.GetMouseButton(0))
            {
                timeAdd = 0;
                if (tipImage)
                {
                    GameObject.Find("UI").transform.Find("noOperationTip").gameObject.SetActive(false);
                    tipImage = false;
                }


            }

            timeAdd += Time.deltaTime;


            if (timeAdd >= noOperationImplyTime)//开始提示
            {
                GameObject.Find("UI").transform.Find("noOperationTip").gameObject.SetActive(true);
                tipImage = true;

            }
            if (timeAdd >= noOperationLongtime)//退出
            {
                meiRenYuThreadDeal.instant.is20005 = true;
            }


            //if (tipImage)
            //{
            //    exsitbeTimeAdd += Time.deltaTime;
            //    if (exsitbeTimeAdd >= exsitbeTime)
            //    {
            //        GameObject.Find("noOperationTip").transform.localScale = Vector3.zero;
            //        tipImage = false;
            //    }

            //}

        }
        else
        {
            timeAdd = 0;
            if (tipImage)
            {
                GameObject.Find("UI").transform.Find("noOperationTip").gameObject.SetActive(false);
                tipImage = false;
            }
        }




    }


}
