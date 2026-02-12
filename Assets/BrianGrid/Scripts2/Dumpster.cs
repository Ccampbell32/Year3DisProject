using UnityEngine;

public class Dumpster : MonoBehaviour
{
    public TaskManager taskManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            taskManager.DumpBags();
        }
    }
}
