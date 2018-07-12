using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyGold : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("destroyMy",5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void destroyMy()
    {
        Destroy(this.gameObject);
    }
}
