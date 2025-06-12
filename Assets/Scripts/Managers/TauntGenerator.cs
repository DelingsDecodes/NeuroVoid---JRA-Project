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

    public string GenerateTaunt(int currentRound)
    {
        string taunt = "";

        var recentMoves = memoryLog.GetLastNMoves(5);
        var mostUsed = recentMoves
            .GroupBy(m => m.playerMove)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        if (mostUsed != null && mostUsed.Count() >= 3)
        {
            taunt += $"Still clinging to {mostUsed.Key}? That’s the third time... predictable.\n";
        }

        if (profile.prefersAggression && memoryLog.GetPassiveMoveRate() > 0.5f)
        {
            taunt += "Weren’t you the aggressive type? You keep holding back.\n";
        }

        if (profile.oftenBluffs && memoryLog.HasStraightPattern())
        {
            taunt += "I expected trickery… but you’ve been oddly honest lately.\n";
        }

        if (string.IsNullOrEmpty(taunt))
        {
            taunt = GetRandomDefaultTaunt();
        }

        return taunt.Trim();
    }

    private string GetRandomDefaultTaunt()
    {
        string[] genericTaunts = {
            "You think I can't see through you? MWahah",
            "Interesting move. Predictable, though.",
            "This duel is more transparent than you think.",
            "You repeat patterns even when I give you chances.",
            "You're playing checkers while I'm playing psychology.",
            "Your mind is an open book and I’ve already read the ending."
        };

        return genericTaunts[Random.Range(0, genericTaunts.Length)];
    }
}
