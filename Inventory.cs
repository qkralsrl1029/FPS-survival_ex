using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool isActivated = false;

    [SerializeField] GameObject go_inventoryBase;
    [SerializeField] GameObject go_SlotParents;
    Slot[] slots;
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
        if(Item.ItemType.Equipment!=_item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].setSlot(_count);
                        return;
                    }
                }
            }
        }
        
        for (int i = 0; i < slots.Length; i++)
        {
            
                if (slots[i].item==null)
                {
                    slots[i].addItem(_item, _count);
                    return;
                }
            
        }
    }

    void TryOpen()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            isActivated = !isActivated;

            if (isActivated)
                OpenInventory();
            else
                CloseInventory();
        }
    }

    void OpenInventory()
    {
        go_inventoryBase.SetActive(true);
    }

    void CloseInventory()
    {
        go_inventoryBase.SetActive(false);
    }
}
