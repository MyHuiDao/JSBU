using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public RectTransform scrollView;
    public GridLayoutGroup glg;
    public RectTransform textView;

    private void OnEnable()
    {
        SizeManager.Instance.OnSizeValueChange += OnSizeValueChange;
    }

    private void OnDisable()
    {
        SizeManager.Instance.OnSizeValueChange -= OnSizeValueChange;
    }
    private void OnSizeValueChange()
    {
        textView.sizeDelta = new Vector2(scrollView.rect.width, textView.rect.height);
        glg.cellSize = new Vector2(textView.rect.width, textView.rect.height + 100);

    }

    // Update is called once per frame
    void Update()
    {

    }

}