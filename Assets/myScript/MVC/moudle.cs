using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


/// <summary>
/// 此类用来接受contrall控制类的信息并处理
/// </summary>
public class moudle:MonoBehaviour
{




    public static moudle instance = null;
    void Awake()
    {
        instance = this;
    }


  

}
