using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlappyPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float boostForce = 100f;
    private float maxSpeed = 6f;
    private GameState gameState;

    public FlappyGameStateEvent gameStateChangeEvent;

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
            default:
            case GameState.Idle:
                StartGame();
                break;

            case GameState.Active:
                Boost();
                break;
        }
    }

    private void StartGame()
    {
        rb.isKinematic = false;
        Boost();
        gameStateChangeEvent.Invoke(GameState.Active);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Obstacle");
        }
        else if (collision.gameObject.CompareTag("ScoreLine"))
        {
            Debug.Log("ScoreLine");
        }
    }

    [System.Serializable]
    public class FlappyGameStateEvent : UnityEvent<GameState>
    {
    }
}