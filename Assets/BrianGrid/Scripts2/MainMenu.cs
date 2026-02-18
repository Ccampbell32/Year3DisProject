using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Game"; // CHANGE THIS

    public void PlayGame()
    {
        Debug.Log("Play button clicked!");
        Debug.Log("Trying to load scene: " + gameSceneName);

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();
    }
}
