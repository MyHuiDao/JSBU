using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yuzhenThreeMove : MonoBehaviour
{

    float x;
    float y;
    float z;
    float moveSpeed = 0.01f;
    public int gj;
    public static bool isStartMove = false; 
    // Use this for initialization
    void Start()
    {
        isStartMove = false;
        x = this.transform.position.x;
        y = this.transform.position.y;
        z = 90;

    }

    // Update is called once per frame
    void Update()
    {
        if (isStartMove)
        {
            if (gj == 1)
            {
                x += moveSpeed;
                y = 3 * Mathf.Cos(x / (18 / Mathf.PI) + Mathf.PI / 2);

                transform.rotation = Quaternion.Euler(-Mathf.Atan((y - this.transform.position.y) / moveSpeed) * 180 / Mathf.PI, 90, 0);

            }
            if (gj == 2)
            {
                x += moveSpeed;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            if (gj == 3)
            {
                x += moveSpeed;
                y = Mathf.Sin(x / (18 / Mathf.PI) + Mathf.PI / 2) * (2.0f / 3.0f) + (1.14f);

                transform.rotation = Quaternion.Euler(-Mathf.Atan((y - this.transform.position.y) / moveSpeed) * 180 / Mathf.PI, 90, 0);
            }
            if (gj == 4)
            {
                x += moveSpeed;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            if (gj == 5)
            {
                x += moveSpeed;
                y = -(Mathf.Sin(x / (18 / Mathf.PI) + Mathf.PI / 2) * (2.0f / 3.0f) + (1.14f));

                transform.rotation = Quaternion.Euler(-Mathf.Atan((y - this.transform.position.y) / moveSpeed) * 180 / Mathf.PI, 90, 0);
            }
            if (gj == 6)
            {
                x += moveSpeed;
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            if (gj == 7)
            {
                x += moveSpeed;
                y = -3 * Mathf.Cos(x / (18 / Mathf.PI) + Mathf.PI / 2);

                transform.rotation = Quaternion.Euler(-Mathf.Atan((y - this.transform.position.y) / moveSpeed) * 180 / Mathf.PI, 90, 0);
            }

             transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(x, y, z), Time.deltaTime);

        }
    }
}
