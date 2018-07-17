using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class WithDraw_Sp : MonoBehaviour {
    public InputField[] withdraw;
    public static WithDraw_Sp instance;
    public bool return_blind;
    public InputField withdraw1;
    // Use this for initialization
    void Start ()
    {
        instance = this;
        GameObject.Find("rule_withdraw").GetComponent<Button>().onClick.AddListener(()=>Canvas_group("Withdraw_rule", "Right_Withdraw"));
        GameObject.Find("back_withdraw").GetComponent<Button>().onClick.AddListener(() => Canvas_group("Right_Withdraw","Withdraw_rule" ));
        GameObject.Find("withdraw_yzm").GetComponent<Button>().onClick.AddListener(() => mobile_yzm());
        GameObject.Find("conversion").GetComponent<Button>().onClick.AddListener(() => with_draw());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// 切换规则与提现
    /// </summary>
    /// <param name="names"></param>
    /// <param name="name2"></param>
    void Canvas_group(string names,string name2)
    {
        RootCanvas.canvas_group(transform.Find(names).GetComponent<CanvasGroup>(), true, 1);
        RootCanvas.canvas_group(transform.Find(name2).GetComponent<CanvasGroup>(), false, 0);
    }




    void mobile_yzm()
    {
        return_blind = true;
        if (netConnect.instance.m_state == login_state.visitor)
        {
           // netConnect.instance.Ani(24);
        }
        else if (hallHttp.instance.mobilenumber == true)
        {
            httpConnect.GET(this, httpConnect.URL + "/user/userMobileSendMsg?mobile=" + GameObject.Find("mobile_number").GetComponent<Text>().text, null, mobileSendMsg, httpError);
       
        }
        else 
        {
            transform.localScale = Vector3.zero;
            GameObject.Find("mobileBindle").transform.localScale = Vector3.one;
        }
       
    }
    void mobileSendMsg(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
            //netConnect.instance.Ani(5);
        }
        else if ((string)jso["code"] == "-1")
        {
            if ((string)jso["msg"] == httpConnect.net[8].data)
            {
                //netConnect.instance.Ani(8);
            }
            else if ((string)jso["msg"] == httpConnect.net[9].data)
            {
               // netConnect.instance.Ani(9);
            }
        }
    }





     void with_draw()
    {
        WWWForm form = new WWWForm();
        if (withdraw[0].text!=""&& withdraw[1].text != ""&& withdraw[2].text != ""&& withdraw[3].text != "")
        {
            form.AddField("cash", withdraw[0].text);
            form.AddField("zhifubaoAccount", withdraw[1].text);
            form.AddField("zhifubaoName", withdraw[2].text);
            form.AddField("yzCode", withdraw[3].text);
            byte[] b = form.data;
            httpConnect.GET(this, httpConnect.URL + "/user/getCash", b, STS, httpError);
        }
        else
        {
            //netConnect.instance.Ani(19);

        }
       
    }
    void STS(string str)
    {
        withdraw1.text = str;
        if (netConnect.instance.m_state == login_state.visitor)
        {
            //netConnect.instance.Ani(24);
        }
       // Debug.Log(str);
        JsonData jso = JsonMapper.ToObject(str);
        if ((string)jso["code"] == "0")
        {
           // netConnect.instance.Ani(22);
        }
        if ((string)jso["code"] == "-1")
        {
            if ((string)jso["msg"] == httpConnect.net[10].data)
            {
               // netConnect.instance.Ani(10);
            }
            if ((string)jso["msg"] == httpConnect.net[21].data)
            {
                //netConnect.instance.Ani(21);
            }
            if ((string)jso["msg"] == httpConnect.net[23].data)
            {
                //netConnect.instance.Ani(23);
            }
          
        }
        if ((string)jso["code"] == "-2")
        {
            JsonData js = jso["data"];
            Application.OpenURL(js.ToString());
        }
    }




    void httpError(string str)
    {
        
        Debug.Log(str);
    }
}
