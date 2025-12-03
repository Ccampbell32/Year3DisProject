using UnityEngine;

public class PowerScript : MonoBehaviour
{
    private bool switch1Active;
    private GameObject switch1;

    private bool switch2Active;
    private GameObject switch2;

    private float switchCount;
    private bool powerOn;


    private void Start()
    {
        ResetSwitches();
    }
    private void ResetSwitches()
    {
        switch1Active = false;
        switch2Active = false;
    }

    private void Update()
    {
        if (switch1Active && switch2Active)
        {
            powerOn = true;
        }

        if (powerOn)
        {
            // Power is on, perform necessary actions
        }
    }

    public void ActivateSwitch1()
    {
        if (!switch1Active)
        {
            switch1Active = true;
        }
        else if (switch1Active && !switch2Active)
        {
            switch2Active = true;
        }
        else
        {
            Debug.Log("error");
        }

    }
}
