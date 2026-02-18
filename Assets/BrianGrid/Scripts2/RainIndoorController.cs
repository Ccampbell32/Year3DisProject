using UnityEngine;

public class RainIndoorController : MonoBehaviour
{
    public GameObject rainFar;
    public GameObject rainNear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainFar.SetActive(false);
            rainNear.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainFar.SetActive(true);
            rainNear.SetActive(true);
        }
    }
}
