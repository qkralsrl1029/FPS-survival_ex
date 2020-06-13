
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour
{
    [SerializeField] Animator anim;

    float gunAccuracy;      //상태변화에 따른 총의 정확도

    [SerializeField] GameObject goCrosshairHud;     //크로스헤어 비활성화용.(도끼나 맨손 사용시)
    [SerializeField] GunController theGuncontroller;

   

    //playerScript에서 호출할 예정. 왜냐면 플레이어의 상태변화는 playerScript에서 관리하기때문.
   public void WalkingAnimation(bool _flag)
    {
        anim.SetBool("isWalk", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        anim.SetBool("isRun", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        anim.SetBool("isCrouch", _flag);
    }
    public void FinrSightAnimation(bool _flag)
    {
        anim.SetBool("Finesight", _flag);
    }
    public void ShootingAnimation()
    {
        //getbool을 통해 현재 플레이어의 상태를 받아오고, 그상태에 따라 차등적인 크로스헤어 애니메이션 실행
        if (anim.GetBool("isWalk"))
            anim.SetTrigger("walkFire");
        else if (anim.GetBool("isCrouch"))
            anim.SetTrigger("crouchFire");
        else
            anim.SetTrigger("idleFire");
    }

    public float GetAccuracy()
    {
        if (anim.GetBool("isWalk"))
            gunAccuracy = 0.08f;
        else if (anim.GetBool("isCrouch"))
            gunAccuracy = 0.02f;
        else if (theGuncontroller.GetFinesightMode())
            gunAccuracy = 0.005f;
        else
            gunAccuracy = 0.04f;

        return gunAccuracy;

    }
   
}
