using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kefu : MonoBehaviour
{
    private Text m_text;
    private ContentSizeFitter content;
    // Use this for initialization
    void Start()
    {
        m_text = GetComponent<Text>();
        content = GetComponent<ContentSizeFitter>();
        word();
        Debug.Log(m_text.preferredWidth);
    }
    void word()
    {
        if (m_text.preferredWidth > 1000)
        {
            content.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            GetComponent<RectTransform>().sizeDelta = new Vector2(1000, GetComponent<RectTransform>().rect.height);
        }
    }
}
