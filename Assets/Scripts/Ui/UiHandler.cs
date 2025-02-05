using System;
using System.Collections.Generic;
using Ui.Screens;
using UnityEngine;

namespace Ui
{
    public class UiHandler : MonoBehaviour
    {
        [SerializeField] private GameplayScreen gameplayScreen;
        [SerializeField] private MenuScreen menuScreen;
        [SerializeField] private PauseScreen pauseScreen;
        [SerializeField] private SettingsScreen settingsScreen;
        
        [SerializeField] UiManager uiManager;

        private void Awake()
        {
            uiManager.OnSetUpPlayerHealth += UiManagerOnSetUpPlayerHealth;
            uiManager.OnSetUpEnemyHealth += UiManagerOnSetUpEnemyHealth;
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

        private void UiManagerOnUpdatePlayerHealth(float arg2)
        {
            gameplayScreen.UpdatePlayerHealth(arg2);
        }

        private void UiManagerOnUpdateEnemyHealth(float arg2)
        {
            gameplayScreen.UpdateEnemyHealth(arg2);
        }

        public void SwitchScreen(GameScreens currentGameScreen , GameScreens newGameScreen)
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
                case GameScreens.SettingsScreen:
                    settingsScreen.gameObject.SetActive(state);
                    break;
                case GameScreens.GamePlayScreen:
                    gameplayScreen.gameObject.SetActive(state);
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
        SettingsScreen,
        GamePlayScreen     
    }
}