﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public struct buttonAll
{
    public Button btn;
    public bool click;
}
public class otherContral : MonoBehaviour
{

   
    public bool returnGameScene = false;
    public static otherContral instant = null;
    // Use this for initialization
    public List<buttonAll[]> btns = new List<buttonAll[]>();
   

    void Start()
    {
        GameObject.Find("Main Camera").transform.rotation = Quaternion.Euler(Vector3.zero);//在选择分区把摄像机摆正

        instant = this;
    }

    // Update is called once per frame
    void Update() { }



    public void returnToHall()
    {
       
        //canvasGroup.blocksRaycasts = true;
        //sheltObj.SetActive(true);
        returnGameScene = true;
        saveDate.startCenterPos = 0;
        //CClient.ClientSocket.instant().ws.Close();

        Instantiate(ResouseManager.Instance.LOADP/*load*/, GameObject.Find("Canvas").transform); 

        SceneManager.LoadScene("gameScene");//返回到大厅




    }
    public void openRuler()
    {
        RootCanvas.canvas_group(GameObject.Find("slip").GetComponent<CanvasGroup>(), true, 1);
        GameObject.Find("selectBck").transform.Find("slip").transform.Find("Scroll View").transform.gameObject.SetActive(true);
    }
    public void exitRuler()
    {
        RootCanvas.canvas_group(GameObject.Find("slip").GetComponent<CanvasGroup>(), false, 0);
        GameObject.Find("selectBck").transform.Find("slip").transform.Find("Scroll View").transform.gameObject.SetActive(false);
    }


    public void getAllButton()
    {
        Button[] button;
        button = FindObjectsOfType(typeof(Button)) as Button[];
        for (int i = 0; i < button.Length; i++)
        {
            buttonAll[] b = new buttonAll[2];
            b[0].btn = button[i];
            b[1].click = false;
            btns.Add(b);
        }
        //Debug.Log(btns.Count);
        //foreach (Button btn in button)
        //{
        //    btn.onClick.AddListener(() =>
        //    {
        //        for (int i = 0; i < btns.Count; i++)
        //        {
        //            if (btns[i][0].btn == btn)
        //            {
        //                if()
        //            }
        //        }
        //    });

        //}
    }
}
