using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TauntGenerator
{
    private PlayerProfile profile;
    private MemoryLog memoryLog;

    public TauntGenerator(PlayerProfile profile, MemoryLog memoryLog)
    {
        this.profile = profile;
        this.memoryLog = memoryLog;
    }

    // Call this every round to get a smart remark from the AI
    public string GenerateTaunt(int currentRound)
    {
        string taunt = "";

        // Example 1: Detect repetitive behavior
        var recentMoves = memoryLog.GetLastNMoves(5); 
        var mostUsed = recentMoves
            .GroupBy(m => m.playerMove)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        if (mostUsed != null && mostUsed.Count() >= 3)
        {
            taunt += $"Still clinging to {mostUsed.Key}? That’s the third time... predictable.\n";
        }

        // Example 2: Profile contradiction
        if (profile.prefersAggression && memoryLog.GetPassiveMoveRate() > 0.5f)
        {
            taunt += "Weren’t you the aggressive type? You keep holding back.\n";
        }

        // Example 3: Bluff detection
        if (profile.oftenBluffs && memoryLog.HasStraightPattern())
        {
            taunt += "I expected trickery… but you’ve been oddly honest lately.\n";
        }

        // Fallback generic remark
        if (string.IsNullOrEmpty(taunt))
        {
            taunt = GetRandomDefaultTaunt();
        }

        return taunt.Trim();
    }

    private string GetRandomDefaultTaunt()
    {
        string[] genericTaunts = {
            "You think I can't see through you?",
            "Interesting move. Predictable, though.",
            "This duel is more transparent than you think."
        };

        return genericTaunts[Random.Range(0, genericTaunts.Length)];
    }
}
