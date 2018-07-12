using UnityEngine;
using System.Collections;
using System.IO;


/// <summary>
/// 下载图片并保存在本地
/// </summary>
public class landPicture : MonoBehaviour
{


    WWW www;                     //请求
    string filePath;             //保存的文件路径
    Texture2D texture2D;         //下载的图片
    public Transform m_tSprite;  //场景中的一个Sprite

    void Start()
    {
        //保存路径
        filePath = Application.dataPath + "/Resources/picture.jpg";
    }

    void Update()
    {
        //点击鼠标左键开始下载
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("开始下载");
            StartCoroutine(LoadImg());

        }
    }


    IEnumerator LoadImg()
    {
        //开始下载图片
        www = new WWW("http://hd.com/shoujikuang.png");//图像这里选择模式为RGB 16位  32位都行。图像大于10M的话在图像大小处把分辨率降低压缩。
        yield return www;

       
        //下载完成，保存图片到路径filePath
        texture2D = www.texture;
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);


        //将图片赋给场景上的Sprite
        Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        
        m_tSprite.GetComponent<SpriteRenderer>().sprite = tempSp;
        
        //Debug.Log("加载完成");

    }
}
