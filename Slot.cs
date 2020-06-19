using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler, IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler //다중상속 구현이 가능한 인터페이스 사용(마우스 클릭이벤트)
    //마우스 엔터/엑시트, 마우스 클릭, 마우스 드래그, 마우스 입력 종료, 마우스 드록(순서대로)
{
    public Item item;
    public int itemCount;
    public Image itemImage;
    [SerializeField] Text countText;
    [SerializeField] GameObject go_CountImage;
    
    

   
    ItemEffects theItemEffect;      //아이템 사용시 호출

    private void Start()
    {
        theItemEffect = FindObjectOfType<ItemEffects>();
        
    }


    public void addItem(Item _item,int _count=1)        //아이템 획득
    {
        //현재 자기자신 슬롯에 받아온 아이템과 이미지정보등을 저장
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;
        if (item.itemType != Item.ItemType.Equipment)   //장비가 아닐때에만 개수를보여주는 이미지 가시화
        {
            go_CountImage.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            go_CountImage.SetActive(false);
        }
        setColor(1);    //정보 저장후에 비가시화 해제
    }

    public void setSlot(int _count)
    {
        itemCount += _count;
        countText.text = itemCount.ToString();
        if (itemCount <= 0)
            ClearSlot();
    }

    void setColor(float _alpha)     //이미지 투명도 조절, 사용안할때는 투명하게
    {
        Color color=itemImage.color;
        color.a = _alpha;
        itemImage.color = color;

    }

    void ClearSlot()        //슬롯 초기화
    {
        itemCount = 0;
        item = null;
        itemImage.sprite = null;
        setColor(0);
        countText.text = "0";
        go_CountImage.SetActive(false);

    }

    void ChangeSlot()
    {
        //drop에서 호출됐기 때문에 드랍되는 슬롯이 디폴트값임
        //슬롯 교환하기 전 교환 전 슬롯의 정보를 임시 저장해둘 변수, 드랍되는 슬롯의 정보들
        Item _temp = item;
        int _tempCount = itemCount;


        addItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);   //드래그 해온 정보들을 드랍되는 슬롯에 저장.   
        if (_temp != null)                                                                //쌍방교환되기 전에 드랍된 슬롯이 빈슬롯이었는지 먼저확인(드래그해온 슬롯은 미리 검사완료)
            DragSlot.instance.dragSlot.addItem(_temp, _tempCount);
        else                                                                             //빈슬롯이었으면 드래그해온 슬롯 초기화, 드래그해온 슬롯 정보를 static값에 저장해서 바로 바꾸기 가능
            DragSlot.instance.dragSlot.ClearSlot();
    }


    //슬롯 창 내에서 이벤트를 감지하는 인터페이스 7종(우클릭, 좌클릭 시작, 좌클릭 드래그, 드래그 종료, 드랍, 엔터 , 엑싯)


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button==PointerEventData.InputButton.Right)        //해당스크립트가 적용된 객체안에서 마우스버튼이 클릭되면
        {
            if(item!=null)
            {                               
                theItemEffect.useItem(item);
                if(item.itemType==Item.ItemType.Used)
                    setSlot(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.dragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.dragSlot!=null)
            ChangeSlot();
    }

    public void OnPointerEnter(PointerEventData eventData)      //마우스가 슬롯 안으로 들어오면 해당 아이템 설명 띄우기
    {
        if(item!=null)
            theItemEffect.showTooltip(item,this.transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)       //나가면 설명창 비활성화
    {
            theItemEffect.hideTooltip();
    }
}
