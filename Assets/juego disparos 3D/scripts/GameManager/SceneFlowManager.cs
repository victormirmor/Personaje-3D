using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameOverScene = "GameOver";

    public void PauseGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentGameState = GameState.Pause;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentGameState = GameState.InGame;

        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.currentGameState = GameState.GameOver;

        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverScene);
    }

    public void ReturnToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetStats();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}