using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private string iconTagName;
    [SerializeField] private GameObject puzzleIconName;
    [SerializeField] private GameObject searchableIconName;
    [SerializeField] private GameObject weaponIconName;
    [SerializeField] private GameObject powerIconName;

    [SerializeField] private GameObject lockpickGame;
    [SerializeField] private GameObject searchingGame;
    [SerializeField] private GameObject powerGame;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private PowerScript powerScript;
    private bool searchesFinished;
    private bool lockpickGameFinished;

    private bool gotWeapon;
    private bool  powerOn;

    public delegate void WinHandler();
    public static event WinHandler WeaponGot;

    void Start()
    {
        searchesFinished = false;
        lockpickGameFinished = false;
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
                        if (gotWeapon)
                        WeaponGot();
                    }

                    else if (hit.transform.name == powerIconName.name)
                    {
                        ActivatePowerGame();
                    }

                }
            }
        }
    }

    public void ActivatePowerGame()
    {
        if(powerOn) return;

        powerIconName.SetActive(true);
    }

    public void DeactivatePowerGame()
    {
        powerIconName.SetActive(false);
    }

    public void CompletePowerGame()
    {
        powerOn = true;
        powerIconName.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }


    public void ActivateSearchMiniGame()
    {
        if(searchesFinished) return;

        searchingGame.SetActive(true);
        
    }
    public void DeactivateSearchMiniGame()
    {
        searchingGame.SetActive(false);
    }
    public void CompleteSearchMiniGame()
    {
        searchesFinished = true;
        searchingGame.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }


    public void ActivatePickMiniGame()
    {
        //Debug.Log("activate");
        if(lockpickGameFinished) return;

        lockpickGame.SetActive(true);
    }
    public void DeactivatePickMiniGame()
    {
        //Debug.Log("deactivate");
        lockpickGame.SetActive(false);
    }
    public void CompletePickMiniGame()
    {
        lockpickGameFinished = true;
        lockpickGame.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }
}
