/**
 * DynamicScrollViewItemExample.cs
 * 
 * @author mosframe / https://github.com/mosframe
 * 
 */

namespace Mosframe
{



    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DynamicScrollViewItemExample : MonoBehaviour
    {
        public Sprite[] image;
        public Animator[] animator;
       
        //public Text  title;
        public Sprite tiYanImg;     
        public Text text;
        /// <summary>
        /// 传图片和显示文本
        /// </summary>
        /// <param name="index">代表第几个图片，如0为体闲，1新手等</param>
        /// <param name="_text"></param>
        public void onUpdateItem(int img,string _text)
        {
            

            //this.background.sprite = image[id-1];
            this.transform.GetComponent<Image>().sprite = image[img-1];
            Instantiate(animator[img-1],this.transform);

            text.text = _text;
        }

    }
}
