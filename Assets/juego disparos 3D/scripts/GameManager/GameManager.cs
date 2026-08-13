using UnityEngine;
using CompleteProject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Estado de Salud y Vidas")]
    public int hearts = 3-00;
    public int maxHearts = 100;
    public int lives = 3;

    [Header("Economía y Puntuación")]
    public int points = 0;
    public int coins = 0;

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

    public void ApplyDamage(int amount)
    {
        hearts -= amount;
        if (hearts < 0) hearts = 0;
    }

    public void Heal(int amount)
    {
        hearts += amount;
        if (hearts > maxHearts) hearts = maxHearts;
    }

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public void ProcessPlayerDeath()
    {
        lives--;
        Debug.Log($"<color=orange>[GameManager] ProcessPlayerDeath() ejecutado. Vidas restantes: {lives}</color>");

        // 1. Destruir todos los enemigos en juego registrados por el EnemySpawn
        EnemySpawn spawner = FindObjectOfType<EnemySpawn>();
        if (spawner != null)
        {
            spawner.ClearAllEnemies();
        }

        // 2. Gestionar la reaparición o el Game Over
        if (lives > 0)
        {
            hearts = maxHearts;

            PlayerRespawn respawnSystem = FindObjectOfType<PlayerRespawn>();
            if (respawnSystem != null)
            {
                respawnSystem.Respawn();
            }
        }
        else
        {
            Debug.Log("<color=red><b>[GAME OVER]</b> Se agotaron las vidas completas.</color>");
        }
    }
}