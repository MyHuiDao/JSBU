using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using CClient;
using UnityEngine.EventSystems;

/**
 * 该脚本用来控制游戏主界面中backButton,settingButton的UI脚本
 * 挂在ScriptHolder上
 * */
public class MainSceneUI : MonoBehaviour
{
    //  public Toggle muteToogle;
    public GameObject settingPanel;
    public Sprite[] m_sprite;
    public Sprite[] switch_sprite;
    public Image[] switch_image;
    //public Image switch_image2;
    private bool test;
    public Image m_image;
    private bool abc;
    private bool def;
    [SerializeField] Transform m_panel;
    private static MainSceneUI instance;
    public static MainSceneUI Instance
    {
        get
        {
            return instance;
        }
    }
    //Animation SkillFrame = null;
    bool isMoveSkill = false;
    GameObject skillFrame = null;
    GameObject skillButton = null;
    bool isOpenLvPanel = false;
    float time = 0;
    //public Vector3 location = new Vector3(-284.65f, 150.8067f, 0);
    void Start()
    {

        instance = this;
        skillButton = GameObject.Find("popOrPush");
        skillFrame = GameObject.Find("SkillFrame");
        GameObject.Find("popOrPush").GetComponent<Button>().onClick.AddListener(delegate () { skillFrameMove(isMoveSkill); });

    }


    private void Update()
    {
        if (isOpenLvPanel)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                m_image.sprite = m_sprite[1];
                test = false;
                tween();
                time = 0;
                isOpenLvPanel = false;
            }
        }
        if (Input.GetMouseButtonDown(0)&& EventSystem.current.IsPointerOverGameObject() == false)
        {
            skillButton.transform.localScale = Vector3.one;
            skillFrame.transform.DOLocalMoveX(727.8f, 0.2f);
            isMoveSkill = true; 
        }
    }
    /// <summary>
    /// 技能框移动
    /// </summary>
    void skillFrameMove(bool _ismove)
    {
        if (_ismove)
        {
            skillButton.transform.localScale = new Vector3(-1, 1, 1);
            skillFrame.transform.DOLocalMoveX(619.25f, 0.2f);
            isMoveSkill = false;
        }
        else
        {
            skillButton.transform.localScale = Vector3.one;
            skillFrame.transform.DOLocalMoveX(727.8f, 0.2f);
            isMoveSkill = true;
        }

    }
    public void SwitchMute(bool isOn)
    {
        AudioManager.Instance.SwitchMuteState(isOn);
    }


    //设置按钮
    public void OnSettingButtonDown()
    {
        settingPanel.SetActive(true);
    }

    //设置中的close button
    public void OnCloseButtonDown()
    {
        settingPanel.SetActive(false);
    }
    /// <summary>
    /// 调出设置界面
    /// </summary>
    public void return_sprite()
    {
        if (test == false)
        {
            m_image.sprite = m_sprite[0];//代表图标
            test = true;
            tween_1();
        }
        else
        {
            m_image.sprite = m_sprite[1];
            test = false;
            tween();
        }
    }
    /// <summary>
    /// 设置面板界面关闭
    /// </summary>
    public void tween()
    {
        m_panel.DOScale(0, 0.1f);
        m_panel.DOLocalMoveY(462, 0.1f).SetEase(Ease.Linear);



    }
    /// <summary>
    /// 设置面板界面打开
    /// </summary>
    public void tween_1()
    {
        //m_image.sprite = m_sprite[1];
        m_panel.DOScale(1f, 0.1f);
        m_panel.DOLocalMoveY(25, 0.1f).SetEase(Ease.Linear);
        isOpenLvPanel = true;
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="group"></param>
    public void open_canvas(CanvasGroup group)
    {
        //tween_1();
        //Debug.Log("打开设置");
        group.alpha = 1;
        group.blocksRaycasts = true;
    }
    /// <summary>
    /// 关闭界面
    /// </summary>
    /// <param name="group"></param>
    public void close_canvas(CanvasGroup group)
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
    }
    public void change()
    {
        if (abc == true)
        {
            switch_image[0].sprite = switch_sprite[1];
            abc = !abc;
        }
        else
        {
            switch_image[0].sprite = switch_sprite[0];
            abc = !abc;
        }

    }
    public void change_1()
    {
        if (def == true)
        {
            switch_image[1].sprite = switch_sprite[1];
            def = !def;
        }
        else
        {
            switch_image[1].sprite = switch_sprite[0];
            def = !def;
        }
    }
}
