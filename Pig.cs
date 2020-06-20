using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] string AnimalName;
    [SerializeField] int Hp;
    [SerializeField] float WalkSpeed;
    [SerializeField] float WalkTime;
    [SerializeField] float DelayTime;       //각각 행동들 진행 시간

    [SerializeField] float RunSpeed;
    [SerializeField] float RunTime;

    Vector3 direction;                      //회전 방향
    Vector3 _rotation = new Vector3();      //임시저장 변수
    float currentTime;                      //딜레이타임 체크 변수
    float currentSpeed;

    bool isWalking = false;
    bool isRunning=false;
    bool isAction = false;


    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider box;
    [SerializeField] AudioClip[] soundIdle;
    [SerializeField] AudioClip soundDamaged;
    [SerializeField] AudioClip soundDead;

    AudioSource theAudio;



    // Start is called before the first frame update
    void Start()
    {
        currentTime = DelayTime;
        isAction = true;        //시작하자마자 랜덤 액션 실행
        theAudio=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAction)
        {
            Move();
            Rotatin();
            ElapseTime();
        }
    }
    void Move()
    {
        if(isWalking||isRunning)
        {
            rigid.MovePosition(this.transform.position + (transform.forward * currentSpeed * Time.deltaTime));
        }
    }

    void Rotatin()
    {
        if (isWalking||isRunning)
        {
            _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f,direction.y,0f), 0.01f);     //현재 각도에서 일정각도로 회전
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    void ElapseTime()
    {
        if(isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime<0)
            {
                reset();        //매행동 시간이끝나면 다시 리셋
            }
        }
    }

    void reset()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;

        currentSpeed = WalkSpeed;
        anim.SetBool("Walk", isWalking);
        anim.SetBool("Run", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);      //랜덤회전값
        RandomAction();
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
    void Walk()
    {
        isWalking = true;
        currentSpeed = WalkSpeed;
        anim.SetBool("Walk",isWalking);
        currentTime = WalkTime;
    }

    public void Run(Vector3 targetPos)             //피격시 호출
    {
        direction = Quaternion.LookRotation(this.transform.position - targetPos).eulerAngles;       //현재위치에서 플레이어 반대 방향으로 각도 설정,x와z값은 버그방지를위해 0으로 설정
        currentSpeed = RunSpeed;
        currentTime = RunTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Run", isRunning);
    }

    public void Damaged(int damage, Vector3 targetPos)      //피격시 호출
    {
        if (isAction)
        {
            if (Hp <= 0)                                        //체력이 0 이하면 죽음
            {
                PlayeSE(soundDead);
                anim.SetTrigger("Dead");
                isAction = false;
                return;
            }
            PlayeSE(soundDamaged);
            Hp--;
            anim.SetTrigger("Hurt");
            Run(targetPos);                                  //도망치기 함수 호출, 플레이어 위치 전달
        }
    }

    void PlayeSE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
    
}
