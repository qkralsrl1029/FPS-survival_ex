using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] protected string AnimalName;
    [SerializeField] protected int Hp;
    [SerializeField] protected float WalkSpeed;
    [SerializeField] protected float WalkTime;
    [SerializeField] protected float DelayTime;       //각각 행동들 진행 시간

    [SerializeField] protected float RunSpeed;
    [SerializeField] protected float RunTime;

    protected Vector3 destination;                      //목적지
    protected Vector3 _rotation = new Vector3();      //임시저장 변수
    protected float currentTime;                      //딜레이타임 체크 변수
    

    protected bool isWalking = false;
    protected bool isRunning = false;
    protected bool isAction = false;
    protected NavMeshAgent nav;

    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody rigid;
    [SerializeField] protected BoxCollider box;
    [SerializeField] protected AudioClip[] soundIdle;
    [SerializeField] protected AudioClip soundDamaged;
    [SerializeField] protected AudioClip soundDead;

    protected AudioSource theAudio;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DelayTime;
        isAction = true;        //시작하자마자 랜덤 액션 실행
        theAudio = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();     //rigidbody못씀 ㅠㅠ
    }

    // Update is called once per frame
    void Update()
    {
        if (isAction)
        {
            Move();
            ElapseTime();
        }
    }
    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //rigid.MovePosition(this.transform.position + (transform.forward * currentSpeed * Time.deltaTime));            네비게이션 컴퍼넌트쓸땐 사용 못함
            nav.SetDestination(transform.position + destination*5f);
        }
    }

    
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                reset();        //매행동 시간이끝나면 다시 리셋
            }
        }
    }

    protected virtual void reset()
    {
        isWalking = false;
        isAction = true;
        isRunning = false;
        nav.ResetPath();        //리셋시킬때 기존 설정한 목적지 초기화
        nav.speed = WalkSpeed;
        anim.SetBool("Walk", isWalking);
        anim.SetBool("Run", isRunning);
        destination.Set(Random.Range(-0.2f,0.2f),0,Random.Range(0.5f,1f));    
        
    }

    

   
    protected void Walk()
    {
        isWalking = true;
        nav.speed = WalkSpeed;
        anim.SetBool("Walk", isWalking);
        currentTime = WalkTime;
    }

    

    public virtual void Damaged(int damage, Vector3 targetPos)      //피격시 호출
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
           
        }
    }

    protected void PlayeSE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
