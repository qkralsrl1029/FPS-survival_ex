using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemImage;
    [SerializeField] Text countText;
    [SerializeField] GameObject go_CountImage;
   

    public void addItem(Item _item,int _count=1)        //아이템 획득
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;
        if (item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            go_CountImage.SetActive(false);
        }
        setColor(1);
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
}
