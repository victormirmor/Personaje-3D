using UnityEngine;

public class Damage_PowerUP : MonoBehaviour
{
    [Header("Configuración del PickUp")]
    public int value = 50;

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra tiene el Tag del jugador
        if (other.CompareTag(GameTags.Player))
        {
            // 1. Aplicar daño desde el HealthManager (se encarga del sonido, flash y UI)
            if (HealthManager.Instance != null)
            {
                HealthManager.Instance.TakeDamage(value);
            }

            // 2. Descontar o ajustar puntos en el GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddPoints(-value);
            }

            Debug.Log("Perdiste " + value + " de vida");

            // 3. Destruir el ítem recolectable
            Destroy(gameObject);
        }
    }
}