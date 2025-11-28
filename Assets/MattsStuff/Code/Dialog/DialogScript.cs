using UnityEngine;
using UnityEngine.UI;   
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DialogScript : MonoBehaviour
{
    public string[] sentences;
    [SerializeField] public Queue<string> dialogLines;
    [SerializeField] private GameObject DialogBox;
    
    public TextMeshProUGUI dialogText;
    void Start()
    {
        DialogBox.SetActive(true);
        dialogLines = new Queue<string>();
        StartDialog(new List<string>(sentences));
    }

    public void StartDialog(List<string> lines)
    {
        dialogLines.Clear();

        foreach (string line in lines)
        {
            dialogLines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogLines.Count == 0)
        {
            EndDialog();
            return;
        }

        string line = dialogLines.Dequeue();
        dialogText.text = line;
    }
    private void EndDialog()
    {
        DialogBox.SetActive(false);
        Debug.Log("Dialog ended.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }
}
