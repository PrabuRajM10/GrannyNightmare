using UnityEngine;

namespace State_Machine.PlayerStateMachine
{
    public abstract class PlayerStateMachineBase
    {
        protected PlayerStates.PlayerStateMachine _context;
        protected StateMachineHandle stateHandle;

        public PlayerStateMachineBase(PlayerStates.PlayerStateMachine context, StateMachineHandle playerStateHandle)
        {
            _context = context;  
            stateHandle= playerStateHandle;  
        }
        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnExitState();
        public abstract void CheckSwitchState();

        protected void SwitchStates(PlayerStateMachineBase newPlayerState)
        {
            Debug.Log(" SwitchStates " + newPlayerState.ToString());
            OnExitState();
            newPlayerState.OnEnterState();
            _context.CurrentPlayerState = newPlayerState;
        }

    }
}
