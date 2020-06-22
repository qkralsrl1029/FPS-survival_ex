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

    bool isWalk = false;
    public static bool isRun = false;
    bool isGrounded = true;
    bool isCrouch = false;
    Vector3 lastPos;        //움직임체크 변수


    [SerializeField] float crouchPosY;
    float originPosY;
    float applyCrouch;      //앉기or일어날때 변하는 높이를 한번에 담아줄 변수
   

    Rigidbody rigid;
    CrosshairScript theCrosshair;
    GunController theGuncontroller;
    StatusController theStatusController;       //플레이어가 걷거나 뛰는등의 행동을 하면 플레이어의 sp를 닳게할 함수를 호출
    [SerializeField] Camera theCamera;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();      //rigidbody컴퍼넌트를 기존 설정한 rigid변수에 할당
        theCrosshair = FindObjectOfType<CrosshairScript>(); //하이레키창에서 선언한 타입에 맞는 옵젝을 찾아서 넣어줌
        theGuncontroller = FindObjectOfType<GunController>();
        theStatusController = FindObjectOfType<StatusController>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;       //카메라가 player에 상속되어 있기때문에 localposition사용
        applyCrouch = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canMove)
        {
            TryCrouch();
            TryJump();
            IsRun();       //move함수 위에 있어야 뛰는중인지 걷는중인지 알수있음
            Move();
            MoveCheck();
            if (!Inventory.isActivated)
            {
                cameraRatation();
                characterRotation();
            }
        }
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

        theCrosshair.CrouchingAnimation(isCrouch);  //앉아있는상태에 맞는 크로스헤어 실행 
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

    IEnumerator CrouchCoroutine()       //병렬적 실행 함수, 앉기모드
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
            if (isGrounded&&theStatusController.getCurrentSp()>0)
            {
                rigid.velocity = transform.up * jumpForce;               
                isGrounded = false;
                theStatusController.DecreaseStamina(50);
            }
            //뛸때 크로스헤어 변하는 로직 구현하기
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = true;       
    }

    void IsRun()
    {
        if (!isCrouch)      //앉아있을때는 못뜀
        {
            if (Input.GetKey(KeyCode.LeftShift)&& theStatusController.getCurrentSp() > 0)   //sp가 0보다 클때만
            {
                theGuncontroller.CancelFineSight();     //정조준상태일경우 해제
                isRun = true;
                applySpeed = runSpeed;
                theStatusController.DecreaseStamina(10);
                theCrosshair.RunningAnimation(isRun);   //뛰고있을경우 그에맞는 크로스헤어 변경
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)|| theStatusController.getCurrentSp() <=0)  //뛰는도중 sp가 0보다 작아졌을때 뛰기 취소
        {
            isRun = false;
            applySpeed = walkSpeed;

            theCrosshair.RunningAnimation(isRun);
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

    void MoveCheck()        //크로스헤어 최신화를 위한 움직임체크 함수
    {
        if (!isRun&&!isCrouch)     //달릴때나 앉아있을땐 굳이 체크X
        {
            //if (Vector3.Distance(lastPos,this.transform.position)>=0.01f)    //이전 위치와 현대위치가 다르다면?-->움직였다고 간주. 단 일정 여유를 줘서 경사로에서 미끄러지는 등에 대한 예외처리
            //    isWalk = true;
            //else
            //    isWalk = false;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                isWalk = true;
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
                isWalk = false;
           

            theCrosshair.WalkingAnimation(isWalk);      //걷고있는지 상태에 따라 알맞은 애니메이션 파라매터값 전달
            lastPos = this.transform.position;
        }
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
