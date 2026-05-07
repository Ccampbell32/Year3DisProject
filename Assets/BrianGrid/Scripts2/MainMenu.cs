using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string carSceneName = "CarScene"; 

    public void PlayGame()
    {
        Debug.Log("Play button clicked!");
        Debug.Log("Trying to load scene: " + carSceneName);

        SceneManager.LoadScene(carSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            PlayGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}