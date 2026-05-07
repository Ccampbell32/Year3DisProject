using UnityEngine;

public class CarCameraShake : MonoBehaviour
{
    public float shakeAmount = 0.02f;
    public float speed = 2f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * speed) * shakeAmount;
        float y = Mathf.Cos(Time.time * speed) * shakeAmount;

        transform.localPosition = startPos + new Vector3(x, y, 0);
    }
}
    