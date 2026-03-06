using UnityEngine;

public class HeartbeatController : MonoBehaviour
{
    public Transform enemy;
    public float maxDistance = 10f;

    AudioSource heartbeat;

    void Start()
    {
        heartbeat = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, enemy.position);

        if (distance < maxDistance)
        {
            if (!heartbeat.isPlaying)
                heartbeat.Play();

            float intensity = 1 - (distance / maxDistance);

            heartbeat.volume = Mathf.Lerp(0.2f, 1f, intensity);
            heartbeat.pitch = Mathf.Lerp(0.8f, 1.5f, intensity);
        }
        else
        {
            // Smoothly fade out the heartbeat
            heartbeat.volume = Mathf.Lerp(heartbeat.volume, 0f, Time.deltaTime * 2f);

            if (heartbeat.volume < 0.05f && heartbeat.isPlaying)
            {
                heartbeat.Stop();
            }
        }
    }
}