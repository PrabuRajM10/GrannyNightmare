using System;
using Gameplay;
using ObjectPooling;
using State_Machine.EnemyStateMachine;
using State_Machine.PlayerStateMachine.PlayerStates;
using Ui;
using UnityEngine;
using AudioType = Gameplay.AudioType;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] GameAudios gameAudios;
        [SerializeField] private EnemyStateMachine enemy;
        [SerializeField] private PlayerStateMachine player;

        [SerializeField]private GameState currentState;
        

        private void Awake()
        {
            SoundManager.Init(gameAudios , poolManager);
        }

        private void OnEnable()
        {
            uiManager.UpdateGameState += UpdateGameState;
        }

        private void OnDisable()
        {
            uiManager.UpdateGameState -= UpdateGameState;

        }


        void Start()
        {
            SoundManager.PlaySound(AudioType.Bg ,default , true);
        }

        private void UpdateGameState(GameScreens screen)
        {
            switch (screen)
            {
                case GameScreens.MenuScreen:
                    SwitchState(GameState.Menu);
                    break;
                case GameScreens.PauseScreen:
                    SwitchState(GameState.Pause);
                    break;
                case GameScreens.GamePlayScreen:
                    SwitchState(GameState.Gameplay);
                    break;
                case GameScreens.GameResult:
                    SwitchState(GameState.GameResult);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
            }
        }

        void SwitchState(GameState state)
        {
            HandleStateExit();
            currentState = state;
            HandleStateEnter();
        }

        private void HandleStateEnter()
        {
            switch (currentState)
            {
                case GameState.Menu:
                    HandleOnMenuStateEnter();
                    break;
                case GameState.Gameplay:
                    HandleOnGameplayStateEnter();
                    break;
                case GameState.Pause:
                    HandleOnPauseStateEnter();
                    break;
                case GameState.GameResult:
                    HandleOnResultStateEnter();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void HandleStateExit()
        {
            switch (currentState)
            {
                case GameState.Menu:
                    HandleOnMenuStateExit();
                    break;
                case GameState.Gameplay:
                    HandleOnGameplayStateExit();
                    break;
                case GameState.Pause:
                    HandleOnPauseStateExit();
                    break;
                case GameState.GameResult:
                    HandleOnResultStateExit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleOnMenuStateEnter()
        {
            inputManager.CanProcessInput = false;
            SoundManager.PlaySound(AudioType.Bg ,default , true);
        }

        private void HandleOnResultStateEnter()
        {
            inputManager.CanProcessInput = false;
            enemy.Reset();
            player.Reset();
            SoundManager.StopAudio(AudioType.BossFightMusic);
            SoundManager.PlaySound(AudioType.Bg , default , true);
        }

        private void HandleOnPauseStateEnter()
        {
            player.ResetMovementAndRotation();
            Time.timeScale = 0;
        }

        private void HandleOnGameplayStateEnter()
        {
            Cursor.lockState = CursorLockMode.Locked;
            enemy.StartEnemyBehaviour();
            SoundManager.StopAudio(AudioType.Bg);
            SoundManager.PlaySound(AudioType.BossFightMusic , default , true);
        }

        private void HandleOnResultStateExit()
        {
            inputManager.CanProcessInput = true;
        }

        private void HandleOnMenuStateExit()
        {
            inputManager.CanProcessInput = true;
        }

        private void HandleOnPauseStateExit()
        {
            Time.timeScale = 1;
        }

        private void HandleOnGameplayStateExit()
        {

            Cursor.lockState = CursorLockMode.None;
            
        }

    }
    
    public enum GameState
    {
        Menu,
        Gameplay,
        Pause,
        GameResult,       
    }
}
