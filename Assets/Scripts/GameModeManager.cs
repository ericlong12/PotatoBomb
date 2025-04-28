using UnityEngine;

public enum GameMode
{
    Classic,
    Rhythm
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    public GameMode currentMode = GameMode.Classic; // Default = classic

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // So it survives scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
