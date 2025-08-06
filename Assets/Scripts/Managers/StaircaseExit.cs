using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StaircaseExit : MonoBehaviour
{
    public string nextSceneName = "InstructionScene"; 

    public void LeaveStaircase()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
