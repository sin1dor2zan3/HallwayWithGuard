using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour {

    public void ReplayButton() 
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitButton() 
    {
        Application.Quit();
    }

    public void MainMenuButton()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
