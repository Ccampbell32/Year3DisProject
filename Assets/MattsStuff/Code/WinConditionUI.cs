using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinConditionUI : MonoBehaviour
{
    [SerializeField] private GameObject winText;

    [SerializeField] private GameObject Tire;
    [SerializeField] private GameObject TireOutline;

    [SerializeField] private GameObject Keys;    
    [SerializeField] private GameObject KeysOutline;    

    [SerializeField] private GameObject Bat;
    [SerializeField] private GameObject BatOutline;

    private int winCount; 

    private bool gotKey;
    private bool gotBat;
    private bool gotTire;


    void Start()
    {
        gotKey = false;
        gotBat = false;
        gotTire = false;

        winCount = 0;
        winText.SetActive(false);

        Tire.SetActive(false);
        TireOutline.SetActive(true);
        Keys.SetActive(false);      
        KeysOutline.SetActive(true);
        Bat.SetActive(false);
        BatOutline.SetActive(true);

        LockpickingMiniGame.LockPickWin += GotKey;
        //GameStateManager.SearchFinished += GotBat;
        GameStateManager.WeaponGot += GotTire;
    }

    void GotKey()
    {
        if(!gotKey )
        {
            Keys.SetActive(true);
            KeysOutline.SetActive(false);
            winCount += 1;
            gotKey = true;
        }
    }
    void GotBat()
    {
        Bat.SetActive(true);
        BatOutline.SetActive(false);
        winCount += 1;
        gotBat = true;
    }
    void GotTire()
    {
        if(!gotTire)
        {
            Tire.SetActive(true);
            TireOutline.SetActive(false);
            winCount += 1;
            gotTire = true;
        }
        
    }
    void Update()
    {
        if (winCount >= 4)
        {
            winText.SetActive(true);
        }
    }
}
