using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine.UI;

public class ceshiTiaoZhuan : MonoBehaviour {

	// Use this for initialization
	void Start () {

        
      
       


    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void dd()
    {

        //WWW a = new WWW("http://showdoc.hd.com/");
        //Application.OpenURL(a.url);

        // Application.OpenURL("http://showdoc.hd.com/");
        //System.Diagnostics.Process.Start("IExplore.exe", "http://www.sina.com.cn/");



        //Application.ExternalEval(("window.open('http://math.xpu.owvlab.net/virexp/s/exp/20177261.exe','_self')"));



        string t=GameObject.Find("fuzhi").GetComponent<Text>().text;

        TextEditor te = new TextEditor();
        te.content = new GUIContent(t);
        te.SelectAll();
        te.Copy();
    }


   
}
