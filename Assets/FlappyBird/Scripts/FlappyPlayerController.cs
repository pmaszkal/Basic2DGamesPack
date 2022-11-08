using System;
using UnityEngine;

public class FlappyPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float boostForce = 100f;
    private float maxSpeed = 6f;
    private GameState gameState;

    public event Action<GameState> gameStateChangeEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        gameState = FlappyGameManager.Instance.gameState;
        switch (gameState)
        {
            case GameState.Idle:
                break;

            case GameState.Active:
                ClampSpeed();
                break;

            case GameState.GameOver:
                break;
        }
    }

    private void ClampSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void OnClick()
    {
        switch (gameState)
        {
            case GameState.Idle:
                StartGame();
                break;

            case GameState.Active:
                Boost();
                break;

            case GameState.GameOver:
                break;
        }
    }

    private void StartGame()
    {
        gameState = GameState.Active; //to prevent starting the game twice when quick double click
        gameStateChangeEvent?.Invoke(GameState.Active);
        rb.isKinematic = false;
        Boost();
    }

    private void Boost()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity = Vector2.zero;
        }
        Vector2 boostForceVector = new Vector2(0f, boostForce);
        rb.AddForce(boostForceVector);
    }

    private void HandleDeath()
    {
        gameStateChangeEvent?.Invoke(GameState.GameOver);
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameState == GameState.GameOver)
            return;
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleDeath();
        }
        else if (collision.gameObject.CompareTag("ScoreLine"))
        {
            HandleGetScore();
        }
    }

    private void HandleGetScore()
    {
        FlappyGameManager.Instance.AddScore();
    }
}