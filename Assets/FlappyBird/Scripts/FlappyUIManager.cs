using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyUIManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverMenu;

    private void OnEnable()
    {
        FlappyGameManager.Instance.OnGameOver += ShowGameOverMenu;
    }

    private void ShowGameOverMenu()
    {
        Debug.Log("help");
        GameOverMenu.SetActive(true);
    }
}