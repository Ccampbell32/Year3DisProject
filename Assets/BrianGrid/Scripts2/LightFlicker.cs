using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    private Light myLight;
    private float originalIntensity;

    [Header("Flicker Settings")]
    [Tooltip("Minimum time between flickers")]
    public float minWaitTime = 0.5f;
    [Tooltip("Maximum time between flickers")]
    public float maxWaitTime = 4.0f;
    
    [Tooltip("How long the light stays completely off during a flicker")]
    public float flickerOffDuration = 0.1f;

    void Start()
    {
        myLight = GetComponent<Light>();
        originalIntensity = myLight.intensity;
        
        // Start the infinite flicker loop
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        // Add a random initial delay so all lights don't flicker at the exact same time
        yield return new WaitForSeconds(Random.Range(0f, 2f));

        while (true)
        {
            // Wait for a random amount of time before breaking
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Only flicker if the light is currently turned on (e.g., if a light switch hasn't turned it off)
            if (myLight != null && myLight.enabled)
            {
                // Create a rapid stutter effect (1 to 3 quick flashes) like a broken bulb
                int stutters = Random.Range(1, 4); 

                for (int i = 0; i < stutters; i++)
                {
                    // Turn OFF
                    myLight.intensity = 0f;
                    
                    // Stay off for a split second
                    yield return new WaitForSeconds(Random.Range(0.05f, flickerOffDuration));
                    
                    // Turn back ON
                    myLight.intensity = originalIntensity;
                    
                    // Very short pause before the next stutter
                    yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
                }
            }
        }
    }
}
