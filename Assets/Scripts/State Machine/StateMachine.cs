using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class StateMachine : MonoBehaviour
{
    protected bool  _isWalking, _canRun, _isCrouching, _isKilling , _canRest , _isRunButtonPressed ;
    
    [SerializeField] protected float walkSpeed, runSpeed, crouchWalkSpeed, rotationFactor = 1f;
    [FormerlySerializedAs("_movementSpeed")] [SerializeField] protected float movementSpeed;
    [FormerlySerializedAs("_restTime")] [SerializeField] protected float restTime;
    protected StateMachineHandle _states;
    protected int _isCrouchingHash, _isCrouchWalkingHash,_playerMovementXHash , _playerMovementZHash , _isWalkingHash;
    protected StateMachineBase _currentState;
    protected Animator _animator;
    protected NavMeshAgent _agent;

    public StateMachineBase CurrentState { get => _currentState;
        set => _currentState = value;
    }
    public bool CanRun { get => _canRun;
        set => _canRun = value;
    }
    public bool IsWalking => _isWalking;
    public bool IsCrouching => _isCrouching;
    public bool IsKilling => _isKilling;
    public float MovementSpeed { set => movementSpeed = value; }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

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
    public Animator CharacterAnimator => _animator;
    public bool CanRest => _canRest;
    public virtual void HandleKill() { }

    public void SetAgentSpeed(float speed)
    {
        _agent.speed = speed;
    }

}
