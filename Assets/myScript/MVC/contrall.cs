using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using DG.Tweening;


/// <summary>
/// 用来接收服务器发送的消息并处理信息，方法后面的表示要跟服务器对应
/// </summary>
public class contrall
{


    public int fishMakerNumSort = 0;
    string joinUserId = null;//保存进入房间的用户ID
    public Dictionary<int, string> userDict = new Dictionary<int, string>();//使ID与位置相对应
    public bool isZhuJi = false;//判断自己是否是主机
    public Dictionary<string, int> everyOnegoldDict = new Dictionary<string, int>();//保存每个人的金币
    public string addFileGoldNum = "0";//每炮添加的金币数
    public static contrall instance = null;
    public List<string> judgeIsMove = new List<string>();//用来判断中途加入的鱼是否能立即游动
    public bool isCanClearFish = false;
    public int yuZhenGroupCard;
    private Object thisLock = new Object();
    private Object thisLock1 = new Object();
    private float height = 450;
    public string message;
    //int joinRoom = 0;
    //int joinRoom1 = 0;
    //int joinRoom2 = 0;
    //int joinRoom3 = 0;
    //int joinRoom4 = 0;
    public static contrall instant()
    {
        if (instance == null) instance = new contrall();
        return instance;
    }

    public void contrallAddMethod()
    {
        CallBackFun.shareCallBack.addMethod(this);//把此类所有方法添加到callBack的字典中，方法类型为do1111等

    }


    /// <summary>
    /// 下面四个为进入游戏区返回信息
    /// </summary>
    /// <param name="o"></param>
    public void do20002(object o)
    {
        loadSelectArea.instant.is20002 = true;
    }
    public void do20003(object o)
    {
        Debug.Log(o.ToString());
    }

