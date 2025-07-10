using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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

    public float revealDelay = 2.5f;

    void Start()
    {

        cardFront.gameObject.SetActive(false);
        cardFront.transform.localScale = new Vector3(0, 1, 1);

        aiCardFront.gameObject.SetActive(false);
        aiCardFront.transform.localScale = new Vector3(0, 1, 1);


        moveNameText.gameObject.SetActive(false);
        aiMoveNameText.gameObject.SetActive(false);

        StartCoroutine(FlipBothCards());
    }

    IEnumerator FlipBothCards()
    {
        yield return new WaitForSeconds(revealDelay);

  
        LeanTween.scaleX(cardBack.gameObject, 0, 0.4f).setEaseInBack();
        LeanTween.scaleX(aiCardBack.gameObject, 0, 0.4f).setEaseInBack();

        yield return new WaitForSeconds(0.4f);

    
        cardBack.gameObject.SetActive(false);
        aiCardBack.gameObject.SetActive(false);

        cardFront.gameObject.SetActive(true);
        aiCardFront.gameObject.SetActive(true);

   
        LeanTween.scaleX(cardFront.gameObject, 1, 0.4f).setEaseOutBack();
        LeanTween.scaleX(aiCardFront.gameObject, 1, 0.4f).setEaseOutBack();

   
        moveNameText.text = GameResults.playerFinalMove;
        aiMoveNameText.text = GameResults.aiFinalMove;

        moveNameText.gameObject.SetActive(true);
        aiMoveNameText.gameObject.SetActive(true);
    }
}
