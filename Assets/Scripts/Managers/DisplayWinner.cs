using TMPro;
using UnityEngine;

public class WinnerDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TMP_Text winnerText;

    private void Awake()
    {
        if (winnerPanel != null)
            winnerPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (turnManager != null)
            turnManager.OnGameWon += DisplayWinner;
    }

    private void OnDisable()
    {
        if (turnManager != null)
            turnManager.OnGameWon -= DisplayWinner;
    }

    private void DisplayWinner(string winnerName)
    {
        if (winnerPanel == null || winnerText == null)
        {
            Debug.LogError(
                "WinnerDisplay is missing UI references!",
                this
            );
            return;
        }

        winnerText.text = $"{winnerName} Wins!";

        winnerPanel.SetActive(true);
    }
}