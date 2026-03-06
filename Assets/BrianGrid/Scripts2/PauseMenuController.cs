using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToPause()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}