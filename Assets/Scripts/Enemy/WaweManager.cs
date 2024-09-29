using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWaveBar enemyWaveBar;
    private int currWave = 0;
    private int killCount = 0;
    private int totalEnemiesInWave = 0;
    public void StartNewWave()
    {
        StartWave(currWave);
    }
    private void StartWave(int waveIndex)
    {
        var enemies = enemySpawner.SpawnWave(waveIndex);
        totalEnemiesInWave = enemies.Count;
        killCount = 0;
        enemyWaveBar.UpdateWaveInfo(waveIndex + 1, totalEnemiesInWave, killCount);
    }
    public void CompleteWave()
    {
        ++currWave;
        if (currWave >= SceneManager.Instance.config.Waves.Length)
        {
            GlobalEvent.GameWinEvent?.Invoke();
            return;
        }
        StartNewWave();
    }
    public void PlusOneTotalEnemy()
    {
        totalEnemiesInWave++;
    }
    public void UpdateKillCount()
    {
        killCount++;
        enemyWaveBar.UpdateWaveInfo(currWave + 1,  totalEnemiesInWave, killCount);
    }
}
