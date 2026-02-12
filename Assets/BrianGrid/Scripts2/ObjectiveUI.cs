using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI taskText;

    private void Update()
    {
        if (ObjectiveManager.Instance == null) return;

        objectiveText.text = "OBJECTIVE:\n" + ObjectiveManager.Instance.currentObjective;

        if (ObjectiveManager.Instance.activeTasks.Count > 0)
            taskText.text = "TASK:\n" + ObjectiveManager.Instance.activeTasks[0];
        else
            taskText.text = "TASK:\nNone";
    }
}
