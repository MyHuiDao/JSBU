using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishCrown : MonoBehaviour
{


    public static fishCrown instance = null;
    public static bool fishAlreadyHave = false;
    public bool isStop = false;
    GameObject[] fish;
    public Sprite first;
    public Sprite second;

    // Use this for initialization
    void Start()
    {
        instance = this;
        isStop = false;
        fish = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {

        if (fishAlreadyHave)
        {
            fish[0] = GameObject.Find("fish1");
            fish[1] = GameObject.Find("fish2");
            fish[2] = GameObject.Find("fish3");
            fish[3] = GameObject.Find("fish4");

            fishAlreadyHave = false;

        }
        if (timeJiShi2.startGame)
        {
            if (!isStop)
            {


                for (int i = 0; i < 2; i++)
                {
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (fish[i].transform.position.x < fish[j].transform.position.x)
                        {
                            GameObject g1;
                            g1 = fish[i];
                            fish[i] = fish[j];
                            fish[j] = g1;
                        }

                    }
                }

                fish[0].transform.Find("crown").transform.GetComponent<SpriteRenderer>().sprite = first;
                fish[1].transform.Find("crown").transform.GetComponent<SpriteRenderer>().sprite = second;
                fish[2].transform.Find("crown").transform.GetComponent<SpriteRenderer>().sprite = null;
                fish[3].transform.Find("crown").transform.GetComponent<SpriteRenderer>().sprite = null;
            }
        }


    }
}
