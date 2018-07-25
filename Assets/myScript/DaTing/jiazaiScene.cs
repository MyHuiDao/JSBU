using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
using UnityEngine.SceneManagement;
public class Globe
{
    public static string nextSceneName;
}
public class jiazaiScene : MonoBehaviour
{  
    public Slider loadingSlider;
    public Text loadingText;
    private float loadingSpeed = 1;
    private float targetValue;
    private AsyncOperation operation;
    private string token;
    public static GameObject scene;
    // Use this for initialization  
    void Start()
    {
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        loadingSlider.value = 0.0f;
        //scene = Resources.Load("Halls/Hall") as GameObject;
        if (SceneManager.GetActiveScene().name == "jiaZaiScene")
        {
            StartCoroutine(AsyncLoading());  //启动协程  
        }
    }

    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
        operation.allowSceneActivation = false;  //阻止当加载完成自动切换  
        yield return operation;
    }
    // Update is called once per frame  
    void Update()
    {
        targetValue = operation.progress;
        if (operation.progress >= 0.9f)
        { 
            targetValue = 1.0f;//operation.progress的值最大为0.9  
        }
        if (targetValue != loadingSlider.value)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, targetValue, Time.deltaTime * loadingSpeed); //插值运算  
            if (Mathf.Abs(loadingSlider.value - targetValue) < 0.01f)
            {
                loadingSlider.value = targetValue;
            }
        }
        loadingText.text = ((int)(loadingSlider.value * 100)).ToString() + "%";
        if ((int)(loadingSlider.value * 100) == 100)
        {
            operation.allowSceneActivation = true; //允许异步加载完毕后自动切换场景  
        }
    }
}