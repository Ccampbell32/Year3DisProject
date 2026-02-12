using UnityEngine;

public class UIFlicker : MonoBehaviour
{
    CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        InvokeRepeating(nameof(Flicker), 0, Random.Range(0.1f, 0.4f));
    }

    void Flicker()
    {
        cg.alpha = Random.Range(0.85f, 1f);
    }
}
