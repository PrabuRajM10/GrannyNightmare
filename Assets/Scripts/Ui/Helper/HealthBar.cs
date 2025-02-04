using UnityEngine;
using UnityEngine.UI;

namespace Ui.Helper
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image healthBar;

        public void UpdateHealthBar(float maxHealth, float currentHealth)
        {
            healthBar.fillAmount = (currentHealth / maxHealth) * 100;
        }
        
        int CountDigitsBeforePoint(int num)
        {
            num = Mathf.Abs(num); // Handle negative numbers

            if (num < 1) return 1; // If it's between 0 and 1 (e.g., 0.45), it has 1 digit before the decimal

            return (int)Mathf.Floor(Mathf.Log10(num)) + 1;
        }
    }
}
