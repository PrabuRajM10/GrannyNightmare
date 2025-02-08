using System;
using Managers;
using Ui.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Screens
{
    public class GameplayScreen : MonoBehaviour
    {
        [SerializeField] UiManager uiManager;
        [SerializeField] InputManager inputManager;
        [SerializeField] private HealthBar playerHealth;
        [SerializeField] private HealthBar enemyHealth;

        private void OnEnable()
        {
            inputManager.HandlePauseInput += PauseGame;
        }

        private void OnDisable()
        {
            inputManager.HandlePauseInput -= PauseGame;
        }

        public void SetUpEnemyHealth(float f)
        {
            enemyHealth.SetUp(f);
        }

        public void SetUpPlayerHealth(float value)
        {
            playerHealth.SetUp(value);
        }
        public void UpdatePlayerHealth(float currentHealth, float duration)
        {
            playerHealth.UpdateHealthBar(currentHealth, duration);
        }
        public void UpdateEnemyHealth(float currentHealth , float duration)
        {
            enemyHealth.UpdateHealthBar(currentHealth , duration);
        }

        void PauseGame()
        {
            uiManager.SwitchScreen(GameScreens.GamePlayScreen , GameScreens.PauseScreen);
        }
    }
}
