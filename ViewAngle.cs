using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewAngle : MonoBehaviour
{

    [SerializeField] float viewAngle;
    [SerializeField] float viewDistance;
    [SerializeField] LayerMask targetMask;
    [SerializeField] Pig thePig;
    

   

    // Update is called once per frame
    void Update()
    {
        View();
    }

    void View()
    {
        Vector3 leftBoundary=BoundaryAngle(-viewAngle*0.5f);
        Vector3 rightBoundary=BoundaryAngle(viewAngle * 0.5f);
        
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);       //범위(구) 내에 있는 모든 컬라이더를 저장(시야각 내에 있는 물체들 저장)
        for (int i = 0; i < _target.Length; i++)
        {
            Transform temp = _target[i].transform;      //시야거리 내에 걸린 컬라이더의 이름이 플레이어라면
            if(temp.transform.name=="Player")
            {
                Vector3 _direction = (temp.position - this.transform.position).normalized;      //자신과 플레이어를 잇는 방향벡터
                float _angle = Vector3.Angle(_direction, this.transform.forward);               //자신의 전방방향과 이전의 방향벡터와의 각도를 구하고

                if(_angle<viewAngle*0.5f)                                                      //이각도가 시야각의 절반보다작으면-->시야 내 있는것으로 간주
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, _direction, out hit, viewDistance))     //플레이어와 자신 사이에 방해물이 없는지 레이저를 쏴서 확인
                    {
                        if (hit.transform.name == "Player")
                            thePig.Run(hit.transform.position);
                    }                       
                }
            }
        }
    }

    Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;      //z축 기준이기때문에 y값이 변동
        return new Vector3(Mathf.Sin(_angle*Mathf.Deg2Rad),0f, Mathf.Cos(_angle * Mathf.Deg2Rad));      //삼각함수의 성질을 이용해서 시야각의 max값 설정(각 좌우 기준값)
    }
}
