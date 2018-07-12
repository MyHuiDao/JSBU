using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nose : MonoBehaviour {

    public Transform meiRenYuLightTrans;
    public GameObject meiRenYu_Yu0;
    public GameObject meiRenYu_Yu1;
    public GameObject meiRenYu_Yu2;
    public RectTransform meiRenYuParent;
    public GameObject meiRenYu_paopao;
    private List<GameObject> paopaoList = new List<GameObject>();
    public GameObject meirenyu_gold;


    private Vector3 meiRenYu_Yu0S;
    private Vector3 meiRenYu_Yu1S;
    private Vector3 meiRenYu_Yu2S;
    Vector3[] yu0Path = new Vector3[3];
    Vector3[] yu1Path = new Vector3[5];
    Vector3[] yu2Path = new Vector3[5];

    public GameObject liKuiPiYuNoseL;
    public GameObject liKuiPiYuNoseR;
    public Transform liKuiPiYuLightTrans;

    public Transform DaNaoTianGongLightTrans;
    public GameObject SWK;

    public GameObject blue_fish;
    public GameObject red_fish;

    //public List<Transform> lightTrans;
    void Start () {
        meiRenYu_Yu0S = meiRenYu_Yu0.transform.localPosition;//鱼
        meiRenYu_Yu1S = meiRenYu_Yu1.transform.localPosition;
        meiRenYu_Yu2S = meiRenYu_Yu2.transform.localPosition;


        //键值对儿的形式保存iTween所用到的参数
        Hashtable args = new Hashtable();
        //旋转的角度
        args.Add("rotation", new Vector3(0, 0, -5));

        //是否使用局部角度(默认为false)
        args.Add("islocal", true);
        //动画的速度
        //args.Add("speed",10f);
        //动画的时间
        args.Add("time", 0.5f);
        //延迟执行时间
        args.Add("delay", 0.01f);

        //这里是设置类型，iTween的类型又很多种，在源码中的枚举EaseType中
        args.Add("easeType", iTween.EaseType.linear);
        //三个循环类型 none loop pingPong (一般 循环 来回)  
        //args.Add("loopType", "none");
        //args.Add("loopType", "loop"); 
        args.Add("loopType", iTween.LoopType.pingPong);
        iTween.RotateTo(liKuiPiYuNoseL, args);//胡子
       
        //iTween.RotateTo(liKuiPiYuNoseR, args);
        iTween.RotateTo(liKuiPiYuNoseR, iTween.Hash("rotation", new Vector3(0,0,5), "time", 0.5f,"delay", 0.0f,"islocal", true, "easetype", "linear", "loopType", "pingPong"));

        iTween.MoveTo(blue_fish, iTween.Hash("y", 171, "speed", 30, "movetopath", false, "islocal", true, "easetype", "linear", "loopType", "pingPong"));//鱼儿快跑之蓝鱼

        iTween.MoveTo(red_fish, iTween.Hash("y", 191, "speed", 30, "movetopath", false, "islocal", true, "easetype", "linear", "loopType", "pingPong"));//鱼儿快跑之红鱼

        iTween.MoveTo(meirenyu_gold, iTween.Hash("y", 301, "speed", 30, "movetopath", false, "islocal", true, "easetype", "linear", "loopType", "pingPong"));//美人鱼之元宝特效

        iTween.ScaleTo(SWK, iTween.Hash("scale", Vector3.one*0.9f, "speed", 0.3f, "movetopath", false, "islocal", true, "easetype", "linear", "loopType", "pingPong"));//大闹天宫之孙悟空

        float stepX = meiRenYuParent.rect.width / 5;
        //Debug.Log("stepX:" + stepX);
        yu0Path[0] = meiRenYu_Yu0S;
        yu1Path[0] = meiRenYu_Yu1S;
        yu2Path[0] = meiRenYu_Yu2S;
        for (int i1 = 1; i1 < 5;++i1)
        {
            yu1Path[i1] = new Vector3(meiRenYu_Yu1S.x - stepX * i1, meiRenYu_Yu1S.y, 0);
            yu2Path[i1] = new Vector3(meiRenYu_Yu2S.x + stepX * i1, meiRenYu_Yu2S.y, 0);
            if(i1 >= 3)
            {
                yu0Path[i1 - 2] = new Vector3(meiRenYu_Yu0S.x - (stepX*3/4) * (i1-2), meiRenYu_Yu0S.y, 0);
            }
        }

        iTween.MoveTo(meiRenYu_Yu0, iTween.Hash("path", yu0Path, "speed", 100,"movetopath",false,"islocal", true,"easetype","linear","loopType", "loop"/*,"oncomplete", "AnimationEnd","oncompletetarget", meiRenYu_Yu0*/));
        iTween.MoveTo(meiRenYu_Yu1, iTween.Hash("path", yu1Path, "speed", 100,"movetopath",false, "islocal", true,"easetype","linear","loopType", "loop"/*, "oncomplete", "AnimationEnd", "oncompletetarget", meiRenYu_Yu1*/));
        iTween.MoveTo(meiRenYu_Yu2, iTween.Hash("path", yu2Path, "speed", 100,"movetopath",false, "islocal", true,"easetype","linear","loopType", "loop"/*, "oncomplete", "AnimationEnd", "oncompletetarget", meiRenYu_Yu2*/));


        //汽泡
        for (int pp = 0; pp < 5; ++pp)
        {
            //起始位置
            float spX = -meiRenYuParent.rect.width / 2 + pp*stepX + 30;
            float spY = -meiRenYuParent.rect.height / 2 + 30;
            GameObject paopao = Instantiate(meiRenYu_paopao, meiRenYuParent);
            paopao.transform.localPosition = new Vector3(spX, spY, 0);
            paopao.transform.localScale = Vector3.one;


            //移动
            //随机速度
            int rs = Random.Range(50, 100);
            //终点位置
            float epx = (int)Random.Range(-meiRenYuParent.rect.width / 2 + 30, meiRenYuParent.rect.width / 2 - 30);
            float epY = (int)Random.Range(meiRenYuParent.rect.height / 2 - 30, meiRenYuParent.rect.height / 2 - 50);

            iTween.MoveTo(paopao, iTween.Hash("position", new Vector3(epx, epY, 0), "speed", rs, "movetopath", false, "islocal", true, "easetype", "linear", "loopType", "loop"));
        }


    }
	


	// Update is called once per frame
	void Update () {
        meiRenYuLightTrans.Rotate(new Vector3(0, 0, 1));
        liKuiPiYuLightTrans.Rotate(new Vector3(0, 0, 1));
        DaNaoTianGongLightTrans.Rotate(new Vector3(0, 0, 1));
 //       if(paopaoList.Count > 0)
 //       {
 //           for (int i = 0; i < paopaoList.Count; ++i)
 //           {
 //               //随机速度
 //               int rs = Random.Range(50, 100);
 //               //终点位置
 //               float epx = (int)Random.Range(meiRenYuParent.rect.height / 2 - 30, meiRenYuParent.rect.height / 2 - 50);
 //               float epY = (int)Random.Range(meiRenYuParent.rect.width / 2 - 30, meiRenYuParent.rect.width / 2 - 50);
 //               paopaoList[i].transform.Translate(epx * Time.deltaTime * rs, epY * Time.deltaTime * rs, 0);
 //           }
 //       }

	}



}
