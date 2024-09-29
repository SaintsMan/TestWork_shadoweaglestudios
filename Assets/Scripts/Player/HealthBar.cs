using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Text countText;
    [SerializeField] private Image healthFill; 
    [SerializeField] private float lerpSpeed = 1.0f; 
    [Header("Colors")]
    [SerializeField] private Color healthyColor = Color.green;  
    [SerializeField] private Color lowHealthColor = Color.red;  
    private float maxHealth;
    private Coroutine currentLerpCoroutine;

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
        healthFill.fillAmount = 1f; 
        healthFill.color = healthyColor;
        countText.text = $"{maxHealth}/{maxHealth}";
    }

    public void SetHealth(float health)
    {
        if (currentLerpCoroutine != null)
        {
            StopCoroutine(currentLerpCoroutine);
        }
        currentLerpCoroutine = StartCoroutine(SmoothHealthChange(health));
    }

    private IEnumerator SmoothHealthChange(float targetHealth)
    {
        float startFill = healthFill.fillAmount;
        float targetFill = targetHealth / maxHealth;
        float elapsedTime = 0f;

        float startHealthValue = maxHealth * startFill; 
        float currentHealthValue;

        while (elapsedTime < lerpSpeed)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpSpeed;

            healthFill.fillAmount = Mathf.Lerp(startFill, targetFill, t);

            healthFill.color = Color.Lerp(lowHealthColor, healthyColor, healthFill.fillAmount);

            currentHealthValue = Mathf.Lerp(startHealthValue, targetHealth, t);
            countText.text = $"{Mathf.Round(currentHealthValue)}/{maxHealth}";

            yield return null;
        }

        healthFill.fillAmount = targetFill;
        healthFill.color = Color.Lerp(lowHealthColor, healthyColor, healthFill.fillAmount);
        countText.text = $"{Mathf.Round(targetHealth)}/{maxHealth}";
    }
}
