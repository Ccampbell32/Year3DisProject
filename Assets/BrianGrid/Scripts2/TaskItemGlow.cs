using UnityEngine;

public class TaskItemGlow : MonoBehaviour
{
    [Tooltip("Add a Point Light as a child to this object, and drag it here!")]
    public Light itemLight;

    [Header("Glow Settings")]
    public float pulseSpeed = 2f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.5f;

    void Start()
    {
        if (itemLight != null)
        {
            itemLight.enabled = false; // Start with the light off
        }
    }

    void Update()
    {
        // Only turn the light on and pulse it if the tasks have started
        if (itemLight != null && TaskManager.Instance != null)
        {
            if (TaskManager.Instance.tasksStarted)
            {
                // Ensure the light is actually on
                if (!itemLight.enabled) itemLight.enabled = true;

                // Make the light pulse smoothly using PingPong
                // This creates an eerie breathing effect
                float pulse = Mathf.PingPong(Time.time * pulseSpeed, maxIntensity - minIntensity);
                itemLight.intensity = minIntensity + pulse;
            }
        }
    }
}
