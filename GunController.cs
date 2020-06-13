using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GunScript currentGun;
    [SerializeField] Vector3 originPos;     //정조준시 되돌아올 본래 포지션

    float currentFireRate;          //연사속도
    bool isReload = false;          //재장전상태
    bool isFineMode = false;        //정조준상태
    AudioSource theAudio;
    RaycastHit hitInfo;             //총알발사시 피격대상 정보저장변수
    [SerializeField] Camera theCam;     //총알이 1인칭 시점에 맞게 플레이어기준 가운데에서 발사되도록 그 화면을 가져옴
    [SerializeField] GameObject hitEffect;  //피격이펙트
    CrosshairScript theCrosshair;

    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
        theCrosshair = FindObjectOfType<CrosshairScript>();
       
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }

   

    void GunFireRateCalc()      //연사속도에 맞게 발사하기 위해서 정해진 연사속도주기로 발사될수있게 변수 설정
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    void TryFire()              //마우스 버튼이 눌려져있고 전에 발사된후로 연사속도가 지났다면 총알발사,재장전중이 아닐때
    {
        if(Input.GetButton("Fire1")&&currentFireRate<=0&&!isReload)
        {
            Fire();
        }
    }//발사 시도

    void Fire()                
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount <= 0)      //탄알집의 총알이 0보다 클때만 발사, 아니면 재장전      
            {
                CancelFineSight();      //총알없을때 정조준 해제
                StartCoroutine(ReloadCoroutine());
            }
            else
                Shoot();
        }       
    }//발사전 계산

    void Shoot()
    {
        theCrosshair.ShootingAnimation();           //발사시 플레이어의 상태에 따른(idle,walk,crouch) 크로스헤어 애니메이션 실행

        currentGun.currentBulletCount--;            //발사할때마다 현재 탄알집의 총알 --
        currentFireRate = currentGun.fireRate;      //연사속도 재계산
        playSE(currentGun.fireSound); 
        currentGun.muzzleFlash.Play();
        Hit();      //피격이벤트 호출
        //총기반동. 발사와 반동은 병렬적으로 처리되어야하기때문에 코루틴 사용
        StopAllCoroutines();        //코루틴 중복 실행을 막기위해 먼저 기존 실행되던 함수들 멈추기
        StartCoroutine(RetroActionCoroutine());
    }//발사후 계산

    void Hit()      //피격 이벤트. 총알 오브젝트를 생성하지않고 그 효과만 줌.
    {
        if(Physics.Raycast(theCam.transform.position,
            theCam.transform.forward+new Vector3(Random.Range(-theCrosshair.GetAccuracy()-currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy), Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),0)
            ,out hitInfo,currentGun.range))
        {
            //if문 안에 두번째 조건은 플레이어의 행동에 따라(walk,run,idle,fineSight,couch) 정확도를 달리하여 피격당하는 위치에 랜덤값을 줌
            //발사 위치는 절대값이므로 월드좌표계 사용
            var clone=Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //프리팹생성, .point-->실제 좌표, lookRatation-->피격당한 방형으로 이펙트 생성
            Destroy(clone, 2f);
            //메모리 관리를위해 일정 시간후 이펙트 제거
        }
    } 

    void playSE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }

    void TryReload()        //수동 재장전
    {
        //r키가 눌렸을때, 재장전중이 아니고 탄알집이 가듲찬 상태가 아니라면 재장전.
        if(Input.GetKeyDown(KeyCode.R)&&!isReload&&currentGun.currentBulletCount<currentGun.reloadBulletCount)
        {
            CancelFineSight();      //정조준중이면 정조준 해제
            StartCoroutine(ReloadCoroutine());
        }
    }
    IEnumerator ReloadCoroutine()       //재장전시 애니메이션상으로는 재장전 모션이 있지만 프로그램 상으로는 매우 빠른 시간 안에 일어나기때문에 재장전과 발사가 동시에 일어남. 이를 방지하기위해 코루틴 사용
    {
        if(currentGun.carryBulletCount>0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;   //현재 소유한 총알을 버리지않고 재장전시 사용하는듯한 효과. 개인적으로 짧고 쉬운 코드인데 굉장히 신선하고 좋았음. 이렇게도 짤수있구나
            currentGun.currentBulletCount = 0;
            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount>=currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("총알없음");
        }
    }

    void TryFineSight()     //정조준 모드
    {
        if(Input.GetButtonDown("Fire2")&&!isReload)     //재장전 중에 정조준이 실행되면 코루틴끼리 겹쳐서 isReload가 true인 상태로 코루틴이 끝남-->아무것도 작동안됨-->재장전중일때는 정조준 못하게막음
        {
            FineSight();
        }
    }

    public void CancelFineSight()
    {
        if (isFineMode)
            FineSight();
    }

    void FineSight()
    {
        isFineMode = !isFineMode;   //함수 두개만들필요 없이 참이면 거짓,거짓이면 참 반환
        currentGun.anim.SetBool("FineSight", isFineMode);       //trigger를 쓰면 참 상태일때 애니메이션이 계속 실행됨.
        theCrosshair.FinrSightAnimation(isFineMode);            //정조준모드 상태에 따라 크로스헤어 애니메이션 실행
        if(isFineMode)
        {
            StopAllCoroutines();        //lerp함수는 근사치기 때문에 while문 안에서 끝나지않음. 따라서 정조준이 반복될경우 기존 실행되고있던 정조준 관련 함수들이 중복되어 이상한 position값을 가지게됨
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()        //코루틴을 사용해서 정조준시 자연스러운 총의 움직임 구현. 애니메이션 X ,직접 객체의 local position 이동
    {
        while(currentGun.transform.localPosition!=currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator FineSightDeactivateCoroutine()       
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack=new Vector3(currentGun.retroActionForce,originPos.y,originPos.z) ;     //정조준안했을때 최대반동
        Vector3 retroActionRecoilBack=new Vector3(currentGun.retroActionFineSightForce,currentGun.fineSightOriginPos.y,currentGun.fineSightOriginPos.z) ;      //정조준했을때 최대반동

        if(!isFineMode)
        {
            currentGun.transform.localPosition = originPos;     //반복되는 반동모션 실행시에 역동감을 더하기위해 처음 위치에서 실행

            while(currentGun.transform.localPosition.x<=currentGun.retroActionForce-0.02f)      //lerp함수의 부정확성에 따른 여유값
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }//반동시작

            while(currentGun.transform.localPosition!=originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }//반동끝난후 원위치
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;     //반복되는 반동모션 실행시에 역동감을 더하기위해 처음 위치에서 실행

            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)      //lerp함수의 부정확성에 따른 여유값
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }//반동시작

            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }//반동끝난후 원위치
        }
    }

    public GunScript GetGun()
    {
        return currentGun;
    }
    public bool GetFinesightMode()
    {
        return isFineMode;
    }
}