    /// <summary>
    /// 进入房间用户信息.只发给自己
    /// </summary>
    /// <param name="o"></param>
    public void do20001(object o)
    {
        //Debug.Log("收到20001");
        FishMaker.listGroup.Clear();
        //Debug.Log(1);

        addFileGoldNum = ((JsonData)o)["fireNum"].ToString();
        for (int i = 0; i < ((JsonData)o)["userList"].Count; i++)//判断在四个里面的哪一个位置
        {
            if (((JsonData)o)["userList"][i] == null)
            {
                continue;
            }
            //Debug.Log("用户" + ((JsonData)o)["userList"][i].ToString());
            userDict.Add(i, ((JsonData)o)["userList"][i].ToString());//把用户ID跟位置对应起来

            if (httpView.id == (((JsonData)o)["userList"][i].ToString()))//自己
            {

                Debug.Log("自己的位置"+i);
                meiRenYuThreadDeal.gosWeiZhi = i;
                GunFollow.posWeizhi = i;
                GameController.gosWeiZhi = i;
            }
        }
        //Debug.Log("2");
        if ((httpView.id == ((JsonData)o)["userId"].ToString()))//如果自己是主机ID，做个记录
        {
            isZhuJi = true;
        }
       // Debug.Log("3");
        for (int i = 0; i < userDict.Count; i++)
        {
            if (((JsonData)o)["userGoldMap"].Keys.Contains(userDict[i]))
            {
                everyOnegoldDict.Add(userDict[i], (int)(((JsonData)o)["userGoldMap"][userDict[i]]));//添加每个人的金币

            }
        }
        //Debug.Log("4");
        for (int i = 0; i < userDict.Count; i++)//返回的子弹的级别为1-9
        {
            if (((JsonData)o)["userFireLevelMap"].Keys.Contains(userDict[i]))
            {
                GameController.bulletLevel.Add(userDict[i], (int)(((JsonData)o)["userFireLevelMap"][userDict[i]]));//添加每个人子弹的级别
                if ((int)(((JsonData)o)["userFireLevelMap"][userDict[i]]) >= 0 && (int)(((JsonData)o)["userFireLevelMap"][userDict[i]]) <= 2)
                {
                    Debug.Log(1);
                    GameController.gunShapes.Add(userDict[i], 0);
                }
                else if ((int)(((JsonData)o)["userFireLevelMap"][userDict[i]]) >= 3 && (int)(((JsonData)o)["userFireLevelMap"][userDict[i]]) <= 5)
                {
                    Debug.Log(2);
                    GameController.gunShapes.Add(userDict[i], 1);
                }
                else
                {
                    Debug.Log(3);
                    GameController.gunShapes.Add(userDict[i], 2);

                }
            }

        }
        //Debug.Log("5");
        if (((JsonData)o)["fishList"].Count != 0)//代表我是中途进来的
        {

            List<string> listID = new List<string>(); //代表一波鱼  

            for (int i = 0; i < ((JsonData)o)["fishList"].Count; i++)
            {
                FishAttr f = new FishAttr();
                f.fishType = ((JsonData)o)["fishList"][i]["level"].ToString();
                f.id = ((JsonData)o)["fishList"][i]["id"].ToString();
                f.runPoint = ((JsonData)o)["fishList"][i]["runPoint"].ToString();
                f.Speed = float.Parse(((JsonData)o)["fishList"][i]["speed"].ToString());
                f.initialX = guiJi.instant().guiJiDict[f.runPoint][0].x;
                f.initialY = guiJi.instant().guiJiDict[f.runPoint][0].y;
                listID.Add(f.id);
                FishMaker.fish.Add(f.id, f);//把鱼保存到字典里
            }
            FishMaker.listGroup.Add(listID);
            FishMaker.isHalfGoGame = true;
        }
        //Debug.Log("进行场景跳转的操作");
        Mosframe.DynamicScrollView.instant.loadMeiRenYu();//场景跳转        
        meiRenYuThreadDeal.is20001 = true;


    }
    /// <summary>
    /// 进入房间的人的信息，发给所有人
    /// </summary>
    public void do20017(object o)
    {
        Debug.Log("收到20017");
        joinUserId = ((JsonData)o)["userId"].ToString();
        //添加用户到字典中,如果进入的是我自己，则不添加
        if (((JsonData)o)["userId"].ToString() != httpView.id)
        {
            Debug.Log("收到200171");
            GameController.gunShapes.Add(((JsonData)o)["userId"].ToString(), 0);//添加枪
            for (int i = 0; i < userDict.Count; i++)
            {
                if (userDict[i] == null)
                {
                    userDict[i] = ((JsonData)o)["userId"].ToString();
                    meiRenYuThreadDeal.instant.oGold = o;
                    meiRenYuThreadDeal.instant.goldTarget = i;
                    meiRenYuThreadDeal.instant.is20017 = true;
                    break;
                }

                if (i == userDict.Count - 1)
                {
                    userDict.Add(i + 1, ((JsonData)o)["userId"].ToString());
                    meiRenYuThreadDeal.instant.oGold = o;
                    meiRenYuThreadDeal.instant.goldTarget = i + 1;
                    meiRenYuThreadDeal.instant.is20017 = true;
                    break;
                }
            }
        }
        if (instant().isZhuJi && ((JsonData)o)["userId"].ToString() != httpView.id)//如果本机是主机并且进入房间的不是自己，则需要转发房间信息给进来的人
        {
            Debug.Log("收到200172");
            WebButtonSendMessege.instant().giveCurrentRoomMsg();

        }
    }
    /// <summary>
    /// 只发给主机，发给我，我就是主机
    /// </summary>
    public void do20018(object o)
    {
        isZhuJi = true;
    }

    /// <summary>
    /// 鱼群信息
    /// </summary>
    /// <param name="o"></param>
    public void do20004(object o)
    {
        //Debug.Log("收到20004");
        List<string> listID = new List<string>(); //代表一波鱼   
        for (int i = 0; i < ((JsonData)o).Count; i++)
        {

            FishAttr f = new FishAttr();

            f.fishType = ((JsonData)o)[i]["level"].ToString();

            f.id = ((JsonData)o)[i]["id"].ToString();

            f.runPoint = ((JsonData)o)[i]["runPoint"].ToString();

            f.Speed = float.Parse(((JsonData)o)[i]["speed"].ToString());

            listID.Add(f.id);
            FishMaker.fish.Add(f.id, f);//把鱼保存到字典里
        }
        FishMaker.listGroup.Add(listID);
        
        FishMaker.do20004Order.Add(0);
        //FishMaker.instant.secondTimeStart();//产生鱼
    }

