using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyWaveBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text wavesText;
    [SerializeField] private Text enemiesText;
    [SerializeField] private Image enemyFillImage;

    [SerializeField] private float fillSpeed = 0.5f;
    private bool isFilling = false;

    public void UpdateWaveInfo(int currentWave, int totalEnemiesInWave, int currentKillCount)
    {
        wavesText.text = $"Wave: {currentWave}/{SceneManager.Instance.config.Waves.Length}";
        enemiesText.text = $"Enemies: {currentKillCount}/{totalEnemiesInWave}";

        float fillAmount = (float)currentKillCount / totalEnemiesInWave;
        StartCoroutine(QueueSmoothFill(fillAmount));
    }

    private IEnumerator QueueSmoothFill(float targetFillAmount)
    {
        while (isFilling)
        {
            yield return null;
        }

        yield return StartCoroutine(SmoothFill(targetFillAmount));
    }

    private IEnumerator SmoothFill(float targetFillAmount)
    {
        isFilling = true;
        float initialFill = enemyFillImage.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < fillSpeed)
        {
            elapsedTime += Time.deltaTime;
            enemyFillImage.fillAmount = Mathf.Lerp(initialFill, targetFillAmount, elapsedTime / fillSpeed);
            yield return null;
        }

        enemyFillImage.fillAmount = targetFillAmount;
        isFilling = false;
    }
}
