using UnityEngine;

public class RainIndoorController : MonoBehaviour
{
    public GameObject rainFar;
    public GameObject rainNear;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (rainFar != null) rainFar.SetActive(false);
            if (rainNear != null) rainNear.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (rainFar != null) rainFar.SetActive(true);
            if (rainNear != null) rainNear.SetActive(true);
        }
    }
}
