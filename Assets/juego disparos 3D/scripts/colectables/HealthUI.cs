using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referencias de UI Base")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Image damageFlashImage;

    [Header("UI de Escudo")]
    [SerializeField] private Image shieldIconImage;
    [SerializeField] private Text shieldText;

    [Header("Efecto de Daño Visual")]
    [SerializeField] private float flashSpeed = 5f;
    [SerializeField] private Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private bool isDamaged;

    void Awake()
    {
        // 1. Ocultar el texto del contador al arrancar
        if (shieldText != null)
        {
            shieldText.text = "";
        }

        // 2. Establecer el Alpha del icono de escudo en 0 (transparente) al arrancar
        if (shieldIconImage != null)
        {
            Color c = shieldIconImage.color;
            c.a = 0f;
            shieldIconImage.color = c;
        }
    }

    public void TriggerDamageFlash()
    {
        isDamaged = true;
    }

    // Activa/desactiva la visibilidad del icono de escudo
    public void SetShieldUIState(bool active)
    {
        if (shieldIconImage != null)
        {
            Color c = shieldIconImage.color;
            c.a = active ? 1f : 0f;
            shieldIconImage.color = c;
        }

        if (!active && shieldText != null)
        {
            shieldText.text = "";
        }
    }

    // Actualiza el texto con el tiempo restante del escudo
    public void UpdateShieldTimer(float remainingTime)
    {
        if (shieldText != null)
        {
            if (remainingTime > 0)
            {
                shieldText.text = Mathf.CeilToInt(remainingTime).ToString();
            }
            else
            {
                shieldText.text = "";
            }
        }
    }

    void Update()
    {
        HandleFlashEffect();
        UpdateUI();
    }

    private void HandleFlashEffect()
    {
        if (damageFlashImage == null) return;

        if (isDamaged)
        {
            damageFlashImage.color = flashColour;
        }
        else
        {
            damageFlashImage.color = Color.Lerp(damageFlashImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        isDamaged = false;
    }

    public void UpdateUI()
    {
        if (GameManager.Instance == null) return;

        int current = GameManager.Instance.hearts;
        int max = GameManager.Instance.maxHearts;

        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        if (healthText != null)
        {
            healthText.text = current + " / " + max;
        }
    }
}