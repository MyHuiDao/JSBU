using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animat : MonoBehaviour {

    public Animator Ani;
    float ani_time;
	// Use this for initialization
	void Start ()
    {
       // Ani.enabled = true;
        //Debug.Log(Ani.GetCurrentAnimatorStateInfo(0).length);
    }
	
	// Update is called once per frame
	void Update ()
    {
         ani_time += Time.deltaTime;
        if (ani_time>1.65f)
        {
            Ani.enabled = false;
        }
        if(ani_time> 3.5f)
        {
            Ani.enabled = true;
            ani_time = 0;
        }
    }
}
