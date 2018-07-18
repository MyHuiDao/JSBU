using UnityEngine;

/**
 * 枪旋转脚本
 * 枪管的方向随着鼠标的位置旋转
 * 挂在每一个枪上
 * */
public class GunFollow : MonoBehaviour
{
    public RectTransform UGUICanvas;
    public Camera mainCamera;
    public static int posWeizhi;//自己在那个位置
    public static float angle;//需要点击一下炮弹发给服务器的角度

    Vector3 mousePos;   //鼠标点转换后的世界坐标
    private void Start()
    {

        UGUICanvas = GameObject.Find("Order90Canvas").GetComponent<RectTransform>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        if (GameController.Instance.isXuanZhongFish)//自动锁定，返回的是鱼跟炮的角度
        {
           
            mousePos = GameController.Instance.lockFish.transform.position;
            getAngle();
        }
        else
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(UGUICanvas, new Vector2(Input.mousePosition.x, Input.mousePosition.y), mainCamera, out mousePos);
            getAngle();
        }      
    }

    void getAngle()
    {
       
        
        float z;        //枪管应该循转的角度，from和to的连线和它们一个指定轴向的夹角
        if (mousePos.x > transform.position.x)  //右边 mousePos - transform.position是负数
        {
            //from和to的连线和它们一个指定轴向的夹角
            z = -Vector3.Angle(Vector3.up, mousePos - transform.position);
            angle = z;
        }
        else
        {   //左边
            z = Vector3.Angle(Vector3.up, mousePos - transform.position);
            angle = z;
        }    
        if (posWeizhi ==1)
        {
            if (z < -90 || z > 90)
            {
                transform.localRotation = Quaternion.Euler(0, 0, z);
            }
        }
        else
        {
            if (z > -90 && z < 90)
            {
                transform.localRotation = Quaternion.Euler(0, 0, z);
            }
        }
    }
}

