using System;
using UnityEngine;

namespace Ui
{
    [CreateAssetMenu(fileName ="UiManager", menuName = "ScriptableObjects/UiManager")]
    public class UiManager : ScriptableObject
    {
        public event Action<float> OnUpdatePlayerHealth;
        public event Action<float> OnUpdateEnemyHealth;
        public event Action<float> OnSetUpEnemyHealth;
        public event Action<float> OnSetUpPlayerHealth;
        public event Action<GameScreens ,GameScreens> OnSwitchScreen;

        public void UpdatePlayerHealth(float currentHealth)
        {
            OnUpdatePlayerHealth?.Invoke(currentHealth);
        }
        public void UpdateEnemyHealth(float currentHealth)
        {
            OnUpdateEnemyHealth?.Invoke(currentHealth);
        }

        public void SwitchScreen(GameScreens currentScreen, GameScreens nextScreen)
        {
            OnSwitchScreen?.Invoke(currentScreen, nextScreen);  
        }

        public void SetUpPlayerHealth(float maxHealth)
        {
            OnSetUpPlayerHealth?.Invoke(maxHealth);
        }
        public void SetUpEnemyHealth(float maxHealth)
        {
            OnSetUpEnemyHealth?.Invoke(maxHealth);
        }
    }
}

