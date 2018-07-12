using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t1 : MonoBehaviour {

    public GameObject[] destinationObj;
    GameObject desObj;
    bool isNextObj = true;
    int i = 0;
    Vector3 vector;
	void Start () {
        vector = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        if(isNextObj)
        {
            isNextObj = false;
            if (i < destinationObj.Length)
            {
                desObj = destinationObj[i];
                if (Mathf.Abs(Vector3.Distance(transform.localPosition, desObj.transform.localPosition)) < 3.1f)
                {
                    ++i;
                    transform.localPosition = desObj.transform.localPosition;
                    isNextObj = true;

                }
                if(i>=destinationObj.Length)
                {
                    transform.localPosition = vector;
                    i = 0;
                }
            }
        }
        if(i < destinationObj.Length)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, desObj.transform.localPosition, 10.1f);

        }

	}
}
