using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages UI interactions for Neurovoid Protocol: move buttons, round info, taunts.

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Move Buttons")]
    public Button[] moveButtons;                     // The clickable move buttons
    public TextMeshProUGUI[] moveButtonLabels;       // Text labels inside each button

    [Header("UI Text")]
    public TextMeshProUGUI aiTauntText;              // Where the AI taunts appear
    public TextMeshProUGUI roundCounterText;         // Round info display

    private Move[] availableMoves;

    public void SetAvailableMoves(Move[] moves)
{
    Debug.Log($" SetAvailableMoves called with {moves.Length} moves");

    if (moveButtons.Length != moves.Length || moveButtonLabels.Length != moves.Length)
    {
        Debug.LogError($"UIManager: Array mismatch. " +
            $"Buttons = {moveButtons.Length}, Labels = {moveButtonLabels.Length}, Moves = {moves.Length}");
        return;
    }

    availableMoves = moves;

    for (int i = 0; i < moves.Length; i++)
    {
        int index = i;

        Debug.Log($"Button {i}: Set label to {moves[i].name}");

        if (moveButtonLabels[i] != null)
        {
            moveButtonLabels[i].text = moves[i].name;
        }
        else
        {
            Debug.LogWarning($"UIManager: Missing label for button {i}.");
        }

        if (moveButtons[i] != null)
        {
            moveButtons[i].onClick.RemoveAllListeners(); // Ensure no stacking
            moveButtons[i].onClick.AddListener(() => OnMoveButtonClicked(index));
        }
        else
        {
            Debug.LogWarning($"UIManager: Missing button at index {i}.");
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
}
