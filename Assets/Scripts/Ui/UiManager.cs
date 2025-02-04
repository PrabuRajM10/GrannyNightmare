using System;
using UnityEngine;

namespace Ui
{
    [CreateAssetMenu(fileName ="UiManager", menuName = "ScriptableObjects/UiManager")]
    public class UiManager : ScriptableObject
    {
        public event Action<float,float> OnUpdatePlayerHealth;
        public event Action<float,float> OnUpdateEnemyHealth;
        public event Action<GameScreens ,GameScreens> OnSwitchScreen;

        public void UpdatePlayerHealth(float maxHealth, float currentHealth)
        {
            OnUpdatePlayerHealth?.Invoke(maxHealth, currentHealth);
        }
        public void UpdateEnemyHealth(float maxHealth, float currentHealth)
        {
            OnUpdateEnemyHealth?.Invoke(maxHealth, currentHealth);
        }

        public void SwitchScreen(GameScreens currentScreen, GameScreens nextScreen)
        {
            OnSwitchScreen?.Invoke(currentScreen, nextScreen);  
        }
    }
}

