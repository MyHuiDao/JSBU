using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UDragEnhanceView : EventTrigger
{
    private EnhanceScrollView enhanceScrollView;
    //public GameObject middleParent;
    public void SetScrollView(EnhanceScrollView view)
    {
        enhanceScrollView = view;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        GetComponent<Button>().enabled = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        GetComponent<Button>().enabled = false;
        if (enhanceScrollView != null)
            enhanceScrollView.OnDragEnhanceViewMove(eventData.delta);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        GetComponent<Button>().enabled = true;
        if (enhanceScrollView != null)
            enhanceScrollView.OnDragEnhanceViewEnd();
    }
}
