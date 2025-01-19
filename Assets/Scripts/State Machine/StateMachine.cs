using System.Collections;
using System.Collections.Generic;
using State_Machine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class StateMachine : MonoBehaviour
{
    protected bool  _isWalking, _canRun, _isCrouching, _isKilling , _canRest , _isRunButtonPressed , _isChasing , _isAttacking , _isWaiting;
    
    [SerializeField] protected float walkSpeed, runSpeed, crouchWalkSpeed, rotationFactor = 1f;
    [FormerlySerializedAs("_movementSpeed")] [SerializeField] protected float movementSpeed;
    [FormerlySerializedAs("_restTime")] [SerializeField] protected float restTime;
    protected StateMachineHandle _states;
    protected int _isCrouchingHash, _isCrouchWalkingHash,_playerMovementXHash , _playerMovementZHash , _isWalkingHash , _isChasingHash , _isAttackingHash , _isWaitingHash;
    protected PlayerStateMachineBase _currentPlayerState;
    protected Animator _animator;
    protected NavMeshAgent _agent;

    public PlayerStateMachineBase CurrentPlayerState { get => _currentPlayerState;
        set => _currentPlayerState = value;
    }
    public bool CanRun { get => _canRun;
        set => _canRun = value;
    }
    public bool IsWalking => _isWalking;
    public bool IsChasing => _isChasing;
    public bool IsAttacking => _isAttacking;
    public bool IsCrouching => _isCrouching;
    public bool IsKilling => _isKilling;
    public bool IsWaiting => _isWaiting;
    public float MovementSpeed { set => movementSpeed = value; }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float RestTime
    {
        get => restTime;
        set => restTime = value;
    }

    public float CrouchWalkSpeed => crouchWalkSpeed;
    public int IsCrouchingHash { get => _isCrouchingHash;
        set => _isCrouchingHash = value;
    }
    public int IsCrouchWalkingHash { get => _isCrouchWalkingHash;
        set => _isCrouchWalkingHash = value;
    }
    
    public int IsWalkingHash { get => _isWalkingHash;
        set => _isWalkingHash = value;
    }

    public int IsChasingHash => _isChasingHash;
    public int IsWaitingHash => _isWaitingHash;
    public int IsAttackingHash => _isAttackingHash;
    public Animator CharacterAnimator => _animator;
    public bool CanRest => _canRest;
    public void SetAgentSpeed(float speed)
    {
        _agent.speed = speed;
    }

}
