using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Footsteps")]
    public AudioSource footstepSource;
    public AudioClip[] footstepSounds;
    public float walkStepInterval = 0.6f;
    public float sprintStepInterval = 0.4f;
    
    private CharacterController controller;
    private float stepTimer;

    [Header("Heartbeat")]
    public AudioSource heartbeatSource;
    public AudioClip heartbeatSound;
    [Tooltip("The tag of your enemy object")]
    public string enemyTag = "Enemy";
    public float maxHeartbeatDistance = 15f;
    public float minHeartbeatDistance = 2f;
    
    [Header("Heartbeat Camera Shake")]
    [Tooltip("Leave empty to auto-find the Main Camera")]
    public Transform cameraTransform;
    public float maxShakeIntensity = 0.06f;
    public float shakeSpeed = 30f;
    
    private Transform enemyTransform;
    private Vector3 originalCameraPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Setup Heartbeat Source if assigned
        if (heartbeatSource != null && heartbeatSound != null)
        {
            heartbeatSource.clip = heartbeatSound;
            heartbeatSource.loop = true;
            heartbeatSource.volume = 0f; // Start silent
            heartbeatSource.Play();
        }

        // Setup Camera Shake
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        if (cameraTransform != null)
        {
            originalCameraPos = cameraTransform.localPosition;
        }
    }

    void Update()
    {
        HandleFootsteps();
        HandleHeartbeat();
    }

    void HandleFootsteps()
    {
        if (footstepSource == null || footstepSounds.Length == 0) return;

        // Check if the player is moving (velocity > 0.1f)
        if (controller.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            
            if (stepTimer <= 0f)
            {
                // Play a random footstep sound
                AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                footstepSource.PlayOneShot(clip);
                
                // Adjust interval based on how fast we are moving (sprint vs walk)
                // Assuming speed > 5 means sprinting based on SimpleFPSController
                if (controller.velocity.magnitude > 5f)
                    stepTimer = sprintStepInterval;
                else
                    stepTimer = walkStepInterval;
            }
        }
        else
        {
            // Reset timer so the next step happens immediately when we start walking
            stepTimer = 0f;
        }
    }

    void HandleHeartbeat()
    {
        if (heartbeatSource == null || heartbeatSound == null) return;

        // Try to find the enemy if we haven't found them yet
        if (enemyTransform == null)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag(enemyTag);
            if (enemy != null) enemyTransform = enemy.transform;
        }

        // If no enemy is in the scene, stay silent
        if (enemyTransform == null)
        {
            heartbeatSource.volume = 0f;
            return;
        }

        // Calculate distance between player and enemy
        float dist = Vector3.Distance(transform.position, enemyTransform.position);

        if (dist < maxHeartbeatDistance)
        {
            // Calculate how close the enemy is (0 = far, 1 = right next to us)
            float closeness = 1f - Mathf.InverseLerp(minHeartbeatDistance, maxHeartbeatDistance, dist);
            
            // Adjust volume and pitch based on distance
            // Volume fades in from 0 to 1
            heartbeatSource.volume = closeness; 
            
            // Pitch increases slightly as they get closer to simulate a faster heartbeat
            heartbeatSource.pitch = 1f + (closeness * 0.5f); 

            // Apply Camera Shake based on closeness
            if (cameraTransform != null)
            {
                // We use Perlin noise for smooth, erratic shaking
                float shakeX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f;
                float shakeY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f;
                
                // Closeness squared makes the shake start small and ramp up exponentially when very close
                float currentIntensity = maxShakeIntensity * (closeness * closeness);
                
                cameraTransform.localPosition = originalCameraPos + new Vector3(shakeX, shakeY, 0) * currentIntensity;
            }
        }
        else
        {
            heartbeatSource.volume = 0f; // Silent when out of range

            // Reset camera position when safe
            if (cameraTransform != null)
            {
                cameraTransform.localPosition = originalCameraPos;
            }
        }
    }
}
