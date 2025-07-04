using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalCardReveal : MonoBehaviour
{
    public Image cardFront;
    public Image cardBack;
    public TextMeshProUGUI moveNameText;
    public float revealDelay = 2.5f;

    void Start()
    {
        cardFront.gameObject.SetActive(false);
        moveNameText.gameObject.SetActive(false);

        StartCoroutine(FlipCardAfterDelay());
    }

    IEnumerator FlipCardAfterDelay()
    {
        yield return new WaitForSeconds(revealDelay);

        cardBack.gameObject.SetActive(false);
        cardFront.gameObject.SetActive(true);
        moveNameText.text = GameResults.playerFinalMove;
        moveNameText.gameObject.SetActive(true);
    }
}
