using Gameplay;
using Managers;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace State_Machine.PlayerStateMachine.PlayerStates
{
    public class PlayerStateMachine : MonoBehaviour , IDamageable
    {
        private PlayerStateMachineBase  previousPlayerState;
        private CharacterController characterController;
        private Vector3 characterCurrentMovementVector;
        private Vector2 characterMoveInput;
        Vector2 smoothedInput;

        private Vector2 look;
        private const float Threshold = 0.01f;
    
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        private bool isAiming , isFiring;
        private float playerRotationTargetY;
        private readonly int sprintMultiplier = 2;
        private int isAimingHash;
        private int isDeadHash;
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
        private bool isDead;

        // private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";


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
        [SerializeField] InputManager inputManager;
        [SerializeField] private Transform lookAtTarget;
        [SerializeField] private MultiAimConstraint aimConstraint;
        [SerializeField] private TwoBoneIKConstraint[] handIks;
        
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float crouchWalkSpeed;
        [FormerlySerializedAs("health")] [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;

        public PlayerStateMachineBase CurrentPlayerState { get => currentState;
            set => currentState = value;
        }
        public bool CanRun { get => canRun;
            set => canRun = value;
        }
        public bool IsWalking => isWalking;
        public bool IsCrouching => isCrouching;
        public bool IsDead => isDead;
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
        
        public int IsDeadHash => isDeadHash;    
        public Animator CharacterAnimator => animator;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            isCrouchingHash = Animator.StringToHash("IsCrouching");
            isCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
            playerMovementXHash = Animator.StringToHash("x");
            playerMovementZHash = Animator.StringToHash("z");
            isAimingHash = Animator.StringToHash("IsAiming");  
            isDeadHash = Animator.StringToHash("IsDead");  

            states = new StateMachineHandle(this);
            currentState = states.Idle();
            currentState.OnEnterState();
            currentHealth = maxHealth;
            
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
            inputManager.HandleMoveInput += HandleInput_Move;
            inputManager.HandleRunInputStart += HandleInput_Run_OnStart;
            inputManager.HandleRunInputCanceled += HandleInput_Run;
            inputManager.HandleCrouchInput += HandleInput_Crouch;
            inputManager.HandleLookInput += HandleInput_Look;
            inputManager.HandleFireInput += HandleInput_Fire;
        }

        private void OnDisable()
        {
            inputManager.HandleMoveInput += HandleInput_Move;
            inputManager.HandleRunInputStart += HandleInput_Run_OnStart;
            inputManager.HandleRunInputCanceled += HandleInput_Run;
            inputManager.HandleCrouchInput += HandleInput_Crouch;
            inputManager.HandleLookInput += HandleInput_Look;
            inputManager.HandleFireInput += HandleInput_Fire;
        }
        void HandleInput_Move(InputAction.CallbackContext context)
        {
            characterMoveInput = context.ReadValue<Vector2>();
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
        }
    
        void HandleInput_Fire(InputAction.CallbackContext context)
        {
            isFiring = context.ReadValueAsButton();
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
            if (look.sqrMagnitude >= Threshold && !lockCameraPosition)
            {
                float deltaTimeMultiplier = inputManager.IsCurrentDeviceMouse() ? 1.0f : Time.deltaTime;

                playerRotationTargetY += look.x * deltaTimeMultiplier;
            
            }
            transform.rotation = Quaternion.Euler(0,playerRotationTargetY, 0.0f);
        }
        private void CameraRotation()
        {
            if (look.sqrMagnitude >= Threshold && !lockCameraPosition)
            {
                float deltaTimeMultiplier = inputManager.IsCurrentDeviceMouse() ? 1.0f : Time.deltaTime;

                cinemachineTargetYaw += look.x * deltaTimeMultiplier;
                cinemachineTargetPitch += look.y * deltaTimeMultiplier;
            
            }

            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }
    
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("[TakeDamage] damage " + damage);
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                isDead = true;  
            }
        }

        public void OnDead()
        {
            aimConstraint.weight = 0;
            foreach (var handIk in handIks)
            {
                handIk.weight = 0;
            }
            lookAtTarget.gameObject.SetActive(false);
            rifleController.EnableWeapon(false);
            // animator.SetLayerWeight(1,0); // upper body layer (for aim if implemented)
        }
    }
}
