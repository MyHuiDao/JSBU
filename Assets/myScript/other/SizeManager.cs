using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeManager : MonoBehaviour
{
    public System.Action OnSizeValueChange;
    private static SizeManager instance;
    float currentWidth;
    float currentHight;

    void Start()
    {

    }

    void Update()
    {

    }


    public static SizeManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<SizeManager>();
            return instance;
        }
    }

    private void OnGUI()
    {
        if (currentWidth != Screen.width || currentHight != Screen.height)
        {
            
            if (OnSizeValueChange != null)
            {
                OnSizeValueChange();
                //textView.sizeDelta = new Vector2(scrollView.rect.width, textView.rect.height);
                //glg.cellSize = new Vector2(textView.rect.width, textView.rect.height + 100);
                Debug.Log("尺寸有变化....");
                Debug.Log("sw:" + Screen.width + " sh:" + Screen.height);
            }
            currentWidth = Screen.width;
            currentHight = Screen.height;
        }
    }
}