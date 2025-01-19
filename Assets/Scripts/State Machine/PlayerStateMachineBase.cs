using State_Machine.States.PlayerStates;
using UnityEngine;

namespace State_Machine
{
    public abstract class PlayerStateMachineBase
    {
        protected PlayerStateMachine _context;
        protected StateMachineHandle stateHandle;

        public PlayerStateMachineBase(PlayerStateMachine context, StateMachineHandle playerStateHandle)
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
