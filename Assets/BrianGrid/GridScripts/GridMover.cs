using UnityEngine;
using System.Collections.Generic;

public class GridMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Grid Reference")]
    public GridManager gridManager;

    private Vector2Int currentGridPos;
    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private List<Vector2> interactbleTiles;

    [Header("Matt's amazing stuff")]
    [SerializeField] private bool isFreeze;
    [SerializeField] private GameObject puzzleIcon;
    [SerializeField] private GameObject searchableIcon;
    [SerializeField] private GameObject weaponIcon;

    public void FreezeGridMoves()
    {
        isFreeze = true;
        Debug.Log("freeze");
    }

    public void UnfreezeGridMoves()
    {
        isFreeze = false;
        Debug.Log("unfreeze");
    }
    private void Start()
    {
        puzzleIcon.SetActive(false);
        searchableIcon.SetActive(false);
        weaponIcon.SetActive(false);

        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();

        currentGridPos = gridManager.GetClosestGridPosition(transform.position);
        targetWorldPos = gridManager.GetWorldPosition(currentGridPos.x, currentGridPos.y);
        transform.position = targetWorldPos;

        gridManager.SetPlayerPosition(currentGridPos);
        
        LockpickingMiniGame.freezeGridMove += FreezeGridMoves;
        LockpickingMiniGame.unfreezeGridMoves += UnfreezeGridMoves;  
    }

    private void Update()
    {
        if (isFreeze) return;
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
        CheckForInteractive(currentGridPos);
    }

    public Vector2Int GetCurrentGridPos() => currentGridPos;

    private void CheckForInteractive(Vector2Int currentGridPos)
    {
        foreach (Vector2 interactiveGridPos in gridManager.Puzzletiles)
        {
            if (currentGridPos == interactiveGridPos)
            {
                puzzleIcon.SetActive(true);
            }
            else
            {
                puzzleIcon.SetActive(false);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.SearcjableTilesList)
        {
            if (currentGridPos == interactiveGridPos)
            {
                searchableIcon.SetActive(true);
            }
            else
            {
                searchableIcon.SetActive(false);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.WeaponTile)
        {
            if (currentGridPos == interactiveGridPos)
            {
                weaponIcon.SetActive(true);
            }
            else
            {
                weaponIcon.SetActive(false);
            }
        }
    }

}
