using System;
using System.Collections.Generic;
using Ui.Screens;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField] private GameplayScreen gameplayScreen;
        [SerializeField] private MenuScreen menuScreen;
        [SerializeField] private PauseScreen pauseScreen;
        [SerializeField] private GameResultScreen gameResultScreen;
        
        [SerializeField] UiManager uiManager;
        [SerializeField] private GameScreens initialScreen = GameScreens.MenuScreen;

        private void Awake()
        {
            uiManager.OnSetUpPlayerHealth += UiManagerOnSetUpPlayerHealth;
            uiManager.OnSetUpEnemyHealth += UiManagerOnSetUpEnemyHealth;
            EnableScreen(initialScreen, true);

        }

        private void OnDestroy()
        {
            uiManager.OnSetUpPlayerHealth -= UiManagerOnSetUpPlayerHealth;
            uiManager.OnSetUpEnemyHealth -= UiManagerOnSetUpEnemyHealth;
        }


        private void OnEnable()
        {
            uiManager.OnUpdateEnemyHealth += UiManagerOnUpdateEnemyHealth;
            uiManager.OnUpdatePlayerHealth += UiManagerOnUpdatePlayerHealth;
            uiManager.OnSwitchScreen += SwitchScreen;
        }

        private void OnDisable()
        {
            uiManager.OnUpdateEnemyHealth -= UiManagerOnUpdateEnemyHealth;
            uiManager.OnUpdatePlayerHealth -= UiManagerOnUpdatePlayerHealth;
            uiManager.OnSwitchScreen -= SwitchScreen;
        }
        private void UiManagerOnSetUpEnemyHealth(float obj)
        {
            gameplayScreen.SetUpEnemyHealth(obj);
        }

        private void UiManagerOnSetUpPlayerHealth(float value)
        {
            gameplayScreen.SetUpPlayerHealth(value);
        }

        private void UiManagerOnUpdatePlayerHealth(float arg2 , float duration)
        {
            gameplayScreen.UpdatePlayerHealth(arg2, duration);
        }

        private void UiManagerOnUpdateEnemyHealth(float arg2, float duration)
        {
            gameplayScreen.UpdateEnemyHealth(arg2, duration);
        }

        private void SwitchScreen(GameScreens currentGameScreen , GameScreens newGameScreen)
        {
            EnableScreen(currentGameScreen, false);
            EnableScreen(newGameScreen, true);
        }

        void EnableScreen(GameScreens screen , bool state)
        {
            switch (screen)
            {
                case GameScreens.MenuScreen:
                    menuScreen.gameObject.SetActive(state);
                    break;
                case GameScreens.PauseScreen:
                    pauseScreen.gameObject.SetActive(state);
                    break;
                case GameScreens.GamePlayScreen:
                    gameplayScreen.gameObject.SetActive(state);
                    break;
                case GameScreens.GameResult:
                    gameResultScreen.gameObject.SetActive(state);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
            }
        }
        
    }

    public enum GameScreens
    {
        MenuScreen,
        PauseScreen,
        GamePlayScreen,
        GameResult
    }
}