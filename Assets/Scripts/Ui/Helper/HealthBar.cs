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

        public void UpdateHealthBar(float currentHealth , float duration)
        {
            LeanTween.value(healthBar.value , currentHealth , duration).setOnUpdate(f => healthBar.value = f);
        }
    }
}
