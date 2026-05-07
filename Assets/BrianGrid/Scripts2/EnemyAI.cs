using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [Tooltip("Leave this empty. It will automatically find the player!")]
    public Transform player;

    [Header("Movement")]
    public float spawnDelay = 3f;
    public float catchDistance = 1.5f;

    [Header("Jump Scare")]
    [Tooltip("Leave this empty. It will create a red flash automatically if you don't assign one.")]
    public GameObject jumpScareCanvas;
    public float jumpScareDuration = 2f;
    public string gameOverSceneName = "GameOver";

    private NavMeshAgent agent;

    [Header("Footsteps")]
    [Tooltip("Leave this empty. It will add its own AudioSource.")]
    public AudioSource footstepSource;
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;

    float stepTimer;
    bool active = false;
    bool isCatching = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Enemy is missing a NavMeshAgent component! It cannot move!");
        }
        
        // Auto-find player if not assigned
        if (player == null)
        {
            PlayerInteract p = FindObjectOfType<PlayerInteract>();
            if (p != null) player = p.transform;
            else 
            {
                GameObject playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null) player = playerObj.transform;
            }
        }

        // Auto-add AudioSource for footsteps if missing
        if (footstepSource == null)
        {
            footstepSource = gameObject.AddComponent<AudioSource>();
            footstepSource.spatialBlend = 1f; // Make it 3D sound
            footstepSource.volume = 0.6f;
        }

        if (agent != null) agent.isStopped = true;

        if (jumpScareCanvas != null) jumpScareCanvas.SetActive(false);

        Invoke(nameof(ActivateEnemy), spawnDelay);
    }

    void ActivateEnemy()
    {
        active = true;
        if (agent != null && agent.isOnNavMesh) agent.isStopped = false;
    }

    void Update()
    {
        if (!active || player == null || isCatching) return;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            
            if (agent.velocity.magnitude > 0.1f)
            {
                HandleFootsteps();
            }
        }

        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            StartCoroutine(CatchSequence());
        }
    }

    System.Collections.IEnumerator CatchSequence()
    {
        isCatching = true;
        
        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;

        if (jumpScareCanvas != null)
        {
            jumpScareCanvas.SetActive(true);
        }
        else
        {
            // Auto-create a jump scare red screen if none was assigned
            GameObject autoCanvas = new GameObject("AutoJumpScareCanvas");
            Canvas canvas = autoCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
            
            GameObject imgObj = new GameObject("RedScreen");
            imgObj.transform.SetParent(autoCanvas.transform, false);
            Image img = imgObj.AddComponent<Image>();
            img.color = new Color(0.8f, 0f, 0f, 0.8f); // Bloody red
            
            RectTransform rect = img.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }

        yield return new WaitForSeconds(jumpScareDuration);

        if (!string.IsNullOrEmpty(gameOverSceneName) && Application.CanStreamedLevelBeLoaded(gameOverSceneName))
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void HandleFootsteps()
    {
        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = Random.Range(0.4f, 0.7f);
        }
    }

    void PlayFootstep()
    {
        if (footstepSource == null || footstepClips == null || footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        footstepSource.PlayOneShot(footstepClips[index]);
    }
}
