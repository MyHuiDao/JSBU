using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class star_itween : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
        //iTween.FadeTo(gameObject, iTween.Hash("alpha", 0f,"loopType","loop", "namedcolorvalue", iTween.NamedValueColor._Color, "time", 10f));
        GetComponent<Image>().DOFade(0, 1).SetLoops(-1,LoopType.Yoyo);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
