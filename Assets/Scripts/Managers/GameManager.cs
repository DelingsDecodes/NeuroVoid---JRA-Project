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

    public Move[] allMoves;
    private TauntGenerator tauntGenerator;

    private int currentRound = 1;
    private int totalRounds = 5;

    void Start()
    {
        Debug.Log("GameManager Start() running");

        // Check if required references are assigned
        if (uiManager == null || playerManager == null || aiManager == null ||
            memoryLog == null || questionnaireManager == null || postGameSummary == null)
        {
            Debug.LogError("GameManager: Missing references in Inspector.");
            return;
        }

        // Load moves from JSON
        allMoves = MoveLoader.LoadMoves();
        if (allMoves == null || allMoves.Length == 0)
        {
            Debug.LogError("GameManager: No moves loaded from JSON.");
            return;
        }

        // Get player profile from questionnaire
        PlayerProfile profile = questionnaireManager.GetProfile();
        aiManager.Initialize(profile);
        tauntGenerator = new TauntGenerator(profile, memoryLog);

        // Initialize UI
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

        // Generate taunt for this round
        string taunt = tauntGenerator.GenerateTaunt(currentRound);
        uiManager.DisplayAITaunt(taunt);

        // Store move history
        playerManager.AddMove(move);
        memoryLog.LogRound(currentRound, move, aiMove);

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
        uiManager.DisplayAITaunt(result);

        postGameSummary.ShowSummary(allMoves, prediction);
    }

    // Direct access for buttons
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
        // Unlock cards 
        CardAnimator[] cards = FindObjectsOfType<CardAnimator>();
        foreach (var card in cards)
        {
            card.UnlockCard();
        }
    }
}