    /// <summary>
    /// 接收主机鱼群子弹信息
    /// </summary>
    /// <param name="o"></param>
    public void do20012(object o)
    {
        if (joinUserId == httpView.id)
        {
            for (int i = 0; i < ((JsonData)o).Count; i++)
            {
                if (FishMaker.fish.ContainsKey(((JsonData)o)[i]["id"].ToString()))
                {
                    FishMaker.fish[((JsonData)o)[i]["id"].ToString()].initialX = float.Parse(((JsonData)o)[i]["x"].ToString());
                    FishMaker.fish[((JsonData)o)[i]["id"].ToString()].initialY = float.Parse(((JsonData)o)[i]["y"].ToString());
                    FishMaker.fish[((JsonData)o)[i]["id"].ToString()].nextDian = int.Parse(((JsonData)o)[i]["n"].ToString());
                    contrall.instant().judgeIsMove.Add(((JsonData)o)[i]["id"].ToString());
                }
            }
            FishMaker.do20004Order.Add(1);
            //FishMaker.instant.secondTimeStart();//产生鱼         
        }
    }
    /// <summary>
    /// 下面三个为捕鱼开炮返回信息，返回给所有人
    /// </summary>
    /// <param name="o"></param>
    public void do20006(object o)
    {
        //Debug.Log("收到了20006信息");
       // Debug.Log("-><color=#9400D3>" + "服务器接收到的总金币数" + "</color>" + ((JsonData)o)["sumGold"].ToString());
     


        if (((JsonData)o)["fire"]["userId"].ToString() == httpView.id)
        {
           // Debug.Log("???");
            GameController.lastBulletFire = true;
        }
        returnBulletMsg r = new returnBulletMsg();
        r._id = ((JsonData)o)["fire"]["id"].ToString();
        //Debug.Log(1);
        r._gold = (int)(((JsonData)o)["useGold"]);
      //  Debug.Log(1);
        r._angle = double.Parse((((JsonData)o)["fire"]["angle"].ToString()));
       // Debug.Log(1);
        r._bulletLevel = (int)(((JsonData)o)["fire"]["level"]);
       // Debug.Log(1);
        r.user = ((JsonData)o)["fire"]["userId"].ToString();
        r._SumGold = ((JsonData)o)["sumGold"].ToString();
       // Debug.Log(1);
        GameController.returnBullet.Add(r);
      //  Debug.Log(1);

        //GameController.Instance.openFire(((JsonData)o)["fire"]["id"].ToString(), (int)(((JsonData)o)["useGold"]), double.Parse((((JsonData)o)["fire"]["angle"].ToString())), ((JsonData)o)["fire"]["userId"].ToString(), (int)(((JsonData)o)["fire"]["level"]));//返回子弹信息，并告诉可以发射
    }
    /// <summary>
    /// 创建子弹金币不足
    /// </summary>
    /// <param name="o"></param>
    public void do20007(object o)
    {
      
        GameController.lastBulletFire = true;
        meiRenYuThreadDeal.instant.is20007 = true;
       
    }
    /// <summary>
    /// 创建子弹体验币不足
    /// </summary>
    /// <param name="o"></param>
    public void do20008(object o)
    {
       
        GameController.lastBulletFire = true;
        meiRenYuThreadDeal.instant.is20007 = true;


    }
    /// <summary>
    /// 退出房间,发给所有人
    /// </summary>
    /// <param name="o"></param>
    public void do20005(object o)
    {
       
        if (o.ToString() == httpView.id)
        {
            //退出房间时，保存的字典数据应该全部清零,看看是否需要清零fishmaker等字典
            Debug.Log("我自己退出");
            meiRenYuThreadDeal.instant.exitRoom();
            return;
        }
        for (int i = 0; i < userDict.Count; i++)//谁离开房间，则把它去除
        {
            if (userDict[i] == o.ToString())
            {
                Debug.Log((JsonData)o + "离开了房间");
                GameController.gunShapes.Remove(userDict[i]);//删除             
                meiRenYuThreadDeal.instant.setGoldText(i);
                userDict[i] = null;
                break;
            }
        }

    }
    /// <summary>
    /// 离开房间后清理所有
    /// </summary>
    public void clearAll()
    {
        CClient.ClientSocket.instant().messegeQueue.Clear();
        FishMaker.deadFish.Clear();
        FishMaker.moveFish.Clear();
        FishMaker.do20004Order.Clear();
        //joinRoom = 0;
        userDict.Clear();
        everyOnegoldDict.Clear();
        GameController.gunShapes.Clear();
        GameController.bulletLevel.Clear();
        FishMaker.isNeedFishDestroy = false;
        FishMaker.is20004Scencod = false;
        FishMaker.isHalfGoGame = false;
        everyOnegoldDict.Clear();
        FishMaker.fish.Clear();
        FishMaker.fishTarget.Clear();
        FishMaker.SaveNet.Clear();
        FishMaker.listGroup.Clear();
        judgeIsMove.Clear();
        fishArrayContral.listGroupYuzhen.Clear();
        isCanClearFish = false;
        meiRenYuThreadDeal.destroyBulletList.Clear();
      

    }
    /// <summary>
    /// 鱼自然消失
    /// </summary>
    /// <param name="o"></param>
    public void do20010(object o)
    {

        //if (fishArrayContral.listGroupYuzhen.Count == 1)//如果处于鱼阵状态
        //{
        //    if (fishArrayContral.instant.yuzhenPrefab!=null)
        //    {
              
        //        meiRenYuThreadDeal.instant.is20010 = true;
             
        //        fishArrayContral.instant.yuzhenPrefab = null;
        //    }

        //    //if (fishArrayContral.listGroupYuzhen[0].Count > 0)
        //    //{
        //    //    if (fishArrayContral.listGroupYuzhen[0].Contains(o.ToString()))
        //    //    {
        //    //        fishArrayContral.listGroupYuzhen[0].Remove(o.ToString());
        //    //    }
        //    //    if (fishArrayContral.listGroupYuzhen[0].Count == 0)
        //    //    {
        //    //        meiRenYuThreadDeal.instant.is20010 = true;
        //    //        fishArrayContral.listGroupYuzhen.Clear();
        //    //        fishArrayContral.instant.yuZhenTarget.Clear();
        //    //        fishArrayContral.instant.yuzhenFinish = false;
        //    //    }
        //    //}
        //}
        //else
        //{
            for (int i = 0; i < FishMaker.listGroup.Count; i++)
            {

                if (FishMaker.listGroup[i].Contains(o.ToString()))
                {
                    FishMaker.listGroup[i].Remove(o.ToString());
                    break;
                }
            }

        //}
        FishMaker.fish.Remove(o.ToString());//删除fish类
        FishMaker.deadFish.Add(o.ToString());
    }
    public void fishTargetRemove(string id)
    {

        FishMaker.fishTarget.Remove(id);//删除鱼

    }
    /// <summary>
    /// 捕鱼消失
    /// </summary>
    /// <param name="o"></param>
    public void do20009(object o)
    {
       



            //销毁子弹
            //Debug.Log(1);
            destroyBullet d = new destroyBullet();
            //Debug.Log(1);
            d._fireID = ((JsonData)o)["fireId"].ToString();
            //Debug.Log(1);
            d._fishList = o;
            //Debug.Log(1);
            d._gold = (int)(((JsonData)o)["getGold"]);//总金币数
            //Debug.Log(1);
            d.sumgold = ((JsonData)o)["sumGold"].ToString();
            //Debug.Log(1);
            meiRenYuThreadDeal.destroyBulletList.Add(d);//注意进入房间之前就受到了此消息
            //Debug.Log(1);
       
    }

