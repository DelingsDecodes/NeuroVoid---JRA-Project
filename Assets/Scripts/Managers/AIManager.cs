using System.Collections.Generic;
using UnityEngine;

// Adaptive AI that tracks player behavior and selects strategic responses.
public class AIManager : MonoBehaviour
{
    public PlayerProfile profile;
    public List<Move> playerMoveHistory;
    public Move currentAIMove;

    private int currentPhase = 1; // 1 = early, 2 = mid, 3 = final

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

    private void UpdateStrategy(int round)
    {
        if (round > 3 && round <= 6)
            currentPhase = 2;
        else if (round > 6)
            currentPhase = 3;
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
            case 1: return PredictBasedOnProfile(availableMoves);
            case 2: return PredictBasedOnHistory(availableMoves);
            case 3: return PredictFinalMove(availableMoves);
            default: return GetRandomMove(availableMoves);
        }
    }

    private Move PredictBasedOnProfile(Move[] availableMoves)
    {
        if (profile == null) return GetRandomMove(availableMoves);

        if (profile.prefersAggression)
        {
            // Counter aggression with control or reflection
            return FindMove(availableMoves, MoveType.Control)
                ?? FindMove(availableMoves, MoveType.Reflection)
                ?? GetRandomMove(availableMoves);
        }

        if (profile.oftenBluffs)
        {
            // Counter bluffing with aggressive pressure
            return FindMove(availableMoves, MoveType.Aggression)
                ?? GetRandomMove(availableMoves);
        }

        // Default profile counter
        return FindMove(availableMoves, MoveType.Bluff)
            ?? GetRandomMove(availableMoves);
    }

    private Move PredictBasedOnHistory(Move[] availableMoves)
    {
        if (playerMoveHistory.Count == 0)
            return GetRandomMove(availableMoves);

        Move lastPlayerMove = playerMoveHistory[playerMoveHistory.Count - 1];

        // Try choosing something different than what the player just did
        foreach (Move move in availableMoves)
        {
            if (move.type != lastPlayerMove.type)
            {
                currentAIMove = move;
                return move;
            }
        }

        // Fallback: just pick first option
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

        // 15% chance AI makes a mistake to seem more human
        if (Random.value < 0.15f)
            return GetRandomMove(availableMoves);

        // Influence scores based on profile
        float aggressionBias = profile?.prefersAggression == true ? 0.8f : 0.2f;
        float bluffBias = profile?.oftenBluffs == true ? 0.6f : 0.3f;

        // Count player move history
        int aggressionCount = 0, controlCount = 0, bluffCount = 0;

        foreach (var move in playerMoveHistory)
        {
            switch (move.type)
            {
                case MoveType.Aggression: aggressionCount++; break;
                case MoveType.Control: controlCount++; break;
                case MoveType.Bluff: bluffCount++; break;
            }
        }

        float scoreAggression = aggressionCount + aggressionBias + Random.Range(-0.2f, 0.2f);
        float scoreControl = controlCount + (1 - aggressionBias);
        float scoreBluff = bluffCount + bluffBias;

        // Choose move with highest score
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
