using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fish_go : MonoBehaviour {
    private float xx;
    public float left;
    public float right;
    public float chengji;
    // Use this for initialization
    void Start ()
    {
        xx = transform.position.x;
    }
	// Update is called once per frame
	void Update ()
    {
        swim();
    }
    public void swim()
    {
        float juli = Mathf.PingPong(Time.time * 2, 20);
        float m_rotate = xx - juli;
        transform.position = new Vector3(m_rotate*chengji, 0, 0);

        if (transform.position.x < left)
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        if (transform.position.x > right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
