using Ui.Helper;
using UnityEngine;

namespace Ui.Screens
{
    public class GameplayScreen : MonoBehaviour
    {
        [SerializeField] private HealthBar playerHealth;
        [SerializeField] private HealthBar enemyHealth;

        public void UpdatePlayerHealth(float maxHealth, float currentHealth)
        {
            playerHealth.UpdateHealthBar(maxHealth, currentHealth);
        }
        public void UpdateEnemyHealth(float maxHealth, float currentHealth)
        {
            enemyHealth.UpdateHealthBar(maxHealth, currentHealth);
        }
    }
}
