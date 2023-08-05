using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isActivated = true;

    // 스피드 조정 변수 
    [SerializeField] // 보호수준 유지 , inspecter창에서 수정 가능 (예외도 있음)
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    [SerializeField]
    private float swimSpeed;
    [SerializeField]
    private float swimFastSpeed;
    [SerializeField]
    private float upSwimSpeed;

    private float applySpeed; // walkSpeed || runSpeed를 대입하면 applySpeed만 사용 가능

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true;

    // 움직임 체크 변수
    private Vector3 lastPos;

    // 앉는 정도를 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 땅 착지 여부 
    private CapsuleCollider capsuleCollider;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계 
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // 필요 컴포넌트 
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid; // Collider로 충돌 영역을 설정하고 Rigidbody는 collider에 물리학을 입힘
    private GunController theGunController;
    private Crosshair theCrosshair;
    private StatusController theStatusController;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        // 초기화
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y; // 자식객체는 localPosition 사용 
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated && GameManager.canPlayerMove)
        {
            WaterCheck();
            IsGround();
            TryJump();
            if (!GameManager.isWater)
            {
                TryRun();
            }
            TryCrouch();
            Move();
            MoveCheck();
            CameraRotation();
            CharacterRotation();
        }
    }

    private void WaterCheck()
    {
        if (GameManager.isWater)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                applySpeed = swimFastSpeed;
            else
                applySpeed = swimSpeed;
        }
    }

    // 앉기 시도 
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // 앉는 동작 
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    // 부드러운 앉기 동작 실행 
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null; // 한 프레임 대기 
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    // 지면 체크 
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        // 대각선같은 약간의 오차범위를 위해 0.1f 만큼 더함
        theCrosshair.JumpingAnimation(!isGround);
    }

    // 점프 시도 
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
            Jump();
        else if (Input.GetKey(KeyCode.Space) && GameManager.isWater)
            UpSwim();
    }

    private void UpSwim()
    {
        myRigid.velocity = transform.up * upSwimSpeed;
    }

    // 점프 
    private void Jump()
    {
        // 앉은 상태에서 점프시 앉음 해제 
        if (isCrouch)
            Crouch();
        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce; // velocity : myRigid가 현재 움직이고 있느 속도 
    }

    // 달리기 시도 
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    // 달리기 실행 
    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    // 달리기 취소 
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    // 움직임 실행 
    private void Move()
    {   // Unity 3D 공간에서는 x가 좌,우 z가 앞,뒤 담당 
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // 기본값 (1,0,0) EX) 우방향키 입력시 (1,0,0) * 1 -> (1,0,0) || 좌방향키 입력시 (1,0,0) * -1 -> (-1,0,0)
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        // 기본값 (0,0,1) EX) 앞방향키 입력시 (0,0,1) * 1 -> (0,0,1) || 뒤방향키 입력시 (0,0,1) * -1 -> (0,0,-1)

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        // normalized : 이동거리 합이 1로 나오도록 정규화 EX) (1,0,0) + (0,0,1) 을 (0.5,0,0.5) 로 바꿔줌

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    // 움직임 체크 
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }

    }

    // 캐릭터의 좌우 회전
    private void CharacterRotation()
    {  
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0, _yRotation, 0) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 상하 카메라 회전 
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        // currentCameraRotationX를 최소 -currentCameraRotationX, 최대 currentCameraRotationX로 고정

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
    }
}
