using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
public class pixel : MonoBehaviour
{
    static int loadNumber = 0;//保证只加载一次
    private static pixel instance;
    public static pixel Instance
    {
        get
        {

            return instance;
        }


    }

    public List<Item> itemList;


    private float x;
    private float y;


    void Awake()
    {
        instance = this;
        if (loadNumber == 0)
        {
            ParseItemJson();
        }
    }


    private void Update()
    {
    }
    /// <summary>
    /// 解析物品Json
    /// </summary>
    public void ParseItemJson()
    {
        loadNumber = 1;
		/*
		  TextAsset textAsset = Resources.Load<TextAsset>("json");
		  string itemsJson = textAsset.text;
		  //Debug.Log(textAsset.text);
		  JsonData jj = JsonMapper.ToObject(itemsJson);

		  //以后加入多条轨迹，得多维数组
		  for (int j = 0; j < jj["date"].Count; j++)
		  {
			  itemList = new List<Item>();//代表一条轨迹

			  for (int i = 0; i < jj["date"][j].Count; i++)
			  {

				  x = float.Parse((jj["date"][j][i]["x"].ToString()));
				  y = float.Parse((jj["date"][j][i]["y"].ToString()));
				  Item item = new Item(x, y);
				  itemList.Add(item);

			  }
			  guiJi.instant().guiJiDict.Add("gj" + (j + 1), itemList);
		  }
	*/

		ParsePath _path = new ParsePath();
        List<Vector2[]> pathes = _path.JieXiPath();
        if (pathes == null)
            return;
        for (int k = 0; k < pathes.Count; ++k)
        {
			itemList = new List<Item>();//代表一条轨迹
            for (int k1 = 0; k1 < pathes[k].Length;++k1)
            {
                Item item = new Item(pathes[k][k1].x, pathes[k][k1].y);
                itemList.Add(item);
            }
            guiJi.instant().guiJiDict.Add("gj" + (k + 1), itemList);
        } 
        
    }
}











