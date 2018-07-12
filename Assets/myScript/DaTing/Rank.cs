using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Rank : MonoBehaviour
{
    private string names;
    private string c;

    void Start()
    {
        for (int i = 4; i < httpConnect.toggle_list.Count; i++)
        {
            int b = i;
            GameObject.Find(httpConnect.toggle_list[b].name).GetComponent<Toggle>().onValueChanged.AddListener((bool value) => Choose_huoyue(b,value));
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void Choose_huoyue(int a, bool abc)
    {
        if (abc)
        {
            rankGrag.instance.delete();
            GameObject.Find(httpConnect.toggle_list[a].name).GetComponent<Toggle>().interactable = false;
            abc = true;
            if (GameObject.Find(httpConnect.toggle_list[4].name).GetComponent<Toggle>().isOn == true)
            {
                httpSend.instant.rank("0");
            }
            else
            {
                httpSend.instant.rank("1");
            }
            switch (a)
            {   
                case 6:
                    huoyue("在线时长");
                    break;
                //case 7:
                //    huoyue("游戏币");
                //    break;
            }
        }
        else
        {
            GameObject.Find(httpConnect.toggle_list[a].name).GetComponent<Toggle>().interactable = true;
        }
    }
    public void huoyue(string text1)
    {
        GameObject.Find("Ranktop").transform.GetChild(3).GetComponent<Text>().text = text1;
    }
    public void webs(string a)
    {
        httpSend.instant.rank(a);
    }
}