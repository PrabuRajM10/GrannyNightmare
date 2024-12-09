using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float walkSpeed , runSpeed , crouchWalkSpeed;

    ActorInput playerInput;
    CharacterController characterController;
    Animator animator;      
    Vector3 characterCurrentMovementVector;
    Vector2 characterMoveInput;
    bool isMovementPressed , isRunning , isCrouching , isKilling;
    [SerializeField] float rotationFactor = 1f;
    int isWalkinghash , isRunninghash , isCrouchingHash , isCrouchWalkingHash , isKillingHash;
    Transform lockedEnemy;
    [SerializeField] Text killHint;
    private void Awake()
    {
        playerInput = new ActorInput();   
        characterController = GetComponent<CharacterController>();
        animator= GetComponent<Animator>();

        isWalkinghash = Animator.StringToHash("IsWalking");
        isRunninghash = Animator.StringToHash("IsRunning");
        isCrouchingHash = Animator.StringToHash("IsCrouching"); 
        isCrouchWalkingHash = Animator.StringToHash("IsCrouchWalking");
        isKillingHash = Animator.StringToHash("Kill");


        playerInput.PlayerMove.Move.started += context => HandleInput_Move(context);
        playerInput.PlayerMove.Move.canceled += context => HandleInput_Move(context);
        playerInput.PlayerMove.Move.performed += context => HandleInput_Move(context); 
        playerInput.PlayerMove.Run.started += context => HandleInput_Run_OnStart(context);
        playerInput.PlayerMove.Run.canceled += context => HandleInput_Run(context);
        playerInput.PlayerMove.Crouch.canceled += context => HandleInput_Crouch(context);
        playerInput.PlayerMove.Kill.started += context => HandleInput_Kill(context);
    }

    void HandleInput_Move( InputAction.CallbackContext context )
    {
        characterMoveInput = context.ReadValue<Vector2>();
        characterCurrentMovementVector.x = characterMoveInput.x;
        characterCurrentMovementVector.z = characterMoveInput.y;
        isMovementPressed = characterMoveInput.x != 0 || characterMoveInput.y != 0;
    }
    void HandleInput_Run(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    void HandleInput_Kill(InputAction.CallbackContext context)
    {
        isKilling = true;
        HandleKill();
    }

    void HandleInput_Run_OnStart(InputAction.CallbackContext context)
    {
        isCrouching = false;
        isRunning = context.ReadValueAsButton();
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
    void HandleAnimation()
    {
        bool isWalking_AnimParam = animator.GetBool(isWalkinghash);
        bool isRunning_AnimParam = animator.GetBool(isRunninghash);
        bool isCrouching_AnimParam = animator.GetBool(isCrouchingHash);
        bool isCrouchWalking_AnimParam = animator.GetBool(isCrouchWalkingHash);



        if (isMovementPressed && !isWalking_AnimParam && !isCrouching) animator.SetBool(isWalkinghash, true);
        else if(!isMovementPressed && isWalking_AnimParam) animator.SetBool(isWalkinghash, false);

        if ((isMovementPressed && isRunning) && !isRunning_AnimParam)
        {
            animator.SetBool(isCrouchingHash, false);
            animator.SetBool(isRunninghash, true);
        }
        else if ((!isMovementPressed || !isRunning) && isRunning_AnimParam) animator.SetBool(isRunninghash, false);

        if (isCrouching && !isCrouching_AnimParam) animator.SetBool(isCrouchingHash, true);
        else if (!isCrouching&& isCrouching_AnimParam) animator.SetBool(isCrouchingHash, false);

        if((isCrouching && isMovementPressed) && !isCrouchWalking_AnimParam) animator.SetBool(isCrouchWalkingHash, true);
        else if((!isMovementPressed || !isCrouching) && isCrouchWalking_AnimParam) animator.SetBool(isCrouchWalkingHash, false);
    }

    void HandleRotation()
    {
        Vector3 positionToLook;

        positionToLook.x  = characterMoveInput.x;
        positionToLook.y = 0.0f;
        positionToLook.z = characterMoveInput.y;
        Quaternion currentRot = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRot = Quaternion.LookRotation(positionToLook);
            transform.rotation = Quaternion.Slerp(currentRot, targetRot, Time.deltaTime * rotationFactor);
        }
    }

    void Update()
    {
        HandleAnimation();
        HandleRotation();
        HandleGravity();

        if(!isKilling)
        {
            if (isRunning && !isCrouching) Move(runSpeed);
            else if (isCrouching) Move(crouchWalkSpeed);
            else Move(walkSpeed);
        }
    }

    void Move(float speed)
    {
        characterController.Move(characterCurrentMovementVector * Time.deltaTime * speed);
    }

    void HandleKill()
    {
        HandleKillHintUI(false);
        var newPos = new Vector3(transform.position.x, transform.position.y, lockedEnemy.transform.position.z - 1.5f);
        transform.position = newPos;
        transform.LookAt(lockedEnemy.transform.position);
        
        characterController.enabled = false;

        animator.SetTrigger(isKillingHash);
        var enemyAnimator = lockedEnemy.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("IsDead");
        }
        lockedEnemy.GetComponent<Collider>().enabled = false;
        lockedEnemy = null;

        characterController.enabled = true;

    }
    void HandleKillHintUI(bool canShow)
    {
        killHint.gameObject.SetActive(canShow);
    }

    private void OnEnable()
    {
        playerInput.PlayerMove.Enable();
    }

    private void OnDisable()
    {
        playerInput.PlayerMove.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "Enemy")
        //{
        //    lockedEnemy = other.transform;
        //    HandleKillHintUI(true);
        //}
    }

    public void KillConfirmed()
    {
        isKilling = false;
    }

}
