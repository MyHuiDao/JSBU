using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_effect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("destroy",0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
