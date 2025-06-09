using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Displays a psychological questionnaire and builds the player's profile.
public class QuestionnaireManager : MonoBehaviour
{
    public GameObject questionnairePanel;
    public Text questionText;
    public Button optionAButton;
    public Button optionBButton;

    private int currentQuestionIndex = 0;
    private PlayerProfile profile;

    private List<QuestionData> questions;

    private class QuestionData
    {
        public string question;
        public string optionA;  // Maps to true
        public string optionB;  // Maps to false
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
        profile = new PlayerProfile();
        SetupQuestions();
        ShowQuestion(currentQuestionIndex);
    }

    private void SetupQuestions()
    {
        questions = new List<QuestionData>
        {
            new QuestionData(
                "You're caught in a trap. Do you strike first or wait for a chance?",
                "Strike first", "Wait it out",
                answer => profile.prefersAggression = answer),

            new QuestionData(
                "Do you prefer to plan every step, or feel it out as you go?",
                "Plan it all", "Go with the flow",
                answer => profile.prefersControl = answer),

            new QuestionData(
                "When you lose, do you adapt… or get even?",
                "Get even", "Adapt wisely",
                answer => profile.seeksRevenge = answer),

            new QuestionData(
                "How often do you bluff when playing games?",
                "I bluff a lot", "Rarely bluff",
                answer => profile.oftenBluffs = answer),

            new QuestionData(
                "What’s worse: being predictable or being chaotic?",
                "Predictable", "Chaotic",
                answer => profile.fearsPredictability = answer)
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
        questionText.text = q.question;

        optionAButton.GetComponentInChildren<Text>().text = q.optionA;
        optionBButton.GetComponentInChildren<Text>().text = q.optionB;

        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();

        optionAButton.onClick.AddListener(() => OnAnswer(true));
        optionBButton.onClick.AddListener(() => OnAnswer(false));
    }

    private void OnAnswer(bool pickedA)
    {
        questions[currentQuestionIndex].applyAnswer.Invoke(pickedA);
        currentQuestionIndex++;
        ShowQuestion(currentQuestionIndex);
    }

    private void FinishQuiz()
    {
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
    }

    public PlayerProfile GetProfile()
    {
        return profile;
    }
}
