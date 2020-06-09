using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float lookSensitivity;
    [SerializeField] float cameraRotationLimit;
    float currentCameraRotationX = 0;
   

    Rigidbody rigid;
    [SerializeField] Camera theCamera;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();      //rigidbody컴퍼넌트를 기존 설정한 rigid변수에 할당
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        cameraRatation();
        characterRotation();
    }

    void Move()
    {
        float _movedirX = Input.GetAxisRaw("Horizontal");
        float _movediyZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHor = this.transform.right * _movedirX;
        Vector3 _moveVer = this.transform.forward * _movediyZ;

        Vector3 _velocity = (_moveHor + _moveVer).normalized*walkSpeed; //대각선방향으로 이동시 속도가 기존설정값보다 커지는것을 막기위해 표준화

        rigid.MovePosition(this.transform.position + _velocity * Time.deltaTime);
    }

    void cameraRatation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotatonX = _xRotation * lookSensitivity;
        currentCameraRotationX += _cameraRotatonX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); //카메라 각도 임계치 고정        

        theCamera.transform.localEulerAngles = new Vector3(-currentCameraRotationX, 0, 0);
    }

    void characterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0)*lookSensitivity;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
