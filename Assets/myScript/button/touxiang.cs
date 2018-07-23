using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touxiang : MonoBehaviour {
    private Sprite head_sprite;
    private Text m_text;
    private ContentSizeFitter content;
    public  bool add;
    public static touxiang instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
        head_photos();
        m_text = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        content = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<ContentSizeFitter>();
        word();
    }
	// Update is called once per frame
	void Update ()
    {
        if (add == true)
        {
            test2();
            add = false;
        }
    }
    void head_photos()//聊天头像
    {
        if (netConnect.instance.m_state == login_state.visitor)
        {
            transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = weiXinLoad.instance.headSprite;//httpView.touXiang;
        }
        if (netConnect.instance.m_state == login_state.wechat)
        {
            StartCoroutine(image(httpView.head_));
        }
    }
    IEnumerator image(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(www.texture);
        head_sprite = Sprite.Create(www.texture, new Rect(0, 0, 132, 132), Vector2.zero);
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite= head_sprite;
    }
    void word()//控制聊天框长度
    {
      if(m_text.preferredWidth >1000)
        {
            content.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(1000, GetComponent<RectTransform>().rect.height);        
        }
    }
    void test2()//显示当前聊天内容
    {    
        float tt = transform.parent.GetComponent<RectTransform>().sizeDelta.y - transform.parent.GetComponent<RectTransform>().position.y;
         transform.parent.GetComponent<RectTransform>().Translate(0, tt/2f, 0);
    }
}
