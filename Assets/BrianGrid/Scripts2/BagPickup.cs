using UnityEngine;

public class BagPickup : MonoBehaviour
{
    public TaskManager taskManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            taskManager.CollectBag();
            gameObject.SetActive(false);
        }
    }
}
