using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnim : MonoBehaviour
{
    public string closeWeaponName;
    public float range;     //공격범위
    public int damage;
    public float workSpeed;
    public float attackDelay;
    public float attackDelayA;  //공격 활성화 시점
    public float attackDelayB;  //공격 비활성화 시점

    //무기 형태 구분
    public bool isAxe;
    public bool isPickaxe;
    public bool isHand;


    public Animator anim;

    
}
