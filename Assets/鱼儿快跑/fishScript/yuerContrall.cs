using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class yuerContrall : MonoBehaviour
{


    public GameObject[] champainCombat;
    public GameObject[] secondCombat;
    GameObject combatParent;



    public List<Image> touzhuTarget = new List<Image>();//保存投注对象
    public static List<int> touzhuNum = new List<int>();

    public static yuerContrall instance = null;
    public bool returnGameScene = false;

    public bool is10008 = false;
    public object o;
    // Use this for initialization
    void Start()
    {

        instance = this;

        GameObject.Find("exitRuler").GetComponent<Button>().onClick.AddListener(exitRuler);
        GameObject.Find("rulerBtn").GetComponent<Button>().onClick.AddListener(openRuler);
        combatParent = GameObject.Find("rightCombat");
        GameObject.Find("return").GetComponent<Button>().onClick.AddListener(returnToHall);
        initialJieMian();//初始化开始界面
        touzhuNum.Clear();
        initialTouZhuNum();
        yuerSendMSg.instant().getCombatGain(1, 10);//获取战绩


    }

    // Update is called once per frame
    void Update()
    {

        if (is10008)
        {
            deal10008();
            is10008 = false;
        }


    }

    void initialJieMian()
    {
        //GameObject.Find("touXiang").GetComponent<Image>().sprite = httpView.touXiang;
        GameObject.Find("nickNameText").GetComponent<Text>().text = saveDate.nickname;
        GameObject.Find("codeText").GetComponent<Text>().text = saveDate.code;
        GameObject.Find("moneyText").GetComponent<Text>().text = saveDate.gold.ToString();
        GameObject.Find("selftouxiang").GetComponent<Image>().sprite = weiXinLoad.instance.headSprite;//httpView.touXiang;

    }
    void initialTouZhuNum()
    {
        for (int i = 0; i < 16; i++)
        {
            touzhuNum.Add(0);
        }

    }

    /// <summary>
    /// 返回到大厅
    /// </summary>
    void returnToHall()
    {
        returnGameScene = true;
        GameObject _obj = Instantiate(weiXinLoad.instance.loadP, GameObject.Find("Canvas").transform);
        _obj.transform.localScale = new Vector3(0.5f,0.5f,1);
        //CClient.ClientSocket.instant().ws.Close();
        SceneManager.LoadSceneAsync("gameScene");
    }





    public void deal10008()
    {
        Debug.Log("收到10008");
        for (int i = 0; i < ((JsonData)o).Count; i++)
        {
            if (((JsonData)o)[i]["status"].ToString() == "2")
            {
                string[] s = ((JsonData)o)[i]["result"].ToString().Split(',');
                Instantiate(champainCombat[int.Parse(s[0])-1], combatParent.transform);
                Instantiate(secondCombat[int.Parse(s[1])-1], combatParent.transform);
                //for (int j = 0; j < s.Length; j++)
                //{
                //    if (s[j].ToString() == "1")
                //    {
                //        Instantiate(champainCombat[j], combatParent.transform);
                //        break;
                //    }
                //}
                //for (int j = 0; j < s.Length; j++)
                //{
                //    if (s[j].ToString() == "2")
                //    {
                //        Instantiate(secondCombat[j], combatParent.transform);
                //        break;
                //    }
                //}
            }

        }
    }

    void openRuler()
    {
        GameObject.Find("ruler").transform.localScale = Vector3.one;
    }

    void exitRuler()
    {
        GameObject.Find("ruler").transform.localScale = Vector3.zero;
    }

}
