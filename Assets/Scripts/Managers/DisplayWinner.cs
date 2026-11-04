
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerDisplay : MonoBehaviour
{
    [System.Serializable]
    private class PlayerWinnerImage
    {
        public string playerName;
        public Sprite winnerSprite;
    }

    [Header("References")]
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private Image winnerImage;

    [Header("Player Winner Images")]
    [SerializeField] private PlayerWinnerImage[] playerWinnerImages;

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
        if (winnerPanel == null || winnerImage == null)
        {
            Debug.LogError("WinnerDisplay is missing UI references!", this);
            return;
        }

        // Find the winning player's image.
        foreach (PlayerWinnerImage player in playerWinnerImages)
        {
            if (player.playerName == winnerName)
            {
                winnerImage.sprite = player.winnerSprite;
                winnerImage.enabled = true;
                winnerPanel.SetActive(true);
                return;
            }
        }

        Debug.LogWarning($"No winner image found for {winnerName}!", this);

        winnerPanel.SetActive(true);
    }
}
