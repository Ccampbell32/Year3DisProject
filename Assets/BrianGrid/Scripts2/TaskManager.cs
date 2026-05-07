using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    public TMP_Text objectiveText;
    [Tooltip("Text element for player's inner thoughts/hints")]
    public TMP_Text hintText;

    [Header("Requirements")]
    public int bloodNeeded = 5;
    public int photosNeeded = 7;
    public int trashNeeded = 2;
    public int bonesRequired = 5;

    int bloodDone = 0;
    int photosDone = 0;
    int trashDone = 0;
    int lightsOn = 0;
    
    public int bonesFound = 0;

    public bool tasksStarted = false;
    private Coroutine hintCoroutine;

    [Header("Scare Transition")]
    [Tooltip("An audio source to play the scare sound (can just be on this TaskManager object)")]
    public AudioSource globalAudioSource;
    public AudioClip scareSound;
    [Tooltip("Drag your scene's main Directional Light here so we can mess with it")]
    public Light directionalLight;

    [Header("Escape Timer")]
    public float escapeTimeLimit = 60f;
    private float currentEscapeTime;
    private bool isEscapeTimerRunning = false;

    [Header("Altar")]
    public GameObject altarPrefab;
    [Tooltip("Enemy that spawns for the jump scare ending")]
    public GameObject enemyPrefab;
    [Tooltip("Enemy that spawns when the altar first appears to hunt you")]
    public GameObject huntingEnemyPrefab;
    public Transform huntingEnemySpawnPoint;
    public Transform altarSpawnPoint;
    public GameObject[] altarDecorations;
    public int offeringsPlaced = 0;
    bool altarSpawned = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (objectiveText != null) objectiveText.text = ""; // Hide until note is read
        if (hintText != null) hintText.text = ""; // Hide hint
    }

    void Update()
    {
        if (isEscapeTimerRunning)
        {
            currentEscapeTime -= Time.deltaTime;
            UpdateUI(); // Refresh the timer text

            if (currentEscapeTime <= 0f)
            {
                isEscapeTimerRunning = false;
                
                // Transition to "escaped but not gone" scene
                if (Application.CanStreamedLevelBeLoaded("EscapedScene"))
                {
                    SceneManager.LoadScene("EscapedScene");
                }
                else
                {
                    Debug.LogError("EscapedScene not found in Build Settings! Please create it.");
                    objectiveText.text = "<color=red>TIME'S UP! (Please create EscapedScene)</color>";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GiveHint();
        }

        // --- DEBUG SKIP TO PHASE 2 ---
        // Press F12 to instantly finish all chores, spawn the altar, and give max bones!
        if (Input.GetKeyDown(KeyCode.F12))
        {
            bloodDone = bloodNeeded;
            photosDone = photosNeeded;
            trashDone = trashNeeded;
            bonesFound = bonesRequired;
            tasksStarted = true;

            // Give the player max bones in their inventory
            PlayerInteract player = FindObjectOfType<PlayerInteract>();
            if (player != null) player.bonesHeld = bonesRequired;

            // Trigger the scare transition and spawn the altar
            CheckAltar();
            UpdateUI();
        }
    }

    public void GiveHint()
    {
        if (hintText == null) return;
        
        string hint = "";

        if (!tasksStarted)
        {
            hint = "I should probably read that note on the table first...";
        }
        else if (!altarSpawned)
        {
            if (bloodDone < bloodNeeded) hint = "There's still blood to clean up. I need to find the mop.";
            else if (trashDone < trashNeeded) hint = "I still need to dump the rest of the trash bags.";
            else if (photosDone < photosNeeded) hint = "I haven't found all the photos yet. I should keep looking.";
            else hint = "I think I'm done with the chores... wait, what was that?";
        }
        else
        {
            if (bonesFound < bonesRequired) hint = "I need to find more bones scattered around...";
            else if (offeringsPlaced < altarDecorations.Length) hint = "I have the bones, I just need to place them on the altar.";
            else hint = "It's done... I need to get out.";
        }

        if (hintCoroutine != null) StopCoroutine(hintCoroutine);
        hintCoroutine = StartCoroutine(ShowHintRoutine(hint));
    }

    IEnumerator ShowHintRoutine(string hint)
    {
        // Use italicized quotes to make it look like an inner thought subtitle
        hintText.text = "<i>\"" + hint + "\"</i>";
        yield return new WaitForSeconds(4f);
        hintText.text = "";
    }

    public void CompleteTask(TaskType task)
    {
        switch (task)
        {
            case TaskType.CleanBlood:
                bloodDone++;
                if (bloodDone >= bloodNeeded)
                {
                    PlayerInteract player = FindObjectOfType<PlayerInteract>();
                    if (player != null) player.RemoveMop();
                }
                break;

            case TaskType.Photos:
                photosDone++;
                break;

            case TaskType.DumpTrash:
                trashDone++;
                break;
        }

        CheckAltar();
        UpdateUI();
    }

    void CheckAltar()
    {
        if (altarSpawned) return;

        if (bloodDone >= bloodNeeded && photosDone >= photosNeeded && trashDone >= trashNeeded)
        {
            altarSpawned = true;
            StartCoroutine(PhaseTwoScareTransition());
        }
    }

    IEnumerator PhaseTwoScareTransition()
    {
        // 1. Play Scare Sound
        if (globalAudioSource != null && scareSound != null)
        {
            globalAudioSource.PlayOneShot(scareSound);
        }

        // 2. Lights Out / Red Hue
        Color originalColor = Color.white;
        float originalIntensity = 1f;
        
        if (directionalLight != null)
        {
            originalColor = directionalLight.color;
            originalIntensity = directionalLight.intensity;
            
            // Make the light creepy red and dim
            directionalLight.color = Color.red;
            directionalLight.intensity = 0.1f;
        }

        // Wait a terrifying moment for the player to panic in the red light
        yield return new WaitForSeconds(2.5f);

        // 3. Spawn the Altar and the Hunter
        Instantiate(altarPrefab, altarSpawnPoint.position, altarSpawnPoint.rotation);
        
        if (huntingEnemyPrefab != null && huntingEnemySpawnPoint != null)
        {
            Instantiate(huntingEnemyPrefab, huntingEnemySpawnPoint.position, huntingEnemySpawnPoint.rotation);
        }

        UpdateUI();

        // 4. Restore lighting, but leave it dimmer than before to maintain tension
        if (directionalLight != null)
        {
            directionalLight.color = originalColor;
            directionalLight.intensity = originalIntensity * 0.5f; // Half as bright!
        }

        // 5. Start the repeating red flash effect
        StartCoroutine(RedFlashRoutine(originalColor, originalIntensity));
    }

    IEnumerator RedFlashRoutine(Color origColor, float origIntensity)
    {
        while (altarSpawned && offeringsPlaced < altarDecorations.Length)
        {
            // Wait anywhere from 3 to 8 seconds
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            // Only trigger if we haven't won the game yet
            if (directionalLight != null && offeringsPlaced < altarDecorations.Length)
            {
                // Snap to creepy red
                directionalLight.color = Color.red;
                directionalLight.intensity = 0.1f;

                // Stay red for a short burst (half a second to 1.5 seconds)
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

                // Make sure we didn't win while the light was red before returning it to normal
                if (offeringsPlaced < altarDecorations.Length)
                {
                    directionalLight.color = origColor;
                    directionalLight.intensity = origIntensity * 0.5f; // Keep it dim!
                }
            }
        }
    }

    public void RegisterLightTurnedOn()
    {
        lightsOn++;
    }

    public void RegisterLightTurnedOff()
    {
        lightsOn--;
        if (lightsOn < 0) lightsOn = 0;
    }

    public void StartTasks()
    {
        tasksStarted = true;
        UpdateUI();
    }

    public void AddBone()
    {
        bonesFound++;
        UpdateUI();

        if (bonesFound >= bonesRequired && !isEscapeTimerRunning && offeringsPlaced < altarDecorations.Length)
        {
            isEscapeTimerRunning = true;
            currentEscapeTime = escapeTimeLimit;
            
            if (globalAudioSource != null && scareSound != null)
            {
                globalAudioSource.PlayOneShot(scareSound);
            }
        }
    }

    public void PlaceOneOffering()
    {
        if (offeringsPlaced < altarDecorations.Length)
        {
            if (altarDecorations[offeringsPlaced] != null)
                altarDecorations[offeringsPlaced].SetActive(true);
        }

        offeringsPlaced++;

        if (offeringsPlaced >= altarDecorations.Length)
        {
            isEscapeTimerRunning = false; // Stop the timer when they win!
            StartCoroutine(WinSequence());
        }
        else
        {
            UpdateUI(); // Update UI if they placed one but didn't win yet
        }
    }

    IEnumerator WinSequence()
    {
        objectiveText.text = "It is done...";

        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, altarSpawnPoint.position, altarSpawnPoint.rotation);
        }

        // --- BEACON OF LIGHT EFFECT ---
        // 1. Create a massive bright light at the altar
        GameObject beacon = new GameObject("BeaconLight");
        beacon.transform.position = altarSpawnPoint.position;
        Light bLight = beacon.AddComponent<Light>();
        bLight.type = LightType.Point;
        bLight.color = Color.white;
        bLight.intensity = 0f;
        bLight.range = 50f;

        // 2. Create a pure white Canvas to fade the screen
        GameObject fadeCanvasObj = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Draw over everything
        
        GameObject fadeImageObj = new GameObject("FadeImage");
        fadeImageObj.transform.SetParent(fadeCanvasObj.transform, false);
        Image fadeImage = fadeImageObj.AddComponent<Image>();
        fadeImage.color = new Color(1f, 1f, 1f, 0f); // Start transparent
        
        // Stretch the image to fill the screen
        RectTransform rect = fadeImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // 3. Fade in over 3 seconds
        float fadeDuration = 3f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float percent = timer / fadeDuration;
            
            // Brighten the physical world light
            bLight.intensity = Mathf.Lerp(0f, 20f, percent); 
            
            // Fade the screen image to solid white
            fadeImage.color = new Color(1f, 1f, 1f, percent);

            yield return null; // Wait until next frame
        }

        // Ensure it is completely solid white at the end
        fadeImage.color = new Color(1f, 1f, 1f, 1f);
        
        // Wait one final second in pure white blindness
        yield return new WaitForSeconds(1f);

        // Load the Win Scene
        SceneManager.LoadScene("WinScene");
    }

    void UpdateUI()
    {
        if (!tasksStarted) return;

        if (!altarSpawned)
        {
            objectiveText.text =
                "Clean Blood (" + bloodDone + "/" + bloodNeeded + ")\n" +
                "Collect Photos (" + photosDone + "/" + photosNeeded + ")\n" +
                "Dump Trash (" + trashDone + "/" + trashNeeded + ")";
        }
        else
        {
            string baseText =
                "Something has appeared...\n\n" +
                "Check the Cemetery (Left)\n" +
                "Check the Ruins (North)\n\n" +
                "Gather Remains (" + bonesFound + "/" + bonesRequired + ")\n" +
                "Place Offerings on Altar";

            if (isEscapeTimerRunning)
            {
                baseText += "\n\n<color=red><b>ESCAPE TIME: " + Mathf.Ceil(currentEscapeTime).ToString() + "s</b></color>";
            }

            objectiveText.text = baseText;
        }
    }

    public enum TaskType
    {
        CleanBlood,
        Photos,
        DumpTrash,
        Lights
    }
}