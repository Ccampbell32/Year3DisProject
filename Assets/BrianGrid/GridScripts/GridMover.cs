using UnityEngine;

public class GridMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Grid Reference")]
    public GridManager gridManager;

    private Vector2Int currentGridPos;
    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();

        currentGridPos = gridManager.GetClosestGridPosition(transform.position);
        targetWorldPos = gridManager.GetWorldPosition(currentGridPos.x, currentGridPos.y);
        transform.position = targetWorldPos;

        gridManager.SetPlayerPosition(currentGridPos);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                transform.position = targetWorldPos;
                isMoving = false;
                gridManager.OnPlayerArrivedAtTile(currentGridPos);
            }
        }
    }

    public void MoveToTile(int x, int y)
    {
        if (isMoving) return;
        if (!gridManager.IsValidTile(x, y)) return;

        targetWorldPos = gridManager.GetWorldPosition(x, y);
        currentGridPos = new Vector2Int(x, y);
        isMoving = true;
        gridManager.SetPlayerPosition(currentGridPos);
    }

    public Vector2Int GetCurrentGridPos() => currentGridPos;
}
