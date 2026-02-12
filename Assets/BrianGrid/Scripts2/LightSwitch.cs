using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public Light roomLight;

    bool isOn = true;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;
        roomLight.enabled = isOn;
    }
}
