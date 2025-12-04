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

    [SerializeField] private GameObject Power;

    [SerializeField] private GameObject PowerOutline;

    [SerializeField] private GameObject CanEscapeObj;

    public static event WinHandler CanLeave;

    private bool onceLeave;
    private int characterCount = 0;

    private bool gotKey;
    private bool gotBat;
    private bool gotTire;
    private bool gotPower;
    private bool canEscape;

    void Start()
    {
        gotKey = false;
        gotBat = false;
        gotTire = false;
        gotPower = false;
        onceLeave = false;

        CanEscapeObj.SetActive(false);
        winText.SetActive(false);

        Tire.SetActive(false);
        TireOutline.SetActive(true);
        Keys.SetActive(false);
        KeysOutline.SetActive(true);
        Bat.SetActive(false);
        BatOutline.SetActive(true);
        Power.SetActive(false);
        PowerOutline.SetActive(true);

        GameStateManager.LockPickWin += GotKey;
        GameStateManager.SearchFinished += GotTire;
        GameStateManager.WeaponGot += GotBat;
        GameStateManager.PowerOn += GotPower;
        GameStateManager.Escape += OnEscape;
    }

    void GotKey()
    {
        if (!gotKey)
        {
            Keys.SetActive(true);
            KeysOutline.SetActive(false);
            gotKey = true;
        }
    }
    void GotBat()
    {
        if (!gotBat)
        {
            Bat.SetActive(true);
            BatOutline.SetActive(false);
            gotBat = true;
        }

    }
    void GotTire()
    {
        if (!gotTire)
        {
            Tire.SetActive(true);
            TireOutline.SetActive(false);
            gotTire = true;
        }

    }

    void GotPower()
    {
        {
            if (!gotPower)
            gotPower = true;
            Power.SetActive(true);
            PowerOutline.SetActive(false);
        }
    }

    void OnEscape()
    {
        characterCount += 1;
        Debug.Log("EscapeNo: " + characterCount);
        if (characterCount == 2)
        {
            Debug.Log("2 Escape");
            canEscape = true;
        }
    }

    void Update()
    {
        if (gotKey && gotTire && gotBat && gotPower && !onceLeave && winText.gameObject.activeSelf == false)
        {
            Debug.Log("can leave");
            CanEscapeObj.SetActive(true);
            onceLeave = true;
            CanLeave();
        }
        if(onceLeave && canEscape)
        {
            winText.SetActive(true);
        }

    }
}
