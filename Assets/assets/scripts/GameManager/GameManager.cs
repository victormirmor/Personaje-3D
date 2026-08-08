#region previous assignments
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion
#region GameStates
public enum GameState
{
    inGame, pause, gameOver
}
#endregion

public class GameManager : MonoBehaviour
{

    #region Statics
    public static GameManager gameManager;
    public GameState currentGameState;
    public static float _Musicvol, _SFXvol;
    public static int lives = 10, Points = 0, Coins = 0, Hearts = 100;
    #endregion
    #region 
    [SerializeField] GameObject Player;
    [SerializeField] Vector3 position;
    public float espera_Reespawn;
    [SerializeField] string Tag_Seguir = "player";
    public string Main_Menu;
    
    [SerializeField] private int Life, Healt, Colectables;
    [SerializeField] float volMusic, volSFX;
    #endregion
    #region Sound
    public float Musicvol, SFXvol;
    [SerializeField] SoundFXManagerv FXManager;
    [Header("Sound Efects")]
    public AudioClip Dead;
    public AudioClip Danger;
    [SerializeField]private bool _IsCoin = false;
    #endregion
    public bool IsCoin
    {
        get => _IsCoin;
        set => _IsCoin = value;
    }

    #region Metodos Unity
    void Awake()
    {
        Singleton();
        SearchManagers();
    }
    void Update()
    {
        livesCount();
        UpdateInts();
        HeartsCount();
        HealtScore();
        SearchManagers();
        UpdateSound();
    }

    #endregion

    #region metodos extras

    public void StartGame()
    {
        currentGameState = GameState.inGame;
        Time.timeScale = 1;
    }
    public void StartGame_over()
    {
        currentGameState = GameState.inGame;
        Time.timeScale = 1;
        ResetGame();
        SceneManager.LoadScene(Main_Menu);
    }
    public void PauseGame()
    {
        currentGameState = GameState.pause;
        Time.timeScale = 0;
    }
    public void GameOver(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }
    public void HealtScore()
    {
        if (Hearts >= 300)
        {
            Hearts = 100;
            lives++;
        }
    }
    public void DeadMain()
    {
        ResetGame();
        SceneManager.LoadScene(Main_Menu);

    }
    public void UpdateSound()
    {
        _Musicvol = Musicvol;
        _SFXvol = SFXvol;

    }

    public void ResetGame()
    {
        lives = 8;
        Hearts = 100;
        Points = 0;
        Coins = 0;
        Time.timeScale =
        Time.timeScale == 0 ? 1 : 0;
    }
    public void NextLive()
    {
        Time.timeScale = 1;
        Player.transform.position = position;
        Player.SetActive(true);
    }
    void livesCount()
    {
        if (lives == 0)
        {
            GameOver(Main_Menu);

        }
    }
    void HeartsCount()
    {
        if (Hearts <= 0)
        {
            Debug.Log("has muerto");
            FXManager.SoundPlay(Dead, 1);
            Player.SetActive(false);
            lives--;
            Hearts = 100;
            Time.timeScale = 0;
            Invoke("NextLive", espera_Reespawn);
        }
    }
    void UpdateInts()
    {
        Life = lives;
        Healt = Hearts;
        Colectables = Coins;
    }

    void SearchManagers()
    {

        if (FXManager == null)
        {
            FXManager = FindObjectOfType<SoundFXManagerv>();
        }
        if (Player == null)
        {
            Player = GameObject.FindWithTag(Tag_Seguir);
        }
    }
    #endregion

    #region singleton

    void Singleton()
    {
        if (gameManager != null)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
}

