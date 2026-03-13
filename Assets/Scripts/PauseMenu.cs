using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    bool gamePaused = false;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] InputActionReference pauseAction;

    [SerializeField] GameObject firstButton;

    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    void OnEnable()
    {
        pauseAction.action.performed += TogglePause;
        pauseAction.action.Enable();
    }

    void OnDisable()
    {
        pauseAction.action.performed -= TogglePause;
        pauseAction.action.Disable();
    }

    void TogglePause(InputAction.CallbackContext context)
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
        else
        {
            Time.timeScale = 1;
            gamePaused = false;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}