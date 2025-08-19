using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tauntText;

    public void ShowAITaunt(string taunt)
    {
        if (tauntText != null)
        {
            tauntText.text = taunt;
            tauntText.gameObject.SetActive(true);
        }
    }

    public void HideAITaunt()
    {
        if (tauntText != null)
        {
            tauntText.gameObject.SetActive(false);
        }
    }
}
