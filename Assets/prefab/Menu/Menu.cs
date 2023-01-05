using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public PauseMenu menu;
   public void StartGame()
    {
        SceneManager.LoadScene("Game");
        menu.Resume();
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
