using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    [Header("UI")]
    public TMP_Text objectiveText;

    [Header("Tasks")]
    public int bloodToClean = 5;
    public int trashToDump = 3;

    int bloodCleaned = 0;
    int trashDumped = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    // -------- BLOOD --------
    public void CleanedBlood()
    {
        bloodCleaned++;
        UpdateUI();
        CheckCompletion();
    }

    // -------- TRASH --------
    public void DumpedTrash()
    {
        trashDumped++;
        UpdateUI();
        CheckCompletion();
    }

    // -------- UI UPDATE --------
    void UpdateUI()
    {
        objectiveText.text =
            "Tasks:\n" +
            "Clean Blood: " + bloodCleaned + "/" + bloodToClean + "\n" +
            "Dump Trash: " + trashDumped + "/" + trashToDump;
    }

    void CheckCompletion()
    {
        if (bloodCleaned >= bloodToClean &&
            trashDumped >= trashToDump)
        {
            objectiveText.text = "All Tasks Complete...";
            Debug.Log("ALL TASKS COMPLETE");
        }
    }
}
