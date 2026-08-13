using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Tiempos de Espera")]
    [SerializeField] private float deathAnimationDelay = 2.0f;
    [SerializeField] private float invisibleTime = 1.5f;

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        GameObject player = GameObject.FindWithTag(GameTags.Player);

        if (player == null)
        {
            Debug.LogError("[PlayerRespawn] ERROR: No se encontró el objeto con Tag 'Player'.");
            yield break;
        }

        // 1. Esperar animación de muerte
        yield return new WaitForSeconds(deathAnimationDelay);

        // 2. Ocultar el player
        player.SetActive(false);

        // 3. Pausa de invisibilidad
        yield return new WaitForSeconds(invisibleTime);

        // 4. Volver a activar al player en el mismo sitio
        player.SetActive(true);

        // 5. Restablecer controles y salud
        if (HealthManager.Instance != null)
        {
            HealthManager.Instance.ResetPlayerState();
        }

        Debug.Log("<color=cyan><b>[PlayerRespawn]</b> Jugador reactivado en el mismo lugar.</color>");
    }
}