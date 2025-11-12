using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TileSelector : MonoBehaviour
{
    private Renderer rend;
    private Color baseColor;
    private GridManager gridManager;

    [HideInInspector] public int x, y;
    [HideInInspector] public bool isWalkable = true;
    [HideInInspector] public int moveCost = 1;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
    }

    public void Init(GridManager manager, int gridX, int gridY, bool walkable, int cost)
    {
        gridManager = manager;
        x = gridX;
        y = gridY;
        isWalkable = walkable;
        moveCost = cost;
    }

    private void OnMouseDown()
    {
        if (gridManager == null) return;
        gridManager.OnTileClicked(x, y);
    }

    public void Highlight(Color color)
    {
        if (rend != null)
            rend.material.color = color;
    }

    public void ResetColor()
    {
        if (rend != null)
            rend.material.color = baseColor;
    }
}
