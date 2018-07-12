using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jishi : MonoBehaviour {


    float time = 0;
    int t=0;
    float speed;
    float A;
    float B;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        time += Time.deltaTime;
        if (time >= 1)
        {

            speed = (A + B * Mathf.Sin(t + 1));
            time = 0;
        }
	}
}
