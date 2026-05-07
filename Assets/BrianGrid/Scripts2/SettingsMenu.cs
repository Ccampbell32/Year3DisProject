using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;

    [Header("Brightness")]
    public Image brightnessPanel;

    [Header("UI Sound")]
    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    // ---------------- CLICK SOUND ----------------
    public void PlayClick()
    {
        if (uiAudioSource != null && clickSound != null)
        {
            uiAudioSource.pitch = Random.Range(0.95f, 1.05f);
            uiAudioSource.PlayOneShot(clickSound);
        }
    }

    // ---------------- VOLUME ----------------
    public void SetVolumeLow()
    {
        PlayClick();
        SetVolume(0.33f);
    }

    public void SetVolumeMedium()
    {
        PlayClick();
        SetVolume(0.66f);
    }

    public void SetVolumeHigh()
    {
        PlayClick();
        SetVolume(0.99f);
    }

    void SetVolume(float value)
    {
        if (value < 0.001f) value = 0.001f;

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("Volume", value);
    }

    // ---------------- BRIGHTNESS ----------------
    public void SetBrightnessLow()
    {
        PlayClick();
        SetBrightness(0.33f);
    }

    public void SetBrightnessMedium()
    {
        PlayClick();
        SetBrightness(0.66f);
    }

    public void SetBrightnessHigh()
    {
        PlayClick();
        SetBrightness(0.99f);
    }

    void SetBrightness(float value)
    {
        Color c = brightnessPanel.color;
        c.a = 1 - value;
        brightnessPanel.color = c;

        PlayerPrefs.SetFloat("Brightness", value);
    }

    // ---------------- LOAD SAVED ----------------
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.66f);
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.66f);

        SetVolume(savedVolume);
        SetBrightness(savedBrightness);
    }
}