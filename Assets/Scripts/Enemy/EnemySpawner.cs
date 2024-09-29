using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    public List<Enemie> Enemies;
    public List<Enemie> SpawnWave(int waveIndex)
    {
        Enemies = new List<Enemie>();
        var wave = SceneManager.Instance.config.Waves[waveIndex];
        foreach (var character in wave.Characters)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            var enemy = Instantiate(character, pos, Quaternion.identity).GetComponent<Enemie>();
            Enemies.Add(enemy);
        }
        return Enemies;
    }

    public void RemoveEnemie(Enemie enemie)
    {
        Enemies.Remove(enemie);
        waveManager.UpdateKillCount();
        if (Enemies.Count == 0)
        {
            waveManager.CompleteWave();
        }
    }
    public void Spawn2LittleGoblins()
    {
        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            var enemy = Instantiate(SceneManager.Instance.config.littleEnemy, pos, Quaternion.identity).GetComponent<Enemie>();
            Enemies.Add(enemy);
            waveManager.PlusOneTotalEnemy();
        }
    }
}
