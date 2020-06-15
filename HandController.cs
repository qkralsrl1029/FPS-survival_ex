using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController     //근접무기 스크립트를 상속받아 코드의 재활용
{
    public static bool isActivate = false;

    public override void CloseWeaponChange(HandAnim _hand)  //부모클래스에서의 함수를 오버라이드해서 추가편집
    {
        base.CloseWeaponChange(_hand);
        isActivate = true;
    }

    protected override IEnumerator HitCoroutine()       //부모객체의 추상 코루틴이었던 hit를 재정의
    {
        while(isSwing)
        {
            if(CheckObject())
            {
                isSwing = false;
            }
            yield return null;
        }
    }

    void Update()
    {
        if (isActivate)
            TryAttack();
    }
}
