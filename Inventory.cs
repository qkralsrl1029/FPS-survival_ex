using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour      //인벤토리 스크립트
{
    public static bool isActivated = false; //인벤토리 활성화 체크 변수

    [SerializeField] GameObject go_inventoryBase;   //베이스
    [SerializeField] GameObject go_SlotParents;     //그리드 레이아웃
    Slot[] slots;                                   //슬롯들(총 20개)

    public Slot[] GetSlot(){return slots; }

    [SerializeField] Item[] items;
    public void LoadToInven(int arrayNum, string itemName, int itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == itemName)
                slots[arrayNum].addItem(items[i], itemNum);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotParents.GetComponentsInChildren<Slot>(); //배열 안에 일괄 할당
    }

    // Update is called once per frame
    void Update()
    {
        TryOpen();
    }

    public void AcquireItem(Item _item,int _count=1)
    {
        if(Item.ItemType.Equipment!=_item.itemType)     //장비아이템이 아닐때
        {
            for (int i = 0; i < slots.Length; i++)      //20개의 슬롯들을 돌면서
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)   //기존 보유중인 아이템과 획득할 아이템이 같을때
                    {
                        slots[i].setSlot(_count);                   //획득 개수만큼 최신화(동일 슬롯)
                        return;
                    }
                }
            }
        }                                                          //위 조건에 걸리지않았다면,( 장비아이템이거나, 기존 보유중인 아이템이 아닐때)
        
        for (int i = 0; i < slots.Length; i++)                     //슬롯들을 돌면서
        {
            
                if (slots[i].item==null)                          //빈곳을 찾고
                {
                    slots[i].addItem(_item, _count);             //획득할 아이템 추가
                    return;
                }
            
        }
    }

    void TryOpen()
    {
        if(Input.GetKeyDown(KeyCode.I)&&!GameManager.isPause)       //일시정치상태에서는 인벤토리 비활성화
        {
            isActivated = !isActivated;

            if (isActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    void OpenInventory()        //인벤토리 창 활성화
    {
        GameManager.inventoryOpen = true;
        go_inventoryBase.SetActive(true);
    }

    void CloseInventory()     //인벤토리 창 비활성화
    {
        GameManager.inventoryOpen = false;
        go_inventoryBase.SetActive(false);
    }
}
