using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fail : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(method);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void method()
    {
        weiXinLoad.show = true;
        SceneManager.LoadSceneAsync("landScene");
        Destroy(gameObject);
        
    }
}
