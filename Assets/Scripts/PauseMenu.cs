using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPause;
    public static bool IsGamePaused = false;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            if (IsGamePaused)
                Resume();
            else
                Pause();
        }    
    }

    public void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
