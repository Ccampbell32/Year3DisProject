using UnityEngine;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("Current Objective")]
    public string currentObjective;

    [Header("Tasks")]
    public List<string> activeTasks = new List<string>();
    public List<string> completedTasks = new List<string>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetObjective(string objective)
    {
        currentObjective = objective;
    }

    public void AddTask(string task)
    {
        if (!activeTasks.Contains(task))
            activeTasks.Add(task);
    }

    public void CompleteTask(string task)
    {
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
            completedTasks.Add(task);
        }
    }
}

