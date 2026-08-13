using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Referencias de UI del HUD")]
    [SerializeField] private Text livesText;
    //[SerializeField] private Text coinsText;
    [SerializeField] private Text scoreText;

    private void Update()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (GameManager.Instance == null) return;

        if (livesText != null)
        {
            livesText.text = GameManager.Instance.lives.ToString();
        }

        /*if (coinsText != null)
        {
            coinsText.text = GameManager.Instance.coins.ToString();
        }*/

        if (scoreText != null)
        {
            scoreText.text = GameManager.Instance.points.ToString();
        }
    }
}