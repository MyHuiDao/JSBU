using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFit : MonoBehaviour {

    const float devHeight = 14.4f;//设计的尺寸宽度
    const float devfWidth = 25.6f;//设计的尺寸高度

	void Start () {
        //获取屏幕的高
        float screenHeight = Screen.height;
        Debug.Log("screenHeight = " + screenHeight);

        //拿到相机正交属性设置摄像机大小
        float orthographisSize = this.GetComponent<Camera>().orthographicSize;

        //得到宽高比
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        //实际的宽高比和摄像机的orthographisSize值来计算出摄像机的宽度值
        float cameraWith = orthographisSize * 2 * aspectRatio;
        Debug.Log("camerawith = " + cameraWith);
        if(cameraWith < devfWidth)
        {
            //将尺寸宽度 / 2倍的宽高比=相机的大小
            orthographisSize = devfWidth / (2 * aspectRatio);
            Debug.Log("new orthographicSize = " + orthographisSize);
            this.GetComponent<Camera>().orthographicSize = orthographisSize;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
