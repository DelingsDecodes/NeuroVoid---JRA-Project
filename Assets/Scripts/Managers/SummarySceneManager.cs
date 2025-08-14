using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class SummarySceneManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI resultText;

    void Start()
    {
        if (GameResults.Instance == null)
        {
            Debug.LogWarning("SummarySceneManager: GameResults instance not found.");
            return;
        }

        if (summaryText != null)
            summaryText.text = GameResults.Instance.GetAdvancedSummary();

      
        if (resultText != null)
            resultText.text = GameResults.Instance.GetFinalResult();
    }

    public void RestartGame()
    {
        if (GameResults.Instance != null)
            GameResults.Instance.Reset();

        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
