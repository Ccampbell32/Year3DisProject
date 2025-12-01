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

    private bool gotKey;
    //private bool gotBat;
    private bool gotTire;


    void Start()
    {
        gotKey = false;
        //gotBat = false;
        gotTire = false;

        winText.SetActive(false);

        Tire.SetActive(false);
        TireOutline.SetActive(true);
        Keys.SetActive(false);      
        KeysOutline.SetActive(true);
        Bat.SetActive(false);
        BatOutline.SetActive(true);

        LockpickingMiniGame.LockPickWin += GotKey;
        SearchingScript.SearchFinished += GotTire;
        //GameStateManager.WeaponGot += GotBat;
    }

    void GotKey()
    {
        if(!gotKey )
        {
            Keys.SetActive(true);
            KeysOutline.SetActive(false);
            gotKey = true;
        }
    }
    void GotBat()
    {
        Bat.SetActive(true);
        BatOutline.SetActive(false);
        //gotBat = true;
    }
    void GotTire()
    {
        if(!gotTire)
        {
            Tire.SetActive(true);
            TireOutline.SetActive(false);
            gotTire = true;
        }
        
    }
    void Update()
    {
        if (gotKey && gotTire)
        {
            winText.SetActive(true);
        }
    }
}
