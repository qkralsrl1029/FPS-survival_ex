using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pig : WeakAnimal       //이중상속  (동물-->온순한동물-->돼지,닭,토끼...)
{
    void Wait()
    {
        currentTime = DelayTime;
    }
    void Eat()
    {
        anim.SetTrigger("Eat");
        currentTime = DelayTime;
    }
    void Peek()
    {
        anim.SetTrigger("Peek");
        currentTime = DelayTime;
    }

    void RandomAction()
    {

        int _rand = Random.Range(0, 6);     //걷기가 조금 더 많이 나오게 설정
        int soundRand = _rand % 3;
        PlayeSE(soundIdle[soundRand]);

        switch (_rand)
        {
            case 0:
                Wait();
                break;
            case 1:
                Eat();
                break;
            case 2:
                Peek();
                break;
            case 3:
                Walk();
                break;
            case 4:
                Walk();
                break;
            case 5:
                Walk();
                break;
            default:
                return;
        }

    }

    protected override void reset()
    {
        base.reset();
        RandomAction();
    }
}
