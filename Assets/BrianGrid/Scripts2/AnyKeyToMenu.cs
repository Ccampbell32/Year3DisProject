using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToMenu : MonoBehaviour
{
    public string mainMenuScene = "MainMenu";
    public float delay = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > delay && Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}