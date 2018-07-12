using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class yuerThreadDeal : MonoBehaviour {


    public static yuerThreadDeal instance = null;
    public bool is10000 = false;
    public string roomId;
    public string roomPersonNum;


    public bool is10001 = false;
    public string target;
    public string moneyAdd;

    public bool is10002 = false;
    float time = 0;
    bool startJishi = false;


    public bool is10010 = false;
    public string money;


    public bool is10011 = false;
    public int roomPeopleNum;

    public bool is10006 = false;
    public string leavePersonNum;

    void Start () {
        instance = this;

	}

    
    // Update is called once per frame
    void Update () {
        if (startJishi)
        {
            time += Time.deltaTime;
            if (time >= 2)
            {
                try
                {
                    GameObject.Find("touzhuMoneyNotEnough").transform.localScale = Vector3.zero;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Data);//不做处理
                }
              
                time = 0;
                startJishi = false;
            }
        }


        if (is10000)
        {
            deal10000();
            is10000 = false;
        }
        if (is10002)
        {
            deal10002();
            is10002 = false;
        }
        if (is10010)
        {
            deal10010();
            is10010 = false;
        }
        if (is10011)
        {
            deal10011();
            is10011 = false;
        }
        if (is10006)
        {
            deal10006();
            is10006 = false;
        }
        if (is10001)
        {
            deal10001();
            is10001 = false;
        }
    }



    public void deal10000()
    {
        GameObject.Find("roomText").GetComponent<Text>().text = roomId;
        if (int.Parse(roomPersonNum) <= 10)
        {
            GameObject.Find("personNum").GetComponent<Text>().text = "10";
        }
        else
        {
            GameObject.Find("personNum").GetComponent<Text>().text = roomPersonNum;
        }
        
    }

    public void deal10002()
    {
        Destroy(yuerContrall.instance.touzhuTarget[0].gameObject);  
        yuerContrall.instance.touzhuTarget.RemoveAt(0);

        GameObject.Find("touzhuMoneyNotEnough").transform.localScale = Vector3.one;
        startJishi = true;
        
    }



    public void deal10010()
    {
        GameObject.Find("moneyText").GetComponent<Text>().text = money;
    }


    public void deal10011()
    {
        if (GameObject.Find("personNum") == null)
        {
            saveDate.roomPeopleNum = roomPeopleNum;
            if (roomPeopleNum <= 10)
            {
                GameObject.Find("personNum1").GetComponent<Text>().text ="10";
            }
            else
            {
                GameObject.Find("personNum1").GetComponent<Text>().text = roomPeopleNum.ToString();
            }
          
        }
        else
        {
            saveDate.roomPeopleNum=roomPeopleNum;
            if (roomPeopleNum <= 10)
            {
                GameObject.Find("personNum").GetComponent<Text>().text = "10";
            }
            else
            {
                GameObject.Find("personNum").GetComponent<Text>().text = roomPeopleNum.ToString();
            }
        }
        
    }


    public void deal10006()
    {
        
        if (GameObject.Find("personNum") == null)
        {

            GameObject.Find("personNum1").GetComponent<Text>().text = leavePersonNum;
        }
        else
        {

            GameObject.Find("personNum").GetComponent<Text>().text = leavePersonNum;
        }
    }


    public void deal10001()
    {
       
        Text t= GameObject.Find("middle1").transform.Find(initialStart.touzhuEquit[target].ToString()).transform.Find("clipText").GetComponent<Text>() ;
        if (t.text == "")
        {
            t.text = (int.Parse(moneyAdd) / 1000).ToString() + "k";
        }
        else
        {
            t.text = (int.Parse(t.text.Substring(0, t.text.Length - 1)) + int.Parse(moneyAdd) / 1000).ToString() + "k";
        }
        
    }

}
