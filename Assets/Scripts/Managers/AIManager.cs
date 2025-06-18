using System.Collections.Generic;
using UnityEngine;

// Adaptive AI that tracks player behavior and selects strategic responses.
public class AIManager : MonoBehaviour
{
    public PlayerProfile profile;
    public List<Move> playerMoveHistory;
    public Move currentAIMove;

    private int currentPhase = 1;

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
        UpdateStrategy(playerMoveHistory.Count);
    }

    public void UpdateStrategy(int round)
    {
        if (round > 3 && round <= 6) currentPhase = 2;
        else if (round > 6) currentPhase = 3;
    }

    public Move DecideMove(Move[] availableMoves)
    {
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogWarning("AIManager: No available moves provided.");
            return null;
        }

        switch (currentPhase)
        {
            case 1:
                return PredictBasedOnProfile(availableMoves);
            case 2:
                return PredictBasedOnHistory(availableMoves);
            case 3:
                return PredictFinalMove(availableMoves);
            default:
                return GetRandomMove(availableMoves);
        }
    }

    private Move PredictBasedOnProfile(Move[] availableMoves)
    {
        if (profile != null)
        {
            if (profile.prefersAggression)
            {
                return FindMove(availableMoves, MoveType.Control)
                    ?? FindMove(availableMoves, MoveType.Reflection)
                    ?? GetRandomMove(availableMoves);
            }
            else if (profile.oftenBluffs)
            {
                return FindMove(availableMoves, MoveType.Aggression)
                    ?? GetRandomMove(availableMoves);
            }
        }
        return FindMove(availableMoves, MoveType.Bluff)
            ?? GetRandomMove(availableMoves);
    }

    private Move PredictBasedOnHistory(Move[] availableMoves)
    {
        if (playerMoveHistory.Count == 0) return GetRandomMove(availableMoves);

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

    public Move PredictFinalMove(Move[] availableMoves)
    {
        if (availableMoves == null || availableMoves.Length == 0)
        {
            Debug.LogWarning("AIManager: No available moves for final prediction.");
            return null;
        }

        // 15% chance to make a mistake to humanize AI
        bool makeMistake = Random.value < 0.15f;
        if (makeMistake)
        {
            return GetRandomMove(availableMoves);
        }

        float aggressionScore = profile != null && profile.prefersAggression ? 0.8f : 0.2f;
        float unpredictabilityScore = profile != null && profile.oftenBluffs ? 0.6f : 0.3f;

        int aggressionCount = 0;
        int controlCount = 0;
        int bluffCount = 0;

        foreach (Move move in playerMoveHistory)
        {
            if (move.type == MoveType.Aggression) aggressionCount++;
            if (move.type == MoveType.Control) controlCount++;
            if (move.type == MoveType.Bluff) bluffCount++;
        }

        float scoreAggression = aggressionCount + aggressionScore + Random.Range(-0.2f, 0.2f);
        float scoreControl = controlCount + (1 - aggressionScore);
        float scoreBluff = bluffCount + unpredictabilityScore;

        if (scoreAggression >= scoreControl && scoreAggression >= scoreBluff)
            return FindMove(availableMoves, MoveType.Aggression) ?? GetRandomMove(availableMoves);
        else if (scoreControl >= scoreBluff)
            return FindMove(availableMoves, MoveType.Control) ?? GetRandomMove(availableMoves);
        else
            return FindMove(availableMoves, MoveType.Bluff) ?? GetRandomMove(availableMoves);
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
