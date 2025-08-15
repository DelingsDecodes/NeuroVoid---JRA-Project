using UnityEngine;
using System.Collections.Generic;


public class GameResults : MonoBehaviour
{
    public static GameResults Instance;

    [Header("Final Moves")]
    public Move playerFinalMove;
    public Move aiFinalMove;
    public string finalTaunt = "";

    [Header("Round Management")]
    public int currentRound = 1;
    public int totalRounds = 5;
    public List<string> roundHistory = new List<string>();

    private int winCount = 0;
    private int lossCount = 0;
    private int tieCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ClearRoundMoves()
    {
        playerFinalMove = null;
        aiFinalMove = null;
        finalTaunt = "";
    }

    public void Reset()
    {
        playerFinalMove = null;
        aiFinalMove = null;
        finalTaunt = "";
        currentRound = 1;

        winCount = 0;
        lossCount = 0;
        tieCount = 0;

        roundHistory.Clear();
    }


    public void AddRoundToHistory()
    {
        if (playerFinalMove != null && aiFinalMove != null)
        {
            string player = playerFinalMove.name;
            string ai = aiFinalMove.name;
            string outcome = EvaluateOutcome(player, ai);

            if (outcome == "win") winCount++;
            else if (outcome == "loss") lossCount++;
            else tieCount++;

            string entry = $"Round {currentRound}: You - {player} | AI - {ai} → {outcome.ToUpper()}";
            roundHistory.Add(entry);
        }
    }

    private string EvaluateOutcome(string player, string ai)
    {
        if (player == ai) return "tie";

        if ((player == "Surge" && ai == "Null") ||
            (player == "Disrupt" && ai == "Surge") ||
            (player == "Loop" && ai == "Disrupt") ||
            (player == "Fracture" && ai == "Loop") ||
            (player == "Null" && ai == "Fracture"))
            return "win";

        return "loss";
    }

  
    public string GetFinalResult()
    {
        if (playerFinalMove == null || aiFinalMove == null)
            return "No Result";

        string outcome = EvaluateOutcome(playerFinalMove.name, aiFinalMove.name);
        return outcome switch
        {
            "win" => "Victory!",
            "loss" => "Defeat...",
            "tie" => "Draw",
            _ => "Unclear"
        };
    }

    public string GetSummary()
    {
        string player = playerFinalMove != null ? playerFinalMove.name : "Unknown";
        string ai = aiFinalMove != null ? aiFinalMove.name : "Unknown";
        string taunt = string.IsNullOrEmpty(finalTaunt) ? "No taunt." : $"\"{finalTaunt}\"";

        return $"Final Round:\nYou played {player}\nAI played {ai}\n\n{taunt}";
    }

    public string GetAdvancedSummary()
    {
        int total = winCount + lossCount + tieCount;
        float winRate = total > 0 ? (winCount / (float)total) * 100f : 0f;
        float tieRate = total > 0 ? (tieCount / (float)total) * 100f : 0f;

        string summary = $"<b>=== Protocol Summary ===</b>\n" +
                         $"Rounds Played: {total}\n" +
                         $"Wins: {winCount}, Losses: {lossCount}, Ties: {tieCount}\n" +
                         $"Win Rate: {winRate:F1}%\n" +
                         $"Tie Frequency: {tieRate:F1}%\n\n";

        if (!string.IsNullOrEmpty(finalTaunt))
            summary += $"<i>AI’s Final Remark:</i>\n\"{finalTaunt}\"\n\n";

        summary += ">>> Analysis Complete. Neural patterns archived. Awaiting next duel...\n";

        return summary;
    }

   
    public List<string> GetFullHistory()
    {
        return roundHistory;
    }
}
