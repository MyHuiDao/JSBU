using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// 此类是用来保存每条鱼的轨迹
/// </summary>
public class guiJi
{

    public static guiJi instance = null;

    public static guiJi instant()
    {

        if (instance == null) instance = new guiJi();
        return instance;


    }  
    public Vector3[] waypoints;
    public PathType pathType = PathType.CatmullRom;
    public Dictionary<string, List<Item>> guiJiDict = new Dictionary<string, List<Item>>(); //保存每个轨迹里的每个点
 
    /// <summary>
    /// 鱼开始游动
    /// </summary>
    public void startMove(string _gj,int startPoint)//startPoint代表开始的点，一般为零，中途戛然房间的则不是
    {
        int j;
        j = startPoint;   
        if (startPoint == 100)
        {
            startPoint = 0;
        }
        waypoints = new Vector3[guiJiDict[_gj].Count-startPoint];
        for (int i = 0; i < guiJiDict[_gj].Count-j; i++)
        {
            waypoints[i] = new Vector3(guiJiDict[_gj][startPoint].x, guiJiDict[_gj][startPoint].y, 90);
            startPoint++;
            
        }



    }


}
