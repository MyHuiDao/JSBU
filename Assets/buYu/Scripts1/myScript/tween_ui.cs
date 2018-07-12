using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class tween_ui : MonoBehaviour {
    public Transform ui_tween;
    private bool gb;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void method()
    {
        if (gb == true)
        {
            gb = false;
            ui_tween.DOScale(0, 0.1f);
            ui_tween.DOLocalMoveY(569, 0.1f).SetEase(Ease.Linear);
        }
        else
        {

            ui_tween.DOScale(1, 0.1f);
            ui_tween.DOLocalMoveY(70, 0.1f).SetEase(Ease.Linear);
            gb = true;
        }
     


    }
}
