using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FlappyGameManager : MonoBehaviour
{
    public static FlappyGameManager Instance { get; private set; }
    private FlappyPlayerController flappyPlayerController;
    public GameState gameState;
    [SerializeField] private float timeBetweenObstaclesSpawn = 3f;
    [SerializeField] private Transform obstacleSpawnPoint;
    [SerializeField] private GameObject obstaclePrefab;
    private float obstacleMinHight = -2.55f;
    private float obstacleMaxHight = 1.56f;
    private int score = 0;

    public event Action<int> OnScoreChanged;

    public event Action OnGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        score = 0;
    }

    private void Start()
    {
        flappyPlayerController = FindObjectOfType<FlappyPlayerController>();
        flappyPlayerController.gameStateChangeEvent += HandleGameStateChange;
    }

    private void HandleGameStateChange(GameState gs)
    {
        // if game started start spawning obstacles
        gameState = gs;
        if (gameState == GameState.Active)
        {
            StartCoroutine(SpawnObstacle());
        }
        if (gameState == GameState.GameOver)
        {
            //handle ending the game
            OnGameOver?.Invoke();
        }
    }

    private IEnumerator SpawnObstacle()
    {
        while (gameState == GameState.Active)
        {
            float randomHeight = Random.Range(obstacleMinHight, obstacleMaxHight);
            Vector3 spawnPosition = new Vector3(obstacleSpawnPoint.position.x, randomHeight, 0f);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenObstaclesSpawn);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore()
    {
        score++;
        OnScoreChanged?.Invoke(score);
        Debug.Log($"score {score}");
    }
}

public enum GameState
{
    Idle,
    Active,
    GameOver
}