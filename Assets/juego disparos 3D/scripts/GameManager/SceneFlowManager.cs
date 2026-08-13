using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameOverScene = "GameOver";

    void Awake(){
        PauseGame();
    }

    public void PauseGame(){
        Time.timeScale = 0f;
    }

    public void ResumeGame(){

        Time.timeScale = 1f;
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverScene);
    }

    public void ReturnToMainMenu()
    {
        if (GameManager.Instance != null)
        {
            //GameManager.Instance.ResetStats();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}