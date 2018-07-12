using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scripts : MonoBehaviour
{
    private float time_clone;
    private GameObject water_clone;
    public Transform image;
    private float horizontal_x;//水平移动
    private float birth_x;//出生的X轴
    private float birth_y;//出生y轴
    private float Angles_z;//角度
    private float vertical_y;//垂直移动
    private float Scale_;//大小
    public int max;
    public int min;
    // Use this for initialization  
    void Start()
    {
        
        //  // 将最初的位置保存到oldPos 
        horizontal_x = Random.Range(-4, 4);
        vertical_y = Random.Range(min, max) * 0.1f;
        birth_x = Random.Range(-7, 10);
        birth_y = Random.Range(-2, -6);
        Angles_z = Random.Range(-20, 20);
        Scale_ = Random.Range(1, 3) * 0.01f;
        transform.eulerAngles = new Vector3(0, 0, Angles_z);
    }

    // Update is called once per frame  
    void Update()
    {
        transform.Translate(horizontal_x * 0.06f*Time.deltaTime, vertical_y * Time.deltaTime, 0);
        if (transform.position.y > -2)
        {
            GetComponent<CanvasGroup>().alpha -= 0.03f;
            if (GetComponent<CanvasGroup>().alpha == 0)
            {
                Destroy(gameObject,0.5f);
                 time_clone += Time.deltaTime;
                if (time_clone > 0.5f)
                {
                    water_clone = Instantiate(gameObject, new Vector3(birth_x, birth_y, 0), Quaternion.identity);
                    water_clone.transform.position = new Vector3(birth_x, birth_y, 0);
                    water_clone.transform.localScale = Vector3.one* Scale_;
                    water_clone.transform.SetParent(image);
                    water_clone.GetComponent<CanvasGroup>().alpha = 1;
                    water_clone.name = "Imagess";
                    time_clone = 0;
                }          
            }
        }   
    }
}