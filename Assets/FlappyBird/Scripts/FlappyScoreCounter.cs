using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlappyScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        FlappyGameManager.Instance.OnScoreChanged += UpdateScore;
    }

    private void OnDestroy()
    {
        FlappyGameManager.Instance.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}