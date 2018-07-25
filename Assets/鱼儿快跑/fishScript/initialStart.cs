using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该类初始化start界面
/// </summary>
public class initialStart : MonoBehaviour
{
    //public static  GameObject yuerStartPrefab=null;
    //public static GameObject prepareAndjieSunaPrefab = null;
    //public static GameObject backgroundPrefab = null;
    // Use this for initialization
    public static Dictionary<string, int> touzhuEquit = new Dictionary<string, int>();
    public static initialStart instance = null;
    public static Camera maincamera = null;
    //public static GameObject yuerStartPrefab = null;//鱼儿快跑预制体
    public AudioSource[] allYinXiao;
    private void Awake()
    {
        //yuerStartPrefab = Resources.Load("yuErKuaiPao/start") as GameObject;
    }
    void Start()
    {
        instance = this;
        //yuerStartPrefab = Resources.Load("yuErKuaiPao/start") as GameObject;
        //prepareAndjieSunaPrefab= Resources.Load("yuErKuaiPao/prepareAndjieSuna") as GameObject;
        //backgroundPrefab = Resources.Load("yuErKuaiPao/background") as GameObject;
        Instantiate(/*m_slider.*//*yuerStartPrefab*/ResouseManager.Instance.KPSTARP);
        saveDate.cameraPos= GameObject.Find("Camera").transform.position;
        saveDate.cameraFllowMePos = GameObject.Find("cameraFllowMe").transform.position;

         maincamera= GameObject.Find("Main Camera").GetComponent<Camera>();

        string[] s = new string[4];
        //为第一次排名设定个假数据
        for (int i = 0; i < 4; i++)
        {
           s[i] = (i+1).ToString();
        }
        saveDate.fishRank = s;


        initialMusic();

        if (touzhuEquit.Count == 0)
        {
            touzhuEquitAdd();
        }
      
        //添加声音
        Music_Control.music_effect(allYinXiao[8]);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void initialMusic()
    {
        allYinXiao = new AudioSource[9];
      
        allYinXiao[0] = GameObject.Find("fishCry1").GetComponent<AudioSource>();
        allYinXiao[1] = GameObject.Find("fishCry2").GetComponent<AudioSource>();
        allYinXiao[2] = GameObject.Find("fishCry3").GetComponent<AudioSource>();
        allYinXiao[3] = GameObject.Find("fishCry4").GetComponent<AudioSource>();
        allYinXiao[4] = GameObject.Find("gameStart").GetComponent<AudioSource>();
        allYinXiao[5] = GameObject.Find("startMusic").GetComponent<AudioSource>();
        allYinXiao[6] = GameObject.Find("gameStart1").GetComponent<AudioSource>();
        allYinXiao[7] = GameObject.Find("gameStart2").GetComponent<AudioSource>();
        allYinXiao[8] = GameObject.Find("btnClickAudio").GetComponent<AudioSource>();
     
    }


    void touzhuEquitAdd()
    {
        touzhuEquit.Add("1,2", 1);
        touzhuEquit.Add("2,1", 2);
        touzhuEquit.Add("3,1", 3);
        touzhuEquit.Add("4,1", 4);
        touzhuEquit.Add("1,3", 5);
        touzhuEquit.Add("2,3", 6);
        touzhuEquit.Add("3,2", 7);
        touzhuEquit.Add("4,2", 8);
        touzhuEquit.Add("1,4", 9);
        touzhuEquit.Add("2,4", 10);
        touzhuEquit.Add("3,4", 11);
        touzhuEquit.Add("4,3", 12);
        touzhuEquit.Add("1", 13);
        touzhuEquit.Add("2", 14);
        touzhuEquit.Add("3", 15);
        touzhuEquit.Add("4", 16);

    }

}
