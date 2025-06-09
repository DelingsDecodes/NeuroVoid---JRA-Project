using System;
using UnityEngine;

[Serializable]
public class PlayerProfile
{
    [Header("Psychological Traits")]
    public bool prefersAggression;      // Tends to use forceful or direct moves
    public bool prefersControl;         // Likes to control flow or disable the opponent
    public bool seeksRevenge;           // Reacts to previous AI actions
    public bool oftenBluffs;            // Misleads or acts unexpectedly
    public bool fearsPredictability;    // Tries to avoid repetition or patterns

    //  Returns a readable version of the profile
    public override string ToString()
    {
        return $"Profile -> Aggressive: {prefersAggression}, Control: {prefersControl}, " +
               $"Revenge: {seeksRevenge}, Bluff: {oftenBluffs}, Avoids Patterns: {fearsPredictability}";
    }
}
