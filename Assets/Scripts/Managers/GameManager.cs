using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public PlayerManager playerManager;
    public AIManager aiManager;
    public MemoryLog memoryLog;
    public QuestionnaireManager questionnaireManager;
    public PostGameSummary postGameSummary;
    public RoundResultDisplay roundResultDisplay; 

    public Move[] allMoves;
    private TauntGenerator tauntGenerator;

    private int currentRound = 1;
    private int totalRounds = 5;

    void Start()
    {
        Debug.Log("GameManager Start() running");

        // Check all references
        if (uiManager == null || playerManager == null || aiManager == null ||
            memoryLog == null || questionnaireManager == null || postGameSummary == null || roundResultDisplay == null)
        {
            Debug.LogError("GameManager: Missing references in Inspector.");
            return;
        }

        // Load moves
        allMoves = MoveLoader.LoadMoves();
        if (allMoves == null || allMoves.Length == 0)
        {
            Debug.LogError("GameManager: No moves loaded from JSON.");
            return;
        }

        // Setup AI and taunts
        PlayerProfile profile = questionnaireManager.GetProfile();
        aiManager.Initialize(profile);
        tauntGenerator = new TauntGenerator(profile, memoryLog);

        // UI setup
        uiManager.SetAvailableMoves(allMoves);
        uiManager.UpdateRoundCounter(currentRound, totalRounds);
        UnlockAllCards();
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

        // Show taunt in pixel bubble
        string taunt = tauntGenerator.GenerateTaunt(currentRound);
        uiManager.ShowAITaunt(taunt);  

        // Log round
        playerManager.AddMove(move);
        memoryLog.LogRound(currentRound, move, aiMove);

        // Outcome message
        string outcome = GetRoundOutcome(move.name, aiMove.name);
        roundResultDisplay.ShowResult(move.name, aiMove.name, outcome);

        currentRound++;
        if (currentRound <= totalRounds)
        {
            uiManager.UpdateRoundCounter(currentRound, totalRounds);
            UnlockAllCards();
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
        uiManager.ShowAITaunt(result); // 🔄 Replaced with bubble fade-in

        postGameSummary.ShowSummary(allMoves, prediction);
    }

    // Shortcut methods for buttons
    public void SelectSurge() => PlayerSelectedMove(GetMoveByName("Surge"));
    public void SelectDisrupt() => PlayerSelectedMove(GetMoveByName("Disrupt"));
    public void SelectLoop() => PlayerSelectedMove(GetMoveByName("Loop"));
    public void SelectFracture() => PlayerSelectedMove(GetMoveByName("Fracture"));
    public void SelectNull() => PlayerSelectedMove(GetMoveByName("Null"));

    private Move GetMoveByName(string name)
    {
        var move = allMoves.FirstOrDefault(m => m.name == name);
        if (move == null) Debug.LogWarning($"Move '{name}' not found in Move list.");
        return move;
    }

    private void UnlockAllCards()
    {
        CardAnimator[] cards = FindObjectsOfType<CardAnimator>();
        foreach (var card in cards)
        {
            card.UnlockCard();
        }
    }

    // Determine round outcome description
    private string GetRoundOutcome(string player, string ai)
    {
        if (player == ai)
            return " A perfect clash!";
        else if (player == "Surge" && ai == "Null") return " You overwhelmed their defense!";
        else if (player == "Disrupt" && ai == "Surge") return " You intercepted them!";
        else if (player == "Loop" && ai == "Disrupt") return " You broke their pattern!";
        else if (player == "Fracture" && ai == "Loop") return " You shattered their cycle!";
        else if (player == "Null" && ai == "Fracture") return " You absorbed the blow!";
        else return " They slipped past your move...";
    }
}
