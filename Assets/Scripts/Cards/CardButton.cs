using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public string moveName; // e.g., "Surge", "Loop", etc.

    public void OnCardClick()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            foreach (Move move in gm.allMoves)
            {
                if (move.name == moveName)
                {
                    gm.PlayerSelectedMove(move);
                    return;
                }
            }
            Debug.LogWarning("Move not found: " + moveName);
        }
        else
        {
            Debug.LogError("GameManager not found in scene.");
        }
    }
}
