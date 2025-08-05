using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class InstructionSceneLoader : MonoBehaviour
{
    public float delayBeforeLoad = 3f; // Time in seconds before transitioning
    public string nextSceneName = "MainScene";

    void Start()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(nextSceneName);
    }
}
