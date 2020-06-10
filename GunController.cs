using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] GunScript currentGun;

    float currentFireRate;
    AudioSource theAudio;


    private void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    void GunFireRateCalc()      //연사속도에 맞게 발사하기 위해서 정해진 연사속도주기로 발사될수있게 변수 설정
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    void TryFire()              //마우스 버튼이 눌려져있고 전에 발사된후로 연사속도가 지났다면 총알발사
    {
        if(Input.GetButton("Fire1")&&currentFireRate<=0)
        {
            Fire();
        }
    }

    void Fire()                 //연사속도 카운트용 변수 초기화 후 발사
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    void Shoot()
    {
        playSE(currentGun.fireSound); 
        currentGun.muzzleFlash.Play();
    }

    void playSE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
