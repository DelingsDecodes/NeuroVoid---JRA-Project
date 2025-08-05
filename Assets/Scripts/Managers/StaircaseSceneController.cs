using UnityEngine;
using UnityEngine.SceneManagement;

public class StaircaseSceneController : MonoBehaviour
{
    public string nextSceneName = "InstructionScene";  
    public float delaySeconds = 3f;

    void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private System.Collections.IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(nextSceneName);
    }
}
