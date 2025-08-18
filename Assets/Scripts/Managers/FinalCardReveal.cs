using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class FinalCardReveal : MonoBehaviour
{
    [Header("Player Card")]
    public Image cardFront;
    public Image cardBack;
    public TextMeshProUGUI moveNameText;

    [Header("AI Card")]
    public Image aiCardFront;
    public Image aiCardBack;
    public TextMeshProUGUI aiMoveNameText;

    [Header("Post Flip")]
    public float revealDelay = 1.0f;
    public float flipDuration = 0.2f;
    public float returnDelay = 1.5f;

    [Header("Taunt Bubble UI")]
    public UIManager uiManager;

    void Start()
    {
        SetupCard(cardFront);
        SetupCard(aiCardFront);

        moveNameText.gameObject.SetActive(false);
        aiMoveNameText.gameObject.SetActive(false);

        StartCoroutine(FlipBothCards());
    }

    void SetupCard(Image front)
    {
        front.gameObject.SetActive(false);
        front.transform.localScale = new Vector3(0, 1, 1);
    }

    IEnumerator FlipBothCards()
    {
        yield return new WaitForSeconds(revealDelay);

        LeanTween.scaleX(cardBack.gameObject, 0, flipDuration).setEaseInBack();
        LeanTween.scaleX(aiCardBack.gameObject, 0, flipDuration).setEaseInBack();
        yield return new WaitForSeconds(flipDuration);

        cardBack.gameObject.SetActive(false);
        aiCardBack.gameObject.SetActive(false);

        cardFront.sprite = GameResults.Instance.playerFinalMove.artwork;
        aiCardFront.sprite = GameResults.Instance.aiFinalMove.artwork;

        cardFront.gameObject.SetActive(true);
        aiCardFront.gameObject.SetActive(true);

        LeanTween.scaleX(cardFront.gameObject, 1, flipDuration).setEaseOutBack();
        LeanTween.scaleX(aiCardFront.gameObject, 1, flipDuration).setEaseOutBack();
        yield return new WaitForSeconds(flipDuration);

        moveNameText.text = GameResults.Instance.playerFinalMove.name;
        aiMoveNameText.text = GameResults.Instance.aiFinalMove.name;

        moveNameText.gameObject.SetActive(true);
        aiMoveNameText.gameObject.SetActive(true);

        
        if (!string.IsNullOrEmpty(GameResults.Instance.finalTaunt) && uiManager != null)
        {
            yield return new WaitForSeconds(0.5f);
            uiManager.ShowAITaunt(GameResults.Instance.finalTaunt);
            yield return new WaitForSeconds(2.5f); 
        }

        GameResults.Instance.AddRoundToHistory();
        GameResults.Instance.currentRound++;

        yield return new WaitForSeconds(returnDelay);

        if (GameResults.Instance.currentRound > GameResults.Instance.totalRounds)
            SceneManager.LoadScene("SummaryScene");
        else
        {
            GameResults.Instance.ClearRoundMoves();
            SceneManager.LoadScene("MainScene");
        }
    }
}
