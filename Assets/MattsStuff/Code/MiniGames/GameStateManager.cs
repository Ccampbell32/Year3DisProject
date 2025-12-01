using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour
{
    
    
    [SerializeField] private string iconTagName;
    [SerializeField] private GameObject puzzleIconName;
    [SerializeField] private GameObject searchableIconName;
    [SerializeField] private GameObject weaponIconName;

    [SerializeField] private GameObject lockpickGame;
    [SerializeField] private GameObject searchingGame;

    private bool searchesFinished;
    private bool lockpickGameFinished;

    public delegate void WinHandler();
    public static event WinHandler WeaponGot;

    void Start()
    {
        searchesFinished = false;
        if (lockpickGame != null)
        {
            DeactivatePickMiniGame();
        }
        if (searchingGame != null)
        {
            DeactivateSearchMiniGame();
        }
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("worked");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (hit.transform.CompareTag(iconTagName))
                {
                    if (hit.transform.name == puzzleIconName.name)
                    {
                        //activate puzzle
                        if(!lockpickGameFinished)
                        {
                            ActivatePickMiniGame();
                        }
                    }

                    else if (hit.transform.name == searchableIconName.name)
                    {
                        //search
                        if(!searchesFinished)
                        {
                            ActivateSearchMiniGame();
                        }
                    }

                    else if (hit.transform.name == weaponIconName.name)
                    {
                        //weapon
                        if (WeaponGot != null)
                        WeaponGot();
                    }

                }
            }
        }
    }

    public void ActivateSearchMiniGame()
    {
        if(searchesFinished) return;
        
        searchingGame.SetActive(true);
        searchesFinished = true;
        
    }
    public void DeactivateSearchMiniGame()
    {
        searchingGame.SetActive(false);
    }

    public void ActivatePickMiniGame()
    {
        //Debug.Log("activate");
        if(lockpickGameFinished) return;

        lockpickGame.SetActive(true);
        lockpickGameFinished = true;
    }
    public void DeactivatePickMiniGame()
    {
        //Debug.Log("deactivate");
        lockpickGame.SetActive(false);
    }
}
