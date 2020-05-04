using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Restart()
    {
        //restart game and unfreeze game
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        //return to main menu 
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //exit application
        Application.Quit();
    }
}
