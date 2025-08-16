using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class QuestionnaireManager : MonoBehaviour
{
    //remove the fade out
    [SerializeField] private GameObject questionnairePanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button optionAButton;
    [SerializeField] private Button optionBButton;
    [SerializeField] private Slider progressBar;
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private TextMeshProUGUI impulsivenessText;
    [SerializeField] private TextMeshProUGUI aggressionText;
    [SerializeField] private TextMeshProUGUI unpredictabilityText;
    [SerializeField] private float typingSpeed = 0.03f;
    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup fadeCanvas;

    private int currentQuestionIndex = 0;
    private PlayerProfile profile;
    private List<QuestionData> questions;
    private Coroutine typingCoroutine;

    private class QuestionData
    {
        public string question;
        public string optionA;
        public string optionB;
        public System.Action<bool> applyAnswer;

        public QuestionData(string q, string a, string b, System.Action<bool> apply)
        {
            question = q;
            optionA = a;
            optionB = b;
            applyAnswer = apply;
        }
    }

    void Start()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.blocksRaycasts = false;
            fadeCanvas.interactable = false;
            fadeCanvas.transform.SetAsLastSibling(); // Ensure on top
        }

        profile = new PlayerProfile();
        SetupQuestions();

        if (questionText == null || optionAButton == null || optionBButton == null)
        {
            Debug.LogError("QuestionnaireManager: One or more UI elements are not assigned!");
            return;
        }

        if (progressBar != null)
        {
            progressBar.maxValue = questions.Count;
            progressBar.value = 0;
        }

        ShowQuestion(currentQuestionIndex);
    }

    private void SetupQuestions()
    {
        questions = new List<QuestionData>
        {
            new QuestionData("A firewall blocks your path. What do you do?",
                "Force it open immediately", "Wait for it to weaken", answer => profile.prefersAggression = answer),
            new QuestionData("You’re one move from winning. Trust instinct or analyze?",
                "Go with my gut", "Calculate and confirm first", answer => profile.prefersControl = answer),
            new QuestionData("You just lost a round. What now?",
                "Strike back harders", "Adapt and shift tactics", answer => profile.seeksRevenge = answer),
            new QuestionData("The opponent is watching your patterns. How do you act?",
                "Throw in fake moves to confuse them", "Stick to solid, honest strategy", answer => profile.oftenBluffs = answer),
            new QuestionData("Who would you rather have as your enemy ",
                "Predictable and steady", "Unstable and random", answer => profile.fearsPredictability = answer)
        };
    }

    private void ShowQuestion(int index)
    {
        if (index >= questions.Count)
        {
            FinishQuiz();
            return;
        }

        var q = questions[index];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(q.question));

        optionAButton.GetComponentInChildren<TextMeshProUGUI>().text = q.optionA;
        optionBButton.GetComponentInChildren<TextMeshProUGUI>().text = q.optionB;

        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();

        optionAButton.onClick.AddListener(() => OnAnswer(true));
        optionBButton.onClick.AddListener(() => OnAnswer(false));

        if (progressBar != null)
            progressBar.value = index + 1;
    }

    private IEnumerator TypeText(string fullText)
    {
        questionText.text = "";
        foreach (char c in fullText)
        {
            questionText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void OnAnswer(bool pickedA)
    {
        questions[currentQuestionIndex].applyAnswer.Invoke(pickedA);
        currentQuestionIndex++;
        ShowQuestion(currentQuestionIndex);
    }

    private void FinishQuiz()
    {
        if (questionnairePanel != null)
            questionnairePanel.SetActive(false);

        Debug.Log("Quiz complete. PlayerProfile seeded.");

        AIManager ai = FindObjectOfType<AIManager>();
        if (ai != null)
        {
            ai.Initialize(profile);
            Debug.Log("AIManager initialized with player profile.");
        }
        else
        {
            Debug.LogWarning("No AIManager found in scene.");
        }

        StartCoroutine(ShowSummaryAndFadeOut());
    }

    private IEnumerator ShowSummaryAndFadeOut()
    {
        if (summaryPanel != null)
        {
            summaryPanel.SetActive(true);
            impulsivenessText.text = $"Impulsiveness: {(profile.prefersControl ? 0.2f : 0.8f):F1}";
            aggressionText.text = $"Aggression: {(profile.prefersAggression ? 0.9f : 0.3f):F1}";
            unpredictabilityText.text = $"Unpredictability: {(profile.oftenBluffs ? 0.7f : 0.4f):F1}";

            if (continueButton != null)
                continueButton.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(2f);

        // Fade to black
        if (fadeCanvas != null)
        {
            float duration = 1.5f;
            float elapsed = 0f;

            fadeCanvas.blocksRaycasts = true;
            fadeCanvas.interactable = true;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
                yield return null;
            }

            fadeCanvas.alpha = 1f;
        }

        SceneManager.LoadScene("StaircaseScene");
    }

    public void ContinueToGame()
    {
        SceneManager.LoadScene("StaircaseScene");
    }

    public PlayerProfile GetProfile()
    {
        return profile;
    }
}
