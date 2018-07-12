using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// 此类用来进行对moudle模型类的消息的处理和显示
/// </summary>
public class view : MonoBehaviour
{


    bool isReceiveMesege;
    string tiYantext;
    string jinBitext;
    string idtext;
    string nametext;
    string heading;
    string userType;
    string isVip;
    string myID;
    public static view instance = null;
    void Awake()
    {
        isReceiveMesege = false;
        instance = this;
    }


    private void Update()
    {
        if (isReceiveMesege)
        {
            if (GameObject.Find("tiYanMoney1").GetComponent<Text>().text != tiYantext)
            {

                GameObject.Find("tiYanMoney1").GetComponent<Text>().text = tiYantext;
            }
            if (GameObject.Find("jinBiMoney1").GetComponent<Text>().text != jinBitext)
            {

                GameObject.Find("jinBiMoney1").GetComponent<Text>().text = jinBitext;
            }
            if (GameObject.Find("IDtext").GetComponent<Text>().text != idtext)
            {

                GameObject.Find("IDtext").GetComponent<Text>().text = idtext;
            }
            if (GameObject.Find("nameText").GetComponent<Text>().text != nametext)
            {

                GameObject.Find("nameText").GetComponent<Text>().text = nametext;
            }
            //if (GameObject.Find("touXiang").GetComponent<Image>().name != heading)
            //{

            //    GameObject.Find("nameText").GetComponent<Text>().text = nametext;
            //}
            if (isVip == "0")
            {

                GameObject.Find("VIP").transform.localScale = Vector3.zero;
            }
            else
            {
                GameObject.Find("VIP").transform.localScale = Vector3.one;
            }

            isReceiveMesege = false;
        }



    }
    /// <summary>
    /// 显示玩家信息
    /// </summary>
    /// <param name="_code">id</param>
    /// <param name="_experience">体验币</param>
    /// <param name="_gold">金币</param>
    /// <param name="_heading">头像</param>
    /// <param name="_id">ID主键</param>
    /// <param name="_nickName">昵称</param>
    /// <param name="_userType">玩家类型，免费玩家：free,付费玩家：pay</param>
    /// /// <param name="_isVip">是否为VIP</param>
    public void viewDo10000(int _code, int _experience, int _gold, string _heading, string _id, string _nickName, string _userType, string _isVip)
    {

        isReceiveMesege = true;
        tiYantext = _experience.ToString();
        jinBitext = _gold.ToString();
        idtext = _code.ToString();
        nametext = _nickName;
        heading = _heading;
        userType = _userType;
        isVip = _isVip;
        myID = _id;
    }

    public void viewDo10001(int _code, int _experience, int _gold, string _heading, string _id, string _nickName, string _userType, string _isVip)
    {


        tiYantext = _experience.ToString();
        jinBitext = _gold.ToString();
        idtext = _code.ToString();
        nametext = _nickName;
        heading = _heading;
        userType = _userType;
        isVip = _isVip;
        myID = _id;
    }


}
