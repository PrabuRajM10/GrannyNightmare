using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.Serialization;

public class PlayerStateMachine : StateMachine
{

    StateMachineBase  _previousState;
    //[SerializeField] float walkSpeed, runSpeed, crouchWalkSpeed , rotationFactor = 1f, _movementSpeed;
    private ActorInput _mainPlayerInput;
    private PlayerInput _playerInput;
    CharacterController _characterController;
    Vector3 _characterCurrentMovementVector;
    Vector2 _characterMoveInput;
    Vector2 _look;
    //bool _isMovementPressed, _isRunning, _isCrouching, _isKilling;
    //int _isWalkinghash, _isRunninghash, _isCrouchingHash, _isCrouchWalkingHash, _isKillingHash;
    [SerializeField] Text killHint;
    Transform _playerTransform , _lockedEnemy;
    private const float Threshold = 0.01f;
    
    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;


    //public StateMachineBase CurrentState { get { return _currentState; } set { _currentState = value; } }
    //public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }
    //public bool IsMovementPressed{ get { return _isMovementPressed; } }
    //public bool IsCrouching { get { return _isCrouching; } }
    //public bool IsKilling { get { return _isKilling; } }
    //public float MovementSpeed { set { _movementSpeed = value; } }
    //public float WalkSpeed { get { return walkSpeed; } }
    //public float RunSpeed { get { return runSpeed; } }
    //public float CrouchWalkSpeed { get { return crouchWalkSpeed; } }
    //public int IsWalkinghash { get { return _isWalkinghash; } set { _isWalkinghash = value; } }
    //public int IsRunninghash { get { return _isRunninghash; } set { _isRunninghash = value; } }
    //public int IsCrouchingHash { get { return _isCrouchingHash; } set { _isCrouchingHash = value; } }
    //public int IsCrouchWalkingHash { get { return _isCrouchWalkingHash; } set { _isCrouchWalkingHash = value; } }
    //public int IsKillingHash { get { return _isKillingHash; } set { _isKillingHash = value; } }

    [SerializeField] Transform PlayerTransform { get { return _playerTransform; } }
    [SerializeField] Transform LockedEnemy { get { return _lockedEnemy; } set { LockedEnemy = value; } }
    [SerializeField] CharacterController CharacterController { get { return _characterController; } }
    
    [FormerlySerializedAs("LockCameraPosition")] [SerializeField] private bool lockCameraPosition = false;
    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] float BottomClamp = -30.0f;
    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] GameObject CinemachineCameraTarget;
    
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

    private void Awake()
    {
        _mainPlayerInput = new ActorInput();
        _playerInput = GetComponent<PlayerInput>();
        _playerTransform= transform;
        _characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        _isWalkinghash = Animator.StringToHash("IsWalking");
        _isRunninghash = Animator.StringToHash("IsRunning");
        _isCrouchingHash = Animator.StringToHash("IsCrouching");
        _isCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
        _isKillingHash = Animator.StringToHash("Kill");

        _states = new StateMachineHandle(this);
        _currentState = _states.Idle();
        _currentState.OnEnterState();

        _mainPlayerInput.PlayerMove.Move.started += context => HandleInput_Move(context);
        _mainPlayerInput.PlayerMove.Move.canceled += context => HandleInput_Move(context);
        _mainPlayerInput.PlayerMove.Move.performed += context => HandleInput_Move(context);
        _mainPlayerInput.PlayerMove.Run.started += context => HandleInput_Run_OnStart(context);
        _mainPlayerInput.PlayerMove.Run.canceled += context => HandleInput_Run(context);
        _mainPlayerInput.PlayerMove.Crouch.canceled += context => HandleInput_Crouch(context);
        _mainPlayerInput.PlayerMove.Kill.started += context => HandleInput_Kill(context);
        _mainPlayerInput.PlayerMove.Look.started += context => HandleInput_Look(context);
        _mainPlayerInput.PlayerMove.Look.canceled += context => HandleInput_Look(context);
        _mainPlayerInput.PlayerMove.Look.performed += context => HandleInput_Look(context); 
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        HandleRotation();
        if(_currentState!=null)_currentState.OnUpdateState();
        //if (!_isKilling)
        //{
        //    if (_isRunning && !_isCrouching) Move(runSpeed);
        //    else if (_isCrouching) Move(crouchWalkSpeed);
        //    else Move(walkSpeed);
        //}
        _characterController.Move(_characterCurrentMovementVector * Time.deltaTime * _movementSpeed);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void OnEnable()
    {
        _mainPlayerInput.PlayerMove.Enable();
    }

    private void OnDisable()
    {
        _mainPlayerInput.PlayerMove.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _lockedEnemy = other.transform;
            HandleKillHintUI(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HandleKillHintUI(false);
        }
    }
    void HandleInput_Move(InputAction.CallbackContext context)
    {
        _characterMoveInput = context.ReadValue<Vector2>();
        _characterCurrentMovementVector.x = _characterMoveInput.x;
        _characterCurrentMovementVector.z = _characterMoveInput.y;
        _isWalking = _characterMoveInput.x != 0 || _characterMoveInput.y != 0;
    }

    void HandleInput_Look(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }
    void HandleInput_Run(InputAction.CallbackContext context)
    {
        _isRunning = context.ReadValueAsButton();
    }

    void HandleInput_Kill(InputAction.CallbackContext context)
    {
        _isKilling = true;
        //HandleKill();
    }

    void HandleInput_Run_OnStart(InputAction.CallbackContext context)
    {
        _isCrouching = false;
        _isRunning = context.ReadValueAsButton();
    }

    void HandleInput_Crouch(InputAction.CallbackContext context)
    {
        _isCrouching = !_isCrouching;
        Debug.Log("HandleInput_Crouch  _isCrouching" + _isCrouching);
    }

    void HandleGravity()
    {
        if (_characterController.isGrounded) _characterCurrentMovementVector.y = -0.05f;
        else _characterCurrentMovementVector.y = -9.8f;
    }

    void HandleRotation()
    {
        Vector3 positionToLook;

        positionToLook.x = _characterMoveInput.x;
        positionToLook.y = 0.0f;
        positionToLook.z = _characterMoveInput.y;
        Quaternion currentRot = transform.rotation;

        if (_isWalking)
        {
            Quaternion targetRot = Quaternion.LookRotation(positionToLook);
            transform.rotation = Quaternion.Slerp(currentRot, targetRot, Time.deltaTime * rotationFactor);
        }
    }


    // Update is called once per frame
    
    
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_look.sqrMagnitude >= Threshold && !lockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    public void KillConfirmed()
    {
        _isKilling = false;
        _currentState = _previousState;     
        _currentState.OnEnterState();
    }
    void HandleKillHintUI(bool canShow)
    {
        killHint.gameObject.SetActive(canShow);
    }
    public override void HandleKill()
    {
        Debug.Log(" HandleKill _currentState" + _currentState);
        if(_currentState != null)
        {
            _previousState = _currentState;
            _currentState = null;
            HandleKillHintUI(false);
            var newPos = new Vector3(transform.position.x, transform.position.y, _lockedEnemy.transform.position.z - 1.5f);
            transform.position = newPos;
            transform.LookAt(_lockedEnemy.transform.position);

            _characterController.enabled = false;

            animator.SetTrigger(_isKillingHash);
            var enemyAnimator = _lockedEnemy.GetComponent<Animator>();
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("IsDead");
            }
            _lockedEnemy.GetComponent<Collider>().enabled = false;
            _lockedEnemy = null;

            _characterController.enabled = true;
        }

    }
}
