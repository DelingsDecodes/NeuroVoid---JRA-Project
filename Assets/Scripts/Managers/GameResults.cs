using UnityEngine;

public class GameResults : MonoBehaviour
{
    public static GameResults Instance { get; private set; }

    public Move playerFinalMove;
    public Move aiFinalMove;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
}
