/**
 * DynamicScrollView.cs
 * 
 * @author mosframe / https://github.com/mosframe
 * 
 */

namespace Mosframe
{
    //命名空间
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using DG.Tweening;
    using UnityEngine.SceneManagement;

    public enum State
    {
        start,
        end,
    }
    /// <summary>
    /// Dynamic Scroll View
    /// </summary>
    //[RequireComponent(typeof(ScrollRect))]

    public abstract class DynamicScrollView : UIBehaviour
    {
        /// <summary> Scroll Direction </summary>
	    public enum Direction
        {
            Vertical,
            Horizontal,
        }
        public static State m_state;
        public int totalItemCount;//游戏分区总数
        public RectTransform itemPrototype = null;//实例化物体
        protected Direction _direction = Direction.Vertical;
        protected LinkedList<RectTransform> _containers = new LinkedList<RectTransform>();
        protected float _prevAnchoredPosition = 0;
        protected int _nextInsertItemNo = 0;
        protected float _itemSize = -50;
        protected int _prevTotalItemCount = 99;
        protected RectTransform _viewportRect = null;
        public RectTransform _contentRect = null;
        private List<RectTransform> _m_rect = new List<RectTransform>();
        public DynamicScrollViewItemExample example;
        Button[] button_index = new Button[6];//最多6 个
        private GameObject m_btnObj;
        private GameObject game_btnObj;
        [SerializeField] CanvasGroup load_ui;
        public GameObject image_ui;
        public static DynamicScrollView instant;
        bool isLoadScene = false;
        //public GameObject gameAreaParent;
        Transform areaScrollViewTransf;
        protected override void Start()
        {
            instant = this;
            //gameAreaParent = GameObject.Find("gameArea");
        }

        public void startInstant()//开始实例化游戏分区
        {
            m_state = State.start;
            this.totalItemCount = getMeiRenYuArea.intstant.fenQuNum;//传入分区数
            this._prevTotalItemCount = this.totalItemCount;
            List<string> btnsName = new List<string>();
            for (int i = 0; i < totalItemCount; i++)
            {
                btnsName.Add(i.ToString());
            }
            for (var i = 0; i < totalItemCount; i++)
            {
                var itemRect = Instantiate(this.itemPrototype);
                itemRect.SetParent(this._contentRect, false);
                button_index[i] = itemRect.gameObject.GetComponent<Button>();
                itemRect.name = i.ToString();//名字按排列顺序起的
                this._containers.AddLast(itemRect);
                this.updateItem(i, getMeiRenYuArea.intstant.listImg[i], itemRect.gameObject);//传图片
            }
            this._contentRect.transform.GetComponent<EnhanceScrollView>().enabled = true;//等都加载完了，在排列显示
            areaScrollViewTransf = this._contentRect.transform;

            otherContral.instant.getAllButton();
            foreach (string btnName in btnsName)
            {

                GameObject btnObj = GameObject.Find(btnName);
                Button btn = btnObj.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    for (int i = 0; i < otherContral.instant.btns.Count; i++)
                    {
                        if (otherContral.instant.btns[i][0].btn == btn)
                        {
                            if (!otherContral.instant.btns[i][1].click)
                            {

                                this.OnClick(btnObj);
                                otherContral.instant.btns[i][1].click = true;

                            }
                            else
                            {

                                otherContral.instant.btns[i][1].click = false;
                            }
                        }
                    }

                });

            }
            GameObject.Find("returnToHall").GetComponent<Button>().onClick.AddListener(delegate ()
            {

                for (int i = 0; i < otherContral.instant.btns.Count; i++)
                {
                    if (otherContral.instant.btns[i][0].btn == GameObject.Find("returnToHall").GetComponent<Button>())
                    {
                        if (!otherContral.instant.btns[i][1].click)
                        {

                            otherContral.instant.returnToHall();
                            otherContral.instant.btns[i][1].click = true;

                        }
                        else
                        {

                            otherContral.instant.btns[i][1].click = false;
                        }
                    }
                }

            });

            GameObject.Find("ruler").GetComponent<Button>().onClick.AddListener(otherContral.instant.openRuler);
            GameObject.Find("rulerExit").GetComponent<Button>().onClick.AddListener(otherContral.instant.exitRuler);
            Music_Control.music_effect(buYuMusicContral.instant.allYinXiao[23]);
        }

        private void Update()
        {
            if (this.totalItemCount != this._prevTotalItemCount)
            {

                this._prevTotalItemCount = this.totalItemCount;

            }
            if (isLoadScene)
            {
                //Debug.Log(1);
                Destroy(GameObject.Find("selectArea" + getMeiRenYuArea.buyuGame + "(Clone)").gameObject);
                //Debug.Log("先销毁分区，下一步实例化捕鱼");
                //GameObject buYuPrefab = Resources.Load("myPrefabs/meiRenYu/MainScene" + getMeiRenYuArea.buyuGame) as GameObject;//取决于点击哪款游戏
                Instantiate(/*m_slider.*//*buYuPrefab*/weiXinLoad.instance.MainScenes[getMeiRenYuArea.buyuGame], GameObject.Find("main").transform);
                //Debug.Log("场景加载。。。。。。。。。。");

                GameObject.Find("Order0Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
//Debug.Log(11);
                GameObject.Find("Order90Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
               // Debug.Log(12);
                isLoadScene = false;
            }
        }
        public void loadMeiRenYu()
        {
           // Debug.Log(2);
            isLoadScene = true;
           // Debug.Log(3);
        }
        public void OnClick(GameObject sender)
        {
            switch (sender.name)
            {
                case "0":
                    format(getMeiRenYuArea.intstant.listID[0],sender);
                    break;
                case "1":
                    format(getMeiRenYuArea.intstant.listID[1],sender);
                    break;
                case "2":
                    format(getMeiRenYuArea.intstant.listID[2],sender);
                    break;
                case "3":
                    format(getMeiRenYuArea.intstant.listID[3],sender);
                    break;
                case "4":
                    format(getMeiRenYuArea.intstant.listID[4],sender);
                    break;
                case "5":
                    format(getMeiRenYuArea.intstant.listID[5],sender);
                    break;
                default:
                    Debug.Log("none");
                    break;
            }
        }
        /// <summary>
        /// 进入选择房间
        /// </summary>
        /// <param name="index_c"></param>
        public void format(string _id,GameObject sender)
        {
            GameObject areaScrollviewChild = areaScrollViewTransf.GetChild(areaScrollViewTransf.childCount - 1).gameObject;
            if (sender == areaScrollviewChild)
            {
                WebButtonSendMessege.instant().selectGameArea(_id);
            }
            }
        /// <summary>
        /// 具体实例化哪一个场
        /// </summary>
        /// <param name="index"></param>
        /// <param name="itemObj"></param>
        private void updateItem(int index, int img, GameObject itemObj)
        {
            var item = itemObj.GetComponent<DynamicScrollViewItemExample>();
            item.onUpdateItem(img, getMeiRenYuArea.intstant.listText[index]);
        }
    }
}
