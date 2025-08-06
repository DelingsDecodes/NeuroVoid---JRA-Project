using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class StaircaseDialog : MonoBehaviour
{
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public GameObject leaveButton;

    [TextArea(2, 5)]
    public string message = "The air is cold. The staircase coils downward into darkness... it seems there's only one way to go.";
    public float typingSpeed = 0.04f;

    private void Start()
    {
        dialogPanel.SetActive(true);
        leaveButton.SetActive(false);
        StartCoroutine(TypeMessage());
    }

    IEnumerator TypeMessage()
    {
        dialogText.text = "";
        foreach (char c in message)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1.5f);
        leaveButton.SetActive(true);
    }
}
