using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class WaveUI : MonoBehaviour
    {
        [Header("Configuración de Display")]
        public float displayDuration = 2f;

        [Header("Referencias de Texto")]
        public Text waveText;
        public Text countdownText;

        private Coroutine hideWaveCoroutine;

        private void Awake()
        {
            if (waveText == null)
            {
                waveText = GetComponent<Text>();
            }

            ClearWaveText();
            ClearCountdownText();
        }

        /// <summary>
        /// Muestra el cartel de la oleada actual durante unos segundos y luego lo oculta.
        /// </summary>
        public void ShowWave(int waveNumber)
        {
            ClearCountdownText();

            if (waveText != null)
            {
                waveText.text = $"WAVE {waveNumber}";
            }

            if (hideWaveCoroutine != null)
            {
                StopCoroutine(hideWaveCoroutine);
            }

            hideWaveCoroutine = StartCoroutine(HideWaveRoutine());
        }

        private IEnumerator HideWaveRoutine()
        {
            yield return new WaitForSeconds(displayDuration);
            ClearWaveText();
        }

        /// <summary>
        /// Muestra la cuenta regresiva previa a la Oleada 1 o entre oleadas.
        /// </summary>
        public void ShowInitialCountdown(int seconds)
        {
            ClearWaveText();

            if (countdownText != null)
            {
                if (seconds > 0)
                {
                    countdownText.text = $"PREPARE! WAVE IN {seconds}...";
                }
                else
                {
                    ClearCountdownText();
                }
            }
        }

        public void ClearWaveText()
        {
            if (waveText != null)
            {
                waveText.text = "";
            }
        }

        public void ClearCountdownText()
        {
            if (countdownText != null)
            {
                countdownText.text = "";
            }
        }
    }
}