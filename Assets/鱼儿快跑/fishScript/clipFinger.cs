using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;



public class clipFinger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public Texture2D cursorTexture;
    int i = 0;
    CanvasGroup c = null;
   

    public Image money;
    Image img = null;



    int selfMoney;
    private void Start()
    {
        switch (this.name)
        {
            case "chip1":
                selfMoney = 1000;
                break;
            case "chip2":
                selfMoney = 2000;
                break;
            case "chip3":
                selfMoney = 5000;
                break;
            case "chip4":
                selfMoney = 10000;
                break;
            case "chip5":
                selfMoney = 20000;
                break;
            case "chip6":
                selfMoney = 50000;
                break;
            case "chip7":
                selfMoney = 100000;
                break;
            default:
                break;
        }

    }

    private void Update()
    {


    }
   
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
       
        if (!startTimeJiShi.instance.isCanDrag)
        {       
            return;
        }
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
      
        this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        img = Instantiate(money, this.transform);
        img.transform.SetParent(GameObject.Find("Canvas").transform);
        diaoyong(false);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        if (!startTimeJiShi.instance.isCanDrag)
        {        
            return;
        }
        if (img == null)
        {
            return;
        }

        RectTransform rect = img.transform as RectTransform;
        Vector3 v;
        bool f = RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out v);
        if (f)
        {
            rect.position = v;

        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        if (img == null)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero);

        if (hit.collider != null && hit.collider.tag == "touZhu")
        {
            Debug.Log(1);
            switch (hit.collider.name)
            {

                
                case "1":
                    Debug.Log("下注第一");
                    yuerContrall.touzhuNum[0] += selfMoney;
                    yuerSendMSg.instant().chipIn("1,2", selfMoney.ToString());
                    break;
                case "2":
                    yuerContrall.touzhuNum[1] += selfMoney;
                    yuerSendMSg.instant().chipIn("2,1", selfMoney.ToString());
                    break;
                case "3":
                    yuerContrall.touzhuNum[2] += selfMoney;
                    yuerSendMSg.instant().chipIn("3,1", selfMoney.ToString());
                    break;
                case "4":
                    yuerContrall.touzhuNum[3] += selfMoney;
                    yuerSendMSg.instant().chipIn("4,1", selfMoney.ToString());
                    break;
                case "5":
                    yuerContrall.touzhuNum[4] += selfMoney;
                    yuerSendMSg.instant().chipIn("1,3", selfMoney.ToString());
                    break;
                case "6":
                    yuerContrall.touzhuNum[5] += selfMoney;
                    yuerSendMSg.instant().chipIn("2,3", selfMoney.ToString());
                    break;
                case "7":
                    yuerContrall.touzhuNum[6] += selfMoney;
                    yuerSendMSg.instant().chipIn("3,2", selfMoney.ToString());
                    break;
                case "8":
                    yuerContrall.touzhuNum[7] += selfMoney;
                    yuerSendMSg.instant().chipIn("4,2", selfMoney.ToString());
                    break;
                case "9":
                    yuerContrall.touzhuNum[8] += selfMoney;
                    yuerSendMSg.instant().chipIn("1,4", selfMoney.ToString());
                    break;
                case "10":
                    yuerContrall.touzhuNum[9] += selfMoney;
                    yuerSendMSg.instant().chipIn("2,4", selfMoney.ToString());
                    break;
                case "11":
                    yuerContrall.touzhuNum[10] += selfMoney;
                    yuerSendMSg.instant().chipIn("3,4", selfMoney.ToString());
                    break;
                case "12":
                    yuerContrall.touzhuNum[11] += selfMoney;
                    yuerSendMSg.instant().chipIn("4,3", selfMoney.ToString());
                    break;
                case "13":
                    yuerContrall.touzhuNum[12] += selfMoney;
                    yuerSendMSg.instant().chipIn("1", selfMoney.ToString());
                    break;
                case "14":
                    yuerContrall.touzhuNum[13] += selfMoney;
                    yuerSendMSg.instant().chipIn("2", selfMoney.ToString());
                    break;
                case "15":
                    yuerContrall.touzhuNum[14] += selfMoney;
                    yuerSendMSg.instant().chipIn("3", selfMoney.ToString());
                    break;
                case "16":
                    yuerContrall.touzhuNum[15] += selfMoney;
                    yuerSendMSg.instant().chipIn("4", selfMoney.ToString());
                    break;
                default:
                    break;
            }



            yuerContrall.instance.touzhuTarget.Add(img);

            if (int.Parse(GameObject.Find("moneyText").GetComponent<Text>().text) >= selfMoney)
            {
                GameObject.Find("moneyText").GetComponent<Text>().text = (int.Parse(GameObject.Find("moneyText").GetComponent<Text>().text) - selfMoney).ToString();
            }
           
            img.transform.SetParent(hit.transform);
            img.transform.SetSiblingIndex(1);
            img.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            img.transform.localPosition = new Vector3(-53.5f,0,0);
            this.transform.localScale = Vector3.one;


        }
        else
        {
            Debug.Log("??");
            Destroy(img.gameObject);
            this.transform.localScale = Vector3.one;

        }


        diaoyong(true);
    }


    
    public void diaoyong(bool isReycast)
    {

        if (img.GetComponent<CanvasGroup>() == null)
        {

            img.transform.gameObject.AddComponent<CanvasGroup>();//添加之后 获得以下？
            c = img.GetComponent<CanvasGroup>();

        }
        else
        {
            if (i == 0)
            {
                c = img.GetComponent<CanvasGroup>();
                i = 1;
            }
        }

        c.blocksRaycasts = isReycast;


    }







}
