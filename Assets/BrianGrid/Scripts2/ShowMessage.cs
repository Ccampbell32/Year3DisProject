using UnityEngine;
using TMPro;
using System.Collections;

public class ShowMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float displayTime = 3f;

    public void Show(string message)
    {
        // Stop if UI is missing
        if (messageText == null)
            return;

        StopAllCoroutines();
        StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        // Safety check before using it
        if (messageText == null)
            yield break;

        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        // Check again in case it was destroyed during the wait
        if (messageText != null)
            messageText.gameObject.SetActive(false);
    }
}