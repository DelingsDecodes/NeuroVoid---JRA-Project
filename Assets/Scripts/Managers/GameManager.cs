using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

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

    private bool moveChosen = false;

    private int currentRound => GameResults.Instance.currentRound;
    private int totalRounds => GameResults.Instance.totalRounds;

    void Start()
    {
        Debug.Log("GameManager Start() running");

        moveChosen = false;

        if (uiManager == null || playerManager == null || aiManager == null ||
            memoryLog == null || questionnaireManager == null || postGameSummary == null || roundResultDisplay == null)
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

        PlayerProfile profile = questionnaireManager.GetProfile();
        aiManager.Initialize(profile);
        tauntGenerator = new TauntGenerator(profile, memoryLog);

        uiManager.SetAvailableMoves(allMoves);
        uiManager.UpdateRoundCounter(currentRound, totalRounds);
        UnlockAllCards();

      
        if (!string.IsNullOrEmpty(GameResults.Instance.finalTaunt))
        {
            Debug.Log("GameManager: Displaying AI taunt from previous round.");
            StartCoroutine(ShowDelayedTaunt(GameResults.Instance.finalTaunt));
            GameResults.Instance.finalTaunt = ""; 
        }
    }

    public void PlayerSelectedMove(Move move)
    {
        if (moveChosen)
        {
            Debug.LogWarning("GameManager: Player already chose a move this round.");
            return;
        }

        if (move == null)
        {
            Debug.LogError("GameManager: PlayerSelectedMove received a null move.");
            return;
        }

        moveChosen = true;

        Debug.Log($"Player selected: {move.name}");

        aiManager.ObservePlayerMove(move);
        Move aiMove = aiManager.DecideMove(allMoves);

        Debug.Log($"AI played: {aiMove?.name}");
        Debug.Log($"AI move artwork is null: {aiMove?.artwork == null}");

        string outcomeMessage = GetRoundOutcome(move.name, aiMove.name);
        string outcomeType = GetOutcomeType(move.name, aiMove.name);
        string dynamicTaunt = GenerateOutcomeTaunt(outcomeType);

        playerManager.AddMove(move);
        memoryLog.LogRound(currentRound, move, aiMove);

        uiManager.AppendToRoundLog($"You played {move.name}, AI played {aiMove.name}", outcomeType);
        roundResultDisplay.ShowResult(move.name, aiMove.name, outcomeMessage);

        GameResults.Instance.playerFinalMove = move;
        GameResults.Instance.aiFinalMove = aiMove;
        GameResults.Instance.finalTaunt = dynamicTaunt;

        StartCoroutine(DelayedSceneSwitch());
    }

    private IEnumerator DelayedSceneSwitch()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("FinalRevealScene");
    }

    public void SelectSurge() => PlayerSelectedMove(GetMoveByName("Surge"));
    public void SelectDisrupt() => PlayerSelectedMove(GetMoveByName("Disrupt"));
    public void SelectLoop() => PlayerSelectedMove(GetMoveByName("Loop"));
    public void SelectFracture() => PlayerSelectedMove(GetMoveByName("Fracture"));
    public void SelectNull() => PlayerSelectedMove(GetMoveByName("Null"));

    private Move GetMoveByName(string name)
    {
        var move = allMoves.FirstOrDefault(m => m.name == name);
        if (move == null) Debug.LogWarning($"Move '{name}' not found.");
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

    private string GetRoundOutcome(string player, string ai)
    {
        if (player == ai) return " A perfect clash!";
        else if (player == "Surge" && ai == "Null") return " You overwhelmed their defense!";
        else if (player == "Disrupt" && ai == "Surge") return " You intercepted them!";
        else if (player == "Loop" && ai == "Disrupt") return " You broke their pattern!";
        else if (player == "Fracture" && ai == "Loop") return " You shattered their cycle!";
        else if (player == "Null" && ai == "Fracture") return " You absorbed the blow!";
        else return " They slipped past your move...";
    }

    private string GetOutcomeType(string player, string ai)
    {
        if (player == ai) return "tie";
        else if (player == "Surge" && ai == "Null") return "win";
        else if (player == "Disrupt" && ai == "Surge") return "win";
        else if (player == "Loop" && ai == "Disrupt") return "win";
        else if (player == "Fracture" && ai == "Loop") return "win";
        else if (player == "Null" && ai == "Fracture") return "win";
        else return "loss";
    }

    private string GenerateOutcomeTaunt(string outcome)
    {
        switch (outcome)
        {
            case "win": return "You win this round... but I see through you.";
            case "loss": return "Defeated again. You should really try harder.";
            case "tie": return "A draw? You're delaying the inevitable.";
            default: return "Hmm... something unexpected.";
        }
    }

    private IEnumerator ShowDelayedTaunt(string taunt)
    {
        yield return new WaitForSeconds(1.2f);
        uiManager.ShowAITaunt(taunt);
    }
}
