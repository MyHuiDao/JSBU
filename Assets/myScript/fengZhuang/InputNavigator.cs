using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UGUI Tab键切换InputField
/// </summary>
public class InputNavigator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private EventSystem system;
    private bool isSelect = false;
    public Direction direction = Direction.vertical;
    public enum Direction
    {
        vertical = 0,
        horizontal = 1
    }
    bool isPC = false;
    bool isAndroid = false;
    bool isIos = false;
    void Start()
    {
#if UNITY_EDITOR
        isPC = true;
#elif UNITY_IPHONE
                        isIos=true;
#elif UNITY_ANDROID
                         isAndroid=true;
#endif
        system = EventSystem.current;
    }


   
    void Update()
    {
        if (isPC)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && isSelect)
            {
                Selectable next = null;
                var current = system.currentSelectedGameObject.GetComponent<Selectable>();

                int mark = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1 : -1;
                Vector3 dir = direction == Direction.horizontal ? Vector3.left * mark : Vector3.up * mark;
                next = GetNextSelectable(current, dir);

                if (next != null)
                {
                    var inputField = next.GetComponent<InputField>();
                    if (inputField == null) return;
                    StartCoroutine(Wait(next));
                }
            }
            //}else if (isAndroid)
            //{
            //    if ( && isSelect)
            //    {
            //        Selectable next = null;
            //        var current = system.currentSelectedGameObject.GetComponent<Selectable>();

            //        int mark = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1 : -1;
            //        Vector3 dir = direction == Direction.horizontal ? Vector3.left * mark : Vector3.up * mark;
            //        next = GetNextSelectable(current, dir);

            //        if (next != null)
            //        {
            //            var inputField = next.GetComponent<InputField>();
            //            if (inputField == null) return;
            //            StartCoroutine(Wait(next));
            //        }
            //    }

        }
    }

    private static Selectable GetNextSelectable(Selectable current, Vector3 dir)
    {
        Selectable next = current.FindSelectable(dir);
        if (next == null)
            next = current.FindLoopSelectable(-dir);
        return next;
    }

    IEnumerator Wait(Selectable next)
    {
        yield return new WaitForEndOfFrame();
        system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelect = false;
    }
}
public static class Develop
{
    public static Selectable FindLoopSelectable(this Selectable current, Vector3 dir)
    {
        Selectable first = current.FindSelectable(dir);
        if (first != null)
        {
            current = first.FindLoopSelectable(dir);
        }
        return current;
    }
}