using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractiveTile : MonoBehaviour
{
    //makes tiles interactive
    [SerializeField] private Renderer rend;

    [SerializeField] private GameObject interactievePrompt;
    [SerializeField] private string interactiveObjName = "PuzzleInteraction";
    [SerializeField] private Collider tileCollider;
    [SerializeField] private GameObject player;
    private Color interactiveColor = Color.magenta;

    void Start()
    {
        if (interactievePrompt == null && !string.IsNullOrEmpty(interactiveObjName))
        {
            interactievePrompt = GameObject.Find(interactiveObjName);
        }
        if (interactievePrompt != null)
        {
            interactievePrompt.SetActive(false);
        }

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

    public void AllowInteraction()
    {
        //if the player is on the tile, allow interaction
        if (interactievePrompt != null)
        {
            Debug.Log("Player on interactive tile, showing prompt");
            interactievePrompt.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("This was triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            AllowInteraction();
        }
    }
}
