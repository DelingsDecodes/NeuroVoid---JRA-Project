using System.Collections.Generic;
using UnityEngine;

// Handles player's data and move history.
public class PlayerManager : MonoBehaviour
{
    public List<Move> moveHistory = new List<Move>();

    // Adds a player move to the history
    public void AddMove(Move move)
    {
        moveHistory.Add(move);
        Debug.Log($" Player move recorded: {move.name}");
    }

    // Optional: Returns the last move made
    public Move GetLastMove()
    {
        if (moveHistory.Count > 0)
            return moveHistory[moveHistory.Count - 1];
        return null;
    }

    // Clears history (e.g. for restart)
    public void ResetHistory()
    {
        moveHistory.Clear();
    }
}
