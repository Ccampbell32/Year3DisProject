using TMPro;
using UnityEngine;

public class TextGlitch : MonoBehaviour
{
    TextMeshProUGUI txt;

    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        InvokeRepeating(nameof(Glitch), 2f, Random.Range(3f, 6f));
    }

    void Glitch()
    {
        txt.text = txt.text.Replace("e", "ë");
        Invoke(nameof(ResetText), 0.2f);
    }

    void ResetText()
    {
        txt.text = txt.text.Replace("ë", "e");
    }
}
