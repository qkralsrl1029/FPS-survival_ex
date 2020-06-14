using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    Vector3 originPos;      //되돌아올 원래 위치
    Vector3 currentPos;     //계산에 사용될 현 위치
    [SerializeField] Vector3 LimitPos;  //최대 이동가능한 위치
    [SerializeField] Vector3 fineSightLimitPos; //정조준상태일때 최대이동가능한 위치
    [SerializeField] Vector3 smoothSway;        //움직임의 부드러움 정도
    [SerializeField] GunController theGunController;
    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //TrySway();
    }

    void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)     //마우스 입력이 있었을때
            Swaying();           
        else
            BackToOriginPos();
    }

    void Swaying()
    {
        //마우스 입력 정도?들을 변수에 저장
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -LimitPos.x, LimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -LimitPos.y, LimitPos.y),
                           originPos.z);        //clamp-->최대/최소값 설정해서 값을(첫번째 인자) 그 안에 가두기. set-->vector3변수에 값 할당
                                                //따라서 위의 구문은 currentPos변수에 기존 설정한 부드러움 정도로 기존설정한 최대이동가능 범위 내에서 마우스 입력값을 넣어줌
        }
        else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                           originPos.z);
        }

        this.transform.localPosition = currentPos;
    }

    void BackToOriginPos()      //마우스 입력이 없을때 원위치로 복귀
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        this.transform.localPosition = currentPos;
    }
}
