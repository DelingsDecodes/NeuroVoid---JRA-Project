using System;
using UnityEngine;

[Serializable]
public class Move
{
    public string name;           // e.g., "Surge"
    public string description;    // Tooltip or hint
    public MoveType type;         // AI/psychological category
}

public enum MoveType
{
    Aggression = 0,   // Direct attack  Surge
    Control = 1,      // Disabling disruption Disrupt
    Loop = 2,         // Repetition Loop
    Reflection = 3,   // Bounce-back Fracture
    Bluff = 4         // Fake-outs Null
}
