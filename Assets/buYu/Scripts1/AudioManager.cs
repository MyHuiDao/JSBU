
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * 音效和bgm控制类
 * 挂在各个scene的ScriptHolder上
 * */
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            return _instance;
        }
    }
  



    private string music_volumn;
    private string music_effects;
    GameObject yinYueOnOff;
    GameObject yinXiaoOnOff;
    
    void Awake()
    {
        _instance = this;
       
    }
    private void Start()
    {
        yinYueOnOff = GameObject.Find("yinYueOnOff");
        yinXiaoOnOff = GameObject.Find("yinXiaoOnOff");
        
        if (Music_Control.yinYueDaoXiao==0 ?yinYueOnOff .GetComponent<Switch>().isOn = true :yinYueOnOff.GetComponent<Switch>().isOn = false);
        if (Music_Control.yinXiaoDaoXiao==0 ? yinXiaoOnOff.GetComponent<Switch>().isOn = true : yinXiaoOnOff.GetComponent<Switch>().isOn = false);
        yinYueOnOff.GetComponent<Switch>().onValueChanged.AddListener((m) => SwitchMuteState(yinYueOnOff.GetComponent<Switch>().isOn));
        yinXiaoOnOff.GetComponent<Switch>().onValueChanged.AddListener((m) => SwitchMuteState1(yinXiaoOnOff.GetComponent<Switch>().isOn));


    }
    /// <summary>
    /// 音乐控制
    /// </summary>
    /// <param name="isOn"></param>
    public void SwitchMuteState(bool isOn)
    {
        if (!isOn)
        {
            buYuMusicContral.instant.backMusic.mute=false;
            Music_Control.mute = true;
            Music_Control.yinYueDaoXiao = buYuMusicContral.instant.backMusic.volume;
        }
        else
        {
            buYuMusicContral.instant.backMusic.mute=true;
            Music_Control.mute = false;
            Music_Control.yinYueDaoXiao = 0;
          
        }

        buYuMusicContral.instant.openMusic();
    }
    /// <summary>
    /// 音效控制
    /// </summary>
    /// <param name="isOn"></param>
    public void SwitchMuteState1(bool isOn)
    {
        if (!isOn)
        {
            for (int i = 0; i < buYuMusicContral.instant.allYinXiao.Length; i++)
            {
               
                buYuMusicContral.instant.allYinXiao[i].mute = false;
               
            }
            Music_Control.yinXiaoDaoXiao = buYuMusicContral.instant.allYinXiao[0].volume;
            if (Music_Control.yinXiaoDaoXiao == 0) {
                Music_Control.yinXiaoDaoXiao = 0.5f;
            }
            Music_Control.effect_mute = true;
        }
        else
        {
            for (int i = 0; i < buYuMusicContral.instant.allYinXiao.Length; i++)
            {
                buYuMusicContral.instant.allYinXiao[i].mute = true;
                
            }
            Music_Control.effect_mute = false;
            Music_Control.yinXiaoDaoXiao = 0;

        }
        buYuMusicContral.instant.openMusic();
    }

   

    //public void PlayEffectSound(AudioClip clip)
    //{
    //    if (!isMute)
    //    {
    //        AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, -5));	//播放音效
    //    }
    //}
    private void Update()
    {
       // print(isMute);
    }
}
