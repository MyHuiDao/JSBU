
using UnityEngine;
public class paomadeng : MonoBehaviour
{
    #region 初始化
    private Vector3[] vec = new Vector3[4];
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            int a = i;
            vec[i] = transform.parent.GetChild(4).GetChild(a).transform.position;
        }
        a();
    }
    #endregion
    #region 跑马灯的方法
    public void a()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            simple(i, i, "b");
        }
    }

    public void b()
    {
        simple(0, 3, "c");
        for (int i = 1; i < transform.childCount; i++)
        {
            simple(i, i - 1, "c");
        }
    }

    public void c()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < 2)
            {
                simple(i, i + 2, "d");
            }
            else
            {
                simple(i, i - 2, "d");
            }
        }
    }

    public void d()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            simple(i, i + 1, "a");
        }
        simple(3, 0, "a");
    }
    #endregion
    #region 通用方法
    void simple(int code, int index, string method)
    {
        iTween.MoveTo(transform.GetChild(code).gameObject, iTween.Hash("position", vec[index], "time", 1, "delay", 4, "oncomplete", method, "oncompletetarget", gameObject));
    }
    #endregion
}