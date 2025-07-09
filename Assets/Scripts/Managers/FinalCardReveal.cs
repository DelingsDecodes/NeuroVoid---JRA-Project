using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FinalCardReveal : MonoBehaviour
{
    public Image cardFront;
    public Image cardBack;
    public TextMeshProUGUI moveNameText;

    public float revealDelay = 2.5f;

    void Start()
    {
        cardFront.gameObject.SetActive(false);
        cardFront.transform.localScale = new Vector3(0, 1, 1); // Prepare for flip

        moveNameText.gameObject.SetActive(false);

        StartCoroutine(FlipCardAfterDelay());
    }

    IEnumerator FlipCardAfterDelay()
    {
        yield return new WaitForSeconds(revealDelay);

     
        LeanTween.scaleX(cardBack.gameObject, 0, 0.4f).setEaseInBack();

        yield return new WaitForSeconds(0.4f);

        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);


        LeanTween.scaleX(cardFront.gameObject, 1, 0.4f).setEaseOutBack();


        moveNameText.text = GameResults.playerFinalMove;
        moveNameText.gameObject.SetActive(true);
    }
}
