using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Referencias de Texto UI")]
    [SerializeField] private Text pointsText;
    [SerializeField] private Text livesText;

    [Header("Formato de Visualización")]
    [SerializeField] private string pointsFormat = "000000"; // Agrega ceros a la izquierda (ej: 000050)

    private int lastPoints = -1;
    private int lastLives = -1;

    void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (GameManager.Instance == null) return;

        // Actualizar Puntos solo si cambiaron
        if (GameManager.Instance.points != lastPoints)
        {
            lastPoints = GameManager.Instance.points;
            if (pointsText != null)
            {
                pointsText.text = lastPoints.ToString(pointsFormat);
            }
        }

        // Actualizar Vidas solo si cambiaron
        if (GameManager.Instance.lives != lastLives)
        {
            lastLives = GameManager.Instance.lives;
            if (livesText != null)
            {
                livesText.text = lastLives.ToString();
            }
        }
    }
}