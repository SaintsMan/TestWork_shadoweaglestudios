using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    public Player Player;
    public GameObject Lose;
    public GameObject Win;

    public LevelConfig config;
    [SerializeField] private WaveManager waveManager;
    public EnemySpawner enemySpawner;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GlobalEvent.DeadSuperGoblin +=  enemySpawner.Spawn2LittleGoblins;
        GlobalEvent.GameOverEvent += GameOver;
        GlobalEvent.GameWinEvent += WinGame;
    }

    private void Start()
    {
        waveManager.StartNewWave();
    }

    private void GameOver()
    {
        Lose.SetActive(true);
    }
    private void WinGame()
    {
        Win.SetActive(true);
    }

    private void OnDestroy()
    {
        GlobalEvent.DeadSuperGoblin -=  enemySpawner.Spawn2LittleGoblins;
        GlobalEvent.GameOverEvent -= GameOver;
        GlobalEvent.GameWinEvent -= WinGame;
    }
}
