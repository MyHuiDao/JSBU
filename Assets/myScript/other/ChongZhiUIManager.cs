using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChongZhiUIManager : MonoBehaviour {

    public RectTransform chongzhitop;
    public GridLayoutGroup glg;


    private void OnEnable()
    {
        SizeManager.Instance.OnSizeValueChange += OnSizeValueChange;
    }

    private void OnDisable()
    {
        SizeManager.Instance.OnSizeValueChange -= OnSizeValueChange;
    }
    private void OnSizeValueChange()
    {
        //理论上
        Vector2 cellSize = glg.cellSize;

        float rw = chongzhitop.rect.width;
        float rh = chongzhitop.rect.height;

        float allSpaceX = rw - cellSize.x * 2;
        float allSpaceY = rh - cellSize.y * 4;

        ////实际上
        float cellSpaceX = glg.padding.left + cellSize.x * 2 + glg.spacing.x;
        float cellSpaceY = glg.padding.top + cellSize.y * 4 + glg.spacing.y;

        Debug.Log("width:" + chongzhitop.rect.width + " height:" + chongzhitop.rect.height);

        //if(cellSpaceY > rh)
        //{


        //}
        Vector2 NewcellSize = new Vector2(rw / 2 - 40, rh / 4 - 40);
        //if(cellSpaceX > rw)
        //{
        //    NewcellSize = new Vector2(rw / 2 - 40, cellSize.y);
        //}
        //if(cellSpaceY < rh)
        //{
        //    NewcellSize = new Vector2(cellSize.x, rh / 4 - 40);
        //}

        glg.cellSize = NewcellSize;
        glg.padding.left = 10;
        glg.padding.right = -10;
        glg.padding.top = 0;
        glg.padding.bottom = 0;
        Vector2 vv2 = new Vector2(20, 10);
        glg.spacing = vv2;



        // 重新计算位置


    }

    // Update is called once per frame
    void Update()
    {

    }
}
