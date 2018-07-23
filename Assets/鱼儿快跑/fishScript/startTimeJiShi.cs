using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startTimeJiShi : MonoBehaviour
{
    //public static GameObject yuerbackgroundPrefab = null;//鱼儿快跑预制体  
    //public static GameObject yuerPrepareAndjieSunaPrefab = null;//鱼儿快跑预制体
    public static startTimeJiShi instance = null;
    public static string[] fishRank;
    public   bool isStartJiShi = false;
    Text jishiText;
    float time = 0;
    public  int daoJiShi ;
    public bool isCanDrag = true;

    private void Awake()
    {
        //yuerPrepareAndjieSunaPrefab = Resources.Load("yuErKuaiPao/prepareAndjieSuna") as GameObject;
        //yuerbackgroundPrefab = Resources.Load("yuErKuaiPao/background") as GameObject; 
    }

    public bool isToPrepare = false;
    void Start()
    {
      
        instance = this;
        GameObject.Find("roomText").GetComponent<Text>().text = saveDate.roomID;
        if (saveDate.roomPeopleNum <= 10)
        {
            GameObject.Find("personNum").GetComponent<Text>().text = "10";
        }
        else
        {
            GameObject.Find("personNum").GetComponent<Text>().text = saveDate.roomPeopleNum.ToString();
        }
      

        initialStart.instance.allYinXiao[5].Play();
        fishRank = saveDate.fishRank;
        GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = initialStart.maincamera; // Camera.main;     
        jishiText = GameObject.Find("startTimeJiShi").transform.Find("Text").GetComponent<Text>();




       
    }



    private void FixedUpdate()
    {
        if (isStartJiShi)
        {

            time += Time.deltaTime;

            if (time >= 1)
            {
               
                jishiText.text = (daoJiShi - 1).ToString();
                daoJiShi--;
                if (jishiText.text == "3")
                {
                    GameObject.Find("start").transform.Find("startBigJiShi").gameObject.SetActive(true);
                    isCanDrag = false;//停止下注
                    saveDate.money = GameObject.Find("moneyText").GetComponent<Text>().text;
                }
                if (jishiText.text == "0"|| jishiText.text == "-1")
                {


                    isStartJiShi = false;

                }
                time = 0;
            }

        }
    }
    void Update()
    {
        if (GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera == null)
        {
            GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = initialStart.maincamera;

        }
   
       

        if (isToPrepare)
        {
            initialStart.instance.allYinXiao[5].Pause();
            if (!isStartJiShi)
            {
                Destroy(GameObject.Find("start(Clone)").gameObject);
                Instantiate(/*m_slider.*//*yuerPrepareAndjieSunaPrefab*/weiXinLoad.instance.kp_prepareAndjieSunaP);
                Music_Control.music_effect(initialStart.instance.allYinXiao[8]);
                Instantiate(/*m_slider.*//*yuerbackgroundPrefab*/weiXinLoad.instance.kp_backgroundP);
                isToPrepare = false;
            }
        }


    }

   
}
