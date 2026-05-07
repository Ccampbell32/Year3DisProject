using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    public float delay = 10f; // how long cutscene plays

    void Start()
    {
        Invoke("LoadGame", delay);
    }

    void LoadGame()
    {
        SceneManager.LoadScene(1); // loads gameplay scene
    }
}