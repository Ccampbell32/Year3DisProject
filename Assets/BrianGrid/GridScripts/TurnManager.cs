using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public bool IsPlayerTurn = true;

    [SerializeField][Range(10, 60)] private float turnTimeLimit;

    [SerializeField] private GameObject handToTurn;

    private float timeCountdown;

    private void Awake()
    {
        Instance = this;
        StartPlayerTurn();
    }

    public void EndTurn()
    {

        //Debug.Log("Force ending turn.");
        IsPlayerTurn = false;

        // Call enemy actions here
        EnemyPhase();
    }

    private void EnemyPhase()
    {
        //Debug.Log("Enemy turn started!");
        // When enemies finish:
        //Debug.Log("Enemy turn ended!");
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        //Debug.Log("Player turn started!");
        timeCountdown = turnTimeLimit;
        handToTurn.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);
        StartCoroutine(StartCountdown());
        IsPlayerTurn = true;
    }

    public IEnumerator StartCountdown()
    {
        Quaternion newAngle;
        while (timeCountdown > 0)
        {
            //Debug.Log("Countdown: " + timeCountdown);         
            newAngle = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, (timeCountdown / turnTimeLimit) * -360f);
            yield return new WaitForSeconds(1.0f);
            timeCountdown--;
        }
        //Debug.Log("Turn time over!");
        EndTurn();
    }

}
