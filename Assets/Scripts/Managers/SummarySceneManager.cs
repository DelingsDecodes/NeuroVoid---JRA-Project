// === SummarySceneManager.cs ===
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class SummarySceneManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI historyText;
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
      
        if (resultText != null)
            resultText.text = GameResults.Instance.GetFinalResult();

       
        if (summaryText != null)
            summaryText.text = GameResults.Instance.GetAdvancedSummary();

        
        if (historyText != null)
        {
            var history = GameResults.Instance.GetFullHistory();
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<b>--- Full Round Log ---</b>");
            foreach (var entry in history)
            {
                builder.AppendLine(entry);
            }

            historyText.text = builder.ToString();
        }

      
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void RestartGame()
    {
        GameResults.Instance.Reset();
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
