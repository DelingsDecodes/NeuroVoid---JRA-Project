using UnityEngine;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    public TextMeshProUGUI roundText;

    void Start()
    {
        UpdateRoundDisplay();
    }

    public void UpdateRoundDisplay()
    {
        int current = GameResults.Instance.currentRound;
        int total = GameResults.Instance.totalRounds;
        roundText.text = $"Round {current} / {total}";
    }
}
