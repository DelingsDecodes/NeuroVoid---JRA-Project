using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//need fixing
public class SummarySceneManager : MonoBehaviour
{
    public TextMeshProUGUI summaryText;
    public TextMeshProUGUI resultText;

    void Start()
    {
        if (summaryText != null)
            summaryText.text = GameResults.Instance.GetSummary();

        if (resultText != null)
            resultText.text = GameResults.Instance.GetFinalResult();
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
