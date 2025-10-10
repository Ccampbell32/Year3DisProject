using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public Vector2Int gridPosition;   // Tile's (x,y) on grid
    private Renderer rend;
    private Color baseColor;
    public bool isReachable = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
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

        isReachable = false;
    }
}
