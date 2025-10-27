using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class GridMover : MonoBehaviour
{
    public GridManager gridManager;
    public float moveSpeed = 3f;
    public int maxMoveDistance = 3;   // how far the cube can move per turn
    public int movesPerTurn = 1;      // number of moves allowed each turn

    public Vector2Int startGridPos = new Vector2Int(0, 0);
    private Vector2Int currentGridPos;
    private bool isMoving = false;
    private int movesUsed = 0;

    private int distance;

    private List<Vector2> interactbleTiles;

    [SerializeField] private GameObject puzzleIcon;
    [SerializeField] private GameObject searchableIcon;
    [SerializeField] private GameObject weaponIcon;


    // --- Added for TurnManager communication ---
    public Vector2Int CurrentGridPos => currentGridPos;  // lets other scripts read the current position

    public void ClearHighlights()
    {
        ClearHighlightsInternal();  // helper method we'll define below
    }


    void Start()
    {

        puzzleIcon.SetActive(false);
        searchableIcon.SetActive(false);
        weaponIcon.SetActive(false);

        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();

        currentGridPos = startGridPos;
        transform.position = gridManager.GetWorldPosition(currentGridPos.x, currentGridPos.y);

        interactbleTiles = gridManager.GetInteractiveTilesList();

        HighlightReachableTiles();
    }

    void Update()
    {
        if (isMoving) return;
        if (movesUsed >= movesPerTurn) return; // no moves left this turn

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TileSelector tile = hit.collider.GetComponent<TileSelector>();

                if (tile != null && tile.isReachable)
                {
                    TryMoveTo(tile.gridPosition);
                }
            }
        }
    }

    public void TryMoveTo(Vector2Int targetGridPos)
    {
        int distance = Mathf.Abs(targetGridPos.x - currentGridPos.x) + Mathf.Abs(targetGridPos.y - currentGridPos.y);

        if (distance <= maxMoveDistance)
        {
            StartCoroutine(MoveAlongGrid(targetGridPos));
        }
        else
        {
            //Debug.Log("That tile is too far!");
        }
    }

    private void CheckForInteractive(Vector2Int currentGridPos)
    {
        foreach (Vector2 interactiveGridPos in gridManager.Puzzletiles)
        {
            if (currentGridPos == interactiveGridPos)
            {
                puzzleIcon.SetActive(true);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.SearcjableTilesList)
        {
            if (currentGridPos == interactiveGridPos)
            {
                searchableIcon.SetActive(true);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.WeaponTile)
        {
            if (currentGridPos == interactiveGridPos)
            {
                weaponIcon.SetActive(true);
            }
        }
    }
    
    IEnumerator MoveAlongGrid(Vector2Int target)
    {
        isMoving = true;

        // Simple X → Y stepping
        while (currentGridPos.x != target.x)
        {
            currentGridPos.x += (target.x > currentGridPos.x) ? 1 : -1;
            yield return MoveToCurrentTile();
        }

        while (currentGridPos.y != target.y)
        {
            currentGridPos.y += (target.y > currentGridPos.y) ? 1 : -1;
            yield return MoveToCurrentTile();
        }

        movesUsed++;
        CheckForInteractive(currentGridPos);

        isMoving = false;

        // End of move: clear and re-highlight for next move if still in turn
        if (movesUsed < movesPerTurn)
        {
            HighlightReachableTiles();
        }

        else
            ClearHighlights();
    }

    IEnumerator MoveToCurrentTile()
    {
        Vector3 targetPos = gridManager.GetWorldPosition(currentGridPos.x, currentGridPos.y);
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
    }

    // 🔹 Highlight all tiles within move range
    void HighlightReachableTiles()
    {
        ClearHighlights();
        foreach (TileSelector tile in FindObjectsOfType<TileSelector>())
        {
            if (tile)
            {
                distance = Mathf.Abs(tile.gridPosition.x - currentGridPos.x) + Mathf.Abs(tile.gridPosition.y - currentGridPos.y);
            }

            if (distance <= maxMoveDistance)
            {
                tile.Highlight(Color.cyan);
                tile.isReachable = true;
            }

        }
    }

    // 🔹 Reset all tiles to normal
    private void ClearHighlightsInternal()
    {
        foreach (TileSelector tile in FindObjectsOfType<TileSelector>())
        {
            if (tile.GetComponent<InteractiveTile>() == null) // Don't reset interactive tiles
            {
                tile.ResetColor();
            }
        }
    }


    // 🔹 Called externally (e.g., by a Turn Manager)
    public void StartNewTurn()
    {
        movesUsed = 0;
        HighlightReachableTiles();
    }
}
