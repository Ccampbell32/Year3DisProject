using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveTile : MonoBehaviour
{
    //makes tiles interactive
    [SerializeField] private Renderer rend;
    [SerializeField] private Collider tileCollider;
    [SerializeField] private GameObject player;
    private Color interactiveColor = Color.magenta;

    void Start()
    {

        if (tileCollider == null)
        {
            tileCollider = GetComponent<Collider>();
            tileCollider.isTrigger = true;
        }

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
        
        TileSelector tile = GetComponent<TileSelector>();
        if (tile != null)
        {
            tile.Highlight(interactiveColor);
        }
    }
}
