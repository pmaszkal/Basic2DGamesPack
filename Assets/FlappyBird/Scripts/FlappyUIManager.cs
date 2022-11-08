using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyUIManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverMenu;

    private void OnEnable()
    {
        FlappyGameManager.Instance.OnGameOver += ShowGameOverMenu;
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void ShowGameOverMenu()
    {
        Debug.Log("help");
        GameOverMenu.SetActive(true);
    }
}