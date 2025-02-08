using System;
using UnityEngine;

namespace Ui
{
    [CreateAssetMenu(fileName ="UiManager", menuName = "ScriptableObjects/UiManager")]
    public class UiManager : ScriptableObject
    {
        private bool isGameWon;

        public bool IsGameWon => isGameWon;
        public event Action<float,float> OnUpdatePlayerHealth;
        public event Action<float,float> OnUpdateEnemyHealth;
        public event Action<float> OnSetUpEnemyHealth;
        public event Action<float> OnSetUpPlayerHealth;
        public event Action<GameScreens ,GameScreens> OnSwitchScreen;
        public event Action<GameScreens> UpdateGameState;

        public void UpdatePlayerHealth(float currentHealth, float duration = 0.5f)
        {
            OnUpdatePlayerHealth?.Invoke(currentHealth,  duration);
        }
        public void UpdateEnemyHealth(float currentHealth, float duration = 0.5f)
        {
            OnUpdateEnemyHealth?.Invoke(currentHealth,  duration);
        }

        public void SwitchScreen(GameScreens currentScreen, GameScreens nextScreen)
        {
            OnSwitchScreen?.Invoke(currentScreen, nextScreen);
            UpdateGameState?.Invoke(nextScreen);
        }

        public void SetUpPlayerHealth(float maxHealth)
        {
            OnSetUpPlayerHealth?.Invoke(maxHealth);
        }
        public void SetUpEnemyHealth(float maxHealth)
        {
            OnSetUpEnemyHealth?.Invoke(maxHealth);
        }

        public void SetGameResult(bool b)
        {
            isGameWon = b;
        }
    }
}

