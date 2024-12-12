using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected bool  _isWalking, _isRunning, _isCrouching, _isKilling , _canRest;
    [SerializeField] protected float walkSpeed, runSpeed, crouchWalkSpeed, rotationFactor = 1f, _movementSpeed , _restTime;
    protected StateMachineHandle _states;
    protected int _isWalkinghash, _isRunninghash, _isCrouchingHash, _isCrouchWalkingHash,_PlayerMovementXHash , _playerMovementZHash, _isKillingHash ;
    protected StateMachineBase _currentState;
    protected Animator animator;

    public StateMachineBase CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool IsRunning { get { return _isRunning; } set { _isRunning = value; } }
    public bool IsWalking => _isWalking;
    public bool IsCrouching => _isCrouching;
    public bool IsKilling => _isKilling;
    public float MovementSpeed { set { _movementSpeed = value; } }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float PlayerMovementXHash => _PlayerMovementXHash; 
    public float PlayerMovementZHash => _playerMovementZHash
; 
    public float CrouchWalkSpeed { get { return crouchWalkSpeed; } }
    public int IsWalkinghash { get { return _isWalkinghash; } set { _isWalkinghash = value; } }
    public int IsRunninghash { get { return _isRunninghash; } set { _isRunninghash = value; } }
    public int IsCrouchingHash { get { return _isCrouchingHash; } set { _isCrouchingHash = value; } }
    public int IsCrouchWalkingHash { get { return _isCrouchWalkingHash; } set { _isCrouchWalkingHash = value; } }
    public Animator CharacterAnimator => animator;
    public int IsKillingHash { get { return _isKillingHash; } set { _isKillingHash = value; } }
    public float RestTime { get { return _restTime; } set { _restTime = value; } }
    public bool CanRest => _canRest;
    public virtual void HandleKill() { }

}
