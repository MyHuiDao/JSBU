using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishArrayFor1 : MonoBehaviour {
    Vector3 v;
    public static bool isStartMove=false;
    // Use this for initialization
    public static fishArrayFor1 instance=null;
    bool isGo = true;
    void Start () {
        instance = this;
        v = new Vector3(0.01f, 0, 0);


        
    }
	
	// Update is called once per frame
	void Update () {
        if (isStartMove)
        {
            transform.Translate(v);
        }

        if(  Mathf.Abs( this.transform.localPosition.x)<=0.1f&&isGo)
        {
            isStartMove = false;
            fishArrayTime.instance.isStartShow = true;
            isGo = false;
        }       
    }
}
