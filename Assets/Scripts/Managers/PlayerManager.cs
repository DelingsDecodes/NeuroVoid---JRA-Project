using System.Collections.Generic;
using UnityEngine;

// Tracks all player moves throughout the game.
public class PlayerManager : MonoBehaviour
{
    private List<Move> playerMoves = new List<Move>();

    // Adds a move to the player's history
    public void AddMove(Move move)
    {
        playerMoves.Add(move);
    }

    // Returns all player moves
    public List<Move> GetPlayerMoves()
    {
        return playerMoves;
    }

    // Returns the last move played by the player
    public Move GetLastPlayerMove()
    {
        if (playerMoves.Count == 0)
        {
            Debug.LogWarning("PlayerManager: No player moves recorded.");
            return null;
        }

        return playerMoves[playerMoves.Count - 1];
    }

    // Clears history (optional, useful for scene resets or replays)
    public void ClearMoves()
    {
        playerMoves.Clear();
    }
}
