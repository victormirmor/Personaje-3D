using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [Header("Referencias de Sub-sistemas")]
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;

    private GameObject player;
    private PlayerControllerCC controller;
    private PlayerShooting playerShooting;
    private Animator anim;
    private Collider playerCollider;

    [Header("Estado de Escudo")]
    public bool isShieldActive = false;

    private bool isDead;
    private bool isProcessingDeath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        BindPlayerReferences();
    }

    public void BindPlayerReferences()
    {
        player = GameObject.FindWithTag(GameTags.Player);
        if (player != null)
        {
            controller = player.GetComponent<PlayerControllerCC>();
            playerShooting = player.GetComponent<PlayerShooting>();
            anim = player.GetComponent<Animator>();
            playerCollider = player.GetComponent<Collider>();
        }

        isDead = false;
        isProcessingDeath = false;

        if (healthUI != null)
        {
            healthUI.UpdateUI();
        }
    }

    /// <summary>
    /// Restablece salud, animaciones y habilita controles tras reaparecer.
    /// </summary>
    public void ResetPlayerState()
    {
        isDead = false;
        isProcessingDeath = false;

        if (playerCollider != null) playerCollider.enabled = true;
        if (playerShooting != null) playerShooting.enabled = true;
        if (controller != null) controller.enabled = true;

        if (anim != null)
        {
            anim.ResetTrigger("Die");
            anim.Rebind();
            anim.Update(0f);
        }

        if (healthUI != null)
        {
            healthUI.UpdateUI();
        }
    }

    public void ActivateShield(float duration)
    {
        StopCoroutine(nameof(ShieldRoutine));
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        isShieldActive = true;

        if (healthUI != null)
        {
            healthUI.SetShieldUIState(true);
        }

        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (healthUI != null)
            {
                healthUI.UpdateShieldTimer(timer);
            }

            yield return null;
        }

        isShieldActive = false;
        if (healthUI != null)
        {
            healthUI.SetShieldUIState(false);
        }
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.Instance == null || isDead || isProcessingDeath || isShieldActive) return;

        GameManager.Instance.ApplyDamage(amount);

        if (healthUI != null)
        {
            healthUI.TriggerDamageFlash();
            healthUI.UpdateUI();
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (GameManager.Instance.hearts <= 0)
        {
            HandleDeath();
        }
    }

    public void Heal(int amount)
    {
        if (GameManager.Instance == null || isDead || isProcessingDeath) return;

        GameManager.Instance.Heal(amount);

        if (healthUI != null)
        {
            healthUI.UpdateUI();
        }
    }

    private void HandleDeath()
    {
        if (isProcessingDeath) return;
        isProcessingDeath = true;
        isDead = true;

        if (playerCollider != null) playerCollider.enabled = false;
        if (playerShooting != null) playerShooting.enabled = false;
        if (controller != null) controller.enabled = false;

        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }

        if (anim != null) anim.SetTrigger("Die");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessPlayerDeath();
        }
    }
}