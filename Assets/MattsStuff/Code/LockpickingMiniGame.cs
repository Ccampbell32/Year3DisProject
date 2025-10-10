using UnityEngine;
using UnityEngine.UI;

public class LockpickingMiniGame : MonoBehaviour
{


    [SerializeField] private GameObject pick;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image breakBar;
    [SerializeField][Range(-90, 90)] private float greenSpotAngle;
    [SerializeField][Range(-90, 90)] private float currentAngle;

    [SerializeField] private float unlockProgress;
    [SerializeField] private float breakingProgress;

    private float percentageTurn; //0-1

    private Vector3 screenPosition;
    private float centreofScreen;

    private bool isPicking;


    private void Start()
    {
        progressBar.fillAmount = 0f;
        breakBar.fillAmount = 0f;

        isPicking = false;
        unlockProgress = 0f;
        breakingProgress = 0f;
        centreofScreen = Screen.width / 2;
        currentAngle = 0f;
        greenSpotAngle = Random.Range(-90f, 90f);
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
                percentageTurn = Mathf.InverseLerp(centreofScreen, Screen.width, screenPosition.z);
                //Debug.Log(percentageTurn);
                currentAngle = percentageTurn * 90f;
            }

            //left side of screen
            else if (screenPosition.x < Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreofScreen, 0, screenPosition.z);
                //Debug.Log(percentageTurn);
                currentAngle = (-percentageTurn * 90f);
            }

            pick.transform.rotation = Quaternion.Euler( pick.transform.rotation.eulerAngles.x,  pick.transform.rotation.eulerAngles.y, currentAngle);
            //Debug.Log(currentAngle);
            #endregion
        }

        #region Unlocking Mechanism

        //Debug.Log(pick.transform.rotation.eulerAngles.z);

        if (breakingProgress >= 40)
        {
            Debug.Log("Broke");
            //Puzzle Failed
        }

        else if (isPicking && currentAngle >= greenSpotAngle - 10f && currentAngle <= greenSpotAngle + 10f)
        {
            unlockProgress += 0.5f;
            progressBar.fillAmount = unlockProgress / 40f;

            Debug.Log("Unlocking");

            if (unlockProgress >= 40)
            {
                Debug.Log("Unlocked");
                Debug.Log("Puzzle Complete");
            }
        }

        else if (isPicking && (currentAngle < greenSpotAngle - 10f || currentAngle > greenSpotAngle + 10f))
        {
            Debug.Log("Breaking");
            breakingProgress += 0.5f;
            breakBar.fillAmount = breakingProgress / 40f;
        }

        else
        {
            unlockProgress = 0f;
            Debug.Log("Locked");
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
    }
}
