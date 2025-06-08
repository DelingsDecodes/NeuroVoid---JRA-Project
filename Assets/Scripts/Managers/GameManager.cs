using System.Collections;
using UnityEngine;

// Central game loop manager for Neurovoid Protocol.
// Manages turns, win tracking, and the final showdown.

public class GameManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public AIManager aiManager;

    public Move[] allMoves;          // Loaded externally (e.g., from MoveLoader)
    public int currentRound = 0;
    public int maxRounds = 5;

    public int playerWins = 0;
    public int aiWins = 0;

    public bool gameEnded = false;

    void Start()
{
    Debug.Log("GameManager Start() running");

    currentRound = 0;
    playerWins = 0;
    aiWins = 0;
    gameEnded = false;

    // TEMP: Hardcoded moves for testing
    allMoves = new Move[]
    {
        new Move { name = "Surge", description = "Attack", type = MoveType.Attack },
        new Move { name = "Disrupt", description = "Defend", type = MoveType.Defense },
        new Move { name = "Loop", description = "Repeat", type = MoveType.Repeat },
        new Move { name = "Fracture", description = "Mirror", type = MoveType.Mirror },
        new Move { name = "Null", description = "Bluff", type = MoveType.Bluff }
    };

    // Set the buttons
    FindObjectOfType<UIManager>().SetAvailableMoves(allMoves);

    // Simulate profile 
    PlayerProfile tempProfile = new PlayerProfile
    {
        prefersAggression = true,
        prefersControl = false,
        seeksRevenge = false,
        oftenBluffs = false,
        fearsPredictability = false
    };

    FindObjectOfType<AIManager>().Initialize(tempProfile);
}


    // Called when the player selects a move via UI.

    public void PlayerSelectedMove(Move selectedMove)
    {
        if (gameEnded || currentRound >= maxRounds) return;

        playerManager.SelectMove(selectedMove);
        aiManager.ObservePlayerMove(selectedMove);

        Move aiMove = aiManager.DecideMove(allMoves);

        Debug.Log($"Round {currentRound + 1}: Player = {selectedMove.name}, AI = {aiMove.name}");

        ResolveRound(selectedMove, aiMove);
        currentRound++;

        if (currentRound >= maxRounds)
        {
            FinalShowdown();
        }
    }

 
    // Compare moves to determine round winner (basic logic for now).
    
    private void ResolveRound(Move playerMove, Move aiMove)
    {
        if (playerMove.type == MoveType.Bluff && aiMove.type != MoveType.Bluff)
        {
            playerWins++;
            Debug.Log("Player outsmarted AI with a bluff.");
        }
        else if (aiMove.type == MoveType.Bluff && playerMove.type != MoveType.Bluff)
        {
            aiWins++;
            Debug.Log("AI bluffed and won.");
        }
        else if (playerMove.type != aiMove.type)
        {
            playerWins++;
            Debug.Log("Player wins round by mismatch.");
        }
        else
        {
            aiWins++;
            Debug.Log("AI wins on tie logic.");
        }
    }

    /// Final round — AI predicts your move and anticipate it aswell

    private void FinalShowdown()
    {
        Move predicted = aiManager.PredictFinalMove(allMoves);
        Debug.Log("AI predicts you'll play: " + predicted.name);

        Move actual = playerManager.GetLastMove();

        if (actual != null && predicted.type == actual.type)
        {
            Debug.Log("AI correctly predicted your final move!");
            aiWins++;
        }
        else
        {
            Debug.Log("You defied the AI's prediction.");
            playerWins++;
        }

        gameEnded = true;
        EndGame();
    }

    private void EndGame()
    {
        Debug.Log($"Game Over. Player Wins: {playerWins}, AI Wins: {aiWins}");

        if (playerWins > aiWins)
        {
            Debug.Log("You broke the AI. You win.");
        }
        else
        {
            Debug.Log("The AI understood you too well. You’re trapped.");
        }
    }
}
