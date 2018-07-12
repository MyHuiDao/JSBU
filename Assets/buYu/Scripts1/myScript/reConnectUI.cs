using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reConnectUI : MonoBehaviour {

    public static bool isShowUi = false;
   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isShowUi)
        {
            if (GameObject.Find("UI") != null)
            {
                GameObject.Find("UI").transform.Find("netNotStead").gameObject.SetActive(true);
            }
            isShowUi = false;
        }
       
    }
}
