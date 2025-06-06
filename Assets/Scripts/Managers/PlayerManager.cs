using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Move> moveHistory = new List<Move>();  // Stores all past moves
    public Move currentMove;                           // Current selected move

    //Call this when the player selects a move (e.g. by clicking a button).

    public void SelectMove(Move selectedMove)
    {
        currentMove = selectedMove;
        moveHistory.Add(selectedMove);

        Debug.Log("Player selected move: " + selectedMove.name);
    }


    // Returns the last move the player made (null if none).
  
    public Move GetLastMove()
    {
        if (moveHistory.Count == 0)
            return null;

        return moveHistory[moveHistory.Count - 1];
    }

    // Clears all move history (for a new game).

    public void ResetHistory()
    {
        moveHistory.Clear();
        currentMove = null;
    }
}