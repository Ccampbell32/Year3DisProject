using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinSceneController : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup winTextGroup;
    public CanvasGroup continueTextGroup;

    [Header("Settings")]
    public float fadeSpeed = 1f;
    public float delayBeforeContinue = 2f;

    [Header("Scene Name")]
    public string menuSceneName = "MainMenu"; // IMPORTANT

    bool canContinue = false;

    void Start()
    {
        StartCoroutine(PlayEnding());
    }

    IEnumerator PlayEnding()
    {
        // Fade in main text
        yield return StartCoroutine(FadeIn(winTextGroup));

        // Wait before continue text
        yield return new WaitForSeconds(delayBeforeContinue);

        // Fade in "Press any key"
        yield return StartCoroutine(FadeIn(continueTextGroup));

        canContinue = true;
    }

    void Update()
    {
        if (canContinue && Input.anyKeyDown)
        {
            LoadMenu();
        }
    }

    IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeSpeed;
            group.alpha = t;
            yield return null;
        }

        group.alpha = 1f;
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}