using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy;  //정확도
    public float fireRate;  //연사속도
    public float reloadTime;
    public int damage;

    public int reloadBulletCount;   //재장전되는 총알의 개수
    public int currentBulletCount;  //현재 탄알집의 통알 개수
    public int maxBulletCount;      //최대 소유 가능한 총알 개수
    public int carryBulletCount;    //현재 소유중인 총알 개수

    public float retroActionForce; //반동세기
    public float retroActionFineSightForce; //정조준시 반동 세기
    public Vector3 fineSightOriginPos;   //정조준시 변동되는 총의 위치

    public Animator anim;

    public ParticleSystem muzzleFlash;  //총구 섬광
    public AudioClip fireSound;         //총알 발사시 소리


    
}
