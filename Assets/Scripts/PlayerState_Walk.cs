using UnityEngine;
public class PlayerState_Walk : StateMachineBase
{
    public PlayerState_Walk(StateMachine context, StateMachineHandle playerStateHandle) : base(context, playerStateHandle){}
    public override void CheckSwitchState()
    {
        if (_context.IsRunning) SwitchStates(stateHandle.Run());
        else if (!_context.IsWalking) SwitchStates(stateHandle.Idle());
        else if (_context.IsCrouching) SwitchStates(stateHandle.Crouch());
        else if (_context.IsKilling) ExecuteKill();
    }

    public override void OnEnterState()
    {
        _context.MovementSpeed = _context.WalkSpeed;
        _context.CharacterAnimator.SetBool(_context.IsWalkinghash, true);
    }

    public override void OnExitState()
    {
        _context.CharacterAnimator.SetBool(_context.IsWalkinghash, false);
    }

    public override void OnUpdateState()
    {
        CheckSwitchState();
    }

}
