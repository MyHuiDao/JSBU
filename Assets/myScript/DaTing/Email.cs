using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Email : MonoBehaviour {
    public  List<int> record=new List<int>();
    private int b;
	// Use this for initialization
	void Start ()
    {
        record_email();
        b = PlayerPrefs.GetInt("count");
        for(int i=0; i < b; i++)
        {
            record.Add(PlayerPrefs.GetInt("shuju"+i.ToString()));
        }
        for (int i = 0; i < record.Count; i++)
        {
            transform.GetChild(record[i]).GetComponent<Image>().sprite = Resources.Load<Sprite>("Rank/read");
        }
    }
	
	// Update is called once per frame
	void Update () {
    }
    /// <summary>
    /// 记录邮件
    /// </summary>
    void record_email()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int a = i;
            transform.GetChild(a).GetComponent<Button>().onClick.AddListener(() => 
            {
                RootCanvas.canvas_group(GameObject.Find("email_view").GetComponent<CanvasGroup>(), false, 0);
                RootCanvas.canvas_group(GameObject.Find("email").GetComponent<CanvasGroup>(), true, 1);
                transform.GetChild(a).GetComponent<Image>().sprite = Resources.Load<Sprite>("Rank/read");
                GameObject.Find("email").transform.GetChild(0).GetComponent<Text>().text = transform.GetChild(a).GetChild(2).GetComponent<Text>().text;
                GameObject.Find("email").transform.GetChild(1).GetComponent<Text>().text = transform.GetChild(a).GetChild(0).GetComponent<Text>().text;
                ss(a);
            });
        }    
    }
    void ss(int ab)
    {
        if (!record.Contains(ab))
        {
            record.Add(ab);            
        }
        for (int i = 0; i < record.Count; i++)
        {
            PlayerPrefs.SetInt("shuju"+i.ToString(), record[i]);
        }
            PlayerPrefs.SetInt("count", record.Count);
    }
}
