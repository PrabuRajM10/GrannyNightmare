using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    [CreateAssetMenu(menuName = "Create InputManager", fileName = "InputManager", order = 0)]
    public class InputManager : ScriptableObject
    {
        private bool canProcessInput;
        private bool gamePaused;

        public bool CanProcessInput
        {
            get => canProcessInput; 
            set => canProcessInput = value;
        }

        public event Action<InputAction.CallbackContext> HandleMoveInput; 
        public event Action<InputAction.CallbackContext> HandleRunInputStart; 
        public event Action<InputAction.CallbackContext> HandleRunInputCanceled; 
        public event Action<InputAction.CallbackContext> HandleCrouchInput; 
        public event Action<InputAction.CallbackContext> HandleLookInput; 
        public event Action<InputAction.CallbackContext> HandleFireInput;
        public event Action HandlePauseInput;

        public event Func<bool> IsDeviceKNM; 

        public void HandleInput_Fire(InputAction.CallbackContext context)
        {
            if(!canProcessInput || gamePaused)return;
            HandleFireInput?.Invoke(context);
        }

        public void HandleInput_Aim(InputAction.CallbackContext context)
        {
        }

        public  void HandleInput_Look(InputAction.CallbackContext context)
        {
            if(!canProcessInput)return;
            HandleLookInput?.Invoke(context);        
        }

        public void HandleInput_Crouch(InputAction.CallbackContext context)
        {
            if(!canProcessInput)return;
            HandleCrouchInput?.Invoke(context);
        }

        public void HandleInput_Run(InputAction.CallbackContext context)
        {
            if(!canProcessInput)return;
            HandleRunInputCanceled?.Invoke(context);
        }

        public void HandleInput_Run_OnStart(InputAction.CallbackContext context)
        {
            if(!canProcessInput)return;
            HandleRunInputStart?.Invoke(context);   
        }

        public void HandleInputMove(InputAction.CallbackContext context)
        {
            if(!canProcessInput)return;
            HandleMoveInput?.Invoke(context);
        }

        public bool IsCurrentDeviceMouse()
        {
            return IsDeviceKNM?.Invoke() ?? false;
        }

        public void HandleInput_Pause()
        {
            if(!canProcessInput)return;
            HandlePauseInput?.Invoke();
        }

        public void OnGamePaused(bool paused)
        {
            gamePaused = paused;
        }
    }
}