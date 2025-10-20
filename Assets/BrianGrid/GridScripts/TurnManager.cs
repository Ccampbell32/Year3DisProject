using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public GridMover player;
    public GridMover enemy;

    [SerializeField] private enum Turn { Player, Enemy }
    private Turn currentTurn = Turn.Player;

    void Start()
    {
        currentTurn = Turn.Player;
        StartPlayerTurn();
    }

    void Update()
    {
        // Manual "End Turn" for testing
        if (currentTurn == Turn.Player && Input.GetKeyDown(KeyCode.Space))
        {
            EndPlayerTurn();
        }
    }

    // --- PLAYER TURN ---
    void StartPlayerTurn()
    {
        //Debug.Log("▶️ Player Turn Start");
        currentTurn = Turn.Player;
        player.StartNewTurn();
    }

    public void EndPlayerTurn()
    {
        //Debug.Log("⏹️ Player Turn End");
        player.ClearHighlights(); // make sure grid resets
        StartCoroutine(StartEnemyTurnAfterDelay());
    }

    IEnumerator StartEnemyTurnAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartEnemyTurn();
    }

    // --- ENEMY TURN ---
    void StartEnemyTurn()
    {
        //Debug.Log("🔴 Enemy Turn Start");
        currentTurn = Turn.Enemy;
        enemy.StartNewTurn();

        // Let the enemy think & move automatically
        StartCoroutine(EnemyMoveRoutine());
    }

    IEnumerator EnemyMoveRoutine()
    {
        // small delay before acting
        yield return new WaitForSeconds(0.5f);

        // pick a simple target: one tile closer to the player
        Vector2Int playerPos = player.CurrentGridPos;
        Vector2Int enemyPos = enemy.CurrentGridPos;

        Vector2Int dir = Vector2Int.zero;
        if (playerPos.x > enemyPos.x) dir.x = 1;
        else if (playerPos.x < enemyPos.x) dir.x = -1;
        else if (playerPos.y > enemyPos.y) dir.y = 1;
        else if (playerPos.y < enemyPos.y) dir.y = -1;

        Vector2Int target = enemyPos + dir;
        enemy.TryMoveTo(target);

        // wait for movement to finish
        yield return new WaitForSeconds(1.5f);

        EndEnemyTurn();
    }

    void EndEnemyTurn()
    {
        Debug.Log("⚪ Enemy Turn End");
        enemy.ClearHighlights();
        StartCoroutine(StartPlayerTurnAfterDelay());
    }

    IEnumerator StartPlayerTurnAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartPlayerTurn();
    }
}
