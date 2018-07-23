
using UnityEngine;
using UnityEngine.UI;


//此类是用来控制美人鱼中的UIbutton事件
public class UIbuttonEvent : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

        GameObject.Find("returnToMeiRenYuArea").GetComponent<Button>().onClick.AddListener(returnToMeiRenYuArea);
        GameObject.Find("yuJian").GetComponent<Button>().onClick.AddListener(Yujian);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 返回到美人鱼选择区域
    /// </summary>
    void returnToMeiRenYuArea()
    {
        //meiRenYuThreadDeal.instant.exitRoom();
        WebButtonSendMessege.instant().exitRoom();//告诉服务器，退出房间



    }
    /// <summary>
    /// 鱼鉴
    /// </summary>
    void Yujian()
    {
        GameObject.Find("yuJianShow").transform.localScale = Vector3.one;

    }
}
