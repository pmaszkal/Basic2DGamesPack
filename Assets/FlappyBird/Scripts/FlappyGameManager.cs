using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        flappyPlayerController = FindObjectOfType<FlappyPlayerController>();
        flappyPlayerController.gameStateChangeEvent.AddListener(HandleGameStateChange);
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
        }
    }

    private IEnumerator SpawnObstacle()
    {
        while (gameState == GameState.Active)
        {
            yield return new WaitForSeconds(timeBetweenObstaclesSpawn);
            float randomHeight = Random.Range(obstacleMinHight, obstacleMaxHight);
            Vector3 spawnPosition = new Vector3(obstacleSpawnPoint.position.x, randomHeight, 0f);
            Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        }
    }
}

public enum GameState
{
    Idle,
    Active,
    GameOver
}