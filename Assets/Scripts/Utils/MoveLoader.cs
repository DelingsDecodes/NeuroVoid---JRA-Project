using UnityEngine;
using System.IO;

public static class MoveLoader
{
    public static Move[] LoadMoves()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Moves.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"MoveLoader: File not found at path: {path}");
            return new Move[0];
        }

        string json = File.ReadAllText(path);
        Move[] moves = JsonHelper.FromJson<Move>(json);

        if (moves == null || moves.Length == 0)
        {
            Debug.LogError("MoveLoader: Failed to parse or empty moves.");
        }

        return moves;
    }
}
