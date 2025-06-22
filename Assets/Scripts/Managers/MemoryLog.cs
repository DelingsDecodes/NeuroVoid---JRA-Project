using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemoryLog : MonoBehaviour
{
    // A single round's player/AI moves and outcome
    public class RoundRecord
    {
        public int roundNumber;
        public string playerMove;
        public string aiMove;
        public string outcome;
    }

    // Complete move history
    private List<RoundRecord> history = new List<RoundRecord>();

    // Log a round's result
    public void LogRound(int round, Move playerMove, Move aiMove, string outcome = "")
    {
        if (playerMove == null || aiMove == null)
        {
            Debug.LogWarning("MemoryLog: Null move detected while logging.");
            return;
        }

        RoundRecord record = new RoundRecord
        {
            roundNumber = round,
            playerMove = playerMove.name,
            aiMove = aiMove.name,
            outcome = outcome
        };

        history.Add(record);
        Debug.Log($"[MemoryLog] Round {round}: Player - {record.playerMove}, AI - {record.aiMove}, Outcome - {outcome}");
    }


    public List<RoundRecord> GetHistory()
    {
        return new List<RoundRecord>(history); // return a copy to prevent modification
    }

    // Clear all logged rounds
    public void ClearLog()
    {
        history.Clear();
    }

    // Get the last N rounds
    public List<RoundRecord> GetLastNMoves(int n)
    {
        int count = Mathf.Clamp(n, 0, history.Count);
        return history.Skip(history.Count - count).ToList();
    }

    // Estimate how often passive moves were used
    public float GetPassiveMoveRate()
    {
        if (history.Count == 0) return 0f;

        int passiveCount = history.Count(r =>
            r.playerMove == "Loop" || r.playerMove == "Null");

        return (float)passiveCount / history.Count;
    }

    // Check if the same move was used 3 times in a row
    public bool HasStraightPattern()
    {
        if (history.Count < 3) return false;

        var last3 = GetLastNMoves(3);
        string firstMove = last3[0].playerMove;

        return last3.All(r => r.playerMove == firstMove);
    }
}
