using Ui.Helper;
using UnityEngine;

namespace Ui.Screens
{
    public class GameplayScreen : MonoBehaviour
    {
        [SerializeField] private HealthBar playerHealth;
        [SerializeField] private HealthBar enemyHealth;

        public void SetUpEnemyHealth(float f)
        {
            enemyHealth.SetUp(f);
        }

        public void SetUpPlayerHealth(float value)
        {
            playerHealth.SetUp(value);
        }
        public void UpdatePlayerHealth(float currentHealth)
        {
            playerHealth.UpdateHealthBar(currentHealth);
        }
        public void UpdateEnemyHealth(float currentHealth)
        {
            enemyHealth.UpdateHealthBar(currentHealth);
        }

    }
}
