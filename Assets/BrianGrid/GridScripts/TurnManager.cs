using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GridMover playerMover;
    public bool IsPlayerTurn = true;

    private void Start()
    {
        if (playerMover == null)
            playerMover = FindObjectOfType<GridMover>();
    }

    private void Update()
    {
        if (IsPlayerTurn)
        {
            // You can extend this for multiple turns or AI later
        }
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        Debug.Log("Player turn ended!");
    }

    public void StartPlayerTurn()
    {
        IsPlayerTurn = true;
        Debug.Log("Player turn started!");
    }
}
