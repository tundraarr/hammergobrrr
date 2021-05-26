using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public bool pauseStatus = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseStatus)
            { 
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseStatus = false;
    }

    public void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pauseStatus = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene("LiamScene 1");
    }

    public void quit()
    {
        Debug.Log("Game has closed");
        Application.Quit();
    }
}
