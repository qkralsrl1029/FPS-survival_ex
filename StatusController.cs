using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int sp;                //체력
    [SerializeField] int spIncreaseSpeed;   //체력 재생성속도
    [SerializeField] int spRechargeTime;    //체력재생성 딜레이
    [SerializeField] int dp;
    [SerializeField] int hunger;
    [SerializeField] int thirsty;
    [SerializeField] int hungryDecreaseTime;
    [SerializeField] int thirstyDecreaseTime;
    [SerializeField] int satisfy;


    int currentHp;
    int currentSp;
    int currentDp;
    int currentHunger;
    int currentThirsty;
    int currentSatisfy;
    int currentSpRechargeTime;
    int currentHungryDecrease;
    int currentThirstyDecrease;

    bool spUsed=false;


    [SerializeField] Image[] images;
    const int HP = 0, DP = 1, SP = 2, HUNGER = 3, THIRSTY = 4, SATISFY = 5;     //인덱스가 상수로 돼있으면 알아보기 어려우므로, 미리 상수화를 시켜둠.

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHunger = hunger;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        gagueUpdate();
        spRecharge(); 
    }

    void Hungry()
    {
        if (currentHunger > 0)
        {
            if (currentHungryDecrease <= hungryDecreaseTime)
                currentHungryDecrease++;
            else
            {
                currentHunger--;
                currentHungryDecrease = 0;
            }
        }
        
    }

    void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecrease <= thirstyDecreaseTime)
                currentThirstyDecrease++;
            else
            {
                currentThirsty--;
                currentThirstyDecrease = 0;
            }
        }
       
    }

    public void DecreaseStamina(int _count)
    {
        spUsed = true;
        currentSpRechargeTime = 0;

        if (currentSp >= _count)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    void spRecharge()
    {
        if(spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
        else
        {
            if (currentSp < sp)
                currentSp += spIncreaseSpeed;
        }
    }

    public int getCurrentSp() { return currentSp; }     //걷거나 뛰기등의 동작을 할때 sp를 비교해서 동작을 수행하기 위해 playerScript에서 호출


    //Hp 증감
    public void IncreaseHp(int _count)
    {
        if (currentHp + _count <= hp)
            currentHp += _count;
        else
            currentHp = hp;
    }

    public void DecreaseHp(int _count)
    {
        if(currentDp>0)
        { DecreaseDp(_count);return; }

        if (currentHp - _count > 0)
            currentHp -= _count;
        else
            Debug.Log("캐릭터 사망");
    }

    //Dp 증감
    public void IncreaseDp(int _count)
    {
        if (currentDp + _count <= dp)
            currentDp += _count;
        else
            currentDp = dp;
    }

    public void DecreaseDp(int _count)
    {
        if (currentDp - _count> 0)
            currentDp -= _count;
        else
            Debug.Log("Dp 0");
    }

    //배고픔 증감
    public void IncreaseHunger(int _count)
    {
        if (currentHunger + _count <= hunger)
            currentHunger += _count;
        else
            currentHunger = hunger;
    }

    public void DecreaseHunger(int _count)
    {
        if (currentHunger - _count > 0)
            currentHunger -= _count;
        else
            currentHunger = 0;
        
    }

    //목마름 증감
    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count <= thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count > 0)
            currentThirsty -= _count;
        else
            currentThirsty = 0;

    }




    void gagueUpdate()                      //플레이어 스테이터스 변수에 따른 이미지 가시화, fillamount를 통해 백분율로 변환된 스테이터스 값들을 기존 설정한 이미지 배열에 각각 넣어줌
    {
        images[HP].fillAmount = (float)currentHp / hp;
        images[DP].fillAmount = (float)currentDp / dp;
        images[SP].fillAmount = (float)currentSp / sp;
        images[HUNGER].fillAmount = (float)currentHunger / hunger;
        images[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        images[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    } 
}
