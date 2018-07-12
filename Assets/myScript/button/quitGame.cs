using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 此类用来退出游戏和滚动公告
/// </summary>
public class quitGame : MonoBehaviour
{
    GameObject gundong;

    void Start()
    {
        gundong = GameObject.Find("gunDongGongGao");
        Button btn1 = this.transform.Find("tuiChu").GetComponent<Button>();
        Button btn2 = this.transform.Find("zhuXiao").GetComponent<Button>();
        //if (Application.platform == RuntimePlatform.IPhonePlayer)
#if UNITY_IOS
        btn2.transform.localPosition = Vector3.Lerp(btn1.transform.localPosition, btn2.transform.localPosition, 0.5f);
        btn1.gameObject.SetActive(false);
#endif
#if UNITY_ANDROID
        btn1.onClick.AddListener(quit);
#endif
        btn2.onClick.AddListener(zhuXiao);
    }
	// Update is called once per frame
	void Update () {
        gongGao();
    }
    /// <summary>
    /// 退出
    /// </summary>
    void quit() {
        Application.Quit();
    }
    /// <summary>
    /// 注销
    /// </summary>
    public void zhuXiao()
    {
        weiXinLoad.show = true;
        SceneManager.LoadSceneAsync("landScene");
    }
    /// <summary>
    /// 滚动公告
    /// </summary>
    void gongGao()
    {
        //Debug.Log(gundong.transform.position);
        gundong.GetComponent<RectTransform>().sizeDelta = new Vector2(gundong.GetComponent<Text>().text.Length * 45.1f, 53.8f);
        gundong.transform.Translate(-Time.deltaTime * 2, 0, 0);
        if (gundong.transform.position.x < -11)
        {
            gundong.transform.position = new Vector3(11, gundong.transform.position.y, 0);
        }
    }
}
