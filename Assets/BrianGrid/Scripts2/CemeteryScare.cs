using UnityEngine;
using System.Collections;

public class CemeteryScare : MonoBehaviour
{
    [Header("Figures")]
    public GameObject figureFar;
    public GameObject figureMid;
    public GameObject figureClose;

    [Header("Audio")]
    public AudioSource scareAudio;
    public AudioClip scareSound;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            
            // Turn off the trigger so it can't happen twice
            if (GetComponent<Collider>() != null) 
                GetComponent<Collider>().enabled = false;
                
            StartCoroutine(ScareSequence());
        }
    }

    IEnumerator ScareSequence()
    {
        // 1. Play the loud noise immediately
        if (scareAudio != null && scareSound != null)
        {
            scareAudio.PlayOneShot(scareSound);
        }

        // 2. Show the monster far away
        if (figureFar != null) figureFar.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        
        // 3. Stutter closer
        if (figureFar != null) figureFar.SetActive(false);
        if (figureMid != null) figureMid.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        // 4. Stutter right in their face!
        if (figureMid != null) figureMid.SetActive(false);
        if (figureClose != null) figureClose.SetActive(true);

        // 5. Keep the final scare active for exactly 5 seconds as requested
        yield return new WaitForSeconds(5f);

        // 6. Disable the scare completely
        if (figureClose != null) figureClose.SetActive(false);
    }
}