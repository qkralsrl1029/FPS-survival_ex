using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = false;



    public override void CloseWeaponChange(HandAnim _hand)
    {
        base.CloseWeaponChange(_hand);      //공통분모
        isActivate = true;                  //부모클래스에서 추가로 편집
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                if (hitInfo.transform.tag == "Rock")
                    hitInfo.transform.GetComponent<RockScript>().Mining();
                else if (hitInfo.transform.tag == "WeakAnimal")
                {
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<WeakAnimal>().Damaged(1, this.transform.position);       //몬스터가 맞았으면 피격이벤트 호출, 이중상속을 통해서 하나만 써줘도 됨
                }
                isSwing = false;
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Weaponmanager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        //Weaponmanager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();

    }
}