    /// <summary>
    /// 鱼开始游动
    /// </summary>
    /// <param name="o"></param>
    public void do20011(object o)
    {
        lock (thisLock)
        {
            if (!FishMaker.isHalfGoGame)
            {
               
                FishMaker.instant.fishMove(o.ToString());
            }
        }
    }



    /// <summary>
    /// 增加炮级别,发给所有人
    /// </summary>
    public void do20013(object o)
    {
        if (GameController.Instance == null)
        {
            while (true)
            {
                if (GameController.Instance != null)
                {
                    break;
                }
            }
        }
        GameController.Instance.addGosClass(((JsonData)o)["userId"].ToString(), (int)((JsonData)o)["level"]);

    }
    /// <summary>
    /// 炮级别最大了
    /// </summary>
    public void do20014(object o)
    {

        Debug.Log("炮级别最大，不做改变");

    }
    /// <summary>
    /// 减小炮级别
    /// </summary>
    public void do20015(object o)
    {
        if (GameController.Instance == null)
        {
            while (true)
            {
                if (GameController.Instance != null)
                {
                    break;
                }
            }
        }
        GameController.Instance.reduceGosClass(((JsonData)o)["userId"].ToString(), (int)((JsonData)o)["level"]);

    }
    /// <summary>
    /// 炮级别最小了
    /// </summary>
    public void do20016(object o)
    {

        Debug.Log("炮级别最小，不做改变");

    }
    /// <summary>
    /// 得到谁使用了什么技能
    /// </summary>
    /// <param name="o"></param>
    public void do20019(object o)
    {
        if (((JsonData)o)["skillName"].ToString() == "frozenFish" && int.Parse(((JsonData)o)["useSkillTarget"].ToString()) == meiRenYuThreadDeal.gosWeiZhi)
        {

            fishArrayContral.instant.is20019 = true;
        }
        if (((JsonData)o)["skillName"].ToString() == "frozenFish")
        {

            mainMeiRenYuContrall.instant.is20019Pause = true;

        }
        if (int.Parse(((JsonData)o)["useSkillTarget"].ToString()) != meiRenYuThreadDeal.gosWeiZhi)
        {

            if (((JsonData)o)["skillName"].ToString() == "autoLockFish")
            {
                mainMeiRenYuContrall.instant.otherAutoLock = true;//有人开启了自动锁定
                if (mainMeiRenYuContrall.instant.otherautofish.ContainsKey(int.Parse(((JsonData)o)["useSkillTarget"].ToString())))
                {
                    mainMeiRenYuContrall.instant.otherautofish.Remove(int.Parse(((JsonData)o)["useSkillTarget"].ToString()));
                }
                mainMeiRenYuContrall.instant.otherautofish.Add(int.Parse(((JsonData)o)["useSkillTarget"].ToString()), ((JsonData)o)["lockFishID"].ToString());

            }
        }
    }
    /// <summary>
    /// 技能结束时间
    /// </summary>
    /// <param name="o"></param>
    public void do20020(object o)
    {

        if (int.Parse(((JsonData)o)["useSkillTarget"].ToString()) != meiRenYuThreadDeal.gosWeiZhi)
        {

            if (((JsonData)o)["skillName"].ToString() == "frozenFish")
            {
                mainMeiRenYuContrall.instant.is20020CancelPause = true;

            }
            if (((JsonData)o)["skillName"].ToString() == "autoLockFish")
            {

                mainMeiRenYuContrall.instant.otherautofish.Remove(int.Parse(((JsonData)o)["useSkillTarget"].ToString()));
                if (mainMeiRenYuContrall.instant.otherautofish.Count == 0)
                {
                    mainMeiRenYuContrall.instant.otherAutoLock = false;

                }

            }
        }
    }
    /// <summary>
    /// 发动技能消耗的金币数
    /// </summary>
    public void do20024(object o)
    {
        int j = 0;
        for (int i = 0; i < userDict.Count; i++)
        {
            if (userDict[i] == ((JsonData)o)["userId"].ToString())
            {
                j = i;
                break;
            }
        }
        meiRenYuThreadDeal.instant.o1 = o;
        meiRenYuThreadDeal.instant.bingDongTarget = j;
        meiRenYuThreadDeal.instant.is20024 = true;



    }
    /// <summary>
    /// 发动技能金币不足
    /// </summary>
    public void do20025(object o)
    {
        Debug.Log("金币不足，不能使用冰冻");
    }
    /// <summary>
    /// 清除所有鱼
    /// </summary>
    public void do20021(object o)
    {
        Debug.Log("20021清除所有鱼。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。");
        //if (GameController.Instance == null)
        //{
        //    while (true)
        //    {
        //        if (GameController.Instance != null)
        //        {
        //            break;
        //        }
        //    }
        //}
        fishArrayContral.instant.everyMakeOn = true;
        isCanClearFish = true;
        
        GameController.Instance.ChangeBg();
    }

