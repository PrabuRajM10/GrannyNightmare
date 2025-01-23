using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;
        private ActorInput mainPlayerInput;
        private PlayerInput playerInput;

        private bool IsCurrentDeviceMouse => playerInput.currentControlScheme == "KeyboardMouse";
        
        private void Awake()
        {
            mainPlayerInput = new ActorInput();
            playerInput = GetComponent<PlayerInput>();
            
            mainPlayerInput.PlayerMove.Move.started += HandleInputMove;
            mainPlayerInput.PlayerMove.Move.canceled += HandleInputMove;
            mainPlayerInput.PlayerMove.Move.performed += HandleInputMove;
            mainPlayerInput.PlayerMove.Run.started += HandleInput_Run_OnStart;
            mainPlayerInput.PlayerMove.Run.canceled += HandleInput_Run;
            mainPlayerInput.PlayerMove.Crouch.canceled += HandleInput_Crouch;
            mainPlayerInput.PlayerMove.Look.started += HandleInput_Look;
            mainPlayerInput.PlayerMove.Look.canceled += HandleInput_Look;
            mainPlayerInput.PlayerMove.Look.performed += HandleInput_Look;
            // mainPlayerInput.PlayerMove.Aim.started += context => HandleInput_Aim(context);
            // mainPlayerInput.PlayerMove.Aim.canceled += context => HandleInput_Aim(context);
            mainPlayerInput.PlayerMove.Fire.started += HandleInput_Fire;
            mainPlayerInput.PlayerMove.Fire.canceled += HandleInput_Fire;
        }
        private void OnEnable()
        {
            mainPlayerInput.PlayerMove.Enable();
            inputManager.IsDeviceKNM += InputManagerOnIsDeviceKNM;
        }

        
        private void OnDisable()
        {
            inputManager.IsDeviceKNM -= InputManagerOnIsDeviceKNM;
            mainPlayerInput.PlayerMove.Disable();
        }
        
        private bool InputManagerOnIsDeviceKNM()
        {
            return IsCurrentDeviceMouse;
        }

        private void HandleInput_Fire(InputAction.CallbackContext context)
        {
            inputManager.HandleInput_Fire(context); 
        }

        private void HandleInput_Aim(InputAction.CallbackContext context)
        {
        }

        private void HandleInput_Look(InputAction.CallbackContext context)
        {
            inputManager.HandleInput_Look(context); 
        }

        private void HandleInput_Crouch(InputAction.CallbackContext context)
        {
            inputManager.HandleInput_Crouch(context); 
        }

        private void HandleInput_Run(InputAction.CallbackContext context)
        {
            inputManager.HandleInput_Run(context); 
        }

        private void HandleInput_Run_OnStart(InputAction.CallbackContext context)
        {
            inputManager.HandleInput_Run_OnStart(context); 
        }

        private void HandleInputMove(InputAction.CallbackContext context)
        {
            inputManager.HandleInputMove(context); 
        }
    }
}