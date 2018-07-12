using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFlow : MonoBehaviour
{
    public GameObject hero;
    Vector3 offset;//计算摄像机与要跟随的对象之间的差距

    public static cameraFlow Instance = null;
    float earlySpeed = 8.9f;//摄像机移动的速度
    float laterSpeed = 11f;//摄像机移动的速度
    //public float smoothTime = 0.01f;  //摄像机平滑移动的时间
    private Vector3 cameraVelocity = Vector3.zero;
    Vector3 v;
    public bool isStart = true;
    float trackLong = 90.4f;
    float time = 0;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        offset = hero.transform.position - this.transform.position;
        v = hero.transform.localPosition;
      
    }


    void Update()
    {
        if (timeJiShi2.startGame)
        {
            
            if (isStart)
            {
               
                
                if (fishMove.isStartSpurt)
                {
                    hero.transform.Translate(Vector3.right * laterSpeed * Time.deltaTime);
                }
                else
                {
                    hero.transform.Translate(Vector3.right * earlySpeed * Time.deltaTime);
                }
               
                if (hero.transform.localPosition.x >= trackLong)
                {
                    isStart = false;

                }
            }
        }

        Move();
    }
    public void Move()
    {

        //this.transform.position = Vector3.MoveTowards(this.transform.position, hero.transform.position - offset, speed*Time.deltaTime);
        this.transform.position = hero.transform.position - offset;

    }
    public static Vector3 ab(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y, a.z * b.z);
    }
}
