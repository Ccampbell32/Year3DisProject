using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public bool IsPlayerTurn = true;

    private void Awake()
    {
        Instance = this;
    }

    public void EndTurn()
    {
        if (IsPlayerTurn)
        {
            Debug.Log("Player ended turn!");
            IsPlayerTurn = false;

            // Call enemy actions here
            EnemyPhase();
        }
    }

    private void EnemyPhase()
    {
        Debug.Log("Enemy turn started!");

        // TODO: Add enemy movement logic here

        // When enemies finish:
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        Debug.Log("Player turn started!");
        IsPlayerTurn = true;
    }
}
