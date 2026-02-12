using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isTrash;
    public bool isDumpster;
    public bool isLight;

    public string GetPrompt(PlayerInteract player)
    {
        if (isTrash && !player.holdingTrash)
            return "Press E to Pick Up";

        if (isDumpster && player.holdingTrash)
            return "Press E to Dump Trash";

        if (isLight)
            return "Press E to Toggle Light";

        return "";
    }

    public void Interact(PlayerInteract player)
    {
        if (isTrash && !player.holdingTrash)
        {
            player.holdingTrash = true;
            gameObject.SetActive(false);
        }

        else if (isDumpster && player.holdingTrash)
        {
            player.holdingTrash = false;
        }

        else if (isLight)
        {
            Light light = GetComponentInChildren<Light>();
            if (light != null)
                light.enabled = !light.enabled;
        }
    }
}
