//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;

//public class IAPExample : MonoBehaviour
//{

    //public List<string> productInfo = new List<string>();

    //[DllImport("__Internal")]
    //private static extern void TestMsg();//测试信息发送

    //[DllImport("__Internal")]
    //private static extern void TestSendString(string s);//测试发送字符串

    //[DllImport("__Internal")]
    //private static extern void TestGetString();//测试接收字符串

    //[DllImport("__Internal")]
    //private static extern void InitIAPManager();//初始化

    //[DllImport("__Internal")]
    //private static extern bool IsProductAvailable();//判断是否可以购买

    //[DllImport("__Internal")]
    //private static extern void RequstProductInfo(string s);//获取商品信息

    //[DllImport("__Internal")]
    //private static extern void BuyProduct(string s);//购买商品

    //测试从xcode接收到的字符串
    //void IOSToU(string s)
    //{
    //    Debug.Log("[MsgFrom ios]" + s);
    //}

    ////获取product列表
    //void ShowProductList(string s)
    //{
    //    productInfo.Add(s);
    //}
    //bool back = false;
    ////获取商品回执
    //void ProvideContent(string s)
    //{
    //    Debug.Log("[MsgFrom ios]proivideContent : " + s);
    //    back = true;
    //}


    //// Use this for initialization
    //void Start()
    //{
    //    //InitIAPManager();
    //}

    //void OnGUI()
    //{

    //    if (Btn("GetProducts"))
    //    {
    //        if (!IsProductAvailable())
    //            throw new System.Exception("IAP not enabled");
    //        productInfo = new List<string>();
    //        RequstProductInfo("com.aladdin.fishpocker1\tcom.aladdin.fishpocker2");
    //    }

    //    GUILayout.Space(40);

    //    if (back)
    //        GUI.Label(new Rect(10, 150, 100, 100), "Message back");

    //    for (int i = 0; i < productInfo.Count; i++)
    //    {
    //        if (GUILayout.Button(productInfo[i], GUILayout.Height(100), GUILayout.MinWidth(200)))
    //        {
    //            string[] cell = productInfo[i].Split('\t');
    //            Debug.Log("[Buy]" + cell[cell.Length - 1]);
    //            BuyProduct(cell[cell.Length - 1]);
    //            GUI.Label(new Rect(10, 10, 100, 200), string.Format("[Buy]{0}", cell[cell.Length - 1]));
    //        }
    //    }
    //}

    //bool Btn(string msg)
    //{
    //    GUILayout.Space(100);
    //    return GUILayout.Button(msg, GUILayout.Width(200), GUILayout.Height(100));
    //}
//}