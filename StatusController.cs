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
        else
            Debug.Log("배고픔 0");
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
        else
            Debug.Log("목마름 0");
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
