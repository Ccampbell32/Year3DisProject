using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        Trash,
        Dumpster,
        Light,
        Mop,
        Blood
    }

    [Header("Type")]
    public InteractType interactType;

    // ---------- PROMPT ----------
    public string GetPrompt(PlayerInteract player)
    {
        switch (interactType)
        {
            case InteractType.Trash:
                if (!player.holdingTrash)
                    return "Press E to Pick Up Trash";
                break;

            case InteractType.Dumpster:
                if (player.holdingTrash)
                    return "Press E to Dump Trash";
                break;

            case InteractType.Light:
                return "Press E to Toggle Light";

            case InteractType.Mop:
                if (!player.holdingMop)
                    return "Press E to Pick Up Mop";
                break;

            case InteractType.Blood:
                if (player.holdingMop)
                    return "Press E to Mop Blood";
                else
                    return "You need a mop";
        }

        return "";
    }

    // ---------- INTERACT ----------
    public void Interact(PlayerInteract player)
    {
        switch (interactType)
        {
            case InteractType.Trash:
                if (!player.holdingTrash)
                {
                    player.holdingTrash = true;
                    gameObject.SetActive(false);
                }
                break;

            case InteractType.Dumpster:
                if (player.holdingTrash)
                {
                    player.holdingTrash = false;
                    TaskManager.Instance.DumpedTrash();
                }
                break;

            case InteractType.Light:
                Light lightComp = GetComponentInChildren<Light>();
                if (lightComp != null)
                    lightComp.enabled = !lightComp.enabled;
                break;

            case InteractType.Mop:
                if (!player.holdingMop)
                {
                    player.holdingMop = true;
                    gameObject.SetActive(false);
                }
                break;

            case InteractType.Blood:
                if (player.holdingMop)
                {
                    TaskManager.Instance.CleanedBlood();
                    Destroy(gameObject);
                }
                break;
        }
    }
}
