using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    // Singleton
    public static HealthManager Instance { get; private set; }

    // Variable global accesible desde otros scripts
    public static int startingHealth = 100;

    public int currentHealth;

    [Header("UI References")]
    public Slider healthSlider;
    public Text healthText; // Referencia al texto en el Canvas (Text (Legacy) o similar)
    public Image damageImage;

    [Header("Effects & Audio")]
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    // Referencias al Player ocultas en el Inspector
    [HideInInspector] public Animator playerAnim;
    [HideInInspector] public AudioSource playerAudio;
    [HideInInspector] public PlayerControllerCC playerMovement;

    private bool isDead;
    private bool damaged;

    private void Awake()
    {
        // Control de instancia Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentHealth = startingHealth;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerReferences();
        InitUI();
    }

    private void FindPlayerReferences()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerAnim = playerObj.GetComponent<Animator>();
            playerAudio = playerObj.GetComponent<AudioSource>();
            playerMovement = playerObj.GetComponent<PlayerControllerCC>();
        }
    }

    private void InitUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    private void Update()
    {
        if (damaged)
        {
            if (damageImage != null)
                damageImage.color = flashColour;
        }
        else
        {
            if (damageImage != null)
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;

        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (playerAudio != null)
            playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (healthText != null)
            healthText.text = currentHealth + " / " + startingHealth;
    }

    private void Death()
    {
        isDead = true;

        if (playerAnim != null)
            playerAnim.SetTrigger("Die");

        if (playerAudio != null && deathClip != null)
        {
            playerAudio.clip = deathClip;
            playerAudio.Play();
        }

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
