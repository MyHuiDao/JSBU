
using UnityEngine;
using System.Collections;
using ZXing;//引入库
using ZXing.QrCode;
using UnityEngine.UI;


public class t2 : MonoBehaviour {
    //定义Texture2D对象和用于对应网站的字符串
    public Texture2D encoded;
    public string Lastresult;
    //定义一个UI，来接收图片
    public RawImage ima;
    public Camera _camera;
    public Button _button;
    void Start()
    {
        encoded = new Texture2D(256, 256);
        Lastresult = "http://www.qq.com";
        _button.onClick.AddListener(Shake);
        //Shake();
    }

    //定义方法生成二维码
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }


    void Update()
    {
        var textForEncoding = Lastresult;
        if (textForEncoding != null)
        {
            //二维码写入图片
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            //生成的二维码图片附给RawImage
            ima.texture = encoded;

        }
    }

    //将图片画出来
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), encoded);
    }


    void Shake()
    {
        //键值对儿的形式保存iTween所用到的参数  
        Hashtable args = new Hashtable();  
        //摇摆的幅度  
        args.Add("amount", new Vector3(1, 0, 0));  
        //args.Add("x", 20);  
        //args.Add("y",10);  
        //args.Add("z", 2);  
      
        //是世界坐标系还是局部坐标系  
        args.Add("islocal", true);  
        //游戏对象是否将面向其方向  
        args.Add("orienttopath", true);  
        //面朝的对象  
        //args.Add("looktarget", new Vector3(1, 1, 1));  
        //args.Add("looktime", 5.0f);  
          
        //动画的整体时间。如果与speed共存那么优先speed  
        args.Add("time", 1f);  
        //延迟执行时间  
        //args.Add("delay", 0.1f);  
  
        //三个循环类型 none loop pingPong (一般 循环 来回)    
        //args.Add("loopType", "none");  
        //args.Add("loopType", "loop");   
        args.Add("loopType", iTween.LoopType.none);  
  
  
        //处理动画过程中的事件。  
        //开始动画时调用AnimationStart方法，5.0表示它的参数  
        args.Add("onstart", "AnimationStart");  
        args.Add("onstartparams", 5.0f);  
        //设置接受方法的对象，默认是自身接受，这里也可以改成别的对象接受，  
        //那么就得在接收对象的脚本中实现AnimationStart方法。  
        args.Add("onstarttarget", gameObject);  
  
  
        //动画结束时调用，参数和上面类似  
        args.Add("oncomplete", "AnimationEnd");  
        args.Add("oncompleteparams", "end");  
        args.Add("oncompletetarget", gameObject);  
  
        //动画中调用，参数和上面类似  
        args.Add("onupdate", "AnimationUpdate");  
        args.Add("onupdatetarget", gameObject);  
        args.Add("onupdateparams", true);  
  
        iTween.ShakePosition(_camera.gameObject, args);  
    }
}
