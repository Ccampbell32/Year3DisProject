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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                Vector3 screenPos =
                    cam.WorldToScreenPoint(hit.collider.bounds.center);

                interactText.transform.position =
                    screenPos + Vector3.up * 30f;

                interactText.text = interactable.GetPrompt(this);
                interactText.enabled = interactText.text != "";

                return;
            }
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

    // -------- RESET --------
    void ClearInteraction()
    {
        currentInteractable = null;
        interactText.enabled = false;
    }
}
