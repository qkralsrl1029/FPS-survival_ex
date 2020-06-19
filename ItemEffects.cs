using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffects : MonoBehaviour
{
    [SerializeField]itemEffect[] itemEffects;           //총 아이템
    [SerializeField]StatusController thePlayerStatus;   //아이템 사용으로 회복될 사용자 정보
    [SerializeField]Weaponmanager theWeaponmanager;     //장비 아이템 사용시 인벤토리 내 장비 장착시에 필요
    [SerializeField]SlotTooltip theSlot;



    public void useItem(Item _item)                     //아이템 사용시 호출
    {
        if (_item.itemType == Item.ItemType.Equipment)              //장비형 아이템    
        {
            StartCoroutine(theWeaponmanager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType==Item.ItemType.Used)                //소모형 아이템
        {
            for (int i = 0; i < itemEffects.Length; i++)            //전체 아이템 탐색
            {
                if(itemEffects[i].itemName==_item.itemName)         //이름이 같으면
                {
                    for (int j = 0; j < itemEffects[i].part.Length; j++)    //아이템이 적용할 부위들 탐색
                    {
                        switch(itemEffects[i].part[j])
                        {
                            //각 status 별로 얼마나 회복시켜줄지 num배열 호출 
                            case "HP":
                                thePlayerStatus.IncreaseHp(itemEffects[i].num[j]);
                                break;
                            case "DP":
                                thePlayerStatus.IncreaseDp(itemEffects[i].num[j]);
                                break;
                            case "SP":
                                thePlayerStatus.IncreaseSp(itemEffects[i].num[j]);
                                break;
                            case "HUNGER":
                                thePlayerStatus.IncreaseHunger(itemEffects[i].num[j]);
                                break;
                            case "THIRSTY":
                                thePlayerStatus.IncreaseThirsty(itemEffects[i].num[j]);
                                break;
                            case "SATISFY":
                                //thePlayerStatus.IncreaseHp(itemEffects[i].num[j]);
                                break;
                            default:
                                break;
                        }
                    }
                    return;
                }
            }
            Debug.Log("해당 아이템이 존재하지 않습니다.");
        }
    }


    public void showTooltip(Item _item, Vector3 pos)
    {
        theSlot.ShowTooltip(_item,pos);
    }

    public void hideTooltip()
    {
        theSlot.HideTooltip();
    }
}




[System.Serializable]   //클래스 객체를 인스펙터창에서 띄우기 위해서
public class itemEffect
{
    public string itemName;     //이름
    [Tooltip("HP,SP,DP,HUNGER,THIRSTY,SATISFY")]
    public string[] part;       //적용 부위
    public int[] num;           //적용부위별로 얼마나 회복시킬지
}
