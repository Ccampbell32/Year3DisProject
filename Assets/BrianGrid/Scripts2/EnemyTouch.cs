using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyTouch : MonoBehaviour
{
    [Header("Jumpscare UI")]
    public CanvasGroup jumpscareImage;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpscareSound;

    [Header("Camera")]
    public CameraShake cameraShake;

    [Header("Scene")]
    public string GameOver = "GameOver";

    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Jumpscare());
        }
    }

    IEnumerator Jumpscare()
    {
        // 🔥 CAMERA SHAKE
        if (cameraShake != null)
            StartCoroutine(cameraShake.Shake(0.4f, 0.25f));

        // 👁 SHOW IMAGE
        if (jumpscareImage != null)
            jumpscareImage.alpha = 1;

        // 🔊 PLAY SOUND
        if (audioSource != null && jumpscareSound != null)
            audioSource.PlayOneShot(jumpscareSound);

        // ⏳ WAIT (IMPORTANT)
        yield return new WaitForSeconds(2f);

        // 💀 LOAD LOSE SCENE
        SceneManager.LoadScene(GameOver);
    }
}