using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyGameManager : MonoBehaviour
{
    public static FlappyGameManager Instance { get; private set; }
    private FlappyPlayerController flappyPlayerController;

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

    private void HandleGameStateChange(FlappyPlayerController.GameState gameState)
    {
        // if game started start spawning obstacles
        Debug.Log(gameState);
    }
}