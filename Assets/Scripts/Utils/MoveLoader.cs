using System.IO;
using UnityEngine;

public static class MoveLoader
{
    public static Move[] LoadMoves()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Moves.json");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"MoveLoader: File not found at path: {filePath}");
            return null;
        }

        string json = File.ReadAllText(filePath);
        Move[] moves = JsonHelper.FromJson<Move>(json);

        if (moves == null || moves.Length == 0)
        {
            Debug.LogError("MoveLoader: Failed to parse or no moves found.");
            return null;
        }

        foreach (var move in moves)
        {
            move.artwork = Resources.Load<Sprite>("CardArt/" + move.name);
            if (move.artwork == null)
            {
                Debug.LogWarning($"MoveLoader: No sprite found for {move.name} in Resources/CardArt/");
            }
        }

        return moves;
    }
}

    
