using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move
{
    public string name;           // e.g., "Surge"
    public string description;    // Tooltip or hint
    public MoveType type;         // Attack, Defense, etc.
}

public enum MoveType
{
    Attack,
    Defense,
    Repeat,
    Mirror,
    Bluff
}