using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] HandAnim currentHand;


    bool isAttack = false;
    bool isSwing = false;
    RaycastHit hitInfo; //가상의 레이저를 발사해서 닿은 물체의 정보를 저장

    public static bool isActivate = false;


    // Update is called once per frame
    void Update()
    {
        if(isActivate)
            TryAttack();
    }

    void TryAttack()
    {
        if(Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                //딜레이 구현을 위해 코루틴 사용
                StartCoroutine(AttackCouroutine());
            }
        }
    }

    IEnumerator AttackCouroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentHand.attackDelayA);  //공격활성화 시간 만큼 딜레이
        isSwing = true;
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentHand.attackDelayB);  //공격비활성화 시간 만큼 딜레이
        isSwing = false;
        yield return new WaitForSeconds(currentHand.attackDelay - (currentHand.attackDelayA + currentHand.attackDelayB));
        //공격 활성화/비활성화 시간만큼을 뺀 총 대기 시간만큼 기다린 후 다음 공격 실행 가능
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false; //한번의 공격 모션에 한번의 데미지만 들어가기위해
            }
            yield return null;
        }
    }

    bool CheckObject()
    {
        //가상의 레이저를 자기자신의 위치에서 정면 방향으로 객체의 공격범위만큼 발사. 충돌체가 있으면 true반환
        if (Physics.Raycast(this.transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            Debug.Log(hitInfo.transform.name);
            return true;
        }
        else
            return false; 
    }

    public void HandChange(HandAnim _hand)      //무기교체
    {
        if (Weaponmanager.currentWeapon != null)   //무기를 바꾸려 하는데 기존에 들고있는 무기가 있다면 비활성화, static으로 선언해두었기 때문에 별도의 객체를 생성하지 않아도 사용 가능
        {
            Weaponmanager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = _hand;
        Weaponmanager.currentWeapon = currentHand.GetComponent<Transform>();     //이건 .transform과 뭐가 다른거지??
        Weaponmanager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = new Vector3(0.12f,-0.72f,-0.61f);
        currentHand.gameObject.SetActive(true);

        isActivate = true;
    }
}
