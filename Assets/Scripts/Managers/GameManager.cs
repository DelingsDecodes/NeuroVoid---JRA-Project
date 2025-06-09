using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public PlayerManager playerManager;
    public AIManager aiManager;

    public Move[] allMoves;  // Reference to available moves

    private int currentRound = 1;
    private int totalRounds = 5;

    void Start()
{
    Debug.Log("GameManager Start() running");

    if (uiManager == null || playerManager == null || aiManager == null)
    {
        Debug.LogError("GameManager: Missing references in Inspector.");
        return;
    }

    // Temporary mockup for demo/testing
    if (allMoves == null || allMoves.Length == 0)
    {
        allMoves = new Move[5];
        allMoves[0] = new Move { name = "Surge", type = MoveType.Attack };
        allMoves[1] = new Move { name = "Disrupt", type = MoveType.Defense };
        allMoves[2] = new Move { name = "Loop", type = MoveType.Repeat };
        allMoves[3] = new Move { name = "Fracture", type = MoveType.Mirror };
        allMoves[4] = new Move { name = "Null", type = MoveType.Bluff };
    }

    PlayerProfile profile = new PlayerProfile
    {
        prefersAggression = false,
        oftenBluffs = false
    };
    aiManager.Initialize(profile);

    uiManager.SetAvailableMoves(allMoves);
    uiManager.UpdateRoundCounter(currentRound, totalRounds);
}


    public void PlayerSelectedMove(Move move)
    {
        if (move == null)
        {
            Debug.LogError("GameManager: PlayerSelectedMove received a null move.");
            return;
        }

        Debug.Log($"Player selected: {move.name}");

        // AI responds
        aiManager.ObservePlayerMove(move);
        Move aiMove = aiManager.DecideMove(allMoves);
        Debug.Log($"AI played: {aiMove.name}");

        // Show taunt (simple placeholder)
        string taunt = $"You played {move.name}. I respond with {aiMove.name}.";
        uiManager.DisplayAITaunt(taunt);

        // Save move history
        playerManager.AddMove(move);

        // Advance round
        currentRound++;
        if (currentRound <= totalRounds)
        {
            uiManager.UpdateRoundCounter(currentRound, totalRounds);
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over");

        // Final prediction
        Move prediction = aiManager.PredictFinalMove(allMoves);
        string result = $"Game over. My final prediction is: {prediction.name}";
        uiManager.DisplayAITaunt(result);

        // You can extend this to load a results screen or restart the game
    }
}
