using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotTooltip : MonoBehaviour        //슬롯 내 아이템 정보 출력
{
    [SerializeField] GameObject go_Base;        //베이스 배경
    [SerializeField] Text itemName;
    [SerializeField] Text itemDescription;
    [SerializeField] Text itemManual;

    public void ShowTooltip(Item _item, Vector3 pos)    //해당 아이템 정보를 해당 슬롯 위치에 활성화
    {
        go_Base.SetActive(true);
        pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.7f, -go_Base.GetComponent<RectTransform>().rect.height * 0.7f, 0);  //해당 슬롯보다 우특하단 위치
        go_Base.transform.position = pos;
        itemName.text = _item.itemName;
        itemDescription.text = _item.itemDescription;

        if (_item.itemType == Item.ItemType.Equipment)
            itemManual.text = "우클릭 - 장착";
        else if(_item.itemType == Item.ItemType.Used)
            itemManual.text = "우클릭 - 복용";
        else
            itemManual.text = "";
    }

    public void HideTooltip()
    {
        go_Base.SetActive(false);
    }
}
