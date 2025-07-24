using UnityEngine;

public class GameResults : MonoBehaviour
{
    public static GameResults Instance;

    public Move playerFinalMove;
    public Move aiFinalMove;
    public string finalTaunt;

    public int currentRound = 1;
    public int totalRounds = 5; 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ClearRoundMoves()
    {
        playerFinalMove = null;
        aiFinalMove = null;
        finalTaunt = "";
    }
}
