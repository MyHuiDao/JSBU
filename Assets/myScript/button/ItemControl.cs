using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemControl : MonoBehaviour
{

    private Text labText;

    public void setItem(string str)
    {
        labText = transform.GetComponentInChildren<Text>();
        labText.text = str;
    }
}
