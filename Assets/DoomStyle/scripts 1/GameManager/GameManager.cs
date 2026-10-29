using UnityEngine;

public enum GameState
{
    InGame,
    Pause,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado del Juego")]
    public GameState currentGameState = GameState.InGame;

    [Header("Datos Globales (Lectura/Escritura)")]
    public int lives = 10;
    public int points = 0;
    public int coins = 0;
    public int hearts = 100;

    [Header("Ajustes de Audio Globales")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void Heal(int amount)
    {
        hearts += amount;
        if (hearts >= 300)
        {
            hearts = 100;
            lives++;
        }
    }

    public void ResetStats()
    {
        lives = 8;
        hearts = 100;
        points = 0;
        coins = 0;
    }
}
