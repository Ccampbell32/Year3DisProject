using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    [Header("Tile Prefabs")]
    public GameObject normalTilePrefab;
    public GameObject mudTilePrefab;
    public GameObject wallTilePrefab;

    [HideInInspector] public TileSelector[,] tiles;

    [Header("References")]
    public GridMover playerMover;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new TileSelector[width, height];
        Vector3 startPos = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = startPos + new Vector3(x * cellSize, 0, y * cellSize);
                GameObject tileObj = Instantiate(normalTilePrefab, worldPos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{x}_{y}";

                TileSelector selector = tileObj.GetComponent<TileSelector>();
                if (selector == null)
                    selector = tileObj.AddComponent<TileSelector>();

                selector.Init(this, x, y, true, 1);
                tiles[x, y] = selector;
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(x * cellSize, 0, y * cellSize);
    }

    public Vector2Int GetClosestGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.z - transform.position.z) / cellSize);
        return new Vector2Int(Mathf.Clamp(x, 0, width - 1), Mathf.Clamp(y, 0, height - 1));
    }

    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height && tiles[x, y] != null && tiles[x, y].isWalkable;
    }

    public void OnTileClicked(int x, int y)
    {
        if (playerMover == null)
            return;

        if (!IsValidTile(x, y))
            return;

        playerMover.MoveToTile(x, y);
    }

    public void HighlightRange(Vector2Int center, int range)
    {
        ClearHighlights();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int distance = Mathf.Abs(x - center.x) + Mathf.Abs(y - center.y);
                if (distance <= range)
                {
                    if (tiles[x, y] != null)
                    {
                        if (tiles[x, y].isWalkable)
                            tiles[x, y].Highlight(Color.cyan);
                        else
                            tiles[x, y].Highlight(Color.red);
                    }
                }
            }
        }
    }

    public void ClearHighlights()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] != null)
                    tiles[x, y].ResetColor();
            }
        }
    }

    public void OnPlayerArrivedAtTile(Vector2Int pos)
    {
        HighlightRange(pos, 3); // Example: highlight around player
    }

    public void SetPlayerPosition(Vector2Int pos)
    {
        HighlightRange(pos, 3);
    }
}
