using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Adaptive AI that tracks player behavior and selects strategic responses.

public class AIManager : MonoBehaviour
{
    public PlayerProfile profile;
    public List<Move> playerMoveHistory;
    public Move currentAIMove;

    public void Initialize(PlayerProfile profileData)
    {
        profile = profileData;
        playerMoveHistory = new List<Move>();
    }

    public void ObservePlayerMove(Move move)
    {
        if (playerMoveHistory == null)
            playerMoveHistory = new List<Move>();

        playerMoveHistory.Add(move);
    }

    public Move DecideMove(Move[] availableMoves)
    {
        if (playerMoveHistory.Count == 0)
        {
            if (profile != null && profile.prefersAggression)
                return FindMove(availableMoves, MoveType.Defense);
            else
                return FindMove(availableMoves, MoveType.Attack);
        }
        else
        {
            Move lastPlayerMove = playerMoveHistory[playerMoveHistory.Count - 1];

            foreach (Move move in availableMoves)
            {
                if (move.type != lastPlayerMove.type)
                {
                    currentAIMove = move;
                    return move;
                }
            }

            currentAIMove = availableMoves[0];
            return currentAIMove;
        }
    }

    public Move PredictFinalMove(Move[] availableMoves)
    {
        if (profile != null && profile.oftenBluffs)
            return FindMove(availableMoves, MoveType.Bluff);

        if (playerMoveHistory.Count >= 2)
        {
            Move last = playerMoveHistory[playerMoveHistory.Count - 1];
            Move secondLast = playerMoveHistory[playerMoveHistory.Count - 2];

            if (last.type == secondLast.type)
                return last;
        }

        return availableMoves[Random.Range(0, availableMoves.Length)];
    }

    private Move FindMove(Move[] list, MoveType type)
    {
        foreach (Move move in list)
        {
            if (move.type == type)
                return move;
        }

        return list[0];
    }
}
