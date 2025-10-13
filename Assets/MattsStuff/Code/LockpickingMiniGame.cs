using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class LockpickingMiniGame : MonoBehaviour
{


    [SerializeField] private GameObject pick;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image breakBar;
    [SerializeField][Range(-90, 90)] private float greenSpotAngle;
    [SerializeField][Range(-90, 90)] private float currentAngle;
    [SerializeField]private float percentLeftToGreen;
    [SerializeField]private float percentRightToGreen;
    [SerializeField][Range(20, 40)] private float FullIndicatorDistance;
    [SerializeField] private float UnlockIndicatorDistance;

    [SerializeField] private float unlockProgress;
    [SerializeField] private float breakingProgress;
    [SerializeField] private float breakThreshold;
    [SerializeField] private float unlockThreshold;

    private float percentageTurn; //0-1

    private Vector3 screenPosition;
    private float centreOfScreen;

    private bool isPicking;

    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LostText ;


    private void Start()
    {
        progressBar.fillAmount = 0f;
        breakBar.fillAmount = 0f;

        isPicking = false;
        unlockProgress = 0f;
        breakingProgress = 0f;
        centreOfScreen = Screen.width / 2;
        currentAngle = 0f;
        greenSpotAngle = Random.Range(-90f, 90f);

        WinText.SetActive(false);
        LostText.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        #region  Rotation with mouse input

        //Mouse Input
        screenPosition = Input.mousePosition;
        //Debug.Log(screenPosition);

        if (!isPicking)
        {
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                //Not on screen do nothing
                return;
            }

            //right side of screen
            else if (screenPosition.x > Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreOfScreen, Screen.width, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = percentageTurn * 90f;
            }

            //left side of screen
            else if (screenPosition.x < Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreOfScreen, 0, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = (-percentageTurn * 90f);

            }

            pick.transform.rotation = Quaternion.Euler( pick.transform.rotation.eulerAngles.x,  pick.transform.rotation.eulerAngles.y, currentAngle);
            //Debug.Log(currentAngle);
            #endregion
        }

        #region Unlocking Mechanism

        //Debug.Log(pick.transform.rotation.eulerAngles.z);

        if (breakingProgress >= breakThreshold)
        {
            Debug.Log("Broke");
           LostText.SetActive(true);       
        }

        else if (isPicking && currentAngle >= greenSpotAngle - FullIndicatorDistance && currentAngle <= greenSpotAngle + FullIndicatorDistance)
        {
            
            percentLeftToGreen = Mathf.InverseLerp(greenSpotAngle - FullIndicatorDistance , greenSpotAngle, currentAngle);
            percentRightToGreen = Mathf.InverseLerp(greenSpotAngle + FullIndicatorDistance, greenSpotAngle, currentAngle);

            //Debug.Log("Percent Left to Green: " + percentLeftToGreen);
            //Debug.Log("Percent Right to Green: " + percentRightToGreen);
            //Debug.Log(percentLeftToGreen * 100);
            //Debug.Log(percentRightToGreen * 100);

            //Unlocking area
            if (currentAngle >= greenSpotAngle - UnlockIndicatorDistance)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                //Debug.Log("Yes");

            }
            //Left side percent
            else if (unlockProgress < (percentLeftToGreen * 100f) && percentRightToGreen == 1f)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                //Debug.Log("Unlocking Left");
            }

            else if (unlockProgress < (percentRightToGreen * 100f) && percentLeftToGreen == 1f)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                //Debug.Log("Unlocking Right");

            }
            else
            {
                //Debug.Log("Breaking");
                breakingProgress += 0.5f;
                breakBar.fillAmount = breakingProgress / breakThreshold;
            }


            //Debug.Log("Unlocking");

            if (unlockProgress >= unlockThreshold)
            {
                WinText.SetActive(true);
                //Debug.Log("Puzzle Complete");
            }
        }

        else if (isPicking && (currentAngle < greenSpotAngle - FullIndicatorDistance || currentAngle > greenSpotAngle + FullIndicatorDistance))
        {
            //Debug.Log("Breaking");
            breakingProgress += 0.5f;
            breakBar.fillAmount = breakingProgress / breakThreshold;
        }

        else
        {
            unlockProgress = 0f;
            progressBar.fillAmount = 0f;
            //Debug.Log("Locked");
        }
        #endregion
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPicking = true;
            //Debug.Log(isPicking);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPicking = false;
            //Debug.Log(isPicking);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            greenSpotAngle = Random.Range(-90f, 90f);
            //Debug.Log("New Green Spot Angle: " + greenSpotAngle);
            unlockProgress = 0f;
            progressBar.fillAmount = 0f;
            breakingProgress = 0f;
            breakBar.fillAmount = 0f;
            WinText.SetActive(false);
            LostText.SetActive(false);
        }
    }
}
