using UnityEngine;
using TMPro;

public class RoundLogUI : MonoBehaviour
{
    public TextMeshProUGUI logText;

    void Start()
    {
        RefreshLog();
    }

    public void RefreshLog()
    {
        logText.text = "";

        foreach (string entry in GameResults.Instance.roundHistory)
        {
            logText.text += entry + "\n";
        }
    }
}
