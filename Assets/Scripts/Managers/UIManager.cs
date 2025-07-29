using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Manages UI for Neurovoid Protocol: move buttons, round info, taunts, and summary.
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("UI Container")]
    public GameObject mainUIContainer;

    [Header("Move Buttons")]
    public Button[] moveButtons;
    public TextMeshProUGUI[] moveButtonLabels;

    [Header("UI Text")]
    public TextMeshProUGUI aiTauntText;
    public TextMeshProUGUI roundCounterText;
    public TextMeshProUGUI roundLogText;
    public ScrollRect roundLogScrollRect;

    [Header("Summary Panel")]
    public GameObject summaryPanel;
    public TextMeshProUGUI summaryText;

    [Header("AI Speech Bubble")]
    public CanvasGroup aiSpeechGroup;
    public TextMeshProUGUI aiSpeechText;

    [Header("Player Speech Bubble")]
    public CanvasGroup playerSpeechGroup;
    public TextMeshProUGUI playerSpeechText;

    private Move[] availableMoves;
    private bool buttonClicked = false;

    public void SetAvailableMoves(Move[] moves)
    {
        if (moves == null || moves.Length == 0)
        {
            Debug.LogError("UIManager: No moves provided.");
            return;
        }

        if (moveButtons.Length != moves.Length || moveButtonLabels.Length != moves.Length)
        {
            Debug.LogError("UIManager: Button or label array length mismatch.");
            return;
        }

        availableMoves = moves;
        buttonClicked = false;

        for (int i = 0; i < moves.Length; i++)
        {
            int index = i;

            if (moveButtonLabels[i] != null)
                moveButtonLabels[i].text = moves[i].name;

            if (moveButtons[i] != null)
            {
                moveButtons[i].interactable = true;
                moveButtons[i].onClick.RemoveAllListeners();
                moveButtons[i].onClick.AddListener(() => OnMoveButtonClicked(index));
            }
        }
    }

    private void OnMoveButtonClicked(int index)
    {
        if (availableMoves == null || index >= availableMoves.Length)
        {
            Debug.LogError("UIManager: Invalid move button index.");
            return;
        }

        if (buttonClicked) return;

        buttonClicked = true;
        DisableAllMoveButtons();
        gameManager.PlayerSelectedMove(availableMoves[index]);
    }

    private void DisableAllMoveButtons()
    {
        foreach (var btn in moveButtons)
        {
            if (btn != null)
                btn.interactable = false;
        }
    }

    public void ShowAITaunt(string taunt)
    {
        if (aiSpeechText == null || aiSpeechGroup == null)
        {
            Debug.LogWarning("UIManager: AI speech UI missing.");
            return;
        }

        StopAllCoroutines();
        aiSpeechText.text = "";
        aiSpeechGroup.alpha = 0;
        aiSpeechGroup.gameObject.SetActive(true);

        LeanTween.cancel(aiSpeechGroup.gameObject);
        LeanTween.alphaCanvas(aiSpeechGroup, 1f, 0.4f).setEaseOutQuad().setOnComplete(() =>
        {
            StartCoroutine(TypeText(aiSpeechText, taunt, 0.02f));
            LeanTween.delayedCall(aiSpeechGroup.gameObject, 4.5f, () =>
            {
                LeanTween.alphaCanvas(aiSpeechGroup, 0f, 0.4f).setEaseInQuad();
            });
        });
    }

    public void ShowPlayerSpeech(string text)
    {
        if (playerSpeechText == null || playerSpeechGroup == null)
        {
            Debug.LogWarning("UIManager: Player speech UI missing.");
            return;
        }

        playerSpeechText.text = "";
        playerSpeechGroup.alpha = 0;
        playerSpeechGroup.gameObject.SetActive(true);

        StartCoroutine(TypeText(playerSpeechText, text, 0.03f));
        LeanTween.alphaCanvas(playerSpeechGroup, 1f, 0.4f).setEaseOutQuad().setOnComplete(() =>
        {
            LeanTween.delayedCall(playerSpeechGroup.gameObject, 4.5f, () =>
            {
                LeanTween.alphaCanvas(playerSpeechGroup, 0f, 0.4f).setEaseInQuad();
            });
        });
    }

    private IEnumerator TypeText(TextMeshProUGUI textComponent, string message, float delay)
    {
        textComponent.text = "";
        foreach (char c in message)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(delay);
        }
    }

    public void DisplayAITaunt(string taunt)
    {
        if (aiTauntText != null)
            aiTauntText.text = taunt;
        else
            Debug.LogWarning("UIManager: aiTauntText not assigned.");
    }

    public void UpdateRoundCounter(int round, int total)
    {
        if (roundCounterText != null)
            roundCounterText.text = $"Round {round} / {total}";
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
            Debug.LogWarning("UIManager: Summary panel or text not assigned.");
        }
    }

    public void AppendToRoundLog(string roundInfo, string outcome)
    {
        if (roundLogText == null) return;

        string colorTag = outcome switch
        {
            "win" => "<color=green>",
            "loss" => "<color=red>",
            "tie" => "<color=gray>",
            _ => "<color=white>"
        };

        roundLogText.text += $"{colorTag}{roundInfo}</color>\n";
        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null;
        if (roundLogScrollRect != null)
            roundLogScrollRect.verticalNormalizedPosition = 0f;
    }

    public void HideUI()
    {
        if (mainUIContainer != null)
            mainUIContainer.SetActive(false);
    }

    public void ShowUI()
    {
        if (mainUIContainer != null)
            mainUIContainer.SetActive(true);
    }
}

