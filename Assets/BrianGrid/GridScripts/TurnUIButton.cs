using UnityEngine;
using UnityEngine.UI;

public class TurnUIButton : MonoBehaviour
{
    private Button button;
    private TurnManager turnManager;

    void Start()
    {
        button = GetComponent<Button>();
        turnManager = FindObjectOfType<TurnManager>();

        // Update button immediately
        UpdateButtonState();
    }

    void Update()
    {
        // Auto disable during enemy turn
        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        if (turnManager == null || button == null) return;

        // Only clickable on player's turn
        button.interactable = turnManager.IsPlayerTurn;
    }
}
