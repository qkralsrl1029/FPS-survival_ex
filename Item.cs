using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]   //create-->메뉴 추가로 이 클래스 객체를 만들수 있음
public class Item : ScriptableObject    //모노비헤이비어와 다르게 옵젝의 컴퍼넌트로 상속시키지않아도 사용가능
{

    public string itemName;
    public ItemType itemType;
    public Sprite itemImage;        //image와 달리 캔버스 없이 띄울수있음
    public GameObject itemPrefab;
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    public string weaponType;       

  
}
