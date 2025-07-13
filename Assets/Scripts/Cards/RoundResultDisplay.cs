using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoundResultDisplay : MonoBehaviour
{
    [SerializeField] private CanvasGroup resultCanvasGroup;
    [SerializeField] private TextMeshProUGUI resultText;

    private void Awake()
    {
        if (resultCanvasGroup != null)
        {
            resultCanvasGroup.alpha = 0f;
        }
    }

    public void ShowResult(string playerMove, string aiMove, string outcomeMessage)
    {
        if (resultCanvasGroup == null || resultText == null)
        {
            Debug.LogWarning("RoundResultDisplay: Missing UI references.");
            return;
        }

        resultText.text = $"You played {playerMove}, AI played {aiMove}.{outcomeMessage}";

        // Fade in
        if (resultCanvasGroup != null)
        {
            LeanTween.alphaCanvas(resultCanvasGroup, 1f, 0.5f).setOnComplete(() =>
            {
             
                if (resultCanvasGroup != null)
                {
                    LeanTween.alphaCanvas(resultCanvasGroup, 0f, 0.5f).setDelay(1.5f);
                }
            });
        }
    }
}
