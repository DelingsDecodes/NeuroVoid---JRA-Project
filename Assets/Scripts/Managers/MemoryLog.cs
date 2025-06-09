using System.Collections.Generic;
using UnityEngine;

// Logs round history for potential AI learning or UI display
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
}
