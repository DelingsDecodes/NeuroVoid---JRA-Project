public class GameResults : MonoBehaviour
{
    public static GameResults Instance;

    public Move playerFinalMove;
    public Move aiFinalMove;
    public string finalTaunt = "";

    public int currentRound = 1;
    public int totalRounds = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Reset()
    {
        playerFinalMove = null;
        aiFinalMove = null;
        finalTaunt = "";
        currentRound = 1;
    }

    public string GetFinalResult()
    {
        if (playerFinalMove == null || aiFinalMove == null) return "No Result";

        string player = playerFinalMove.name;
        string ai = aiFinalMove.name;

        if (player == ai) return "Draw";
        if ((player == "Surge" && ai == "Null") ||
            (player == "Disrupt" && ai == "Surge") ||
            (player == "Loop" && ai == "Disrupt") ||
            (player == "Fracture" && ai == "Loop") ||
            (player == "Null" && ai == "Fracture"))
            return "Victory!";
        else
            return "Defeat...";
    }

    public string GetSummary()
    {
        return $"Final Round:\nYou played {playerFinalMove?.name}\n" +
               $"AI played {aiFinalMove?.name}\n\n" +
               $"\"{finalTaunt}\"";
    }
}
