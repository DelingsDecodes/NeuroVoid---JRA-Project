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
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogWarning("AIManager: No available moves provided.");
            return null;
        }

        // EARLY GAME: use questionnaire profile to bias behavior
        if (playerMoveHistory.Count == 0)
        {
            if (profile != null)
            {
                if (profile.prefersAggression)
                {
                    // Counter aggression with control or reflection
                    return FindMove(availableMoves, MoveType.Control)
                        ?? FindMove(availableMoves, MoveType.Reflection)
                        ?? GetRandomMove(availableMoves);
                }
                else if (profile.oftenBluffs)
                {
                    // Punish bluffing with direct force
                    return FindMove(availableMoves, MoveType.Aggression)
                        ?? GetRandomMove(availableMoves);
                }
            }

            // Default: try tricking or confusing
            return FindMove(availableMoves, MoveType.Bluff)
                ?? GetRandomMove(availableMoves);
        }

        // MID GAME: React to last known move
        Move lastPlayerMove = playerMoveHistory[playerMoveHistory.Count - 1];

        // Avoid repeating the same move type as the player
        foreach (Move move in availableMoves)
        {
            if (move.type != lastPlayerMove.type)
            {
                currentAIMove = move;
                return move;
            }
        }

        // Fallback: Just return something
        currentAIMove = availableMoves[0];
        return currentAIMove;
    }

    public Move PredictFinalMove(Move[] availableMoves)
    {
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogWarning("AIManager: No available moves for final prediction.");
            return null;
        }

        // Use profile to infer possible final move
        if (profile != null && profile.oftenBluffs)
        {
            return FindMove(availableMoves, MoveType.Bluff)
                ?? GetRandomMove(availableMoves);
        }

        // Pattern recognition (simple repetition)
        if (playerMoveHistory.Count >= 2)
        {
            Move last = playerMoveHistory[playerMoveHistory.Count - 1];
            Move secondLast = playerMoveHistory[playerMoveHistory.Count - 2];

            if (last.type == secondLast.type)
                return last;
        }

        return GetRandomMove(availableMoves);
    }

    private Move FindMove(Move[] list, MoveType type)
    {
        foreach (Move move in list)
        {
            if (move.type == type)
                return move;
        }

        return null;
    }

    private Move GetRandomMove(Move[] list)
    {
        return list[Random.Range(0, list.Length)];
    }
}
