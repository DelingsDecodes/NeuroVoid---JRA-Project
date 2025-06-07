using System.Collections.Generic;
using UnityEngine;

/// Tracks every round of the game for memory analysis and UI display.

public class MemoryLog : MonoBehaviour
{
    [System.Serializable]
    public class RoundEntry
    {
        public int roundNumber;
        public Move playerMove;
        public Move aiMove;
        public string result;  // e.g., "Player Win", "AI Win", "Draw", "Bluff Success"
    }

    public List<RoundEntry> roundHistory = new List<RoundEntry>();

    public void LogRound(int round, Move playerMove, Move aiMove, string result)
    {
        RoundEntry entry = new RoundEntry
        {
            roundNumber = round,
            playerMove = playerMove,
            aiMove = aiMove,
            result = result
        };

        roundHistory.Add(entry);

        Debug.Log($"[MemoryLog] Round {round}: Player = {playerMove.name}, AI = {aiMove.name}, Result = {result}");
    }

    public void ClearLog()
    {
        roundHistory.Clear();
    }

    public RoundEntry GetLastRound()
    {
        if (roundHistory.Count == 0) return null;
        return roundHistory[roundHistory.Count - 1];
    }

    public List<RoundEntry> GetFullLog()
    {
        return roundHistory;
    }
}
