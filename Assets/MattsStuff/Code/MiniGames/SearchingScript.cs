using System;
using System.Collections;
using UnityEngine;   
using UnityEngine.UI;


public class SearchingScript : MonoBehaviour
{
    private bool isSearching;
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private Image searchProgressBar;

    public static event WinHandler SearchFinished;

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
        searchProgressBar.fillAmount = 0f;
        //Debug.Log("Searching...");
        yield return new WaitForSeconds(2f);

        isSearching = false;
        //Debug.Log("Search Complete");
        SearchFinished();
        gameStateManager.DeactivateSearchMiniGame();
        
    }

    void Update()
    {
        if (isSearching)
        {
            searchProgressBar.fillAmount += 0.0014f;
        }
    }

}
