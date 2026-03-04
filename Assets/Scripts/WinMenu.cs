using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour {

    public void ReplayButton() 
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitButton() 
    {
        Application.Quit();
    }
    
    public void MenuButton() 
    {
        SceneManager.LoadSceneAsync(0);
    }

}
