using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MemoryLog : MonoBehaviour
{
    public class RoundRecord
    {
        public int roundNumber;
        public string playerMove;
        public string aiMove;
        public string outcome; // e.g., "Win", "Lose", "Draw"
    }

    private List<RoundRecord> history = new List<RoundRecord>();

    public void LogRound(int round, Move playerMove, Move aiMove, string outcome = "")
{
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
        return history;
    }

    public void ClearLog()
    {
        history.Clear();
    }

    //  Return the last N moves from history
    public List<RoundRecord> GetLastNMoves(int n)
    {
        return history.Skip(Mathf.Max(0, history.Count - n)).ToList();
    }

    // Estimate how often passive moves were used
    public float GetPassiveMoveRate()
    {
        int passiveCount = history.Count(r => r.playerMove == "Loop" || r.playerMove == "Null");
        return history.Count == 0 ? 0f : (float)passiveCount / history.Count;
    }

    // Detect same move used 3 times in a row
    public bool HasStraightPattern()
    {
        if (history.Count < 3) return false;

        var last3 = GetLastNMoves(3);
        return last3.All(r => r.playerMove == last3[0].playerMove);
    }
}
