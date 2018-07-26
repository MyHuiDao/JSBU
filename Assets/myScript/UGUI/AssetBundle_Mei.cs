using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class AssetBundle_Mei : MonoBehaviour
{
    bool isDone;
    public static AssetBundle_Mei instanced = null;
    public int changci;
    public static int buyu_changci;
    Action load;
    public    static  GameObject[] obj;
    AssetBundleCreateRequest request = null;
    Slider slider3;
    public Text text3;
    Text text4;
    HttpDownLoad http3;
    bool http3Finish = false;
    bool file;
    public string fileName;
    public int down_load;
    bool start_download;
    string savePath = string.Empty;
    string RemoteIP =
//#if UNITY_ANDROID
    "http://jinshayugang.com/Android/";
//#elif UNITY_IOS
//        "http://jinshayugang.com/Ios/";
//#endif
    public static bool returnHall;
    private string path;
    private GameObject Middle;
    void Awake()
    {
        down_load = PlayerPrefs.GetInt("meiren");
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        savePath = Application.streamingAssetsPath + "/";
#elif UNITY_ANDROID||UNITY_IOS
        savePath= Application.persistentDataPath+"/";
#endif
        path = savePath + fileName;
        if (File.Exists(path)&& down_load==1)
        {
            file = true;
        }
        else
        {
            file = false;
        }
        Middle = GameObject.Find("middle");
    }

    void Start()
    {
        instanced = this;
        GetComponent<Button>().onClick.AddListener(() => downloads());
    }

    void downloads()
    {

    GameObject middleLastObj = Middle.transform.GetChild(Middle.transform.childCount - 1).gameObject;
    if (middleLastObj == gameObject)
    {
        if (file==false)
        {
            http3 = new HttpDownLoad();//下载bundle并LoadScene
            http3.DownLoad(RemoteIP + fileName, savePath, fileName, http3DownLoadState);//BUndle下载优先级最高    
        }
        else
        {
            load += func;
                if (file == true)
                {
                if (returnHall == false)
                {
                    func();
                }
                else
                {
                    GameObject load = Instantiate(gameContrall.instant.Loading, GameObject.Find("Canvas").transform) as GameObject;
                    gameContrall.instant.catchFish(changci, gameObject);
                }
                    
                }
        }
        
        start_download = true;
        }

    }
   

    void http3DownLoadState()
    {
        http3Finish = true;
        LoadLevel();
    }
    void LoadLevel()
    {
        isDone = true;
    }
    string url = "";
    void Update()
    {
        if (start_download == true)
        {
            if (file==false)
            {
                ShowProgress();
            }      
        }     
    }

    /// <summary>
    /// 显示进度条
    /// </summary>
    void ShowProgress()
    {
        if (!http3.isDone)
        {
            GetComponent<Button>().enabled = false;
            transform.GetChild(7).GetComponent<CanvasGroup>().alpha = 1;
            transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount =1- http3.progress;
            float progress = 1-transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount;
          
            if (progress != 0)
            {
                if (progress * 100 < 10)
                {
                    string load_procerss = (progress * 100).ToString().Substring(0, 1);
                    text3.text = load_procerss + "%";
                }
                else if (progress * 100 < 100)
                {
                    string load_procerss = (progress * 100).ToString().Substring(0, 2);
                    text3.text = load_procerss + "%";
                }
            }
        }
        else
        {
            GetComponent<Button>().enabled = true;
            file = true;
            text3.text = "100%";
            down_load = 1;
            PlayerPrefs.SetInt("meiren", down_load);
            transform.GetChild(7).GetChild(0).GetComponent<Image>().fillAmount = 0;
            transform.GetChild(7).GetComponent<CanvasGroup>().alpha = 0;
        
          
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }

    void func()
    {
        StartCoroutine(ABC());
    }
    IEnumerator ABC()
    {
        GameObject load = Instantiate(gameContrall.instant.Loading, GameObject.Find("Canvas").transform) as GameObject;
        request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        AssetBundle ab = request.assetBundle;
        obj = ab.LoadAllAssets<GameObject>();//加载出来放入数组中
        ab.Unload(false);
        returnHall = true;
        if (request.isDone == true)
        {
            gameContrall.instant.catchFish(changci, gameObject);
        }
    }
}
