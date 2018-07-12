using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_go_1 : MonoBehaviour {
    public float xx;
    private GameObject water_clone;
    public Transform birth;
    public Transform image;
    private float time_clone;
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
    }
    public void swim()
    {
        transform.Translate(new Vector3(0, xx*Time.deltaTime, 0));
       // print(transform.position);
        if (transform.position.y >5)
        {
            Destroy(gameObject,3f);
            time_clone += Time.deltaTime;
            if (time_clone > 3)
            {
                water_clone = Instantiate(gameObject, birth.position, Quaternion.identity);
                water_clone.transform.position= birth.position;
                water_clone.transform.localScale = Vector3.one;
                water_clone.transform.SetParent(image);
                time_clone = 0;
            }
        }
    }
}
