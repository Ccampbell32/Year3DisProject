using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}
