using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;
    public TMP_Text interactText;

    [HideInInspector] public bool holdingTrash;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        interactText.enabled = false;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Convert world position to screen position
                Vector3 screenPos = cam.WorldToScreenPoint(hit.collider.bounds.center);

                interactText.transform.position = screenPos + Vector3.up * 30f;

                interactText.text = interactable.GetPrompt(this);
                interactText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact(this);
                }

                return;
            }
        }

        interactText.enabled = false;
    }
}
