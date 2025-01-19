using State_Machine;
using State_Machine.States.EnemyStates;
using State_Machine.States.PlayerStates;

public class StateMachineHandle
{
    PlayerStateMachine _context;

    public StateMachineHandle(PlayerStateMachine context )
    {
        _context = context;
    }   

    public PlayerStateMachineBase Idle()
    {
        return new PlayerPlayerStateIdle(_context, this);
    }
    public PlayerStateMachineBase Walk()
    {
        return new PlayerPlayerStateWalk(_context, this);
    }
    public PlayerStateMachineBase Run()
    {
        return new PlayerPlayerStateRun(_context, this);
    }
    public PlayerStateMachineBase Crouch()
    {
        return new PlayerPlayerStateCrouch(_context, this);
    }
    public PlayerStateMachineBase CrouchWalk()
    {
        return new PlayerPlayerStateCrouchWalk(_context, this);
    }
}
