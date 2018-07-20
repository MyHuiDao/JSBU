using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishArrayTime : MonoBehaviour
{

    public bool isStartShow = false;
    public static fishArrayTime instance = null;
    float time = 0;
    void Start()
    {
        instance = this;
    }




    void Update()
    {

        if (isStartShow)
        {
            if (Mathf.Abs( time - 0 )<= 0.1f)
            {
                for (int i = 0; i <transform.childCount ; i++)
                {
                    if (transform.GetChild(i).name.Substring(0, 1) == "小")
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                }

            }
            if (Mathf.Abs(time - 8f) <= 0.1f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Substring(0, 1) == "红")
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                }

            }
            if (Mathf.Abs(time - 16) <= 0.1f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Substring(0, 1) == "蝴")
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                }
            }
            if (Mathf.Abs(time - 24f) <= 0.1f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Substring(0, 1) == "剑")
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                }
            }
            if (Mathf.Abs(time - 34) <= 0.1f)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Substring(0, 1) == "爱")
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                }
                fishArrayFor1.isStartMove = true;
            }

            time += Time.deltaTime;
        }
    }
}
