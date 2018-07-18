using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loopPoint : MonoBehaviour {

    Text pText;
    int i = 0;
    float interval = 0.0f;
	void Start () {
        pText = transform.GetComponent<Text>();
        pText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        interval += Time.deltaTime;
        if (interval > 0.5f)
        {
            interval = 0;
            pText.text += ".";
            ++i;
            if(i>4)
            {
                i = 0;
                pText.text = "";
            }
        }
	}
}
