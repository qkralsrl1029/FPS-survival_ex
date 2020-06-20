using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CloseWeaponController : MonoBehaviour     //근접무기들의 공통된 부분들의 코드들을 재활용하기위하여 만들어진 부모객체, 각 자식클래스마다 기능이 다르므로 추상 클래스로 구현되었다.
{
    [SerializeField] protected HandAnim currentCloseWeapon;


    protected bool isAttack = false;
    protected bool isSwing = false;
    protected RaycastHit hitInfo; //가상의 레이저를 발사해서 닿은 물체의 정보를 저장
    [SerializeField] protected LayerMask layerMask; //플레이어가 레이캐스트에 걸리는것을 방지

   


    // Update is called once per frame
    //void Update()     //추상클래스는 컴퍼넌트로써 옵젝에 추가할수없음-->업데이트문 실행X, 추상클래스를 상속받은 자식클래스에서 그 기능을 완성하여 실행해야함
    //{
    //    if (isActivate)
    //        TryAttack();
    //}

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1")&&!Inventory.isActivated)       //마우스 버튼 클릭&&공격중이 아닐때
        {
            if (!isAttack)
            {
                //딜레이 구현을 위해 코루틴 사용
                StartCoroutine(AttackCouroutine());
            }
        }
    }

    protected IEnumerator AttackCouroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);  //공격활성화 시간 만큼 딜레이
        isSwing = true;
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);  //공격비활성화 시간 만큼 딜레이
        isSwing = false;
        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - (currentCloseWeapon.attackDelayA + currentCloseWeapon.attackDelayB));
        //공격 활성화/비활성화 시간만큼을 뺀 총 대기 시간만큼 기다린 후 다음 공격 실행 가능
        isAttack = false;
    }

    protected abstract IEnumerator HitCoroutine();       //자식 클래스마다 hit의 기능이 다르므로 추상 코루틴으로 지정
    
        
    

    protected bool CheckObject()
    {
        //가상의 레이저를 자기자신의 위치에서 정면 방향으로 객체의 공격범위만큼 발사. 충돌체가 있으면 true반환
        if (Physics.Raycast(this.transform.position, transform.forward, out hitInfo, currentCloseWeapon.range, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            return true;
        }
        else
            return false;
    }

    //가상함수, 완성된함수이지만 추가 편집이 가능한 함수
    public virtual void CloseWeaponChange(HandAnim _hand)      //무기교체
    {
        if (Weaponmanager.currentWeapon != null)   //무기를 바꾸려 하는데 기존에 들고있는 무기가 있다면 비활성화, static으로 선언해두었기 때문에 별도의 객체를 생성하지 않아도 사용 가능
        {
            Weaponmanager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _hand;
        Weaponmanager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();     //이건 .transform과 뭐가 다른거지??
        Weaponmanager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = new Vector3(0.12f, -0.72f, -0.61f);
        currentCloseWeapon.gameObject.SetActive(true);

    }
}
