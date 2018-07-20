using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class fishMove : MonoBehaviour
{



    float jieSuanJiShi = 5;//结算倒计时
    bool jiesuanToStart = false;
    float jiesuantime = 0;

    float time = 0;
    //int t;
    float speed;
    //float A;
    //float B;
    float trackLong = 97;//（鱼开始到停止的一半）
    bool myselfStart = true;
    int mingci;
    float finishPos = -28.1f;
    float spurt = 50;//冲刺距离

    public static bool isStartSpurt = false;

    public static float cycleVar;
    float angle;
    float angleVar;
    float angleValue;
    float cycleSpeed;
    float sprintSpeed;
    public static float aVar;
    float aSpeedVar;
    public static float bSpeedVar;



    float earlySpeed;//前期速度
    float laterSpeed;//后期速度

    static bool firstCry;
    static bool secondCry;
    static bool thirdCry;

    Transform brotherTransf;
    //小地图
    float bag_litScale;

    static float x=0;//判断游戏过程中的第一二名。
    // Use this for initialization
    void Start()
    {
        isStartSpurt = false;
        firstCry = true;
        secondCry = true;
        thirdCry = true;      
        angle = Random.Range(0, 90);      
        this.transform.localPosition = initialPrepare.instance.trackDict[initialPrepare.instance.trackNum[this.name]];
        mingci = initialPrepare.nameAndNumdict[this.transform.name];  
        earlySpeed = (8 + aVar) + bSpeedVar * Mathf.Sin(angle * ((10 - cycleVar) / 10));
        laterSpeed = 15 - mingci;
        brotherTransf = getBrotherTrasnf(gameObject.name);

    }

    // Update is called once per frame
    void Update()
    {
        if (timeJiShi2.startGame)
        {
            if (myselfStart)
            {
                if (!isStartSpurt)
                {
                    time += Time.deltaTime;
                    if (time >= 1f)
                    {
                        earlySpeed = (8 + aVar) + bSpeedVar * Mathf.Sin((angle++) * ((10 - cycleVar) / 10));
                        time = 0;
                    }
                    transform.Translate(Vector3.right * earlySpeed * Time.deltaTime);
                    if(brotherTransf!=null)
                    {
                        brotherTransf.Translate(Vector3.right * earlySpeed*bag_litScale * Time.deltaTime);
                    }
                    if (this.transform.position.x >= spurt)
                    {
                        isStartSpurt = true;
                    }

                }
                else
                {
                    transform.Translate(Vector3.right * laterSpeed * Time.deltaTime);
                    if(brotherTransf!=null)
                    {
                        brotherTransf.Translate(Vector3.right * laterSpeed *bag_litScale * Time.deltaTime);
                    }
                }

                if (firstCry && this.transform.localPosition.x >= -50)
                {
                    initialStart.instance.allYinXiao[0].Play();
                    firstCry = false;
                }
                if (secondCry && this.transform.localPosition.x >= 0)
                {
                    initialStart.instance.allYinXiao[1].Play();
                    secondCry = false;
                }
                if (thirdCry && this.transform.localPosition.x >= 50)
                {
                    initialStart.instance.allYinXiao[2].Play();
                    thirdCry = false;
                }


                if (this.transform.localPosition.x >= trackLong)
                {
                    myselfStart = false;
                    GameObject.Find("endnum" + mingci).transform.DOLocalMoveX(finishPos, 0.1f);
                    if (mingci == 2)
                    {
                        fishCrown.instance.isStop = true;
                    }
                    if (mingci == 4)
                    {
                        timeJiShi2.startGame = false;

                        //GameObject.Find("Canvas").transform.Find("jieSuan").localScale = Vector3.one;//以前结算界面
                        //GameObject.Find("Content").transform.localPosition = new Vector3(0, 0, 0);
                        initialPrepare.instance.jiesuan();
                        GameObject.Find("moneyText1").GetComponent<Text>().text = (int.Parse(GameObject.Find("moneyText1").GetComponent<Text>().text) + int.Parse(saveDate.jiesuanGetGold)).ToString();
                        jiesuanToStart = true;

                    }

                }
            }

        }


        if (jiesuanToStart)
        {
            jiesuantime += Time.deltaTime;
            if (jiesuantime >= jieSuanJiShi)
            {
                initialStart.instance.allYinXiao[6].Pause();
                Destroy(GameObject.Find("prepareAndjieSuna(Clone)").gameObject);
                Destroy(GameObject.Find("background(Clone)").gameObject);
                Instantiate(/*m_slider.*/initialStart.yuerStartPrefab);
                Music_Control.music_effect(initialStart.instance.allYinXiao[8]);

                //在此处请求倒计时信息
                yuerSendMSg.instant().getCountDown();
                //yuerSendMSg.instant().getCombatGain(1, 10);//获取战绩
                jiesuanToStart = false;
                jiesuantime = 0;

            }

        }

    }

    Transform getBrotherTrasnf(string name)
    {

        SpriteRenderer Bag_SpriteRenderer = GameObject.Find("background(Clone)").GetComponent<SpriteRenderer>();
        Image litBagImage = GameObject.Find("litterBackground").GetComponent<Image>();
        //bag_litScale = Bag_SpriteRenderer.size.x / litBagImage.sprite.rect.width * (Screen.width / 2560.0f);// 2048/1284 = 1.6
        bag_litScale = (litBagImage.sprite.rect.width / Bag_SpriteRenderer.size.x) * (Screen.width / 2560.0f);// 2048/1284 = 1.6
        //float s = Screen.width / 2560.0f;
        //bag_litScale *= s;
        //Debug.LogError("ssss:"+bag_litScale);
        //Debug.LogError("Bag_SpriteRendererW:"+Bag_SpriteRenderer.sprite.rect.width + "===:litBagImageW:"+litBagImage.sprite.rect.width);
       
        if(name.Contains("1"))
        {
            
            Transform fish_Brother = GameObject.Find("fish_Brother1").transform;
            setBrotherTransfPosition(fish_Brother,litBagImage);
            return fish_Brother;
        }

        else if(name.Contains("2"))
        {
            Transform fish_Brother = GameObject.Find("fish_Brother2").transform;
            setBrotherTransfPosition(fish_Brother, litBagImage);
            return fish_Brother;
        }
        else if (name.Contains("3"))
        {
            Transform fish_Brother = GameObject.Find("fish_Brother3").transform;
            setBrotherTransfPosition(fish_Brother, litBagImage);
            return fish_Brother;
        }
        else if (name.Contains("4"))
        {
            Transform fish_Brother = GameObject.Find("fish_Brother4").transform;
            setBrotherTransfPosition(fish_Brother, litBagImage);
            return fish_Brother;
        }

        else
        {
            return null;
        }
    }

    void setBrotherTransfPosition(Transform _transform,Image _image)
    {
        float fish_BrotherY = _image.rectTransform.rect.height / 4;//_image.rectTransform.rect.height / 4 * (initialPrepare.instance.trackNum[this.name] -1);
        if(initialPrepare.instance.trackNum[this.name] == 1)
        {
            fish_BrotherY *= 1.5f;
        }
        else if(initialPrepare.instance.trackNum[this.name] == 2)
        {
            fish_BrotherY *= 0.5f;
        }
        else if (initialPrepare.instance.trackNum[this.name] == 3)
        {
            fish_BrotherY *= -0.5f;
        }
        else{
            fish_BrotherY *= -1.5f;
        }

        Vector3 fish_BrotherP = new Vector3(_transform.localPosition.x, fish_BrotherY, _transform.localPosition.z);
        _transform.localPosition = fish_BrotherP;
    }
}
