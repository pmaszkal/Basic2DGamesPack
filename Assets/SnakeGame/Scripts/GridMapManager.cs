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

    public UnityEvent FoodEatenEvent;

    public GridMapManager(int width, int height)
    {
        this.width = width;
        this.height = height;

        if (FoodEatenEvent == null)
        {
            FoodEatenEvent = new UnityEvent();
        }

        SpawnFood();
    }

    private void SpawnFood()
    {
        foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = SnakeGameAssets.instance.Food;
        foodGameObject.transform.localScale = new Vector2(8, 8);
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    private void HandleFoodEating()
    {
        FoodEatenEvent.Invoke();
        Object.Destroy(foodGameObject);
        SpawnFood();
    }

    public void HandleSnakeMove(Vector2Int snakePosition)
    {
        if (foodGridPosition == snakePosition)
        {
            HandleFoodEating();
        }
    }
}