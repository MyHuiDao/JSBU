using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mosframe;
using CClient;

public class m_slider : MonoBehaviour
{

    //public AudioSource loadingMusic;
    public static m_slider instance;
    [SerializeField] Image m_image;
    [SerializeField] CanvasGroup selt_ui;
    private AsyncOperation jiazai2;
    private float speed = 1.5f;
    private float load;
    private string sceneName;
    private AsyncOperation operation;
    
    public static GameObject buYuPrefab = null;//提前加载捕鱼预制体\
    public static GameObject yuerStartPrefab = null;//鱼儿快跑预制体
    public static GameObject yuerPrepareAndjieSunaPrefab = null;//鱼儿快跑预制体
    public static GameObject yuerbackgroundPrefab = null;//鱼儿快跑预制体                                              
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {


    }

    void Update()
    {


        if (operation == null)
        {
            return;
        }

        load = operation.progress;
        if (operation.progress >= 0.9f)
        {
            load = 1f;
        }
        {
            if (load != m_image.fillAmount)
            {
                m_image.fillAmount = Mathf.Lerp(m_image.fillAmount, load, Time.deltaTime * speed);
                if (Mathf.Abs(m_image.fillAmount - load) < 0.01f)
                {
                    m_image.fillAmount = load;
                }

            }
        }

        if ((int)(m_image.fillAmount * 100) >= 95)
        {
           
            operation.allowSceneActivation = true;


        }
    }

    void OnEnable()
    {
        load = 0;
        try
        {
            sceneName = SceneManager.GetActiveScene().name;
        }
        catch (UnityEngine.UnityException e)
        {
            Debug.Log("sceneName为空:" + e);
        }
    }

    public void GetScene(AsyncOperation ope, bool joinBuYu, bool joinYuEr)
    {
        GameObject.Find("Main Camera").GetComponent<AudioSource>().mute = true;


        if (joinBuYu)
        {
           //switch (getMeiRenYuArea.buyuGame)
            //{
            //    case 0:
            //        buYuPrefab = netConnect.buyu0;
            //        break;
            //    case 1:
            //        buYuPrefab = netConnect.buyu1;
            //        break;
            //    case 2:
            //        buYuPrefab = netConnect.buyu2;
            //        break;
            //}
            buYuPrefab = Resources.Load("myPrefabs/meiRenYu/MainScene" + getMeiRenYuArea.buyuGame) as GameObject;//取决于点击哪款游戏
            //Debug.Log("buYuPreFabName:" + buYuPrefab.name);
           
        }
        if (joinYuEr)
        {
            if (yuerbackgroundPrefab == null)
            {
                Debug.Log("加载资源预制体");
                yuerStartPrefab = Resources.Load("yuErKuaiPao/start") as GameObject;
                //yuerStartPrefab = break_line.yuer0;
            }
            if (yuerPrepareAndjieSunaPrefab == null)
            {
                yuerPrepareAndjieSunaPrefab = Resources.Load("yuErKuaiPao/prepareAndjieSuna") as GameObject;
                //yuerPrepareAndjieSunaPrefab= break_line.yuer1;
            }
               
            if (yuerbackgroundPrefab == null)
            {
                 yuerbackgroundPrefab = Resources.Load("yuErKuaiPao/background") as GameObject; 
                //yuerbackgroundPrefab= break_line.yuer2;
            }
               
        }
        operation = ope;

        operation.allowSceneActivation = false;

    }
}


