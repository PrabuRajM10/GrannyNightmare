using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    [CreateAssetMenu(menuName = "Create InputManager", fileName = "InputManager", order = 0)]
    public class InputManager : ScriptableObject
    {
        public event Action<InputAction.CallbackContext> HandleMoveInput; 
        public event Action<InputAction.CallbackContext> HandleRunInputStart; 
        public event Action<InputAction.CallbackContext> HandleRunInputCanceled; 
        public event Action<InputAction.CallbackContext> HandleCrouchInput; 
        public event Action<InputAction.CallbackContext> HandleLookInput; 
        public event Action<InputAction.CallbackContext> HandleFireInput;

        public event Func<bool> IsDeviceKNM; 

        public void HandleInput_Fire(InputAction.CallbackContext context)
        {
            HandleFireInput?.Invoke(context);
        }

        public void HandleInput_Aim(InputAction.CallbackContext context)
        {
        }

        public  void HandleInput_Look(InputAction.CallbackContext context)
        {
            HandleLookInput?.Invoke(context);        
        }

        public void HandleInput_Crouch(InputAction.CallbackContext context)
        {
            HandleCrouchInput?.Invoke(context);
        }

        public void HandleInput_Run(InputAction.CallbackContext context)
        {
            HandleRunInputCanceled?.Invoke(context);
        }

        public void HandleInput_Run_OnStart(InputAction.CallbackContext context)
        {
            HandleRunInputStart?.Invoke(context);   
        }

        public void HandleInputMove(InputAction.CallbackContext context)
        {
            HandleMoveInput?.Invoke(context);
        }

        public bool IsCurrentDeviceMouse()
        {
            return IsDeviceKNM?.Invoke() ?? false;
        }
    }
}