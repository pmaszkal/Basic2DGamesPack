using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridMapManager
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private SnakeController snakeController;

    public UnityEvent FoodEatenEvent;

    public GridMapManager(int width, int height)
    {
        this.width = width;
        this.height = height;

        if (FoodEatenEvent == null)
        {
            FoodEatenEvent = new UnityEvent();
        }
    }

    public void Init(SnakeController snakeController)
    {
        this.snakeController = snakeController;
        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snakeController.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = SnakeGameAssets.Instance.Food;
        foodGameObject.transform.localScale = new Vector2(8, 8);
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    private void HandleFoodEating()
    {
        FoodEatenEvent.Invoke();
        Object.Destroy(foodGameObject);
        SpawnFood();
    }

    public bool SnakeTryEat(Vector2Int snakePosition)
    {
        if (foodGridPosition == snakePosition)
        {
            HandleFoodEating();
            return true;
        }
        return false;
    }

    public Vector2Int CheckGridConstrains(Vector2Int gridPosition)
    {
        if (gridPosition.x > 20)
            gridPosition.x = 0;
        else if (gridPosition.y > 20)
            gridPosition.y = 0;
        else if (gridPosition.x < 0)
            gridPosition.x = 20;
        else if (gridPosition.y < 0)
            gridPosition.y = 20;
        return gridPosition;
    }
}