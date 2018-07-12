using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iweet : MonoBehaviour {

    Vector3[] path= { new Vector3(2,2,0), new Vector3(4, 4, 0), new Vector3(6, 2, 0), new Vector3(5, 1, 0), new Vector3(9, 6, 0), new Vector3(5, 8, 0) };
	// Use this for initialization
	void Start () {
          tween();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void tween()
    {

        iTween.MoveTo(gameObject, iTween.Hash("path", path, "speed", 2,  "orienttopath", true, "movetopath", false, "looktime", 0.6, "easetype", "easeInOutSine"));
    }
}
