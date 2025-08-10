using System;
using UnityEngine;

//currently Doesnt work
[Serializable]
public class Move
{
    public string name;            
    public string description;     
    public MoveType type;          

    [NonSerialized]
    public Sprite artwork;         
}

public enum MoveType
{
    Aggression,    // For moves like "Surge"
    Control,       // For moves like "Disrupt"
    Bluff,         // For moves like "Loop"
    Reflection,    // For moves like "Fracture"
    Defense        // For moves like "Null"
}
