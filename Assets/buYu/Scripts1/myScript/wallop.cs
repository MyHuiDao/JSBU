using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void stopMove()
    {
        this.GetComponent<Animator>().speed = 0;
    }

}
