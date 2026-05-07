using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeAndLoad : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;
    public float cutsceneLength = 20f; // total time before scene change

    void Start()
    {
        Invoke("StartFade", cutsceneLength - fadeDuration);
    }

    void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(2);
    }
}