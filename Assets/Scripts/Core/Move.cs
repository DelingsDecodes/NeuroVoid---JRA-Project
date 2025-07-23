using System;
using UnityEngine;

using System;
using UnityEngine;

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
    Aggression = 0,    // Direct attack (Surge)
    Control = 1,       // Disruption (Disrupt)
    Loop = 2,          // Repetition (Loop)
    Reflection = 3,    // Bounce-back (Fracture)
    Bluff = 4          // Fake-out (Null)
}