    /// <summary>
    /// 鱼阵生成
    /// </summary>
    /// <param name="o"></param>
    public void do20023(object o)
    {
        Debug.Log("20023鱼阵生成。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。");
        yuZhenGroupCard = int.Parse(((JsonData)o)[0]["group"].ToString());
        Debug.Log(((JsonData)o).Count);
        List<string> listID = new List<string>(); //代表一波鱼   
        Debug.Log(2);
        for (int i = 0; i < ((JsonData)o).Count; i++)
        {
            Debug.Log(3);
            FishAttr f = new FishAttr();
            f.fishType = ((JsonData)o)[i]["level"].ToString();
            f.id = ((JsonData)o)[i]["id"].ToString();
            f.Speed = float.Parse(((JsonData)o)[i]["speed"].ToString());
            listID.Add(f.id);
            FishMaker.fish.Add(f.id, f);//把鱼保存到字典里
        }
        Debug.Log(4);
        fishArrayContral.listGroupYuzhen.Add(listID);
        fishArrayContral.instant.MakeFishesYuZhen();

    }
    /// <summary>
    /// 鱼阵开始游动
    /// </summary>
    /// <param name="o"></param>
    public void do20022(object o)
    {
        Debug.Log("20022鱼阵开始游动。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。。");
        meiRenYuThreadDeal.instant.is20022 = true;
    }
    /// <summary>
    /// 触发鱼阵时间
    /// </summary>
    public void do20026(object o)
    {
        if (meiRenYuThreadDeal.instant != null)
        {
            meiRenYuThreadDeal.instant.yuZhenTime = ((JsonData)o).ToString();
            meiRenYuThreadDeal.instant.is20026 = true;
        }
    }

    //============================================================================================================================================
    /// <summary>
    /// 大厅发送消息成功
    /// </summary>
    /// <param name="o"></param>
    public void do10000(object o)
    {
        Debug.Log(o.ToString());

    }
    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="o"></param>
    public void do10001(object o)
    {
        Debug.Log(o.ToString());
        gameContrall.instant.aa = ((JsonData)o)["msg"].ToString();

        gameContrall.instant.bb = true;
        gameContrall.instant.cc = false;
    }
    /// <summary>
    /// 邮件消息
    /// </summary>
    /// <param name="o"></param>
    public void do10002(object o)
    {
        Debug.Log("有新邮件，接受一下");
    }
    /// <summary>
    /// 用户充值
    /// </summary>
    /// <param name="o"></param>
    public void do10004(object o)
    {
        gameContrall.instant.cc = true;
        //Debug.Log(o.ToString());
        // PayGame.instance.test((string)((JsonData)o));
    }
    /// <summary>
    /// 客服不在线
    /// </summary>
    /// <param name="o"></param>
    public void do10005(object o)
    {
        gameContrall.instant.cc = true;
    }



}
