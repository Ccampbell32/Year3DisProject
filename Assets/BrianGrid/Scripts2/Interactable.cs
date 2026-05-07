using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    public enum InteractType
    {
        Trash, Dumpster, Light, Mop, Blood, Footprint, Photo, Bones, Altar, Paper, Bed
    }

    public InteractType interactType;

    public AudioSource audioSource;
    public AudioClip lightSwitchSound;

    public bool isWeirdPhoto;

    private ShowMessage msg;

    void Start()
    {
        msg = FindObjectOfType<ShowMessage>();
    }

    public string GetPrompt(PlayerInteract player)
    {
        switch (interactType)
        {
            case InteractType.Trash:
                return player.holdingTrash ? "You are already holding trash" : "Press E to Pick Up Trash";

            case InteractType.Dumpster:
                return player.holdingTrash ? "Press E to Dump Trash" : "You have no trash";

            case InteractType.Light:
                return "Press E to Toggle Light";

            case InteractType.Mop:
                return player.holdingMop ? "You already have a mop" : "Press E to Pick Up Mop";

            case InteractType.Blood:
                return player.holdingMop ? "Press E to Mop Blood" : "You need a mop";

            case InteractType.Footprint:
                return player.holdingMop ? "Press E to Clean Footprints" : "You need a mop";

            case InteractType.Photo:
                return "Press E to Pick Up Photo";

            case InteractType.Bones:
                return "Press E to Gather the remains...";

            case InteractType.Altar:
                return (player.bonesHeld > 0 || player.photosToPlace > 0 || player.bloodToPlace > 0) ? "Press E to place offering..." : "Find more remains...";

            case InteractType.Paper:
                return "Press E to Read Note";

            case InteractType.Bed:
                return "Press E to Rest";
        }

        return "";
    }

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
                else if (msg != null)
                {
                    msg.Show("There should be a dumpster nearby.");
                }
                break;

            case InteractType.Dumpster:
                if (player.holdingTrash)
                {
                    player.holdingTrash = false;
                    TaskManager.Instance.CompleteTask(TaskManager.TaskType.DumpTrash);
                }
                else if (msg != null)
                {
                    msg.Show("You are not carrying any trash.");
                }
                break;

            case InteractType.Light:
                Light l = GetComponentInChildren<Light>();
                if (l != null)
                {
                    bool wasOn = l.enabled;
                    l.enabled = !l.enabled;

                    if (audioSource && lightSwitchSound)
                        audioSource.PlayOneShot(lightSwitchSound);

                    if (!wasOn && l.enabled)
                        TaskManager.Instance.RegisterLightTurnedOn();
                    else if (wasOn && !l.enabled)
                    {
                        TaskManager.Instance.RegisterLightTurnedOff();
                        TaskManager.Instance.CompleteTask(TaskManager.TaskType.Lights);
                    }
                }
                break;

            case InteractType.Mop:
                if (!player.holdingMop)
                {
                    player.PickupMop();
                    gameObject.SetActive(false);
                }
                break;

            case InteractType.Blood:
                if (player.holdingMop)
                {
                    TaskManager.Instance.CompleteTask(TaskManager.TaskType.CleanBlood);
                    Destroy(gameObject);
                }
                else if (msg != null)
                {
                    msg.Show("You need a mop.");
                }
                break;

            case InteractType.Photo:
                if (isWeirdPhoto && msg != null)
                    msg.Show("...this looks familiar.");

                TaskManager.Instance.CompleteTask(TaskManager.TaskType.Photos);
                Destroy(gameObject);
                break;

            case InteractType.Bones:
                if (player.bonesHeld < player.maxBones)
                {
                    player.bonesHeld++;
                    TaskManager.Instance.AddBone();
                    Destroy(gameObject);

                    if (msg != null)
                        msg.Show("You pick up the remains...");
                }
                else if (msg != null)
                {
                    msg.Show("You can't carry any more...");
                }
                break;

            case InteractType.Altar:
                if (player.bonesHeld > 0)
                {
                    player.bonesHeld--;
                    TaskManager.Instance.PlaceOneOffering();
                    if (msg != null) msg.Show("You place a bone...");
                }
                else if (player.photosToPlace > 0)
                {
                    player.photosToPlace--;
                    TaskManager.Instance.PlaceOneOffering();
                    if (msg != null) msg.Show("You place a photo...");
                }
                else if (player.bloodToPlace > 0)
                {
                    player.bloodToPlace--;
                    TaskManager.Instance.PlaceOneOffering();
                    if (msg != null) msg.Show("You place the blood...");
                }
                else if (TaskManager.Instance.offeringsPlaced < TaskManager.Instance.altarDecorations.Length)
                {
                    if (msg != null) msg.Show("You need more remains...");
                }
                break;

            case InteractType.Paper:
                StartCoroutine(ReadNote());
                break;

            case InteractType.Bed:
                StartCoroutine(RestAndEnd());
                break;
        }
    }

    IEnumerator ReadNote()
    {
        TaskManager.Instance.objectiveText.text = "Clean the place...";
        yield return new WaitForSeconds(2f);

        TaskManager.Instance.StartTasks();
        gameObject.SetActive(false);
    }

    IEnumerator RestAndEnd()
    {
        TaskManager.Instance.objectiveText.text = "You lie down...";
        yield return new WaitForSeconds(2f);

        TaskManager.Instance.objectiveText.text = "Just for a moment...";
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("JokeScene");
    }
}