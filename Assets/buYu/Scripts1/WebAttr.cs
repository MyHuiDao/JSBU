using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * 做的事情：fish碰到网，受伤害；(受伤害的具体过程交给鱼自己来)
 * 挂在网的预制体上
 * */
public class WebAttr : MonoBehaviour
{
    public float disapperTime;	//网的自动消失时间
    public int damage;
    public string ziDanID;
    public string skillFishTag=null;
    void Start()
    {
       
        Destroy(gameObject, disapperTime);	//网几秒后销毁自己
    }


    private void Update()
    {
        
    }
    //鱼生成挂了动态刚体，故网身上只需要加上boxcollider2d
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
		//网碰到鱼
        if (collision.tag==skillFishTag)
        {
            if (FishMaker.SaveNet.ContainsKey(ziDanID))
            {
                FishMaker.SaveNet[ziDanID].Add(collision.transform.parent.GetComponent<FishAttr>().id);
            }
           
        }

    }
}
