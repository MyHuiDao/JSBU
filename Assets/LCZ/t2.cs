using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class t2 : MonoBehaviour {
    bool isClick = false;
    Vector3 start;
    public GameObject btnObj;
	void Start () {
        btnObj.GetComponent<Button>().onClick.AddListener(onBtnClick);
        start = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onBtnClick()
    {
        Debug.Log("---------");
        isClick = !isClick;
        if(isClick)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(Screen.width / 2 - 50, 0, 0), "speed", 250f, "islocal", true, "easetype", "linear"));
        }else{
            iTween.Stop(gameObject,true);
            transform.localPosition = start;
            //iTween.EaseType.easeInOutBack
        }

        
    }
}
