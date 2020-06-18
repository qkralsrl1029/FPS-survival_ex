using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    [SerializeField] float range;       //아이템획득 사정거리

    bool pickupActivated;
    RaycastHit hitinfo;
    [SerializeField] LayerMask layerMask;   //아이템의 레이어에만 반응하도록 설정
    [SerializeField] Text actionText;
    [SerializeField] Inventory theInventory;



    // Update is called once per frame
    void Update()
    {
        TryAction();
        CheckItem();
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();        //
            PickUp();
        }
    }
    void CheckItem()
    {
        //플레이어 기준으로 전방에 사정거리 안에 있는 물체의 레이어마스크가 item일때 true
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, range, layerMask))
        {
            if (hitinfo.transform.tag == "Item")
                ItemInfoAppear();       //아이템 획득 가능 문구 출력
        }
        else
            ItemInfoDisappear();
    }

    void PickUp()
    {
        if(pickupActivated&&hitinfo.transform!=null)    //획득가능상태일때 
        {
            theInventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickUp>().item);
            Destroy(hitinfo.transform.gameObject);      //옵젝 파괴 및 문구 가리기
            ItemInfoDisappear();

        }
    }


    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName+" 획득 "+"<color=yellow>"+"(E)키"+"</color>"+"를 누르세요";
    }


    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
