using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float strength)
    {
        float time = 0f;

        while (time < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            time += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}