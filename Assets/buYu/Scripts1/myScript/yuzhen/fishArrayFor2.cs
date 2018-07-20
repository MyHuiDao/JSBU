using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishArrayFor2 : MonoBehaviour {
    Vector3 v;
	// Use this for initialization
	void Start () {
        v = new Vector3(0.01f,0,0);	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(v);
	}
}
