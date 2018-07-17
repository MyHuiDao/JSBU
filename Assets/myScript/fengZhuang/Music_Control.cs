using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct HallBtn
{
    public bool Select;
    public GameObject btnObj;
}
public class Music_Control : MonoBehaviour
{
    Slider back_groud_music;//音量控制器
    Slider back_groud_effect;//音量控制器
    AudioSource main_music;
    public static AudioSource effect_music;
    public static bool mute=true;
    public static bool effect_mute=true;
    public static float yinYueDaoXiao = 0.5f;
    public static float yinXiaoDaoXiao = 0.5f;
    public static float saveYinYue = 0.5f;
    public static float saveYinXiao = 0.5f;
    Switch yinyueswitch;
    Switch yinxiaoswitch;
   public static Button[] button;

    // Use this for initialization
    void Start()
    {
        button = FindObjectsOfType(typeof(Button)) as Button[];
        back_groud_music = GameObject.Find("yinYue").GetComponent<Slider>();
        back_groud_effect = GameObject.Find("yinxiao").GetComponent<Slider>();
        main_music = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        effect_music = GameObject.Find("Music").GetComponent<AudioSource>();
        yinyueswitch = GameObject.Find("yinYueOnOff").GetComponent<Switch>();
        yinxiaoswitch = GameObject.Find("yinXiaoOnOff").GetComponent<Switch>();
        
        back_groud_music.value = yinYueDaoXiao;
        back_groud_effect.value = yinXiaoDaoXiao;
        main_music.volume = yinYueDaoXiao;
        effect_music.volume = yinXiaoDaoXiao;
        if (yinYueDaoXiao != 0)
        {
            yinyueswitch.isOn = false;
        }
        if (yinXiaoDaoXiao != 0)
        {
            yinxiaoswitch.isOn = false;
        }
      
        yinxiaoswitch.onValueChanged.AddListener((e) => close_effect(effect_music, back_groud_effect));
        yinyueswitch.onValueChanged.AddListener((m) => close_volumn(main_music, back_groud_music));
      
        music_effect(effect_music); 
    }
    // Update is called once per frame
    void Update()
    {
        control("Main Camera", back_groud_music);
        control("Music", back_groud_effect);
        if (back_groud_music.value == 0)
        {
            yinyueswitch.isOn = true;
        }
        else
        {
            yinyueswitch.isOn = false ;
        }
        if (back_groud_effect.value == 0)
        {
            yinxiaoswitch.isOn = true;
        }
        else
        {
            yinxiaoswitch.isOn = false;
        }
    }
    void control(string name, Slider slider)
    {
        if (name == "Main Camera")
        {
            main_music.volume = slider.value;
            yinYueDaoXiao = main_music.volume;
        }
        else if (name == "Music")
        {
            effect_music.volume = slider.value;
            yinXiaoDaoXiao = effect_music.volume;
        }
    }

    /// <summary>
    /// 音量控制
    /// </summary>
    /// <param name="music"></param>
    /// <param name="_slider"></param>
    void close_volumn(AudioSource music, Slider _slider)
    {
        if (mute == true)
        {
            music.volume = 0;
            mute = false;
            saveYinYue = _slider.value;
            _slider.value = 0;
            yinYueDaoXiao = 0;    
        }
        else
        {
            if(saveYinYue <= 0)
            {
                music.volume = 0.5f;
                _slider.value = 0.5f;
                yinYueDaoXiao = 0.5f; 
            }else{
                music.volume = saveYinYue;
                _slider.value = saveYinYue;
                yinYueDaoXiao = saveYinYue; 
            }
            music.volume = saveYinYue;
                _slider.value = saveYinYue;
                yinYueDaoXiao = saveYinYue; 
            mute = true; 
        }
    }
    /// <summary>
    /// 音效控制
    /// </summary>
    /// <param name="music"></param>
    /// <param name="_slider"></param>
    void close_effect(AudioSource music, Slider _slider)
    {
        if (effect_mute == true)
        {
            music.volume = 0;
            effect_mute = false;
            saveYinXiao = _slider.value;
            _slider.value = 0;
            yinXiaoDaoXiao = 0;
        }
        else
        {
            if (saveYinXiao <= 0)
            {
                music.volume = 0.5f;
                _slider.value = 0.5f;
                yinXiaoDaoXiao = 0.5f;
            }
            else
            {
                music.volume = saveYinXiao;
                _slider.value = saveYinXiao;
                yinXiaoDaoXiao = saveYinXiao;
            }
            effect_mute = true;    
        }
    }
    public static void music_effect(AudioSource music)
    {
        Button[] buttons;
        buttons= FindObjectsOfType(typeof(Button)) as Button[];
        foreach (Button add in buttons)
        {
            add.onClick.AddListener(() =>
            {
                music.Play();
            });
        }
        Toggle[] toggle;
        toggle = FindObjectsOfType(typeof(Toggle)) as Toggle[];
        foreach (Toggle add in toggle)
        {
            add.onValueChanged.AddListener((bool value) =>
            {
                music.Play();
            });
        }
    } 
    /// <summary>
    /// 只点击一次
    /// </summary>
    /// <param name="b"></param>
    void close_only(GameObject b)
    {
        for (int i = 0; i < button.Length; ++i)
        {
            button[i].enabled = true;
        }
        b.GetComponent<Button>().enabled = false;
    }
}