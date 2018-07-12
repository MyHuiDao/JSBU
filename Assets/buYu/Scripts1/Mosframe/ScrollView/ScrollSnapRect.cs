using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

#region 添加组件
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
[RequireComponent(typeof(ScrollRect))]
#endregion
public class ScrollSnapRect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region 公有变量
    [Tooltip("Threshold time for fast swipe in seconds")]
    public float fastSwipeThresholdTime = 0.3f;
    [Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
    public int fastSwipeThresholdDistance = 100;
    [Tooltip("How fast will page lerp to target position")]
    public float decelerationRate = 10f;
    public Transform pageIcon;
    #endregion
    #region 私有变量
    private int _fastSwipeThresholdMaxLimit;
    private ScrollRect _scrollRectComponent;
    private RectTransform _scrollRectRect;
    private RectTransform _container;
    private bool _horizontal;
    private int _pageCount;
    private int _currentPage;
    private bool _lerp;
    private Vector2 _lerpTo;
    private List<Vector2> _pagePositions = new List<Vector2>();
    private bool _dragging;
    private float _timeStamp;
    private Vector2 _startPosition;
    private List<RectTransform> _pagecard = new List<RectTransform>();
    #endregion
    /// <summary>
    /// 初始
    /// </summary>
    void Start()
    {

        _scrollRectComponent = GetComponent<ScrollRect>();
        _scrollRectRect = GetComponent<RectTransform>();
        _container = _scrollRectComponent.content;
        _pageCount = _container.childCount;
        if (_scrollRectComponent.horizontal && !_scrollRectComponent.vertical)
        {
            _horizontal = true;
        }
        else if (!_scrollRectComponent.horizontal && _scrollRectComponent.vertical)
        {
            _horizontal = false;
        }
        else
        {
            _horizontal = true;
        }
        _lerp = false;
        SetPagePositions();
        change_page();
    }
    /// <summary>
    /// 进行
    /// </summary>
    void Update()
    {
        //test();
        if (_lerp)
        {
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            _container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);
            if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f)
            {
                _container.anchoredPosition = _lerpTo;
                _lerp = false;
                _scrollRectComponent.velocity = Vector2.zero;
            }
        }
    }

    public void Test()
    {
        var a = Input.GetTouch(0);
        EventSystem.current.IsPointerOverGameObject(a.fingerId);
    }
    /// <summary>
    /// 添加
    /// </summary>
    void change_page()
    {
        for (int i = 0; i < pageIcon.childCount; i++)
        {
            RectTransform image = pageIcon.GetChild(i).GetComponent<RectTransform>();
            _pagecard.Add(image);
        }
    }
    /// <summary>
    /// 页码位置
    /// </summary>
    private void SetPagePositions()
    {
        int width = 0;
        int height = 0;
        int offsetX = 0;
        int offsetY = 0;
        int containerWidth = 0;
        int containerHeight = 0;

        if (_horizontal)
        {
            width = (int)_scrollRectRect.rect.width;
            offsetX = width / 2;
            containerWidth = width * _pageCount;
            _fastSwipeThresholdMaxLimit = width;
        }
        else
        {
            height = (int)_scrollRectRect.rect.height;
            offsetY = height / 2;
            containerHeight = height * _pageCount;
            _fastSwipeThresholdMaxLimit = height;
        }
        Vector2 newSize = new Vector2(containerWidth, containerHeight);
        _container.sizeDelta = newSize;
        Vector2 newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
        _container.anchoredPosition = newPosition;
        _pagePositions.Clear();
        for (int i = 0; i < _pageCount; i++)
        {
            RectTransform child = _container.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;
            if (_horizontal)
            {
                childPosition = new Vector2(i * width - containerWidth / 2+ offsetX, 0f);
            }
            else
            {
                childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
            }
            child.anchoredPosition = childPosition;
            _pagePositions.Add(-childPosition);
        }
    }
  
    /// <summary>
    /// 跳转页码
    /// </summary>
    /// <param name="aPageIndex"></param>
    private void LerpToPage(int aPageIndex)
    {
        aPageIndex = Mathf.Clamp(aPageIndex, 1, _pageCount);
        _lerpTo = _pagePositions[aPageIndex - 1];
        _lerp = true;
        _currentPage = aPageIndex;
    }
    ///// <summary>
    ///// 最后边的按钮方法
    ///// </summary>
    private void NextScreen()
    {
        LerpToPage(_currentPage + 1);
    }
    /// <summary>
    /// 最左边的按钮方法
    /// </summary>
    private void PreviousScreen()
    {
        LerpToPage(_currentPage - 1);
    }
    //------------------------------------------------------------------------
    private int GetNearestPage()
    {
        Vector2 currentPosition = _container.anchoredPosition;
        float distance = float.MaxValue;
        int nearestPage = _currentPage;
        for (int i = 0; i < _pagePositions.Count; i++)
        {
            float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
            if (testDist < distance)
            {
                distance = testDist;
                nearestPage = i;
            }
        }
        return nearestPage;
    }
    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="aEventData"></param>
    public void OnBeginDrag(PointerEventData aEventData)
    {
        _lerp = false;
        _dragging = false;
    }
    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="aEventData"></param>
    public void OnEndDrag(PointerEventData aEventData)
    {
        float difference;
        if (_horizontal)
        {
            difference = _startPosition.x - _container.anchoredPosition.x;
        }
        else
        {
            difference = -(_startPosition.y - _container.anchoredPosition.y);
        }
        if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
            Mathf.Abs(difference) > fastSwipeThresholdDistance &&
            Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit)
        {
            if (difference > 0)
            {
                NextScreen();
            }
            else
            {
                PreviousScreen();
            }
        }
        _dragging = false;
    }

    //------------------------------------------------------------------------
    public void OnDrag(PointerEventData aEventData)
    {
        if (!_dragging)
        {
            _dragging = true;
            _timeStamp = Time.unscaledTime;
            _startPosition = _container.anchoredPosition;
        }
    }
}
