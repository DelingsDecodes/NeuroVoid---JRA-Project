using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Manages UI interactions for Neurovoid Protocol: move buttons, round info, taunts, and summary panel.
public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Move Buttons")]
    public Button[] moveButtons;
    public TextMeshProUGUI[] moveButtonLabels;

    [Header("UI Text")]
    public TextMeshProUGUI aiTauntText;
    public TextMeshProUGUI roundCounterText;

    [Header("Summary Panel")]
    public GameObject summaryPanel;
    public TextMeshProUGUI summaryText;

    [Header("AI Speech Bubble")]
    public CanvasGroup aiSpeechGroup;
    public TextMeshProUGUI aiSpeechText;

    private Move[] availableMoves;

    public void SetAvailableMoves(Move[] moves)
    {
        if (moves == null || moves.Length == 0)
        {
            Debug.LogError("UIManager: No moves provided to SetAvailableMoves.");
            return;
        }

        if (moveButtons.Length != moves.Length || moveButtonLabels.Length != moves.Length)
        {
            Debug.LogError("UIManager: Array size mismatch.");
            return;
        }

        availableMoves = moves;

        for (int i = 0; i < moves.Length; i++)
        {
            int index = i;

            if (moveButtonLabels[i] != null)
                moveButtonLabels[i].text = moves[i].name;

            if (moveButtons[i] != null)
            {
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => OnMoveButtonClicked(index));
            }
        }
    }

    private void OnMoveButtonClicked(int index)
    {
        if (availableMoves == null || index >= availableMoves.Length)
        {
            Debug.LogError("UIManager: Invalid move selection.");
            return;
        }

        Move selectedMove = availableMoves[index];
        gameManager.PlayerSelectedMove(selectedMove);
    }

    public void DisplayAITaunt(string taunt)
    {
        if (aiTauntText != null)
            aiTauntText.text = taunt;
        else
            Debug.LogWarning("UIManager: aiTauntText not assigned.");
    }

    public void ShowAITaunt(string taunt)
    {
        if (aiSpeechText == null || aiSpeechGroup == null)
        {
            Debug.LogWarning("UIManager: aiSpeechText or aiSpeechGroup not assigned.");
            return;
        }

        StopAllCoroutines();
        aiSpeechText.text = "";
        aiSpeechGroup.alpha = 0;
        aiSpeechGroup.gameObject.SetActive(true);

        LeanTween.cancel(aiSpeechGroup.gameObject);
        LeanTween.alphaCanvas(aiSpeechGroup, 1f, 0.4f).setEaseOutQuad()
            .setOnComplete(() =>
            {
                StartCoroutine(TypeText(taunt));
                LeanTween.delayedCall(aiSpeechGroup.gameObject, 4.5f, () =>
                {
                    LeanTween.alphaCanvas(aiSpeechGroup, 0f, 0.4f).setEaseInQuad();
                });
            });
    }

    private IEnumerator TypeText(string message)
    {
        aiSpeechText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            aiSpeechText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void UpdateRoundCounter(int round, int total)
    {
        if (roundCounterText != null)
        {
            roundCounterText.text = $"Round {round} / {total}";
        }
    }

    public void DisplaySummary(string summary)
    {
        if (summaryPanel != null && summaryText != null)
        {
            summaryPanel.SetActive(true);
            summaryText.text = summary;
        }
        else
        {
            Debug.LogWarning("UIManager: Summary UI not assigned.");
        }
    }
}
