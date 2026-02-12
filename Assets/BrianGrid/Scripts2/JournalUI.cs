using UnityEngine;
using TMPro;

public class JournalUI : MonoBehaviour
{
    public GameObject journalPanel;
    public TextMeshProUGUI completedText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            journalPanel.SetActive(!journalPanel.activeSelf);

        UpdateCompletedTasks();
    }

    void UpdateCompletedTasks()
    {
        completedText.text = "COMPLETED TASKS\n";

        foreach (string task in ObjectiveManager.Instance.completedTasks)
            completedText.text += "✔ " + task + "\n";
    }
}
