using UnityEngine;
using TMPro;

public class SnakeGameManager : MonoBehaviour
{
    private int gridWidth = 20;
    private int gridHeight = 20;
    private int points = 0;
    private GridMapManager gridMapManager;
    [SerializeField] private SnakeController snakeController;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI GameOverText;

    public static SnakeGameManager Instance { get; private set; }

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

        gridMapManager = new GridMapManager(gridWidth, gridHeight);

        if (snakeController == null)
        {
            snakeController = FindObjectOfType<SnakeController>();
        }
        snakeController.Init(gridMapManager);
        gridMapManager.Init(snakeController);

        gridMapManager.FoodEatenEvent.AddListener(AddPoint);
        GameOverText.gameObject.SetActive(false);
    }

    public void HandleSnakeDeath()
    {
        GameOverText.gameObject.SetActive(true);
        Debug.Log("SNAKE IS DEAD");
    }

    private void AddPoint()
    {
        points += 1;
        ScoreText.text = (points * 10).ToString();
        Debug.Log(points);
    }
}