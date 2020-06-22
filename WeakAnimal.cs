using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{
    public void Run(Vector3 targetPos)             //피격시 호출
    {
        destination = new Vector3(transform.position.x - targetPos.x, 0f, transform.position.z - targetPos.z).normalized;
        nav.speed = RunSpeed;
        currentTime = RunTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Run", isRunning);
    }

    public override void Damaged(int damage, Vector3 targetPos)
    {
        base.Damaged(damage, targetPos);
        
        Run(targetPos);                                  //도망치기 함수 호출, 플레이어 위치 전달
    }
    
}
