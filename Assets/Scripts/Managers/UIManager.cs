using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages UI interactions for Neurovoid Protocol: move buttons, round info, taunts, and summary panel.
public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Move Buttons")]
    public Button[] moveButtons;
    public TextMeshProUGUI[] moveButtonLabels;

    [Header("UI Text")]
    public TextMeshProUGUI aiTauntText;
    public TextMeshProUGUI roundCounterText;

    [Header("Summary Panel")]
    public GameObject summaryPanel;
    public TextMeshProUGUI summaryText;

    private Move[] availableMoves;

    public void SetAvailableMoves(Move[] moves)
    {
        if (moves == null || moves.Length == 0)
        {
            Debug.LogError("UIManager: No moves provided to SetAvailableMoves.");
            return;
        }

        if (moveButtons.Length != moves.Length || moveButtonLabels.Length != moves.Length)
        {
            Debug.LogError("UIManager: Array size mismatch.");
            return;
        }

        availableMoves = moves;

        for (int i = 0; i < moves.Length; i++)
        {
            int index = i;

            if (moveButtonLabels[i] != null)
                moveButtonLabels[i].text = moves[i].name;

            if (moveButtons[i] != null)
            {
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => OnMoveButtonClicked(index));
            }
        }
    }

    private void OnMoveButtonClicked(int index)
    {
        if (availableMoves == null || index >= availableMoves.Length)
        {
            Debug.LogError("UIManager: Invalid move selection.");
            return;
        }

        Move selectedMove = availableMoves[index];
        gameManager.PlayerSelectedMove(selectedMove);
    }

    public void DisplayAITaunt(string message)
    {
        if (aiTauntText != null)
        {
            aiTauntText.text = message;
        }
    }

    public void UpdateRoundCounter(int round, int total)
    {
        if (roundCounterText != null)
        {
            roundCounterText.text = $"Round {round} / {total}";
        }
    }

    public void DisplaySummary(string summary)
    {
        if (summaryPanel != null && summaryText != null)
        {
            summaryPanel.SetActive(true);
            summaryText.text = summary;
        }
        else
        {
            Debug.LogWarning("UIManager: Summary UI not assigned.");
        }
    }
}
