using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 保险柜
/// </summary>
public class coin : MonoBehaviour {
    public int [] safe_coin = new int[4];
    private int a;
	// Use this for initialization
	void Start () {
         a = (int)Mathf.Pow(100, 2);
        GameObject.Find("Toggle_pop").GetComponent<Toggle>().onValueChanged.AddListener((bool value) => Change_Toggle("Toggle_pop", value));
        GameObject.Find("Toggle_pus").GetComponent<Toggle>().onValueChanged.AddListener((bool value) => Change_Toggle("Toggle_pus", value));
        for (int i = 0; i < safe_coin.Length; i++)
        {
            int b = i;
            transform.GetChild(b).GetComponent<Button>().onClick.AddListener(() => control(a*safe_coin[b]));
        }
    }
	// Update is called once per frame
    void Change_Toggle(string name,bool abc)
    {
        GameObject.Find("popNum").GetComponent<InputField>().text = null;
        GameObject.Find("pusNum").GetComponent<InputField>().text = null;
        if (abc)
        {
            GameObject.Find(name).GetComponent<Toggle>().interactable = false;
            abc = true;
        }
        else
        {
            GameObject.Find(name).GetComponent<Toggle>().interactable = true;
        }
    }
    void control(int money)
    {
        if (int.Parse(transform.parent.GetChild(3).GetChild(5).GetComponent<Text>().text) > money)//存入
        {
            GameObject.Find("pusNum").GetComponent<InputField>().text = money.ToString();
        }
        else
        {
            GameObject.Find("pusNum").GetComponent<InputField>().text = transform.parent.GetChild(3).GetChild(5).GetComponent<Text>().text;
        }
        if (int.Parse(transform.parent.GetChild(3).GetChild(5).GetComponent<Text>().text) < 0)
        {
            GameObject.Find("pusNum").GetComponent<InputField>().text = "0";
            //netConnect.instance.Ani(17);
        }
        if (int.Parse(transform.parent.GetChild(3).GetChild(6).GetComponent<Text>().text) > money)//取出
        {
            GameObject.Find("popNum").GetComponent<InputField>().text = money.ToString();
        }
        else
        {
            GameObject.Find("popNum").GetComponent<InputField>().text = transform.parent.GetChild(3).GetChild(6).GetComponent<Text>().text;
        }
    }
}
