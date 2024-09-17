using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{

    public void PlayGame(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SNDManager.SNDMInstance.source.PlayOneShot(SNDManager.SNDMInstance.click);
        }
    }
}