using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 2f;
    public TMP_Text interactText;

    [Header("Player State")]
    [HideInInspector] public bool holdingTrash;
    [HideInInspector] public bool holdingMop;
    [HideInInspector] public int bonesHeld = 0;
    public int maxBones = 5;
    [HideInInspector] public int photosToPlace = 4;
    [HideInInspector] public int bloodToPlace = 1;

    [Header("Mop System")]
    public Transform holdPoint;
    public GameObject mopPrefab;

    GameObject currentMop;

    Camera cam;
    Interactable currentInteractable;

    void Start()
    {
        cam = Camera.main;
        interactText.enabled = false;
    }

    void Update()
    {
        CheckInteraction();
        HandleInput();
    }

    // -------- CHECK OBJECT --------
    void CheckInteraction()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        // Use a thick "SphereCastAll" (radius 0.5f). This ensures we can easily hit small or flat objects
        // like photos even if they are resting perfectly flat against a wall or floor.
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, interactRange);

        Interactable bestInteractable = null;
        float closestDistance = float.MaxValue;
        Collider bestCollider = null;

        // Loop through everything the thick ray hit to find the closest interactable object
        foreach (RaycastHit hit in hits)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                if (hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    bestInteractable = interactable;
                    bestCollider = hit.collider;
                }
            }
        }

        if (bestInteractable != null)
        {
            currentInteractable = bestInteractable;

            Vector3 screenPos = cam.WorldToScreenPoint(bestCollider.bounds.center);

            interactText.transform.position = screenPos + Vector3.up * 30f;
            interactText.text = bestInteractable.GetPrompt(this);
            interactText.enabled = interactText.text != "";

            return;
        }

        ClearInteraction();
    }

    // -------- INPUT --------
    void HandleInput()
    {
        if (currentInteractable == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact(this);
        }
    }

    // -------- PICKUP MOP --------
    public void PickupMop()
    {
        if (holdingMop) return;

        currentMop = Instantiate(mopPrefab, holdPoint.position, holdPoint.rotation);
        currentMop.transform.SetParent(holdPoint);

        holdingMop = true;
    }

    // -------- REMOVE MOP --------
    public void RemoveMop()
    {
        if (!holdingMop) return;

        if (currentMop != null)
        {
            Destroy(currentMop);
        }

        holdingMop = false;
    }

    // -------- RESET --------
    void ClearInteraction()
    {
        currentInteractable = null;
        interactText.enabled = false;
    }
}