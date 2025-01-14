using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Serialization;

public class PlayerStateMachine : StateMachine
{
    private StateMachineBase  previousState;
    private ActorInput mainPlayerInput;
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Vector3 characterCurrentMovementVector;
    private Vector2 characterMoveInput;
    Vector2 smoothedInput;

    private Vector2 look;
    private Transform playerTransform , lockedEnemy;
    private const float Threshold = 0.01f;
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;

    private bool isAiming , isFiring;
    private float playerRotationTargetY;
    private readonly int sprintMultiplier = 2;
    private int isAimingHash;

    [SerializeField] private RifleController rifleController;

    [FormerlySerializedAs("LockCameraPosition")] [SerializeField] private bool lockCameraPosition = false;
    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] float BottomClamp = -30.0f;
    
    [FormerlySerializedAs("CinemachineCameraTarget")]
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField]
    private GameObject cinemachineCameraTarget;
    
    [FormerlySerializedAs("CameraAngleOverride")]
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField]
    private float cameraAngleOverride = 0.0f;

    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private Vector2 smoothVelocity;
    [SerializeField] private Transform lookAtTarget;

    private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";
    
    public Vector3 CharacterCurrentMovementVector => characterCurrentMovementVector;

    private void Awake()
    {
        mainPlayerInput = new ActorInput();
        playerInput = GetComponent<PlayerInput>();
        playerTransform= transform;
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _isCrouchingHash = Animator.StringToHash("IsCrouching");
        _isCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
        _isKillingHash = Animator.StringToHash("Kill");
        _playerMovementXHash = Animator.StringToHash("x");
        _playerMovementZHash = Animator.StringToHash("z");
        isAimingHash = Animator.StringToHash("IsAiming");  

        _states = new StateMachineHandle(this);
        _currentState = _states.Idle();
        _currentState.OnEnterState();

        mainPlayerInput.PlayerMove.Move.started += context => HandleInput_Move(context);
        mainPlayerInput.PlayerMove.Move.canceled += context => HandleInput_Move(context);
        mainPlayerInput.PlayerMove.Move.performed += context => HandleInput_Move(context);
        mainPlayerInput.PlayerMove.Run.started += context => HandleInput_Run_OnStart(context);
        mainPlayerInput.PlayerMove.Run.canceled += context => HandleInput_Run(context);
        mainPlayerInput.PlayerMove.Crouch.canceled += context => HandleInput_Crouch(context);
        mainPlayerInput.PlayerMove.Kill.started += context => HandleInput_Kill(context);
        mainPlayerInput.PlayerMove.Look.started += context => HandleInput_Look(context);
        mainPlayerInput.PlayerMove.Look.canceled += context => HandleInput_Look(context);
        mainPlayerInput.PlayerMove.Look.performed += context => HandleInput_Look(context);
        mainPlayerInput.PlayerMove.Aim.started += context => HandleInput_Aim(context);
        mainPlayerInput.PlayerMove.Aim.canceled += context => HandleInput_Aim(context);
        mainPlayerInput.PlayerMove.Fire.started += context => HandleInput_Fire(context);
        mainPlayerInput.PlayerMove.Fire.canceled += context => HandleInput_Fire(context);
    }

    private void Start()
    {
        cinemachineTargetYaw = cinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        HandleRotation();
        if(_currentState!=null)_currentState.OnUpdateState();
        if(isFiring) rifleController.FireRifle();
        
        Vector3 forward = transform.TransformDirection(Vector3.forward) * characterCurrentMovementVector.z; // TransformDirection converts local direction to world space
        Vector3 right = transform.TransformDirection(Vector3.right) * characterCurrentMovementVector.x;

        Vector3 moveDirection = (forward + right).normalized;

        Vector2 movementInput = characterMoveInput * (_canRun ? sprintMultiplier : 1);
        smoothedInput.x = Mathf.SmoothDamp(smoothedInput.x, movementInput.x, ref smoothVelocity.x, smoothTime);
        smoothedInput.y = Mathf.SmoothDamp(smoothedInput.y, movementInput.y, ref smoothVelocity.y, smoothTime);
        smoothedInput.x = Mathf.Clamp(smoothedInput.x, -1, 2);
        smoothedInput.y = Mathf.Clamp(smoothedInput.y, -1, 2);
        
        _animator.SetFloat(_playerMovementXHash , smoothedInput.x);
        _animator.SetFloat(_playerMovementZHash , smoothedInput.y);

        var acceleration = _canRun ? movementSpeed * sprintMultiplier : movementSpeed;

        characterController.Move(moveDirection * Time.deltaTime * movementSpeed);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void OnEnable()
    {
        mainPlayerInput.PlayerMove.Enable();
    }

    private void OnDisable()
    {
        mainPlayerInput.PlayerMove.Disable();
    }
    void HandleInput_Move(InputAction.CallbackContext context)
    {
        characterMoveInput = context.ReadValue<Vector2>();
        Debug.Log("[] [HandleInput_Move] _characterMoveInput "+ (characterMoveInput.x , characterMoveInput.y , _isRunButtonPressed));
        characterCurrentMovementVector.x = characterMoveInput.x;
        characterCurrentMovementVector.z = characterMoveInput.y;
        
        _isWalking = characterMoveInput.x != 0 || characterMoveInput.y != 0;
        if (characterMoveInput.y > 0 && _isRunButtonPressed)
        {
            _canRun = true;
        }
        else
        {
            _canRun = false;    
        }
        // _PlayerMovementXHash  = _characterMoveInput.x;
        // _playerMovementZHash = _characterMoveInput.y;
    }

    void HandleInput_Look(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    void HandleInput_Run(InputAction.CallbackContext context)
    {
        _isRunButtonPressed = context.ReadValueAsButton();
        Debug.Log("[Run] [Canceled] _isRunButtonPressed " + _isRunButtonPressed);
        _canRun = false;    
    }
    void HandleInput_Aim(InputAction.CallbackContext context)
    {
        isAiming = context.ReadValueAsButton();
        Debug.Log("[Aim]  " + isAiming);
        // _animator.SetBool(isAimingHash , isAiming);
    }
    
    void HandleInput_Fire(InputAction.CallbackContext context)
    {
        isFiring = context.ReadValueAsButton();
        // _animator.SetBool(isAimingHash , isAiming);
    }

    void HandleInput_Kill(InputAction.CallbackContext context)
    {
        _isKilling = true;
        //HandleKill();
    }

    void HandleInput_Run_OnStart(InputAction.CallbackContext context)
    {
        _isCrouching = false;
        _isRunButtonPressed = context.ReadValueAsButton();
        Debug.Log("[Run] [Start] _isRunButtonPressed " + _isRunButtonPressed);
        if(characterMoveInput.y > 0) _canRun = true;
    }

    void HandleInput_Crouch(InputAction.CallbackContext context)
    {
        _isCrouching = !_isCrouching;
    }

    void HandleGravity()
    {
        if (characterController.isGrounded) characterCurrentMovementVector.y = -0.05f;
        else characterCurrentMovementVector.y = -9.8f;
    }

    void HandleRotation()
    {
        // Vector3 positionToLook;
        //
        // Debug.Log("[HandleRotation] look " + (_look.x , _look.y));
        //
        // positionToLook.x = _characterMoveInput.x;
        // positionToLook.y = 0.0f;
        // positionToLook.z = _characterMoveInput.y;
        //
        // if (_isWalking)
        // {
        //     Quaternion targetRot = Quaternion.LookRotation(positionToLook);
        //     transform.rotation = Quaternion.Slerp(currentRot, targetRot, Time.deltaTime * rotationFactor);
        // }
        
        if (look.sqrMagnitude >= Threshold && !lockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            playerRotationTargetY += look.x * deltaTimeMultiplier;
            
        }
        
        // _playerRotationTargetY = ClampAngle(_playerRotationTargetY, float.MinValue, float.MaxValue);

        transform.rotation = Quaternion.Euler(0,playerRotationTargetY, 0.0f);
    }

    void SetKLookTargetPosition()
    {
        // var zPos = (cinemachineCameraTarget.transform.TransformDirection(Vector3.forward) * 3f);
        // var localTrans = cinemachineCameraTarget.transform.TransformDirection(cinemachineCameraTarget.transform.position.x , cinemachineCameraTarget.transform.position.y , cinemachineCameraTarget.transform.position.z);  
        // lookAtTarget.position =  cinemachineCameraTarget.transform.position + zPos;
    }

    // Update is called once per frame
    
    
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (look.sqrMagnitude >= Threshold && !lockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            cinemachineTargetYaw += look.x * deltaTimeMultiplier;
            cinemachineTargetPitch += look.y * deltaTimeMultiplier;
            
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
            cinemachineTargetYaw, 0.0f);
        SetKLookTargetPosition();
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
