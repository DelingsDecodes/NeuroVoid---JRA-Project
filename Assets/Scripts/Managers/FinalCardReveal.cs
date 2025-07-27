using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

//need changes 

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
    public TextMeshProUGUI tauntText;
    public float revealDelay = 1.0f;
    public float flipDuration = 0.2f;
    public float returnDelay = 1.5f; // time to wait after taunt before returning

    void Start()
    {
        // Hide fronts and prepare for flip
        SetupCard(cardFront);
        SetupCard(aiCardFront);

        moveNameText.gameObject.SetActive(false);
        aiMoveNameText.gameObject.SetActive(false);
        if (tauntText != null) tauntText.gameObject.SetActive(false);

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

        // Flip backs
        LeanTween.scaleX(cardBack.gameObject, 0, flipDuration).setEaseInBack();
        LeanTween.scaleX(aiCardBack.gameObject, 0, flipDuration).setEaseInBack();

        yield return new WaitForSeconds(flipDuration);

        // Hide backs and set artwork
        cardBack.gameObject.SetActive(false);
        aiCardBack.gameObject.SetActive(false);

        cardFront.sprite = GameResults.Instance.playerFinalMove.artwork;
        aiCardFront.sprite = GameResults.Instance.aiFinalMove.artwork;

        cardFront.gameObject.SetActive(true);
        aiCardFront.gameObject.SetActive(true);

        // Flip fronts in
        LeanTween.scaleX(cardFront.gameObject, 1, flipDuration).setEaseOutBack();
        LeanTween.scaleX(aiCardFront.gameObject, 1, flipDuration).setEaseOutBack();

        yield return new WaitForSeconds(flipDuration);

        // Show move names
        moveNameText.text = GameResults.Instance.playerFinalMove.name;
        aiMoveNameText.text = GameResults.Instance.aiFinalMove.name;

        moveNameText.gameObject.SetActive(true);
        aiMoveNameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f); 

        // Show taunt stored from GameManager
        if (tauntText != null)
        {
            tauntText.text = GameResults.Instance.finalTaunt;
            tauntText.gameObject.SetActive(true);
        }

        // Wait, then go back to game or end
        yield return new WaitForSeconds(returnDelay);

        GameResults.Instance.currentRound++;

        if (GameResults.Instance.currentRound > GameResults.Instance.totalRounds)
        {
            SceneManager.LoadScene("SummaryScene"); // or EndGameScene
        }
        else
        {
            GameResults.Instance.ClearRoundMoves(); // reset taunt and moves
            SceneManager.LoadScene("MainScene"); // go back to card selection
        }
    }
}
