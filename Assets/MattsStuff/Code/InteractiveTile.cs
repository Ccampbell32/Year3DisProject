using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveTile : MonoBehaviour
{
    //makes tiles interactive
    [SerializeField] private Renderer rend;
    
    [SerializeField] private GameObject interactievePrompt; 
    private Color interactiveColor = Color.magenta;

    void Start()
    {
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
            interactievePrompt.SetActive(false);
        }

        TileSelector tile = GetComponent<TileSelector>();
        if (tile != null)
        {
            tile.Highlight(interactiveColor);
        }
    }

    public void AllowInteraction()
    {
        //if the player is on the tile, allow interaction
        if (interactievePrompt != null)
        {
            interactievePrompt.SetActive(true);
        }
    }


}
