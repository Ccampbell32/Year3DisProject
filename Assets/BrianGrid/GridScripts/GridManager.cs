using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;           // Number of columns
    public int height = 10;          // Number of rows
    public float cellSize = 1f;      // Distance between cells

    [Header("Tile Settings")]
    public GameObject tilePrefab;    // The prefab to spawn on each grid cell

    [Header("Visualization")]
    public bool showGizmos = true;
    public Color gridColor = Color.grey;

    [Header("Matts amazing stuff")]
    [SerializeField] public List<Vector2> SearcjableTilesList = new List<Vector2>();
    [SerializeField] public List<Vector2> WeaponTile = new List<Vector2>();
    [SerializeField] public List<Vector2> Puzzletiles = new List<Vector2>();
    [SerializeField] private List<Vector2> AllInteractableTiles = new List<Vector2>();
    

    // Store world positions of each cell
    private Vector3[,] gridPositions;

    void Awake()
    {
        CollectLists();
        GenerateGrid();
    }

    public void CollectLists()
    {
        AllInteractableTiles.AddRange(SearcjableTilesList);
        AllInteractableTiles.AddRange(Puzzletiles);
        AllInteractableTiles.AddRange(WeaponTile);
    }
    void GenerateGrid()
    {
        gridPositions = new Vector3[width, height];

        Vector3 startPos = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = startPos + new Vector3(x * cellSize, 0, y * cellSize);
                gridPositions[x, y] = worldPos;

                if (tilePrefab != null)
                {
                    // Instantiate tile
                    GameObject tile = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
                    tile.name = $"Tile_{x}_{y}";

                    // ✅ Add this part: assign TileSelector and grid coordinates
                    TileSelector selector = tile.GetComponent<TileSelector>();

                    if (selector == null)
                        selector = tile.AddComponent<TileSelector>();

                    selector.gridPosition = new Vector2Int(x, y);

                    foreach (Vector2 intGridPos in Puzzletiles)
                    {
                        if (intGridPos.x == x && intGridPos.y == y)
                        {
                            tile.AddComponent<InteractiveTile>().tileType = InteractiveTile.TileType.Searchable;
                        }
                    }

                    foreach (Vector2 intGridPos in SearcjableTilesList)
                    {
                        if (intGridPos.x == x && intGridPos.y == y)
                        {
                            tile.AddComponent<InteractiveTile>().tileType = InteractiveTile.TileType.Searchable;
                        }
                    }

                    foreach (Vector2 intGridPos in WeaponTile)
                    {
                        if (intGridPos.x == x && intGridPos.y == y)
                        {
                            tile.AddComponent<InteractiveTile>().tileType = InteractiveTile.TileType.Puzzle;

                        }
                    }
                    
                }
            }
        }
    }

    public List<Vector2> GetInteractiveTilesList()
    {
        return AllInteractableTiles; 
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return Vector3.zero;

        return gridPositions[x, y];
    }

    // Optional visual debug
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gridColor;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, 0.1f, cellSize));
            }
        }
    }
}

