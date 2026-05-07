using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Scenes")]
    public string gameSceneName = "GameScene";
    public string menuSceneName = "MainMenu";

    public void Retry()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}