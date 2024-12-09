public class StateMachineHandle
{
    StateMachine _context;

    public StateMachineHandle(StateMachine context )
    {
        _context = context;
    }   

    public StateMachineBase Idle()
    {
        return new PlayerState_Idle(_context, this);
    }
    public StateMachineBase Walk()
    {
        return new PlayerState_Walk(_context, this);
    }
    public StateMachineBase Run()
    {
        return new PlayerState_Run(_context, this);
    }
    public StateMachineBase Crouch()
    {
        return new PlayerState_Crouch(_context, this);
    }
    public StateMachineBase CrouchWalk()
    {
        return new PlayerState_CrouchWalk(_context, this);
    }
    public StateMachineBase Kill()
    {
        return new PlayerState_Kill(_context, this);
    }
    public StateMachineBase Enemy_Idle()
    {
        return new EnemyState_Idle(_context, this);
    }
    public StateMachineBase Enemy_Walk()
    {
        return new EnemyState_Walk(_context, this);
    }
}
