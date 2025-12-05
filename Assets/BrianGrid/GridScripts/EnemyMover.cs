using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 4f;

    private GridManager grid;
    private AStarPathfinding pathfinder;

    private Vector2Int currentGridPos;
    private List<Vector2Int> currentPath;
    private int pathIndex = 0;
    private bool isMoving = false;

    [SerializeField] private List<GameObject> players;

    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
        //pathfinder = FindObjectOfType<AStarPathfinding>();

        currentGridPos = grid.GetClosestGridPosition(transform.position);
    }

    // ---------------------------------------------------------
    // Called by TurnManager during ENEMY TURN
    // ---------------------------------------------------------
    public void TakeTurn()
    {
        if(UnityEngine.Random.Range(0,10)%2 == 0)
        {
            Vector2Int moveToTile = new Vector2Int();
            moveToTile.x = (int)players[0].gameObject.transform.position.x; 
            moveToTile.y = (int)players[0].gameObject.transform.position.y; 

            MoveTowardsPlayer(moveToTile);
        }
        else
        {
            Vector2Int moveToTile = new Vector2Int();
            moveToTile.x = (int)players[1].gameObject.transform.position.x; 
            moveToTile.y = (int)players[1].gameObject.transform.position.y; 

            MoveTowardsPlayer(moveToTile);
        }
    }

    // ---------------------------------------------------------
    // Pathfind toward the player
    // ---------------------------------------------------------
    public void MoveTowardsPlayer(Vector2Int playerPos)
    {
        currentPath = pathfinder.FindPath(currentGridPos, playerPos);

        if (currentPath != null && currentPath.Count > 0)
        {
            pathIndex = 0;
            isMoving = true;
        }
    }

    // ---------------------------------------------------------
    // Move along the calculated path
    // ---------------------------------------------------------
    private void Update()
    {
        if (!isMoving || currentPath == null || pathIndex >= currentPath.Count)
            return;

        Vector3 targetPos = grid.GetWorldPosition(currentPath[pathIndex].x, currentPath[pathIndex].y);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            currentGridPos = currentPath[pathIndex];
            pathIndex++;

            if (pathIndex >= currentPath.Count)
                isMoving = false;
        }
    }
}
