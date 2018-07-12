using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class timeJiShi : MonoBehaviour {

   


    public void startTime()
    {
        GameObject.Find("timeCountDown").transform.localScale = Vector3.one;
        GameObject.Find("timeNum").GetComponent<Animator>().enabled=true;
    }
  
}
