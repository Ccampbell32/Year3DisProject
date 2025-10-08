using UnityEngine;

public class LockpickingMiniGame : MonoBehaviour
{


    [SerializeField] private GameObject pick;
    [SerializeField] private GameObject centreLine;
    [SerializeField][Range(-90, 90)] private float greenSpotAngle;
    [SerializeField][Range(-90, 90)] private float currentAngle;

    [SerializeField] private float unlockProgress;

    private float percentageTurn; //0-1

    private Vector3 screenPosition;
    private float centreofScreen;

    private bool isPicking;


    private void Start()
    {
        isPicking = false;
        unlockProgress = 0f;
        centreofScreen = Screen.width / 2;
        currentAngle = 0f;
        greenSpotAngle = Random.Range(-90f, 90f);
        centreLine.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
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
                percentageTurn = Mathf.InverseLerp(centreofScreen, Screen.width, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = percentageTurn * 90f;
            }

            //left side of screen
            else if (screenPosition.x < Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreofScreen, 0, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = (-percentageTurn * 90f);
            }

            pick.transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
            //Debug.Log(currentAngle);
            #endregion
        }

        #region Unlocking Mechanism

        //Debug.Log(pick.transform.rotation.eulerAngles.z);

        if (isPicking && currentAngle >= greenSpotAngle - 10f && currentAngle <= greenSpotAngle + 10f)
        {
            unlockProgress += 0.5f;

            Debug.Log("Unlocking");

            if (unlockProgress >= 40)
            {
                Debug.Log("Unlocked");
                Debug.Log("Puzzle Complete");
            }
            centreLine.transform.rotation = Quaternion.Euler(0f, unlockProgress * 2, 0f);
        }
        else if (isPicking && (currentAngle < greenSpotAngle - 10f || currentAngle > greenSpotAngle + 10f))
        {
            Debug.Log("Breaking");
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
