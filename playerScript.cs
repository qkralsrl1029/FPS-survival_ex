using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float lookSensitivity;     //카메라 민감도
    [SerializeField] float cameraRotationLimit;
    [SerializeField] float jumpForce;
    [SerializeField] float crouchSpeed;     //앉기 속도
    float currentCameraRotationX = 0;
    float applySpeed;   //걷기or뛸때 변하는 이동속도를 한번에 담아줄 변수
    bool isRun = false;
    bool isGrounded = true;
    bool isCrouch = false;
    [SerializeField] float crouchPosY;
    float originPosY;
    float applyCrouch;      //앉기or일어날때 변하는 높이를 한번에 담아줄 변수
   

    Rigidbody rigid;
    [SerializeField] Camera theCamera;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();      //rigidbody컴퍼넌트를 기존 설정한 rigid변수에 할당
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;       //카메라가 player에 상속되어 있기때문에 localposition사용
        applyCrouch = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        TryCrouch();
        TryJump();
        IsRun();       //move함수 위에 있어야 뛰는중인지 걷는중인지 알수있음
        Move();
        cameraRatation();
        characterRotation();
    }

    void TryCrouch()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            isCrouch = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouch = false;
        }
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouch = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouch = originPosY;
        }

        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouch, theCamera.transform.localPosition.z);
        //player가 아니라 camera의 localposition값을 변경시키면서 앉았을때 시야가 낮아지는 느낌을 줌 
        //이렇게 되면 시야가 너무 갑자기 변해서 부자연스러운 느낌을 주기 때문에 아래와 같은 실행문을 줌

        StartCoroutine(CrouchCoroutine());  //코루틴의 활용

    }

    IEnumerator CrouchCoroutine()       //병렬적 실행 함수
    {
        int count = 0;
        float _posY = theCamera.transform.localPosition.y;

        while(_posY!=applyCrouch)
        {
            _posY = Mathf.Lerp(_posY, applyCrouch, 0.3f);       //일정한 비율로 값 변경,점진적 변화
            theCamera.transform.localPosition = new Vector3(0, _posY, 0); //y값만 변경할수는 없기 때문에 vector3좌표 전체 수정
            if (count > 15)     //복안의 범위 지정. 무한한 실행 방지
                break;
            yield return null;      //한 프레임 대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouch, 0);
    }

    void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rigid.velocity = transform.up * jumpForce;
                isGrounded = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;
    }

    void IsRun()
    {
        if (!isCrouch)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRun = true;
                applySpeed = runSpeed;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            applySpeed = walkSpeed;
        }
    }

    void Move()
    {
        float _movedirX = Input.GetAxisRaw("Horizontal");
        float _movediyZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHor = this.transform.right * _movedirX;
        Vector3 _moveVer = this.transform.forward * _movediyZ;

        Vector3 _velocity = (_moveHor + _moveVer).normalized*applySpeed; //대각선방향으로 이동시 속도가 기존설정값보다 커지는것을 막기위해 표준화

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
