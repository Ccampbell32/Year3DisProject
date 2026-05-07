using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
    public float time = 5f;

    void Start()
    {
        Destroy(gameObject, time);
    }
}