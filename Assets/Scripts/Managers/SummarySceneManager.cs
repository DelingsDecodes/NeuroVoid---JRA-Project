using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SummarySceneManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI resultText;

    void Start()
    {
        if (summaryText != null)
        {
            summaryText.text = GameResults.Instance.GetSummary();
        }

        if (resultText != null)
        {
            resultText.text = GameResults.Instance.GetFinalResult(); // win/loss/tie
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void RestartGame()
    {
        GameResults.Instance.Reset();
        SceneManager.LoadScene("MainScene"); 
    }
}
// need changes
