using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;



/// <summary>
/// 此类是用来把信息转化为json类型
/// 多个重载，实现多个参数的输入
/// </summary>
public class jsonSend
{
    //int code;
    //string data;




    public string jsonMessege(string code, string data)
    {

        string jsonText = "{\"code\":\"" + code + "\",\"data\":\"" + data + "\"}";
        return jsonText;

    }

    public string jsonMessege(string code)
    {

        string jsonText = "{\"code\":\"" + code + "\"}";
        return jsonText;

    }

    public string jsonMessege(string code, object data)
    {

        string jsonText = "{\"code\":\"" + code + "\",\"data\":" + data + "}";
        
        //Debug.Log("fasong....................." + jsonText);
        return jsonText;

    }

}
