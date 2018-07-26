using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prizeGoldShow : MonoBehaviour {


    

    float skipNum=927;//开始跳转的Y值
    float startNum = -921.9f;//开始位置的Y值
    Vector3 v;
    public Vector3 distance=new Vector3(0,40,0);
    float startReduceTime;//开始减少时间
    float jishi=0;
    public bool isStop=false;
    public float stopNum=0;//需要停止的数字
    public bool isStop1 = false;

    public static bool isStart = false;
	// Use this for initialization
	void Start () {
       
        judge();

    }
	

    void judge()
    {
        switch (transform.parent.parent.name)
        {
            case "1":
                startReduceTime = 5.4f;
                break;
            case "2":
                startReduceTime = 5.1f;
                break;
            case "3":
                startReduceTime = 4.8f;
                break;
            case "4":
                startReduceTime = 4.5f;
                break;
            case "5":
                startReduceTime = 4.2f;
                break;
            case "6":
                startReduceTime = 3.9f;
                break;
            case "7":
                startReduceTime = 3.6f;
                break;
            case "8":
                startReduceTime = 3.3f;
                break;
            case "9":
                startReduceTime = 3f;
                break;
        }
    }
	// Update is called once per frame
	void Update () {
        if (isStart)
        {


            if (!isStop)
            {


                jishi += Time.deltaTime;
                if (jishi >= startReduceTime)
                {

                    distance.y -= 5;

                    if (distance.y <= 25)
                    {

                        isStop = true;
                        isStop1 = true;

                    }

                    if (distance.y != 0)
                    {
                        transform.Translate(distance);
                    }

                }
                else
                {
                    transform.Translate(distance);
                }

                if (this.transform.localPosition.y >= skipNum)
                {
                    v = transform.localPosition;
                    v.y = startNum;
                    transform.localPosition = v;
                }
            }
            if (isStop1)
            {
                if (this.transform.localPosition.y >= skipNum)
                {
                    v = transform.localPosition;
                    v.y = startNum;
                    transform.localPosition = v;
                }
                transform.Translate(distance);

                if (this.name == "content" && (Mathf.Abs(this.transform.localPosition.y - stopNum * 91) <= 10))
                {

                    Vector3 v = this.transform.localPosition;
                    v.y = stopNum * 91;
                    this.transform.localPosition = v;
                    distance.y = 0;
                    this.transform.parent.GetChild(1).GetComponent<prizeGoldShow>().isStop1 = false;
                    isStop1 = false;
                }

            }
        }

	}
}
