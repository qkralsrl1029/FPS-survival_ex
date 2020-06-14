using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponmanager : MonoBehaviour
{
    //무기 교체용 스크립트

    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon;      //static은 공유자원. 객체마다 가지고있는 변수가 아닌 스크립트 자체에 포함된 내장변수,  (객체-->스크립트 상으로 접근 후 변경-->모든 객체들의 값 변경됨)
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;
    [SerializeField] string currentWeaponType;

    [SerializeField] float changeWeaponDelayTime;       //무기교체시 걸리는 시간
    [SerializeField] float changeWeaponEndDelayTime;    //무기교체가 끝나는 시간

    //소유중인 전체 무기 관리
    [SerializeField] GunScript[] guns;
    [SerializeField] HandAnim[] hands;

    //관리하기 쉽게 딕셔너리 컬렉션 이용
    Dictionary<string, GunScript> gunDictionary = new Dictionary<string, GunScript>();
    Dictionary<string, HandAnim> handDictionary = new Dictionary<string, HandAnim>();

   
    [SerializeField] GunController theGuncontroller;        //무기 종류는 여러개이지만 크게 손과 총으로 나뉘기 때문에(스크립트도 두개 기준이기때문에) 두개를 기준으로 하나만 실행되게(setactive사용)
    [SerializeField] HandController theHandController;
   

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "subMachineGun1"));

        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type,string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);         //일정 시간 대기(총 집어넣는 애니메이션 실행)

        CancelPreWeaponAction();        //기존 실행중이던 무기관련 활동들 중지하고
        WeaponChange(_type,_name);      //무기교체, 교체할 타입과 이름 parameter로 선언

        yield return new WaitForSeconds(changeWeaponEndDelayTime);      //일정 시간 대기(총 꺼내는 애니메이션 실행)

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    void CancelPreWeaponAction()        //기존 수행중이던 무기 취소
    {
        switch(currentWeaponType)
        {
            case "GUN":                                 //총이라면
                theGuncontroller.CancelFineSight();     //정조준 취소
                theGuncontroller.CancelReload();        //재장전 취소
                GunController.isActivate = false;       //static변수이기 때문에 생성된 객체가 아니라 직접 스크립트상에서의 변수 수정
                break;
            case "HAND":                                //손이라면
                HandController.isActivate = false;      //관련 동작들(공격...)취소
                break;
            
        }
    }

    void WeaponChange(string _type,string _name)       //무기교체 함수
    {
        if(_type=="GUN")
        {
            theGuncontroller.GunChange(gunDictionary[_name]);
        }
        else if(_type=="HAND")
        {
            theHandController.HandChange(handDictionary[_name]);
        }
    }
}
