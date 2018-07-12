using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_go_1 : MonoBehaviour {
    public int speed;
    private float a;
    // Use this for initialization
    void Start ()
    {
    }
	// Update is called once per frame
	void Update ()
    {
        a += Time.deltaTime;
        if (a > 2f)
        {
            swim();
        }
        //print(transform.position);
    }
    public void swim()
    {
        if (transform.position.x < -10f)
        {
            speed = 3;
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        if (transform.position.x > 10f)
        {
            speed = 3;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.Translate(new Vector3(-speed*Time.deltaTime,0, 0));
    }
}
