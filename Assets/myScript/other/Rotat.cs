using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotat : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetChild(3).gameObject.GetComponent<Image>().SetNativeSize();
    }
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(2).transform.Rotate(0, 0, 1);
       
    }
}
