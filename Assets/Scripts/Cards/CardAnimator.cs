using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardAnimator : MonoBehaviour, IPointerClickHandler
{
    private Vector3 originalScale;
    private bool isLocked = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;

        LeanTween.scale(gameObject, originalScale * 1.1f, 0.1f)
                 .setEasePunch()
                 .setOnComplete(() => {
                     transform.localScale = originalScale;
                 });


        LockCard();
    }

    public void LockCard()
    {
        isLocked = true;
    }

    public void UnlockCard()
    {
        isLocked = false;
    }
}
