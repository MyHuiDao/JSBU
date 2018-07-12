using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class timeJiShi2 : MonoBehaviour
{
    public static bool startGame = false;//开始比赛

    Transform uidonghuaLeft = null;
    Transform uidonghuaRight = null;
    // Use this for initialization
    void Start()
    {
        startGame = false ;
        uidonghuaLeft = GameObject.Find("UIdongHua").transform.Find("left");
        uidonghuaRight = GameObject.Find("UIdongHua").transform.Find("right");

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void finishTime()
    {
        GameObject.Find("timeCountDown").transform.localScale = Vector3.zero;
        uidonghuaLeft.GetComponent<Animator>().enabled = false;
        uidonghuaRight.GetComponent<Animator>().enabled = false;
        uidonghuaLeft.DOLocalMoveX(-967.5f, 0.1f);
        uidonghuaRight.DOLocalMoveX(967.5f, 0.1f);

        Destroy(GameObject.Find("timeNum").gameObject);
        startGame = true;
        cameraFlow.Instance.isStart = true;
        //Invoke("dealyOne",3f);
        
    }
    //延迟1s
    void dealyOne()
    {
        cameraFlow.Instance.isStart = true;
    }
}
