using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Linq;
using System.Text;

public class PostGameSummary : MonoBehaviour
{
    public MemoryLog memoryLog;
    public PlayerProfile profile;
    public AIManager aiManager;
    public UIManager uiManager;

    public void ShowSummary(Move[] availableMoves, Move finalPrediction)
    {
        var history = memoryLog.GetHistory();
        if (history == null || history.Count == 0)
        {
            Debug.LogWarning("PostGameSummary: No history to analyze.");
            return;
        }

        StringBuilder summary = new StringBuilder();
        summary.AppendLine("<b> Duel Summary</b>\n");

        // 1. Trait reflection
        if (profile.prefersAggression && memoryLog.GetPassiveMoveRate() > 0.4f)
        {
            summary.AppendLine(" You claimed to be aggressive, but you played passively.\n");
        }
        if (profile.oftenBluffs && memoryLog.HasStraightPattern())
        {
            summary.AppendLine(" You said you bluff, but you played very predictably.\n");
        }

        // 2. Most used move
        var mostUsed = history.GroupBy(h => h.playerMove)
                              .OrderByDescending(g => g.Count())
                              .FirstOrDefault();
        if (mostUsed != null)
        {
            summary.AppendLine($" Most used move: <b>{mostUsed.Key}</b> ({mostUsed.Count()} times)");
        }

        // 3. AI prediction vs actual last move
        string actualLast = history.Last().playerMove;
        if (finalPrediction != null)
        {
            summary.AppendLine($"\n My final prediction: <b>{finalPrediction.name}</b>");
            summary.AppendLine($" Actual last move: <b>{actualLast}</b>");

            if (finalPrediction.name == actualLast)
            {
                summary.AppendLine("<color=green> Prediction was correct.</color>");
            }
            else
            {
                summary.AppendLine("<color=red> Prediction was incorrect.</color>");
            }
        }

        // 4. Closing AI remark
        summary.AppendLine("\n<i>“I learn quickly. Next time, you'll need more than instincts.”</i>");

        // Send to UI
        uiManager.DisplaySummary(summary.ToString());
    }
}
