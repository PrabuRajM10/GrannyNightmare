using UnityEngine;
using UnityEngine.UI;

namespace Ui.Helper
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Slider healthBar;

        public void SetUp(float maxHealth)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        public void UpdateHealthBar(float currentHealth)
        {
            LeanTween.value(healthBar.value , currentHealth , 0.5f).setOnUpdate(f => healthBar.value = f);
        }
    }
}
