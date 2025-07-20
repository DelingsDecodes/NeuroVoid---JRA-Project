using UnityEngine;

public class GameResults : MonoBehaviour
{
    public static GameResults Instance;

    public Move playerFinalMove;
    public Move aiFinalMove;
    public string finalTaunt;  

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
