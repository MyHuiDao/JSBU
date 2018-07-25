using UnityEngine;
using System.Collections.Generic;
using System;

/**
 * 子弹类
 * 做的事情：撞到鱼生成网（至于鱼碰到网受伤害，交给网自己），多久消失(交给Ef_DestroySelf)，撞到border消失
 * 子弹飞的事情交给了Ef_AutoMove
 * 该类会挂载到每种子弹的预制体上
 * */
public class BulletAttr : MonoBehaviour
{
    public int belongTarget = 20;//属于某个对象发射的赋初值不小于4


    public int speed = 10;
    public int damage;
    public GameObject webPrefab;    //子弹碰到鱼生成的网的预制体
    public GameObject boomEffetc;
    GameObject boomHoderParent;
    public string id;//子弹ID

    double x;
    double y;//保存子弹消失位置的坐标
    int i = 0;//这两个是为了确保网展开后再消失了，执行update的方法
    GameObject web = null;//网
    public List<string> colliderFish = new List<string>();//把网碰到的鱼保存在list里面

    public bool isCollderOne = true;//只能碰到一个鱼
    List<GameObject> list = new List<GameObject>();
    private void Start()
    {
        //Invoke("bulletSelfDestroy", 20);
        boomHoderParent = GameObject.Find("boomHoder");
        FishMaker.SaveNet.Add(id, colliderFish);
    }
    private void Update()
    {
        //网消失掉
        if (i == 1 && web == null)
        {
            if (belongTarget == meiRenYuThreadDeal.gosWeiZhi)
            {
                //Debug.Log("对象"+belongTarget+"id......................"+id);
                WebButtonSendMessege.instant().bulletDie("20009", id, x.ToString(), y.ToString(), FishMaker.SaveNet[id]);
                i = 0;
            }


        }

        if (this.transform.localScale == Vector3.zero)//碰到鱼后，没收到返回信息，自动消除
        {
            Invoke("bulletSelfDestroy", 2);
        }
        if(this.transform.localPosition.x<-12|| this.transform.localPosition.x > 12|| this.transform.localPosition.y < -7 || this.transform.localPosition.y >7)//子弹出界
        {
            Invoke("bulletSelfDestroy", 1);
        }
    }
    //border身上挂了刚体，子弹上没有挂刚体，只有boxcollider2d,
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //子弹碰撞到Border,反弹
        if (collision.tag == "up")
        {

            if (this.transform.localEulerAngles.z >= 0)
            {

                float z = (180 - 2 * Mathf.Abs(this.transform.localEulerAngles.z)) + this.transform.localEulerAngles.z;


                transform.localRotation = Quaternion.Euler(0, 0, z);
            }
            else
            {
                float z = -(180 - 2 * Mathf.Abs(this.transform.localEulerAngles.z)) - this.transform.localEulerAngles.z;
                transform.localRotation = Quaternion.Euler(0, 0, z);

            }
        }
        if (collision.tag == "down")
        {


            float z = 180 - Mathf.Abs(this.transform.localEulerAngles.z);
            transform.localRotation = Quaternion.Euler(0, 0, z);


        }
        if (collision.tag == "right" || collision.tag == "left")
        {


            float z = -this.transform.localEulerAngles.z;
            transform.localRotation = Quaternion.Euler(0, 0, z);


        }

        if (isCollderOne)
        {
            if (belongTarget == meiRenYuThreadDeal.gosWeiZhi)//代表是我自己发射的子弹
            {
                if (GameController.Instance.isXuanZhongFish)//开启了自动锁定
                {
                    //Debug.Log("进入自动选中");
                    if (collision.tag == "lockFish")
                    {
                        collision.GetComponent<SpriteRenderer>().color = new Color(255, 0, 30, 1);

                        list.Add(collision.gameObject);
                        Invoke("fishCollider", 0.2f);

                        skillFish("lockFish", collision.transform.position);

                    }
                }
                else
                {

                    //子弹碰撞到鱼，生成网，并销毁自己
                    if (collision.tag == "Fish")
                    {
                        collision.GetComponent<SpriteRenderer>().color = new Color(255, 0, 30, 1);

                        list.Add(collision.gameObject);
                        Invoke("fishCollider", 0.2f);

                        skillFish("Fish", collision.transform.position);

                    }
                }
            }
            else
            {
                if (mainMeiRenYuContrall.instant.otherAutoLock)
                {

                    if (mainMeiRenYuContrall.instant.otherautofish.ContainsKey(belongTarget))
                    {
                        if (collision.tag == "Fish" && collision.transform.parent.GetComponent<FishAttr>().id == mainMeiRenYuContrall.instant.otherautofish[belongTarget])
                        {


                            //GameObject g= Instantiate(GameController.Instance.autoLockEffect, collision.transform.parent.transform);
                            //g.name = "otherLockFish";

                            skillFish("Fish", collision.transform.position);

                        }
                    }


                }
                else
                {
                    if (collision.tag == "Fish")
                    {

                        skillFish("Fish", collision.transform.position);

                    }
                }


            }

        }
    }

    //修改颜色
    void fishCollider()
    {
        if (list[0] != null)
        {
            list[0].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            list.RemoveAt(0);
        }


    }
    void skillFish(string _skillFishTag, Vector3 pos)
    {
        //GameObject g= Instantiate(boomEffetc,boomHoderParent.transform);
        //g.transform.position = this.transform.position;
        web = Instantiate(webPrefab);
        web.GetComponent<WebAttr>().ziDanID = id;
        web.GetComponent<WebAttr>().skillFishTag = _skillFishTag;//能补到的鱼
        web.transform.SetParent(gameObject.transform.parent, false);    //将生成的网和子弹放在同一个容器中
        web.transform.position = pos;
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;//保存消失的位置
        this.gameObject.transform.localScale = Vector3.zero;//先隐藏子弹
        i++;

        isCollderOne = false;
    }
    /// <summary>
    /// 销毁子弹
    /// </summary>
    public void destroyBullet()
    {

        //销毁子弹

        Destroy(gameObject);

    }

    public void bulletSelfDestroy()
    {

        for (int i = 0; i < 4; i++)
        {

            if (GameController.Instance.bulletDict[i].ContainsKey(this.id))
            {

                GameController.Instance.bulletDict[i][this.id].GetComponent<BulletAttr>().destroyBullet();
                GameController.Instance.bulletDict[i].Remove(this.id);
                
                break;
            }
        }
        if (FishMaker.SaveNet.ContainsKey(this.id))
        {

            FishMaker.SaveNet.Remove(this.id);//删除字典中的网
        }
        //Debug.Log("发送20009");
        //if (contrall.instance.isZhuJi)
        //{
        //    CClient.ClientSocket.instant().send("20009", (object)("{\"fireId\":\"" + this.id + "\",\"x\":" + this.transform.localPosition.x.ToString() + ",\"y\":" + this.transform.localPosition.y.ToString() + ",\"fishList\":\"\"}"));
        //}
    }
}
