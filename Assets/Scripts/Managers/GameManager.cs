using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public PlayerManager playerManager;
    public AIManager aiManager;
    public MemoryLog memoryLog;

    public Move[] allMoves;

    private int currentRound = 1;
    private int totalRounds = 5;

    private TauntGenerator tauntGenerator;

    void Start()
    {
        Debug.Log("GameManager Start() running");

        if (uiManager == null || playerManager == null || aiManager == null || memoryLog == null)
        {
            Debug.LogError("GameManager: Missing references in Inspector.");
            return;
        }

        allMoves = MoveLoader.LoadMoves();

        if (allMoves == null || allMoves.Length == 0)
        {
            Debug.LogError("GameManager: No moves loaded from JSON.");
            return;
        }

        PlayerProfile profile = new PlayerProfile
        {
            prefersAggression = false,
            oftenBluffs = false
        };

        aiManager.Initialize(profile);
        tauntGenerator = new TauntGenerator(profile, memoryLog); // Initialize taunt system

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

        aiManager.ObservePlayerMove(move);
        Move aiMove = aiManager.DecideMove(allMoves);
        Debug.Log($"AI played: {aiMove.name}");

        // Use dynamic taunt instead of static string
        string taunt = tauntGenerator.GenerateTaunt(currentRound);
        uiManager.DisplayAITaunt(taunt);

        playerManager.AddMove(move);
        memoryLog.LogRound(currentRound, move, aiMove);

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

        Move prediction = aiManager.PredictFinalMove(allMoves);
        string result = $"Game over. My final prediction is: {prediction.name}";
        uiManager.DisplayAITaunt(result);
    }
}
