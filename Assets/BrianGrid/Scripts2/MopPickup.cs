using UnityEngine;

public class MopPickup : MonoBehaviour
{
    public GameObject mopObject;
    public static bool hasMop = false;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            hasMop = true;
            mopObject.SetActive(false); // despawns mop
        }
    }
}