using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnhanceScrollView : MonoBehaviour
{
    public enum InputSystemType
    {
        UGUIInput,         // use UDragEnhanceView for each item to get drag event
    }
 
    private  InputSystemType inputType = InputSystemType.UGUIInput;// 枚举（暂时没用）

    public AnimationCurve scaleCurve;    // 大小曲线
   
    public AnimationCurve positionCurve; // 位置曲线
    
    public AnimationCurve depthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));//透明度曲线

    [Tooltip("The Start center index")]
    public int startCenterIndex = 0;//子对象第一个显示的图标

    public float cellWidth = 20f;//两图标的间距

    private float totalHorizontalWidth = 500.0f;//自身宽度

    public float yFixedPositionValue = 46.0f;//Y轴坐标

    public float lerpDuration = 0.2f;//灵敏度

    private float mCurrentDuration = 0.0f;

    private int mCenterIndex = 0;

    public bool enableLerpTween = true;

    private EnhanceItem curCenterItem;

    private EnhanceItem preCenterItem;

    private bool canChangeItem = true;

    private float dFactor = 0.2f;

    private float originHorizontalValue = 0.1f;

    public float curHorizontalValue = 0.5f;//滑动的变量

    private int depthFactor = 5;
    // Drag enhance scroll view
    [Tooltip("Camera for drag ray cast")]
    public Camera sourceCamera;//摄影机

    public List<EnhanceItem> listEnhanceItems;//子对象图标

    private List<EnhanceItem> listSortedItems = new List<EnhanceItem>();

    private static EnhanceScrollView instance;
    public static EnhanceScrollView GetInstance//单例
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canChangeItem = true;
        for(int i = 0; i < transform.childCount; i++)
        {
            listEnhanceItems.Add(transform.GetChild(i).GetComponent<EnhanceItem>());
        }
        int count = listEnhanceItems.Count;
        dFactor = (Mathf.RoundToInt((1f / count) * 10000f)) * 0.0001f;
        mCenterIndex = count / 2;
        if (count % 2 == 0)
            mCenterIndex = count / 2 - 1;
        int index = 0;
        for (int i = count - 1; i >= 0; i--)
        {
            listEnhanceItems[i].CurveOffSetIndex = i;
            listEnhanceItems[i].CenterOffSet = dFactor * (mCenterIndex - index);
            listEnhanceItems[i].SetSelectState(false);
            GameObject obj = listEnhanceItems[i].gameObject;

            if (inputType == InputSystemType.UGUIInput)
            {
                UDragEnhanceView script = obj.GetComponent<UDragEnhanceView>();
                if (script != null)
                    script.SetScrollView(this);
            }
            index++;
        }

        // set the center item with startCenterIndex
        if (startCenterIndex < 0 || startCenterIndex >= count)
        {
            Debug.LogError("## startCenterIndex < 0 || startCenterIndex >= listEnhanceItems.Count  out of index ##");
            startCenterIndex = mCenterIndex;
        }
        // sorted items
        listSortedItems = new List<EnhanceItem>(listEnhanceItems.ToArray());
        totalHorizontalWidth = cellWidth * count;//间距变化
        curCenterItem = listEnhanceItems[startCenterIndex];
        curHorizontalValue = 0.5f - curCenterItem.CenterOffSet;
        LerpTweenToTarget(0f, curHorizontalValue, false);
    }

    private void LerpTweenToTarget(float originValue, float targetValue, bool needTween = false)//滑动主要方法
    {
        if (!needTween)
        {
            SortEnhanceItem();
            originHorizontalValue = targetValue;
            UpdateEnhanceScrollView(targetValue);
            this.OnTweenOver();
        }
        else
        {
            originHorizontalValue = originValue;
            curHorizontalValue = targetValue;
            mCurrentDuration = 0.0f;
        }
        enableLerpTween = needTween;
    }

    public void DisableLerpTween()
    {
        this.enableLerpTween = false;
    }
    /// 
    /// Update EnhanceItem state with curve fTime value
    /// 
    public void UpdateEnhanceScrollView(float fValue)
    {
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            EnhanceItem itemScript = listEnhanceItems[i];
            float xValue;
            if (this.name== "areaScrollView")
            {
                xValue= GetXPosValue(fValue, itemScript.CenterOffSet) - 250 * listEnhanceItems.Count;//X轴位置变化
            }
            else
            {
                 xValue = GetXPosValue(fValue, itemScript.CenterOffSet) - 120 * listEnhanceItems.Count;//X轴位置变化
            }
         
            float scaleValue = GetScaleValue(fValue, itemScript.CenterOffSet);
            float depthCurveValue = depthCurve.Evaluate(fValue + itemScript.CenterOffSet);
            itemScript.UpdateScrollViewItems(xValue, depthCurveValue, depthFactor, listEnhanceItems.Count, yFixedPositionValue, scaleValue);
        }
    }

    void Update()
    {
        if (enableLerpTween)
            TweenViewToTarget();
    }

    private void TweenViewToTarget()
    {
        mCurrentDuration += Time.deltaTime;
        if (mCurrentDuration > lerpDuration)
            mCurrentDuration = lerpDuration;

        float percent = mCurrentDuration / lerpDuration;
        float value = Mathf.Lerp(originHorizontalValue, curHorizontalValue, percent);
        UpdateEnhanceScrollView(value);
        if (mCurrentDuration >= lerpDuration)
        {
            canChangeItem = true;
            enableLerpTween = false;
            OnTweenOver();
        }
    }

    private void OnTweenOver()
    {
        if (preCenterItem != null)
            preCenterItem.SetSelectState(false);
        if (curCenterItem != null)
            curCenterItem.SetSelectState(true);
    }

    // Get the evaluate value to set item's scale
    private float GetScaleValue(float sliderValue, float added)
    {
        float scaleValue = scaleCurve.Evaluate(sliderValue + added);
        return scaleValue;
    }

    // Get the X value set the Item's position
    private float GetXPosValue(float sliderValue, float added)
    {
        float evaluateValue = positionCurve.Evaluate(sliderValue + added) * totalHorizontalWidth;
        return evaluateValue;
    }

    private int GetMoveCurveFactorCount(EnhanceItem preCenterItem, EnhanceItem newCenterItem)
    {
        SortEnhanceItem();
        int factorCount = Mathf.Abs(newCenterItem.RealIndex) - Mathf.Abs(preCenterItem.RealIndex);
        return Mathf.Abs(factorCount);
    }

    // sort item with X so we can know how much distance we need to move the timeLine(curve time line)
    static public int SortPosition(EnhanceItem a, EnhanceItem b) { return a.transform.localPosition.x.CompareTo(b.transform.localPosition.x); }
    private void SortEnhanceItem()
    {
        listSortedItems.Sort(SortPosition);
        for (int i = listSortedItems.Count - 1; i >= 0; i--)
            listSortedItems[i].RealIndex = i;
    }

    public float factor = 0.001f;
    // On Drag Move
    public void OnDragEnhanceViewMove(Vector2 delta)//滑动增量控制
    {
        if(this.name== "areaScrollView")
        {
            if (Mathf.Abs(delta.x) > 0.0f)
            {
                curHorizontalValue += delta.x * factor*0.3f ;
                LerpTweenToTarget(0.0f, curHorizontalValue, false);
            }
        }
        else
        {
            if (Mathf.Abs(delta.x) > 0.0f)
            {
                curHorizontalValue += delta.x * factor * 0.1f;
                LerpTweenToTarget(0.0f, curHorizontalValue, false);
            }
        }
       
    }
    // On Drag End
    public void OnDragEnhanceViewEnd()//结束拖拽
    {
        int closestIndex = 0;
        float value = (curHorizontalValue - (int)curHorizontalValue);
        float min = float.MaxValue;
        float tmp = 0.5f * (curHorizontalValue < 0 ? -1 : 1);
        for (int i = 0; i < listEnhanceItems.Count; i++)
        {
            float dis = Mathf.Abs(Mathf.Abs(value) - Mathf.Abs((tmp - listEnhanceItems[i].CenterOffSet)));
            if (dis < min)
            {
                closestIndex = i;
                min = dis;
            }
        }
        originHorizontalValue = curHorizontalValue;
        float target = ((int)curHorizontalValue + (tmp - listEnhanceItems[closestIndex].CenterOffSet));
        preCenterItem = curCenterItem;
        curCenterItem = listEnhanceItems[closestIndex];
        LerpTweenToTarget(originHorizontalValue, target, true);
        canChangeItem = false;
        if(this.name!= "areaScrollView")
        {
            Music_Control.effect_music.Play();

        }else{
            buYuMusicContral.instant.allYinXiao[23].Play();
        }
      
    }
}