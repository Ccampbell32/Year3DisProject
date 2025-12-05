using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void WinHandler();
public class GameStateManager : MonoBehaviour
{
    [Header ("Icons")]
    [SerializeField] private string iconTagName;
    [SerializeField] private GameObject puzzleIconName;
    [SerializeField] private GameObject searchableIconName;
    [SerializeField] private GameObject weaponIconName;
    [SerializeField] private GameObject powerIconName;

    [Header ("Games")]
    [SerializeField] private GameObject lockpickGame;
    [SerializeField] private GameObject searchingGame;
    [SerializeField] private GameObject powerGame;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private PowerScript powerScript;
    private bool searchesFinished;
    private bool lockpickGameFinished;

    private bool gotWeapon;
    private bool powerOn;

    [SerializeField] private int characterCount;

    public static event WinHandler WeaponGot;
    public static event WinHandler LockPickWin;
    public static event WinHandler SearchFinished;
    public static event WinHandler PowerOn;
    public static event WinHandler Escape;


    void Start()
    {
        characterCount = 0;
        searchesFinished = false;
        lockpickGameFinished = false;
        powerOn = false;
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
                        if (!lockpickGameFinished)
                        {
                            ActivatePickMiniGame();
                        }
                    }

                    else if (hit.transform.name == searchableIconName.name)
                    {
                        //Debug.Log("hit search game");
                        if (!searchesFinished)
                        {
                            ActivateSearchMiniGame();
                            //Debug.Log("Activae search game");
                        }
                    }

                    else if (hit.transform.name == weaponIconName.name)
                    {
                        //weapon
                        if (!gotWeapon)
                        {
                            ActivateBat();
                        }
                    }

                    else if (hit.transform.name == powerIconName.name && powerIconName.activeSelf == true)
                    {
                        if (!powerOn)
                        {
                            ActivatePowerGame();
                        }
                    }

                }
            }
        }
    }

    public void ActivatePowerGame()
    {
        //Debug.Log("actiavte power");
        if (powerOn) return;

        powerIconName.SetActive(false);
        powerScript.ActivateSwitch();
    }

    public void DeactivatePowerGame()
    {
        if (powerOn) return;
        //Debug.Log("deactivate power");
        this.powerIconName.SetActive(true);
    }

    public void CompletePowerGame()
    {
        //Debug.Log("complete power");
        if (powerOn) return;

        if (PowerOn != null)
            PowerOn();

        powerOn = true;
        powerIconName.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }


    public void ActivateSearchMiniGame()
    {
        //Debug.Log("activate Search");
        if (searchesFinished) return;

        searchingGame.SetActive(true);
    }
    public void DeactivateSearchMiniGame()
    {
        Debug.Log("deactivate Search");
        searchingGame.SetActive(false);
    }
    public void CompleteSearchMiniGame()
    {
        //Debug.Log("complete Search");
        if (searchesFinished) return;

        if (SearchFinished != null)
            SearchFinished();
        searchesFinished = true;
        searchingGame.SetActive(false);

        gridManager.ReplaceInteractibleTile();
    }


    public void ActivatePickMiniGame()
    {
        //Debug.Log("activate Pick");
        if (lockpickGameFinished) return;

        lockpickGame.SetActive(true);
    }
    public void DeactivatePickMiniGame()
    {
        //Debug.Log("deactivate Pick");
        //lockpickGame.SetActive(false);
    }
    public void CompletePickMiniGame()
    {
        //Debug.Log("complete Pick");
        if (lockpickGameFinished) return;

        if (LockPickWin != null)
            LockPickWin();

        lockpickGameFinished = true;
        //lockpickGame.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }


    public void ActivateBat()
    {
        //Debug.Log("Got bat");
        if (gotWeapon) return;

        if (WeaponGot != null)
            WeaponGot();

        gotWeapon = true;

        weaponIconName.SetActive(false);
        gridManager.ReplaceInteractibleTile();
    }

    public void CharacterEscape()
    {
        Escape();
    }
}
