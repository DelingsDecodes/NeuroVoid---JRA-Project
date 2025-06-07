using UnityEngine;
using UnityEngine.UI;


// Manages UI interactions for Neurovoid Protocol: move buttons, round info, taunts.

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    public Button[] moveButtons; // 5 buttons for Surge, Disrupt (cards)
    public Text aiTauntText;
    public Text roundCounterText;

    private Move[] availableMoves;

    public void SetAvailableMoves(Move[] moves)
    {
        availableMoves = moves;

        // Attach each move to a button
        for (int i = 0; i < moveButtons.Length && i < moves.Length; i++)
        {
            int index = i; // Prevent closure bug
            moveButtons[i].GetComponentInChildren<Text>().text = moves[i].name;
            moveButtons[i].onClick.AddListener(() => OnMoveButtonClicked(index));
        }
    }

    private void OnMoveButtonClicked(int index)
    {
        Move selectedMove = availableMoves[index];
        gameManager.PlayerSelectedMove(selectedMove);
    }

    public void DisplayAITaunt(string message)
    {
        aiTauntText.text = message;
    }

    public void UpdateRoundCounter(int round, int total)
    {
        roundCounterText.text = $"Round {round} / {total}";
    }
}
