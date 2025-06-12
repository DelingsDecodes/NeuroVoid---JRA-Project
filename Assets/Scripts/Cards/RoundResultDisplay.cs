using UnityEngine;
using TMPro;

public class RoundResultDisplay : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI resultText;

    private float showDuration = 2f;

    public void ShowResult(string playerMove, string aiMove, string outcome)
    {
        resultText.text = $"You played <b>{playerMove}</b>\n AI played <b>{aiMove}</b>\n{outcome}";

        // Fade in
        LeanTween.alphaCanvas(canvasGroup, 1f, 0.5f).setOnComplete(() =>
        {
            // Wait then fade out
            LeanTween.delayedCall(showDuration, () =>
            {
                LeanTween.alphaCanvas(canvasGroup, 0f, 0.5f);
            });
        });
    }
}
