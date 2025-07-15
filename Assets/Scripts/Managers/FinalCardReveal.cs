using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FinalCardReveal : MonoBehaviour
{
    [Header("Player Card")]
    public Image cardFront;             
    public Image cardBack;              
    public Image cardFrontImage;       
    public TextMeshProUGUI moveNameText;

    [Header("AI Card")]
    public Image aiCardFront;
    public Image aiCardBack;
    public Image aiCardFrontImage;
    public TextMeshProUGUI aiMoveNameText;

    [Header("Post Flip")]
    public TextMeshProUGUI tauntText;
    public float revealDelay = 2.5f;
    public float flipDuration = 0.4f;

    void Start()
    {
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


        LeanTween.scaleX(cardBack.gameObject, 0, flipDuration).setEaseInBack();
        LeanTween.scaleX(aiCardBack.gameObject, 0, flipDuration).setEaseInBack();

        yield return new WaitForSeconds(flipDuration);

       
        cardBack.gameObject.SetActive(false);
        aiCardBack.gameObject.SetActive(false);

      
        if (cardFrontImage != null)
            cardFrontImage.sprite = GameResults.Instance.playerFinalMove.artwork;
        if (aiCardFrontImage != null)
            aiCardFrontImage.sprite = GameResults.Instance.aiFinalMove.artwork;

  
        cardFront.gameObject.SetActive(true);
        aiCardFront.gameObject.SetActive(true);

        LeanTween.scaleX(cardFront.gameObject, 1, flipDuration).setEaseOutBack();
        LeanTween.scaleX(aiCardFront.gameObject, 1, flipDuration).setEaseOutBack();

        yield return new WaitForSeconds(flipDuration);
       

        moveNameText.text = GameResults.Instance.playerFinalMove.name;
        aiMoveNameText.text = GameResults.Instance.aiFinalMove.name;

        moveNameText.gameObject.SetActive(true);
        aiMoveNameText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);

      
        if (tauntText != null)
        {
            tauntText.text = GetAITaunt();
            tauntText.gameObject.SetActive(true);
        }
    }

    string GetAITaunt()
    {
        string aiMove = GameResults.Instance.aiFinalMove.name;
        string playerMove = GameResults.Instance.playerFinalMove.name;

        if (aiMove == playerMove) return "A clash of minds... evenly matched.";
        else return $"You chose {playerMove}? Predictable.";
    }
}
