using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{

    [SerializeField] int hp;                //바위의 체력
    [SerializeField] float destroyTime;     //파편 제거 시간
    [SerializeField] SphereCollider col;    //돌의 컬라이더, 다른 옵젝들과 충돌을 감지

    [SerializeField] GameObject go_rock;    //일반 바위
    [SerializeField] GameObject go_debris;  //깨진 바위 파편
    [SerializeField] GameObject go_effectPrefabs;   //바위캘때마다 효과

    [SerializeField] string strikeSound;
    [SerializeField] string destroySound;

   

   

    public void Mining()
    {
        SoundManager.instance.PlaySE(strikeSound);      //static으로 선언된 instanse의 play함수를 객체생성하지않고 바로사용 

        var clone=Instantiate(go_effectPrefabs,col.bounds.center,Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if (hp <= 0)
            Destruction();
    }

    void Destruction()
    {
        SoundManager.instance.PlaySE(destroySound);
        col.enabled = false;        //기존의 일반 돌 제거
        Destroy(go_rock);

        go_debris.SetActive(true);  //돌이 없어지면 깨진 파편등장, 일정 시간후 소멸
        Destroy(go_debris, destroyTime);
    }
}
