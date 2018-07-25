using CClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


/**
 * 该类管理了整个游戏的逻辑和UI(也可以搞个负责UI的UIManager)
 * 挂在ScriptHolder上(没有像我之前那样挂在bg或者canvas上)
 * 如：
 * 	点击换枪操作，开火等
 **/

public class GameController : MonoBehaviour
{

    private static GameController _instance;	//单例模式
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }
    public static bool isJoinRoom = false;//判断是加载完否进入了房间  //UI组件，通过拖拽赋值的方式(命名查找的方式也可以)
    public Text[] oneShootCostText;		//当前每炮需要花费多少金币
    public Text[] goldText;//每个人拥有的金币数
    public Sprite isHaveGunShow = new Sprite();//没人的位置显示空
    public GameObject fire_effect1;
    public GameObject fire_effect2;
    public GameObject fire_effect3;
    public Color goldColor;
    public GameObject bgImage;               //背景  
    public Sprite[] bgSprites;          //背景图片的精灵
    int currentBg = 0;
    public Sprite ButtonP;
    public Sprite ButtonM;
    public Transform bgImageParent;
    public Transform bgBeforeParent;
    public Transform bulletHolder;      //子弹的容器
    public Transform fireEffectHolder;      //炮口特效的的容器
    public static int gosWeiZhi;//自己所在炮的位置
    public GameObject gunPanel0 = null;
    public GameObject gunPanel1 = null;
    public GameObject gunPanel2 = null;
    public GameObject gunPanel3 = null;
    Dictionary<int, GameObject[]> GosTarget = new Dictionary<int, GameObject[]>();//存放炮的对象，对应ID位置
    public static Dictionary<string, int> gunShapes = new Dictionary<string, int>();//炮的级别0-2
    public static Dictionary<string, int> bulletLevel = new Dictionary<string, int>();//子弹的等级0-8
    List<int> gosGoldShow = new List<int>();//每炮的花费
    public GameObject[] gunGos0;         //所有的枪的对象，枪的形状0-2
    public GameObject[] gunGos1;
    public GameObject[] gunGos2;
    public GameObject[] gunGos3;
    public GameObject[] bulletGos;      //枪的子弹数组,子弹形状0-8                                   
    bool BaddGosClass = false;
    bool BreduceGosClass = false;
    int currentDealTarget = 0;//当前处理对象
    int currentDealTargetBulletLevel = 0;//当前处理对象子弹级别
    bool isOpenFire = false;
    bool[] isUser = new bool[4];
    //判断是哪位用户返回的子弹信息
    returnBulletMsg[] bulletMsg = new returnBulletMsg[4];
    Dictionary<int, returnBulletMsg> BulletMsg = new Dictionary<int, returnBulletMsg>();//把返回的子弹信息保存在类里面，好做区分
    public Dictionary<string, GameObject>[] bulletDict = new Dictionary<string, GameObject>[4];//保存发射的子弹，string代表每个子弹ID，gameobject代表子弹对象
    Dictionary<int, Dictionary<string, GameObject>> userAndBullet = new Dictionary<int, Dictionary<string, GameObject>>();//用户对象对应子弹
    public static string roomID;
    public bool isAutoFire = false;
    public bool isAutoLock = false;
    public bool isXuanZhongFish = false;
    float ziDanFireTime = 0;//设置子弹间隔
    public GameObject lockFish = null;//自动锁定选中的鱼
    float fireZiDanTimeMid = 0.3f;//发射每发子弹的最大时间间隔
    int canceleAutoLockFish = 6;//当鱼离开界面时，取消自动锁定，根据轨迹点数变化而变化
    public GameObject autoLockEffect = null;//自动锁定标记
    public static List<returnBulletMsg> returnBullet = new List<returnBulletMsg>();
    public static bool lastBulletFire = true;
    public GameObject waveFinger;
    public GameObject waveFingerParent;

    public GameObject g;
    //public GameObject g1;

    void Awake()
    {

        _instance = this;
        Music_Control.music_effect(buYuMusicContral.instant.allYinXiao[23]);
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//防止手机黑屏
        g = new GameObject();
        //g1 = new GameObject();
        bulletMsgToDict();

        gosToDict();
        addGunflow();
        userAndBulletToDict();

    }

    void Update()
    {

        if (returnBullet.Count != 0)
        {
            openFire(returnBullet[0]._id, returnBullet[0]._gold, returnBullet[0]._angle, returnBullet[0].user, returnBullet[0]._bulletLevel, returnBullet[0]._SumGold);
            returnBullet.RemoveAt(0);
        }
        mouseCheck();//射线检测
        for (int i = 0; i < 4; i++)
        {
            if (isOpenFire && isUser[i])
            {
                lock (g)
                {
                    Fire(BulletMsg[i]._id, BulletMsg[i]._gold, BulletMsg[i]._bulletLevel, i, BulletMsg[i]._angle, BulletMsg[i]._SumGold);//发射子弹
                    isOpenFire = false;
                    isUser[i] = false;
                }

            }
        }

        if (BaddGosClass)
        {
            addGosClass1();
            BaddGosClass = false;
        }
        if (BreduceGosClass)
        {
            reduceGosClass1();
            BreduceGosClass = false;
        }
    }
    private void FixedUpdate()
    {
        isCanFire();//检测是否要发射子弹
    }
    void userAndBulletToDict()
    {
        for (int i = 0; i < 4; i++)
        {
            bulletDict[i] = new Dictionary<string, GameObject>();
            userAndBullet.Add(i, bulletDict[i]);

        }
    }
    void addGunflow()
    {

        GosTarget[gosWeiZhi][0].transform.parent.parent.transform.Find("GosClassButton").transform.Find("ButtonP").GetComponent<Button>().onClick.AddListener(OnButtonUp);//提高炮的等级
        GosTarget[gosWeiZhi][0].transform.parent.parent.transform.Find("GosClassButton").transform.Find("ButtonM").GetComponent<Button>().onClick.AddListener(OnButtonDown);//降低炮的等级
        GosTarget[gosWeiZhi][0].AddComponent<GunFollow>();
        GosTarget[gosWeiZhi][1].AddComponent<GunFollow>();
        GosTarget[gosWeiZhi][2].AddComponent<GunFollow>();
    }
    void bulletMsgToDict()
    {
        bulletMsg[0] = new returnBulletMsg();
        bulletMsg[1] = new returnBulletMsg();
        bulletMsg[2] = new returnBulletMsg();
        bulletMsg[3] = new returnBulletMsg();//四个炮台的子弹信息
        BulletMsg.Add(0, bulletMsg[0]);
        BulletMsg.Add(1, bulletMsg[1]);
        BulletMsg.Add(2, bulletMsg[2]);
        BulletMsg.Add(3, bulletMsg[3]);

    }
    /// <summary>
    /// 把炮添加到字典中
    /// </summary>
    void gosToDict()
    {
        GosTarget.Add(0, gunGos0);
        GosTarget.Add(1, gunGos1);
        GosTarget.Add(2, gunGos2);
        GosTarget.Add(3, gunGos3);
    }


    public void initialGold()
    {
        //显示每个人的金币数
        for (int i = 0; i < contrall.instant().userDict.Count; i++)
        {

            if (contrall.instant().everyOnegoldDict.ContainsKey(contrall.instant().userDict[i]))
            {

                goldText[i].text = contrall.instant().everyOnegoldDict[contrall.instant().userDict[i]].ToString();
                GameObject.Find("GunPanel" + i).transform.Find("GosClassButton").Find("ButtonP").GetComponent<Image>().sprite = ButtonP;
                GameObject.Find("GunPanel" + i).transform.Find("GosClassButton").Find("ButtonM").GetComponent<Image>().sprite = ButtonM;
            }

        }
        for (int i = 0; i < 2; i++)
        {
            oneShootCostText[i].text = (int.Parse(contrall.instant().addFileGoldNum)).ToString();
        }
        //显示每个人一炮花费的金币,不用时刻更新，只需加减炮的时候做改变
        for (int i = 0; i < contrall.instant().userDict.Count; i++)
        {

            if (bulletLevel.ContainsKey(contrall.instant().userDict[i]))
            {
                Transform g = GameObject.Find("GunPanel" + i).transform.Find("GunPosGroup");
                if (bulletLevel[contrall.instant().userDict[i]] >= 6)
                {

                    g.GetChild(2).gameObject.SetActive(true);
                    g.GetChild(1).gameObject.SetActive(false);
                    g.GetChild(0).gameObject.SetActive(false);
                }
                else if (bulletLevel[contrall.instant().userDict[i]] >= 3)
                {
                    g.GetChild(2).gameObject.SetActive(false);
                    g.GetChild(1).gameObject.SetActive(true);
                    g.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    g.GetChild(2).gameObject.SetActive(false);
                    g.GetChild(1).gameObject.SetActive(false);
                    g.GetChild(0).gameObject.SetActive(true);
                }

               
                oneShootCostText[i].text = ((bulletLevel[contrall.instant().userDict[i]]) * int.Parse(contrall.instant().addFileGoldNum)).ToString();
            }

        }


    }
    public static bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }


    /// <summary>
    /// 先点击按钮发送要制造子弹的消息，返回信息后才能开始把此子弹发射
    /// </summary>
    void isCanFire()
    {
        //点击了鼠标左键， 并且没有点击到UI上
        //**此处需要把背景的RaycastTarget去掉,否则IsPointerOverGameObject始终 = true

        if (ziDanFireTime > fireZiDanTimeMid && lastBulletFire)
        {


            if (meiRenYuThreadDeal.instant.yuzhenOpenFire)//鱼阵期间不让开火
            {
                if (isAutoFire)
                {
                    //Debug.Log("自动发炮");
                    ziDanFireTime = 0;
                    // Debug.Log("自动发炮");
                    lastBulletFire = false;//上一个子弹发射没接收到服务器消息，禁止发射下一个子弹
                    fireTimeJiShi.isCanFire = true;
                    WebButtonSendMessege.instant().openFire("20006", GunFollow.angle.ToString());//每发射一次炮弹，给服务器传一次角度，发射的是跟鼠标的角度
                }

                else if (Input.GetMouseButton(0))

                {


#if UNITY_IPHONE || UNITY_ANDROID


                    if (IsPointerOverGameObject())

#else

                  if (EventSystem.current.IsPointerOverGameObject())

#endif

                    {// Debug.Log("当前触摸在UI上");
                    }



                    else

                    {
                        //Debug.Log("实例化波纹");
                        //GameObject g = Instantiate(waveFinger, waveFingerParent.transform);
                        //g.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //Vector3 v = g.transform.position;
                        //v.z = 0;
                        //g.transform.position = v;
                        ziDanFireTime = 0;
                        lastBulletFire = false;//上一个子弹发射没接收到服务器消息，禁止发射下一个子弹
                        fireTimeJiShi.isCanFire = true;
                        WebButtonSendMessege.instant().openFire("20006", GunFollow.angle.ToString());//每发射一次炮弹，给服务器传一次角度，发射的是跟鼠标的角度

                    }
                }

            }

        }

        ziDanFireTime += Time.deltaTime;



    }


    /// <summary>
    /// 发射子弹
    /// </summary>
    /// <param name="_id">子弹id</param>
    /// <param name="_gold">金币</param>
    /// <param name="bulletClass">炮的等级</param>
    /// <param name="bulletGos">包含子弹形状的组合</param>
    /// <param name="gunGos">包含炮的形状的组合</param>
    /// <param name="gunShape">炮形状的等级</param>
    void Fire(string _id, int _gold, int _bulletLevel, int target, double _angle, string _sumGold)
    {
        _gold = 0 - _gold;
        // Debug.Log("得到服务器总数：" + _sumGold);
        //Debug.Log("-><color=#9400D3>" + "发炮消耗的金币:" + "</color>" + target + "对象:发炮消耗：" + _gold);

        setGoldText(target, _sumGold);

        int _gunShape = 0;
        GameObject fire_effect;
        if (_bulletLevel >= 1 && _bulletLevel <= 3)
        {
            gunShapes[contrall.instance.userDict[target]] = 0;
            _gunShape = 0;
            fire_effect = fire_effect1;
        }
        else if (_bulletLevel >= 4 && _bulletLevel <= 6)
        {
            gunShapes[contrall.instance.userDict[target]] = 1;
            _gunShape = 1;
            fire_effect = fire_effect2;
        }
        else
        {
            gunShapes[contrall.instance.userDict[target]] = 2;
            _gunShape = 2;
            fire_effect = fire_effect3;
        }
        buYuMusicContral.instant.allYinXiao[0].Play();//播放开火音效         
        GameObject fireEffect = Instantiate(fire_effect); //实例化发射子弹特效      
        fireEffect.transform.SetParent(fireEffectHolder, false);
        fireEffect.transform.position = GosTarget[target][_gunShape].transform.Find("FirePos1").transform.position;
        fireEffect.transform.rotation = GosTarget[target][_gunShape].transform.rotation;
        GameObject bullet = Instantiate(bulletGos[_bulletLevel]);//实例化子弹
        bullet.transform.SetParent(bulletHolder, false);
        bullet.transform.position = GosTarget[target][_gunShape].transform.Find("FirePos1").transform.position; //子弹的位置为每种炮中子弹的生成位置
                                                                                                                //在此处设置炮的回弹效果
        GosTarget[target][_gunShape].GetComponent<Animator>().speed = 1;

        if (gosWeiZhi == target)//自己的子弹
        {

            bullet.transform.rotation = GosTarget[target][_gunShape].transform.rotation; //子弹的方向(旋转角度)为炮的旋转角度
        }
        else//别人的炮和子弹旋转
        {

            GosTarget[target][_gunShape].transform.localRotation = Quaternion.Euler(0, 0, (float)_angle);
            bullet.transform.localRotation = Quaternion.Euler(0, 0, (float)_angle);
            //if (gosWeiZhi == 0 || gosWeiZhi == 1)
            //{
            //    bullet.transform.localRotation = Quaternion.Euler(0, 0, (float)_angle);
            //}
            //else
            //{
            //    bullet.transform.localRotation = Quaternion.Euler(0, 0, (float)_angle + 180);
            //}

        }
        //自己是子弹随炮旋转，别人是炮随子弹旋转
        bullet.GetComponent<BulletAttr>().id = _id;//把返回来的子弹消息id赋给此子弹   
        bullet.GetComponent<BulletAttr>().belongTarget = target;
        bullet.AddComponent<Ef_Move>().dir = Vector3.up;    //子弹的x轴正方向没有处理，故子弹的dir = Vector3.top
        bullet.GetComponent<Ef_Move>().speed = bullet.GetComponent<BulletAttr>().speed; //设置子弹的speed
        userAndBullet[target].Add(_id, bullet);//把子弹保存      
    }
    /**
     * 调大枪的威力
     **/
    public void OnButtonUp()
    {
        //buYuMusicContral.instant.allYinXiao[1].Play();
        WebButtonSendMessege.instant().addButtleClass();//发送加炮等级     
    }
    /// <summary>
    /// 操作炮的等级增加
    /// </summary>
    public void addGosClass(string _id, int _buttleLevel)
    {
        for (int i = 0; i < contrall.instant().userDict.Count; i++)
        {
            if (contrall.instant().userDict[i] == _id)
            {
                currentDealTargetBulletLevel = _buttleLevel;//这儿没有分对象处理，多个人同时加减炮可能出问题
                currentDealTarget = i;
            }
        }

        BaddGosClass = true;
    }
    public void addGosClass1()
    {
        oneShootCostText[currentDealTarget].text = (int.Parse(oneShootCostText[currentDealTarget].text) + int.Parse(contrall.instant().addFileGoldNum)).ToString();

        GosTarget[currentDealTarget][gunShapes[contrall.instance.userDict[currentDealTarget]]].SetActive(false);      //先把当前枪隐藏

        if (currentDealTargetBulletLevel >= 1 && currentDealTargetBulletLevel <= 3)
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 0;
        }
        else if (currentDealTargetBulletLevel >= 4 && currentDealTargetBulletLevel <= 6)
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 1;

        }
        else
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 2;

        }
        //AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);    //换枪音效
        //Instantiate(changeEffect);      //换枪特效    
        GosTarget[currentDealTarget][gunShapes[contrall.instance.userDict[currentDealTarget]]].SetActive(true);       //显示下一个档次的枪

    }

    /**
     * 调小枪的威力
     * */
    public void OnButtonDown()
    {
        buYuMusicContral.instant.allYinXiao[1].Play();
        WebButtonSendMessege.instant().reduceButtleClass();//发送减炮等级      
    }
    /// <summary>
    /// 操作炮的等级减小
    /// </summary>
    public void reduceGosClass(string _id, int _buttleLevel)
    {
        for (int i = 0; i < contrall.instant().userDict.Count; i++)
        {
            if (contrall.instant().userDict[i] == _id)
            {
                currentDealTargetBulletLevel = _buttleLevel;//这儿没有分对象处理，多个人同时加减炮可能出问题
                currentDealTarget = i;
            }
        }
        BreduceGosClass = true;
    }
    public void reduceGosClass1()
    {
        oneShootCostText[currentDealTarget].text = (int.Parse(oneShootCostText[currentDealTarget].text) - int.Parse(contrall.instant().addFileGoldNum)).ToString();


        GosTarget[currentDealTarget][gunShapes[contrall.instance.userDict[currentDealTarget]]].SetActive(false);        //先把当前枪隐藏

        if (currentDealTargetBulletLevel >= 1 && currentDealTargetBulletLevel <= 3)
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 0;
        }
        else if (currentDealTargetBulletLevel >= 4 && currentDealTargetBulletLevel <= 6)
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 1;

        }
        else
        {
            gunShapes[contrall.instance.userDict[currentDealTarget]] = 2;

        }
        //AudioManager.Instance.PlayEffectSound(AudioManager.Instance.changeClip);    //换枪音效
        //Instantiate(changeEffect);      //换枪特效     
        GosTarget[currentDealTarget][gunShapes[contrall.instance.userDict[currentDealTarget]]].SetActive(true);      //显示下一个档次的枪


    }

    /// <summary>
    /// 以下写法是因为出了分线程的错误
    /// </summary>

    //string __id;
    //int __gold;//子弹所花金币
    /// <summary>
    /// 得到开炮用户的子弹信息
    /// </summary>
    /// <param name="_id">子弹ID</param>
    /// <param name="_gold">一炮所花金币</param>
    /// <param name="_angle">角度</param>
    /// <param name="_userID">用户ID</param>
    /// <param name="_bulletLevel">子弹等级</param>
    public void openFire(string _id, int _gold, double _angle, string _userID, int _bulletLevel, string _sumGold)
    {

        for (int i = 0; i < contrall.instant().userDict.Count; i++)
        {
            //Debug.Log(contrall.instant().userDict.Count);

            if (contrall.instant().userDict[i] == _userID)
            {


                BulletMsg[i]._id = _id;

                BulletMsg[i]._gold = _gold;

                BulletMsg[i]._angle = _angle;

                BulletMsg[i]._bulletLevel = _bulletLevel;
                BulletMsg[i]._SumGold = _sumGold;
                isUser[i] = true;//判断是谁返回来的信息
                break;
            }
        }
        //Debug.Log(11);


        isOpenFire = true;


    }



    /// <summary>
    /// 自动判断鼠标射线检测
    /// </summary>
    void mouseCheck()
    {
        if (isAutoLock)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero);
                if (hit.collider != null)
                {

                    GameObject gameObj = hit.collider.gameObject;
                    if (gameObj.tag == "Fish")
                    {
                        if (GameObject.FindGameObjectWithTag("lockFish") != null)
                        {
                            GameObject.FindGameObjectWithTag("lockFish").tag = "Fish";
                        }

                        gameObj.tag = "lockFish";


                        lockFish = gameObj;

                        isXuanZhongFish = true;
                        isAutoFire = true;
                        mainMeiRenYuContrall.instant.automaticFire.transform.localScale = Vector3.zero;
                        mainMeiRenYuContrall.instant.automaticFireMask.transform.localScale = Vector3.one;
                        //加上锁定标记
                        if (GameObject.Find("autoLockEffect(Clone)") != null)
                        {
                            Destroy(GameObject.Find("autoLockEffect(Clone)").gameObject);

                        }
                        Instantiate(autoLockEffect, gameObj.transform.parent.transform);
                        WebButtonSendMessege.instant().releaseSkill("autoLockFish", meiRenYuThreadDeal.gosWeiZhi, gameObj.transform.parent.GetComponent<FishAttr>().id);

                    }
                }

            }
            if (isXuanZhongFish)
            {
                if (contrall.instant().isCanClearFish)
                {
                    if (lockFish == null || lockFish.transform.parent.transform.position.x > 9)
                    {

                        mainMeiRenYuContrall.instant.automaticFire.transform.localScale = Vector3.one;
                        mainMeiRenYuContrall.instant.automaticFireMask.transform.localScale = Vector3.zero;
                        isXuanZhongFish = false;
                        isAutoFire = false;
                        if (lockFish != null)
                        {
                            Destroy(lockFish.transform.parent.transform.Find("autoLockEffect(Clone)").gameObject);
                        }
                    }
                }
                else
                {
                    if (lockFish == null || lockFish.transform.parent.GetComponent<FishAttr>().nextDian == (guiJi.instant().guiJiDict[lockFish.transform.parent.GetComponent<FishAttr>().runPoint].Count - 2))
                    {

                        mainMeiRenYuContrall.instant.automaticFire.transform.localScale = Vector3.one;
                        mainMeiRenYuContrall.instant.automaticFireMask.transform.localScale = Vector3.zero;
                        isXuanZhongFish = false;
                        isAutoFire = false;
                        if (lockFish != null)
                        {
                            Destroy(lockFish.transform.parent.transform.Find("autoLockEffect(Clone)").gameObject);
                        }
                    }
                }
            }
        }

    }
    bool changeBgone = true;
    /// <summary>
    /// 改变背景
    /// </summary>
    public void ChangeBg()
    {
        if (changeBgone)//只需传一次就够了
        {
            meiRenYuThreadDeal.instant.bgImage = bgImage;
            meiRenYuThreadDeal.instant.bgImageParent = bgImageParent;
            meiRenYuThreadDeal.instant.bgBeforeParent = bgBeforeParent;
            meiRenYuThreadDeal.instant.bgSprites = bgSprites;
            changeBgone = false;
        }


        meiRenYuThreadDeal.instant.is20021 = true;

    }



    //修改游戏过程中金币变化
    public void setGoldText(int target, string _gold)
    {
        //goldText[target].text = (int.Parse(goldText[target].text) + _gold).ToString();
        goldText[target].text = _gold;
        //开炮后更新gold

        //lock (g1)
        //{
        //    Debug.Log("改变前：" +goldText[target].text);
        //    goldText[target].text = (int.Parse(goldText[target].text) + _gold).ToString(); //开炮后更新gold
        //    Debug.Log("改变后：" + goldText[target].text);
        //}

    }
}
