using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class destroys : MonoBehaviour {
    public InputField url;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(() => money_st());
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void ceshi()
    {
        PlayerPrefs.DeleteAll();
    }
    void ceshi1()
    {
        Camera.main.cullingMask = 1 << 5;
    }
    public void money_st()
    {
        httpConnect.GET(this, httpConnect.URL+ "/user/getUserGoldBalance", null, STS, httpError);
    }
    void STS(string str)
    {
        JsonData jso = JsonMapper.ToObject(str);
        Debug.Log(str);   
    }
    void httpError(string str)
    {

        Debug.Log(str);
    }
}
