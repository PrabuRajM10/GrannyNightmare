using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StateMachine : MonoBehaviour
{
    protected bool  _isWalking, _canRun, _isCrouching, _isKilling , _canRest , _isRunButtonPressed ;
    
    [SerializeField] protected float walkSpeed, runSpeed, crouchWalkSpeed, rotationFactor = 1f;
    [FormerlySerializedAs("_movementSpeed")] [SerializeField] protected float movementSpeed;
    [FormerlySerializedAs("_restTime")] [SerializeField] protected float restTime;
    protected StateMachineHandle _states;
    protected int _canRunhash, _isCrouchingHash, _isCrouchWalkingHash,_playerMovementXHash , _playerMovementZHash, _isKillingHash ;
    protected StateMachineBase _currentState;
    protected Animator _animator;

    public StateMachineBase CurrentState { get { return _currentState; } set { _currentState = value; } }
    public bool CanRun { get { return _canRun; } set { _canRun = value; } }
    public bool IsWalking => _isWalking;
    public bool IsCrouching => _isCrouching;
    public bool IsKilling => _isKilling;
    public float MovementSpeed { set { movementSpeed = value; } }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float PlayerMovementXHash => _playerMovementXHash; 
    public float PlayerMovementZHash => _playerMovementZHash
; 
    public float CrouchWalkSpeed { get { return crouchWalkSpeed; } }
    public int CanRunhash { get { return _canRunhash; } set { _canRunhash = value; } }
    public int IsCrouchingHash { get { return _isCrouchingHash; } set { _isCrouchingHash = value; } }
    public int IsCrouchWalkingHash { get { return _isCrouchWalkingHash; } set { _isCrouchWalkingHash = value; } }
    public Animator CharacterAnimator => _animator;
    public int IsKillingHash { get { return _isKillingHash; } set { _isKillingHash = value; } }
    
    public int IsAimingHash { get { return _isKillingHash; } set { _isKillingHash = value; } }
    public float RestTime { get { return restTime; } set { restTime = value; } }
    public bool CanRest => _canRest;
    public virtual void HandleKill() { }

}
