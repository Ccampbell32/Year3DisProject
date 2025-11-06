using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnUIManager : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    public Button endTurnButton;

    private TurnManager turnManager;

    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(OnEndTurnClicked);

        UpdateTurnDisplay();
    }

    public void UpdateTurnDisplay()
    {
        if (turnManager == null) return;

        if (turnManager.IsPlayerTurn)
            turnText.text = "Player Turn";
        else
            turnText.text = "Enemy Turn";

        // Disable button on enemy turn
        endTurnButton.interactable = turnManager.IsPlayerTurn;
    }

    void OnEndTurnClicked()
    {
        if (turnManager != null && turnManager.IsPlayerTurn)
        {
            Debug.Log("Player ended turn manually.");
            turnManager.EndPlayerTurn();
            UpdateTurnDisplay();
        }
    }
}
