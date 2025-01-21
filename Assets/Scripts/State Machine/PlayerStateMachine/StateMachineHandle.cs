using State_Machine.PlayerStateMachine.PlayerStates;

namespace State_Machine.PlayerStateMachine
{
    public class StateMachineHandle
    {
        PlayerStates.PlayerStateMachine _context;

        public StateMachineHandle(PlayerStates.PlayerStateMachine context )
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
}
