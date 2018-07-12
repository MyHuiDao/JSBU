using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class mainMeiRenYuContrall : MonoBehaviour
{

    float needLockTime = 17;
    float needPauseTime = 12;
    float LockTime = 0;
    float pauseTime = 0;
    public Dictionary<int, string> otherautofish = new Dictionary<int, string>();//谁锁定了鱼，锁定了哪条鱼
    public bool otherAutoLock = false;//有人开启了自动锁定  
    public static mainMeiRenYuContrall instant;
    public bool is20019Pause = false;
    public bool is20020CancelPause = false;
    Animator BBD;
    Animator BBDD;
    Animator BK;
    public GameObject automaticFireMask = null;
    public GameObject automaticFire = null;
    public GameObject fishPauseMask = null;
    GameObject autoLockMask = null;
    public GameObject snow;
    bool isSnow = false;
    float sonwTime = 0;
    GameObject snowParent = null;

    float yuZhenTime = 0;//计时鱼阵时间
   
    float yuzhenDesign = 180;
    float yuzhenTime1 = 160;

    float noBingDong = 0;
    bool bingDongTime = false;
    
    void Start()
    {
        instant = this;
        snowParent = GameObject.Find("bingDong");
        automaticFireMask = GameObject.Find("automaticFireMask");
        automaticFire = GameObject.Find("automaticFire");
        fishPauseMask = GameObject.Find("fishPauseMask");
        autoLockMask = GameObject.Find("autoLockMask");
        GameObject.Find("fishPause").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (!contrall.instant().isCanClearFish)
            {
                WebButtonSendMessege.instant().releaseSkill("frozenFish", meiRenYuThreadDeal.gosWeiZhi);
            }
            else
            {
                GameObject.Find("noBingDong").transform.localScale = Vector3.one;
                bingDongTime = true;

            }
        });
        GameObject.Find("autoLock").GetComponent<Button>().onClick.AddListener(autoLockFish);
        GameObject.Find("automaticFire").GetComponent<Button>().onClick.AddListener(autoFire);
        GameObject.Find("automaticFireMask").GetComponent<Button>().onClick.AddListener(autoFire);
    }
    void Update()
    {

        //yuZhenTime += Time.deltaTime;
        //if (yuZhenTime >= yuzhenTime1)
        //{
        //    if (contrall.instant().isZhuJi)
        //    {
        //        WebButtonSendMessege.instant().yuZhenTime();
        //    }

        //    yuzhenTime1 += yuzhenDesign;
        //}

        if (GameController.Instance.isAutoLock)
        {
            if (LockTime >= needLockTime)
            {
                autoLockMask.transform.localScale = Vector3.zero;
                GameController.Instance.isAutoLock = false;
                GameController.Instance.isAutoFire = false;
                GameController.Instance.isXuanZhongFish = false;
                LockTime = 0;
                if (GameObject.Find("autoLockEffect(Clone)") != null)
                {
                    Destroy(GameObject.Find("autoLockEffect(Clone)").gameObject);

                }
                automaticFire.transform.localScale = Vector3.one;
                automaticFireMask.transform.localScale = Vector3.zero;
                WebButtonSendMessege.instant().skillOver("autoLockFish", meiRenYuThreadDeal.gosWeiZhi);

            }
            autoLockMask.GetComponent<Image>().fillAmount = 1 - LockTime / needLockTime;
            LockTime += Time.deltaTime;
        }

        if (FishMaker.instant.isPause)
        {
            if (pauseTime >= needPauseTime)
            {

                cancelPause(meiRenYuThreadDeal.gosWeiZhi);
            }
            fishPauseMask.GetComponent<Image>().fillAmount = 1 - pauseTime / needPauseTime;
            pauseTime += Time.deltaTime;
        }

        if (bingDongTime)
        {
            noBingDong += Time.deltaTime;
            if (noBingDong >= 2)
            {
                GameObject.Find("noBingDong").transform.localScale = Vector3.zero;
                bingDongTime = false;
                noBingDong = 0;
            }
        }
        if (is20019Pause)
        {
            pause();
            is20019Pause = false;
        }
        if (is20020CancelPause)
        {
            cancelPause();
            is20020CancelPause = false;
        }
        //雪花效果
        if (isSnow)
        {
            if (sonwTime == 0)
            {
                GameObject g= Instantiate(snow, snowParent.transform);
                int r=Random.Range(1, 7);
                if (r == 3) g.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                if (r == 5) g.transform.localScale = new Vector3(2f, 2f, 2f);
            }

            //sonwTime += Time.deltaTime;
            //if (sonwTime >= 0.2)
            //{
            //    sonwTime = 0;
            //}
        }


    }
    /// <summary>
    /// 取消暂停
    /// </summary>
    public void cancelPause(int target = -1)
    {

        FishMaker.instant.isPause = false;
        fishPauseMask.transform.localScale = Vector3.zero;        iTween.Resume(GameObject.Find("fishGroup"), true);
        pauseTime = 0;
        isSnow = false;
        GameObject.Find("bingDong").transform.Find("BBD").gameObject.SetActive(false);
        GameObject.Find("bingDong").transform.Find("BBDD").gameObject.SetActive(false);
        GameObject.Find("bingDong").transform.Find("BK").gameObject.SetActive(false);
        GameObject[] bingDongFish = GameObject.FindGameObjectsWithTag("Fish");
        for (int i = 0; i < bingDongFish.Length; i++)
        {
            bingDongFish[i].GetComponent<Animator>().enabled = true;
        }
        GameObject bingDongLockFish = GameObject.FindGameObjectWithTag("lockFish");
        if (bingDongLockFish != null)
        {
            bingDongLockFish.GetComponent<Animator>().enabled = true;
        }
        if (target == meiRenYuThreadDeal.gosWeiZhi)
        {
            WebButtonSendMessege.instant().skillOver("frozenFish");
        }


    }
    /// <summary>
    /// 暂停鱼的游动
    /// </summary>
    public void pause(int target = -1)
    {
        buYuMusicContral.instant.allYinXiao[4].Play();
        FishMaker.instant.isPause = true;


        iTween.Pause(GameObject.Find("fishGroup"), true);

        GameObject[] bingDongFish = GameObject.FindGameObjectsWithTag("Fish");
        for (int i = 0; i < bingDongFish.Length; i++)
        {
            bingDongFish[i].GetComponent<Animator>().enabled = false;
        }
        GameObject bingDongLockFish = GameObject.FindGameObjectWithTag("lockFish");
        if (bingDongLockFish != null)
        {
            bingDongLockFish.GetComponent<Animator>().enabled = false;
        }
        GameObject.Find("bingDong").transform.Find("BBD").gameObject.SetActive(true);
        GameObject.Find("bingDong").transform.Find("BBDD").gameObject.SetActive(true);
        GameObject.Find("bingDong").transform.Find("BK").gameObject.SetActive(true);
        isSnow = true;
    }
    /// <summary>
    /// 自动锁定
    /// </summary>
    public void autoLockFish()
    {

        GameController.Instance.isAutoLock = true;
        autoLockMask.transform.localScale = Vector3.one;

    }


    /// <summary>
    /// 自动发炮
    /// </summary>
    void autoFire()
    {

        GameController.Instance.isAutoFire = !GameController.Instance.isAutoFire;
        if (GameController.Instance.isAutoFire)
        {
            automaticFire.transform.localScale = Vector3.zero;
            automaticFireMask.transform.localScale = Vector3.one;

        }
        else
        {
            automaticFire.transform.localScale = Vector3.one;
            automaticFireMask.transform.localScale = Vector3.zero;

        }

    }
}
