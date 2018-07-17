using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldFinish : MonoBehaviour
{


    /// <summary>
    /// 金币扩散动画结束后执行此方法
    /// </summary>
    public void goldfin()
    {
        this.transform.GetComponent<Ef_MoveTo>().enabled = true;
    }
    public void goldfin1()
    {
        Destroy(this.transform.parent.gameObject);
    }
   

    public void closeAnimator()
    {
        this.GetComponent<Animator>().enabled = false;
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }

}
