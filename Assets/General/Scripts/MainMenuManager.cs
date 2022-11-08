using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void LaunchGame(int sceneBuildIndex)
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}