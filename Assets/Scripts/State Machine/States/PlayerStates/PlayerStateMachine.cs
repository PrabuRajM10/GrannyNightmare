using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace State_Machine.States.PlayerStates
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private PlayerStateMachineBase  previousPlayerState;
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
        private Animator animator;
        private int isCrouchingHash;
        private int isCrouchWalkingHash;
        private int playerMovementXHash;
        private int playerMovementZHash;
        private StateMachineHandle states;
        private PlayerStateMachineBase currentState;
        private bool canRun;
        private float movementSpeed;
        private bool isRunButtonPressed;
        private bool isWalking;
        private bool isCrouching;
        private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";


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
        
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float crouchWalkSpeed;

    
        public Vector3 CharacterCurrentMovementVector => characterCurrentMovementVector;
        public PlayerStateMachineBase CurrentPlayerState { get => currentState;
            set => currentState = value;
        }
        public bool CanRun { get => canRun;
            set => canRun = value;
        }
        public bool IsWalking => isWalking;
        public bool IsCrouching => isCrouching;
        public float MovementSpeed { set => movementSpeed = value; }
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;

        public float CrouchWalkSpeed => crouchWalkSpeed;
        public int IsCrouchingHash { get => isCrouchingHash;
            set => isCrouchingHash = value;
        }
        public int IsCrouchWalkingHash { get => isCrouchWalkingHash;
            set => isCrouchWalkingHash = value;
        }
        public Animator CharacterAnimator => animator;

        private void Awake()
        {
            mainPlayerInput = new ActorInput();
            playerInput = GetComponent<PlayerInput>();
            playerTransform= transform;
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            isCrouchingHash = Animator.StringToHash("IsCrouching");
            isCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
            playerMovementXHash = Animator.StringToHash("x");
            playerMovementZHash = Animator.StringToHash("z");
            isAimingHash = Animator.StringToHash("IsAiming");  

            states = new StateMachineHandle(this);
            currentState = states.Idle();
            currentState.OnEnterState();

            mainPlayerInput.PlayerMove.Move.started += context => HandleInput_Move(context);
            mainPlayerInput.PlayerMove.Move.canceled += context => HandleInput_Move(context);
            mainPlayerInput.PlayerMove.Move.performed += context => HandleInput_Move(context);
            mainPlayerInput.PlayerMove.Run.started += context => HandleInput_Run_OnStart(context);
            mainPlayerInput.PlayerMove.Run.canceled += context => HandleInput_Run(context);
            mainPlayerInput.PlayerMove.Crouch.canceled += context => HandleInput_Crouch(context);
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
            if(currentState!=null)currentState.OnUpdateState();
            if(isFiring) rifleController.FireRifle();
        
            Vector3 forward = transform.TransformDirection(Vector3.forward) * characterCurrentMovementVector.z; // TransformDirection converts local direction to world space
            Vector3 right = transform.TransformDirection(Vector3.right) * characterCurrentMovementVector.x;

            Vector3 moveDirection = (forward + right).normalized;

            Vector2 movementInput = characterMoveInput * (canRun ? sprintMultiplier : 1);
            smoothedInput.x = Mathf.SmoothDamp(smoothedInput.x, movementInput.x, ref smoothVelocity.x, smoothTime);
            smoothedInput.y = Mathf.SmoothDamp(smoothedInput.y, movementInput.y, ref smoothVelocity.y, smoothTime);
            smoothedInput.x = Mathf.Clamp(smoothedInput.x, -1, 2);
            smoothedInput.y = Mathf.Clamp(smoothedInput.y, -1, 2);
        
            animator.SetFloat(playerMovementXHash , smoothedInput.x);
            animator.SetFloat(playerMovementZHash , smoothedInput.y);

            var acceleration = canRun ? movementSpeed * sprintMultiplier : movementSpeed;

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
            Debug.Log("[] [HandleInput_Move] _characterMoveInput "+ (characterMoveInput.x , characterMoveInput.y , isRunButtonPressed));
            characterCurrentMovementVector.x = characterMoveInput.x;
            characterCurrentMovementVector.z = characterMoveInput.y;
        
            isWalking = characterMoveInput.x != 0 || characterMoveInput.y != 0;
            if (characterMoveInput.y > 0 && isRunButtonPressed)
            {
                canRun = true;
            }
            else
            {
                canRun = false;    
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
            isRunButtonPressed = context.ReadValueAsButton();
            Debug.Log("[Run] [Canceled] _isRunButtonPressed " + isRunButtonPressed);
            canRun = false;    
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

        void HandleInput_Run_OnStart(InputAction.CallbackContext context)
        {
            isCrouching = false;
            isRunButtonPressed = context.ReadValueAsButton();
            Debug.Log("[Run] [Start] _isRunButtonPressed " + isRunButtonPressed);
            if(characterMoveInput.y > 0) canRun = true;
        }

        void HandleInput_Crouch(InputAction.CallbackContext context)
        {
            isCrouching = !isCrouching;
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
}
