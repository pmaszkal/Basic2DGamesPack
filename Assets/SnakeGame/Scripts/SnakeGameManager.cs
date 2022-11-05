using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameManager : MonoBehaviour
{
    private int gridWidth = 20;
    private int gridHeight = 20;
    private int points = 0;
    private GridMapManager gridMapManager;
    [SerializeField] private SnakeController snakeController;

    private void Awake()
    {
        if (snakeController == null)
        {
            snakeController = FindObjectOfType<SnakeController>();
        }

        gridMapManager = new GridMapManager(gridWidth, gridHeight);

        gridMapManager.FoodEatenEvent.AddListener(AddPoint);
        snakeController.snakeMoveEvent.AddListener(gridMapManager.HandleSnakeMove);
    }

    private void AddPoint()
    {
        points += 1;
        Debug.Log(points);
    }
}