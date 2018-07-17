using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此类用来控制捕鱼音乐
/// </summary>
public class buYuMusicContral : MonoBehaviour
{

    public static buYuMusicContral instant = null;
   
    public AudioSource backMusic;
    public AudioSource[] allYinXiao;
    private string music_volumn;
    private string music_effects;
    public GameObject specialFish1Image;
    public GameObject specialFish2Image;
    public GameObject specialFish3Image;
    // Use this for initialization
    void Start()
    {
        instant = this;
        allYinXiao = new AudioSource[24];
       
      


        allYinXiao[0] = GameObject.Find("fireMusic").GetComponent<AudioSource>();
        allYinXiao[1] = GameObject.Find("huanPaoMusic").GetComponent<AudioSource>();
        allYinXiao[2] = GameObject.Find("getScoreMusic").GetComponent<AudioSource>();
        allYinXiao[3] = GameObject.Find("pressKeyMusic").GetComponent<AudioSource>();
        allYinXiao[4] = GameObject.Find("bingDongMusic").GetComponent<AudioSource>();
        allYinXiao[5] = GameObject.Find("specialFish1").GetComponent<AudioSource>();
        allYinXiao[6] = GameObject.Find("specialFish2").GetComponent<AudioSource>();
        allYinXiao[7] = GameObject.Find("specialFish3").GetComponent<AudioSource>();
        allYinXiao[8] = GameObject.Find("specialFish4").GetComponent<AudioSource>();
        allYinXiao[9] = GameObject.Find("specialFish5").GetComponent<AudioSource>();
        allYinXiao[10] = GameObject.Find("specialFish6").GetComponent<AudioSource>();
        allYinXiao[11] = GameObject.Find("specialFish7").GetComponent<AudioSource>();
        allYinXiao[12] = GameObject.Find("specialFish8").GetComponent<AudioSource>();
        allYinXiao[13] = GameObject.Find("specialFish9").GetComponent<AudioSource>();
        allYinXiao[14] = GameObject.Find("specialFish10").GetComponent<AudioSource>();
        allYinXiao[15] = GameObject.Find("specialFish11").GetComponent<AudioSource>();
        allYinXiao[16] = GameObject.Find("specialFish12").GetComponent<AudioSource>();
        allYinXiao[17] = GameObject.Find("specialFish13").GetComponent<AudioSource>();
        allYinXiao[18] = GameObject.Find("specialFish14").GetComponent<AudioSource>();
        allYinXiao[19] = GameObject.Find("specialFish15").GetComponent<AudioSource>();
        allYinXiao[20] = GameObject.Find("specialFish16").GetComponent<AudioSource>();
        allYinXiao[21] = GameObject.Find("specialFish17").GetComponent<AudioSource>();
        allYinXiao[22] = GameObject.Find("specialFish18").GetComponent<AudioSource>();
        allYinXiao[23] = GameObject.Find("specialFish19").GetComponent<AudioSource>();
        openMusic();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openMusic()
    {
        music_volumn = PlayerPrefs.GetString("yinyue");
        music_effects = PlayerPrefs.GetString("yinxiao");
        if (getMeiRenYuArea.buyuGame == 0)
        {
            if (backMusic == null)
            {
                GameObject.Find("musicContra").transform.Find("backMusic1").gameObject.SetActive(true);
                backMusic = GameObject.Find("backMusic1").GetComponent<AudioSource>();
            }
        }
        else if (getMeiRenYuArea.buyuGame == 1)
        {
            if (backMusic == null)
            {
                GameObject.Find("musicContra").transform.Find("backMusic2").gameObject.SetActive(true);
                backMusic = GameObject.Find("backMusic2").GetComponent<AudioSource>();
            }
        }
        else
        {
            if (backMusic == null)
            {
                GameObject.Find("musicContra").transform.Find("backMusic3").gameObject.SetActive(true);
                backMusic = GameObject.Find("backMusic3").GetComponent<AudioSource>();
            }
        }
        if (Music_Control.yinYueDaoXiao == 0)
        {
            backMusic.mute = true;

        }
        else
        {
            backMusic.mute = false;
            backMusic.volume = Music_Control.yinYueDaoXiao;
            backMusic.Play();
        }
        if (Music_Control.yinXiaoDaoXiao == 0)//改音效大小
        {
            for (int i = 0; i < 23; i++)
            {
                allYinXiao[i].mute = true;
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                allYinXiao[i].mute = false;
                if (i == 0)
                {
                    allYinXiao[i].volume = Music_Control.saveYinXiao / 1.5f;
                    Debug.Log("allYinXiao[i].volume==============:"+allYinXiao[i].volume);
                }
                else
                {
                    allYinXiao[i].volume = Music_Control.yinXiaoDaoXiao;
                }

            }
        }
    }
}
