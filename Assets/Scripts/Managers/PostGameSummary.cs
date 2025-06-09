using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

// Handles post-game analysis and summary display
public class PostGameSummary : MonoBehaviour
{
    public MemoryLog memoryLog;
    public PlayerProfile profile;
    public AIManager aiManager;
    public UIManager uiManager;

    public void ShowSummary(Move[] availableMoves, Move finalPrediction)
    {
        var history = memoryLog?.GetHistory();
        if (history == null || history.Count == 0)
        {
            Debug.LogWarning("PostGameSummary: No move history available for summary.");
            return;
        }

        StringBuilder summary = new StringBuilder();
        summary.AppendLine("<b>Duel Summary</b>\n");

        // 1. Trait reflection
        if (profile != null)
        {
            if (profile.prefersAggression && memoryLog.GetPassiveMoveRate() > 0.4f)
            {
                summary.AppendLine("You claimed to be aggressive, but your style was cautious.");
            }

            if (profile.oftenBluffs && memoryLog.HasStraightPattern())
            {
                summary.AppendLine("You claimed to bluff often, yet your moves followed a clear pattern.");
            }
        }

        // 2. Most used move
        var mostUsed = history.GroupBy(h => h.playerMove)
                              .OrderByDescending(g => g.Count())
                              .FirstOrDefault();
        if (mostUsed != null)
        {
            summary.AppendLine($"\nMost used move: <b>{mostUsed.Key}</b> ({mostUsed.Count()} times)");
        }

        // 3. Final prediction accuracy
        string actualLast = history.Last().playerMove;
        if (finalPrediction != null)
        {
            summary.AppendLine($"\nFinal prediction: <b>{finalPrediction.name}</b>");
            summary.AppendLine($"Your last move: <b>{actualLast}</b>");

            if (finalPrediction.name == actualLast)
            {
                summary.AppendLine("<color=green>Prediction matched your move.</color>");
            }
            else
            {
                summary.AppendLine("<color=red>Prediction missed your move.</color>");
            }
        }

        // 4. Closing remark
        summary.AppendLine("\n<i>“I learn quickly. Next time, you’ll need more than instincts.”</i>");

        // Display summary UI
        if (uiManager != null)
        {
            uiManager.DisplaySummary(summary.ToString());
        }
        else
        {
            Debug.LogWarning("PostGameSummary: UIManager reference is missing.");
        }
    }
}
