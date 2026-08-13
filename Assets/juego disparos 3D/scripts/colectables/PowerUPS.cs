using UnityEngine;
using CompleteProject;

public enum ItemType
{
    Health,
    Coin,
    Shield,
    DamagePlayer,
    DamageEnemy,
    Ammo
}

public class PowerUPS : MonoBehaviour
{
    [Header("Configuración del PickUp")]
    public ItemType type = ItemType.Health;

    [Header("Valores")]
    public int value = 10;
    public float shieldDuration = 5f; // Duración en segundos si es tipo Shield
    public bool destroyOnImpact = true;

    [Header("Munición (Solo para tipo Ammo)")]
    public GameObject bulletPrefabToGive;

    private string targetTag;

    private void Awake()
    {
        // Asignamos la etiqueta objetivo al iniciar según el tipo de ítem
        switch (type)
        {
            case ItemType.DamageEnemy:
                targetTag = GameTags.Enemy;
                break;
            default:
                targetTag = GameTags.Player;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Choque con el entorno (para proyectiles/balas)
        if (other.CompareTag(GameTags.Environment))
        {
            if (destroyOnImpact) Destroy(gameObject);
            return;
        }

        // 2. Procesar únicamente si el objeto que colisiona tiene la etiqueta esperada
        if (other.CompareTag(targetTag))
        {
            switch (type)
            {
                case ItemType.Health:
                    if (HealthManager.Instance != null)
                    {
                        HealthManager.Instance.Heal(value);
                    }
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddPoints(value / 2);
                    }
                    Debug.Log("Sumaste " + value + " de vida");
                    break;

                case ItemType.Coin:
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddCoins(value);
                        GameManager.Instance.AddPoints(value);
                    }
                    Debug.Log("Sumaste " + value + " de dinero");
                    break;

                case ItemType.Shield:
                    if (HealthManager.Instance != null)
                    {
                        HealthManager.Instance.ActivateShield(shieldDuration);
                    }
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddPoints(value);
                    }
                    Debug.Log("Escudo activado durante " + shieldDuration + " segundos");
                    break;

                case ItemType.DamagePlayer:
                    if (HealthManager.Instance != null)
                    {
                        HealthManager.Instance.TakeDamage(value);
                    }
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddPoints(-value);
                    }
                    Debug.Log("Perdiste " + value + " de vida");
                    break;

                case ItemType.DamageEnemy:
                    EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(value, transform.position);
                    }
                    Debug.Log("Infligiste " + value + " de daño al enemigo");
                    break;

                case ItemType.Ammo:
                    var playerShooting = other.GetComponentInChildren<PlayerShooting>();
                    if (playerShooting != null)
                    {
                        playerShooting.ReloadWeapon(bulletPrefabToGive, value);
                        Debug.Log("Recargaste " + value + " balas");
                    }
                    break;
            }

            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
    }
}