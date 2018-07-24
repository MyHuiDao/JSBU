using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResouseManager
{

    static readonly ResouseManager instance = new ResouseManager();

    //版本更新资源
    GameObject VersionUpdateP;
    //用户协议
    //GameObject UserProtocolP;
    //加载中
    GameObject loadP;
    //加载失败
    GameObject loadFailP;
    //大厅Hall
    //GameObject Hall;

    //资源捕鱼加载
    GameObject[] selectAreas = new GameObject[3];//0美人鱼  1李逵鱼   2大闹天宫
    GameObject[] MainScenes = new GameObject[3];
    //比赛rank
    Sprite[] rankSprites = new Sprite[3];

    //加载鱼儿快跑资源
    GameObject kp_startP;
    GameObject kp_prepareAndjieSunaP;
    GameObject kp_backgroundP;

    //话费充值卡Sprite
    Sprite[] huafeiSprites = new Sprite[4];
    //金币图标Sprite
    Sprite[] jinbiSprites = new Sprite[8];
    //默认头像Sprite
    Sprite headSprite;
    //water资源
    Texture[] waterTextures = new Texture[32];

    public static ResouseManager Instance
    {
        get { return instance; }
    }


    public void Start () {
        VersionUpdateP = LoadAesset("myPrefabs/Fish/LoadRes/VersionUpdateUI") as GameObject;
        loadP = LoadAesset("Rank/load") as GameObject;
        loadFailP = LoadAesset("Rank/loadFail") as GameObject;
        for (int i = 0; i < 3; ++i)
        {
            selectAreas[i] = LoadAesset("myPrefabs/meiRenYu/selectArea" + i) as GameObject;
            MainScenes[i] = LoadAesset("myPrefabs/meiRenYu/MainScene" + i) as GameObject;
            rankSprites[i] = LoadSprite("Pay_image/rank_" + (i + 1));
        }

        kp_startP = LoadAesset("yuErKuaiPao/start") as GameObject;
        kp_prepareAndjieSunaP = LoadAesset("yuErKuaiPao/prepareAndjieSuna") as GameObject;
        kp_backgroundP = LoadAesset("yuErKuaiPao/background") as GameObject;

        for (int j = 0; j < huafeiSprites.Length; ++j)
        {
            huafeiSprites[j] = LoadSprite("Pay_image/" + ((j + 1) * 100).ToString()) as Sprite;
        }
        for (int k = 0; k < jinbiSprites.Length; ++k)
        {
            jinbiSprites[k] = LoadSprite("Pay_image/bi_" + (k + 1).ToString()) as Sprite;
        }

        headSprite = LoadSprite("Rank/cat");

        for (int w = 0; w < waterTextures.Length; ++w)
        {
            waterTextures[w] = LoadAesset("Water/waterWaveItem_" + w) as Texture;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadAessets()
    {

    }

    UnityEngine.Object LoadAesset(string path)
    {
        try
        {
            UnityEngine.Object _obj = Resources.Load(path);
            //Debug.LogError(_obj.name);
            return _obj;
        }
        catch (Exception e)
        {
            Debug.LogError(e);

        }
        return null;
    }

    Sprite LoadSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }


    public void UnLoadObj(UnityEngine.Object _obj)
    {
        Resources.UnloadAsset(_obj);

        Resources.UnloadUnusedAssets();
    }


    public GameObject VESIONUPDATEP{

        get { return VersionUpdateP; }
    }

    public GameObject LOADP
    {
        get { return loadP; }
    }

    public GameObject LOADFAILP
    {
        get { return loadFailP; }
    }

    public GameObject SELECTAREA
    {
        get { return selectAreas[getMeiRenYuArea.buyuGame]; }
    }

    public GameObject MAINSCENE{
        get { return MainScenes[getMeiRenYuArea.buyuGame]; }
    }

    public GameObject KPSTARP
    {
        get { return kp_startP; }
    }

    public GameObject KPPREPAREANDJIESUNAP
    {
        get { return kp_prepareAndjieSunaP; }
    }

    public GameObject KPBACKGROUNDP
    {
        get { return kp_backgroundP; }
    }

    public Sprite HEADSPRITE
    {
        get { return headSprite; }
    }

    public Sprite[] RANKSPRITES
    {
        get {return rankSprites; }
    }

    public Sprite[] HUAFEISPRITES
    {
        get { return huafeiSprites; }
    }
    public Sprite[] JINBISPRITES{
        get { return jinbiSprites; }
    }

    public Texture[] WATERTEXTURES{
        get { return waterTextures; }
    }
}
