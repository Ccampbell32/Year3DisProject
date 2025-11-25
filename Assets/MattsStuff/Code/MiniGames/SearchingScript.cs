using System;
using System.Collections;
using UnityEngine;   
using UnityEngine.UI;


public class SearchingScript : MonoBehaviour
{
    private bool isSearching;

    [SerializeField] private Image searchProgressBar;
    void Awake()
    {
        StartSearchCountdown();
        searchProgressBar.fillAmount = 0f;
    }
    private void StartSearchCountdown()
    {
        StartCoroutine(SearchProgress());
    }

    private IEnumerator SearchProgress()
    {
        isSearching = true;
        yield return new WaitForSeconds(2f);

        isSearching = false;
    }

    void Update()
    {
        if (isSearching)
        {
            searchProgressBar.fillAmount += 0.0007f;
        }
    }

}
