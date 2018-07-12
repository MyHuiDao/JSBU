using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 此类是用来控制雪花效果的
/// </summary>
public class snow : MonoBehaviour {

    // Use this for initialization
  
	void Start () {
        this.transform.localPosition = new Vector3(Random.Range(-1251.7f, 1251.7f), Random.Range(701f, 853f),0f);
        Invoke("destroy",8);

	}
	
	// Update is called once per frame
	void Update () {
       
            move();

     
	}

    void move()
    {
        this.transform.Translate(new Vector3(0,-0.05f,0));
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
