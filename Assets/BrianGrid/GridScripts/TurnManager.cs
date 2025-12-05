using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [SerializeField] private LockpickingMiniGame lockpickingMiniGame;
    [SerializeField] private List<SearchingScript> searchingScripts;
    [SerializeField] private PowerScript powerScript;

    [SerializeField] private GameObject handToTurn;

    [SerializeField]
    [Range(10, 60)]
    private float turnTimeLimit = 30f;

    public bool IsPlayerTurn = true;

    private float timeCountdown;

    private void Awake()
    {
        Instance = this;
        StartPlayerTurn();
    }

    // ----------------------------
    // Called when Player Ends Turn
    // ----------------------------
    public void EndTurn()
    {
        IsPlayerTurn = false;

        // Cancel all player actions
        lockpickingMiniGame.FinishGame();

        foreach (SearchingScript searchingScript in searchingScripts)
            searchingScript.CancleSearch();

        powerScript.CanclePower();

        // Start enemy actions
        StartCoroutine(EnemyTurnRoutine());
    }

    // ----------------------------
    // Enemy Turn
    // ----------------------------
    private IEnumerator EnemyTurnRoutine()
    {
        EnemyMover[] enemies = FindObjectsOfType<EnemyMover>();

        // Find player grid pos (first player in scene)
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        Vector2Int playerPos = Vector2Int.zero;

        if (players.Length > 0)
        {
            GridManager grid = FindObjectOfType<GridManager>();
            playerPos = grid.GetClosestGridPosition(players[0].transform.position);
        }

        // Each enemy takes its turn
        foreach (EnemyMover e in enemies)
        {
            e.TakeTurn(playerPos);

            // Wait until enemy finishes movement
            while (e.IsMoving)
                yield return null;

            yield return new WaitForSeconds(0.1f); // small delay
        }

        // End enemy turn → start player turn
        StartPlayerTurn();
    }

    // ----------------------------
    // Player Turn
    // ----------------------------
    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;

        timeCountdown = turnTimeLimit;

        // Reset UI clock hand rotation
        handToTurn.transform.localRotation =
            Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);

        StartCoroutine(StartCountdown());
    }

    // ----------------------------
    // Countdown Timer
    // ----------------------------
    public IEnumerator StartCountdown()
    {
        Quaternion newAngle;

        while (timeCountdown > 0)
        {
            newAngle = Quaternion.Euler(
                Quaternion.identity.x,
                Quaternion.identity.y,
                (timeCountdown / turnTimeLimit) * -360f);

            handToTurn.transform.localRotation =
                Quaternion.Lerp(Quaternion.identity, newAngle, 1f);

            yield return new WaitForSeconds(1.0f);
            timeCountdown--;
        }

        EndTurn(); // times up
    }
}
