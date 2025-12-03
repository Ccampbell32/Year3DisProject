using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Tilemaps;

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
    [SerializeField] private GridMover currentGridMover;
    private Characters currentCharacter;
    [SerializeField] private GameObject currentCharacterObject;

    [SerializeField] private ChangeSelectedCharacter changeSelectedCharacter;

    [Header("Matts amazing stuff")]
    [SerializeField] public List<Vector2> SearchableTilesList = new List<Vector2>();
    [SerializeField] public List<Vector2> WeaponTile = new List<Vector2>();
    [SerializeField] public List<Vector2> Puzzletiles = new List<Vector2>();
    [SerializeField] public List<Vector2> Walltiles = new List<Vector2>();
    [SerializeField] public List<Vector2> Powertiles = new List<Vector2>();
    [SerializeField] private List<Vector2> EscapeTiles = new List<Vector2>();
    [SerializeField] private List<Vector2> AllInteractableTiles = new List<Vector2>();

    // Store world positions of each cell
    private Vector3[,] gridPositions;
    private void Awake()
    {
        CollectLists();
        GenerateGrid();
    }

    public void CollectLists()
    {
        AllInteractableTiles.AddRange(SearchableTilesList);
        AllInteractableTiles.AddRange(Puzzletiles);
        AllInteractableTiles.AddRange(WeaponTile);
    }

    public void AddWallTile(string tileName)
    {
        GameObject tileObj = GameObject.Find(tileName);
        if (tileObj != null)
        {
            Vector3 pos = tileObj.transform.position;
            Vector2Int gridPos = GetClosestGridPosition(pos);
            Walltiles.Add(new Vector2(gridPos.x, gridPos.y));
        }
    }

    public void ReplaceWallTile()
    {
        foreach (Vector2 wallGridPos in Walltiles)
        {
            GameObject wallTileObj = tiles[(int)wallGridPos.x, (int)wallGridPos.y].gameObject;
            Destroy(wallTileObj.GetComponent<WallColide>());
            TileSelector selector = wallTileObj.GetComponent<TileSelector>();
            if (selector != null)
            {
                selector.moveCost = 5;
                selector.isWalkable = false;
                selector.Highlight(Color.red);
            }
        }
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
                tileObj.AddComponent<BoxCollider>().isTrigger = true;
                tileObj.AddComponent<WallColide>();

                TileSelector selector = tileObj.GetComponent<TileSelector>();
                if (selector == null)
                    selector = tileObj.AddComponent<TileSelector>();

                selector.Init(this, x, y, true, 1);
                tiles[x, y] = selector;

                foreach (Vector2 intGridPos in Puzzletiles)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Puzzle;
                    }
                }

                foreach (Vector2 intGridPos in SearchableTilesList)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Searchable;
                    }
                }

                foreach (Vector2 intGridPos in WeaponTile)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Weapon;
                    }
                }

                foreach (Vector2 intGridPos in Powertiles)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Power;
                    }
                }

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
        //Debug.Log($"Tile clicked at ({x}, {y})");

        if (currentGridMover == null)
            return;

        if (!IsValidTile(x, y))
            return;

        currentGridMover.MoveToTile(x, y);
    }

    public void HighlightRange(Vector2Int center, int range)
    {
        ClearHighlights();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int distance = Mathf.Abs(x - center.x) + Mathf.Abs(y - center.y);
                //Debug.Log("Distance: " + distance  + " Center: " + center + " Tile: (" + x + "," + y + ")");
                if (distance + tiles[x,y].moveCost <= range)
                {
                    if (tiles[x, y] != null)
                    {
                        if (tiles[x, y].isWalkable && tiles[x, y].GetComponent<InteractiveTile>() == null)

                            tiles[x, y].Highlight(Color.cyan);

                        else if (tiles[x, y].GetComponent<InteractiveTile>() != null)
                            tiles[x, y].Highlight(Color.magenta);
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
                if (tiles[x, y] != null && tiles[x, y].GetComponent<InteractiveTile>() == null)
                    tiles[x, y].ResetColor();

                tiles[x, y].moveCost = 1;
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

    public void ResetHighlights()
    {
        ClearHighlights();
        HighlightRange(currentGridMover.GetCurrentGridPos(), 3);
    }

    public void SetCurrentCharacter()
    {
        currentCharacterObject = changeSelectedCharacter.GetCurrentCharacterObject();
        currentGridMover = currentCharacterObject.GetComponent<GridMover>();
    }
    public void ReplaceInteractibleTile()
    {
        Debug.Log("replace");
        Vector2Int currentGridPos = currentGridMover.GetCurrentGridPos();
        GameObject currentTileObj = tiles[currentGridPos.x, currentGridPos.y].gameObject;
        Destroy(currentTileObj.GetComponent<InteractiveTile>());

    }


}
