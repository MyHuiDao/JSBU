using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class selectAreaDrag : MonoBehaviour,IEndDragHandler {

    int childCount;
    GameObject gameArea=null;
   

    // Use this for initialization
    void Start () {
        //gameArea = transform.Find("area").Find("gameArea").gameObject;
        // childCount = transform.Find("area").Find("gameArea").childCount;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnEndDrag(PointerEventData eventData)
    {
        float minDistance;
        for (int i = 0; i < childCount; i++)
        {
            gameArea.transform.GetChild(i);

        }
    }
}
