using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HeadshotUI : MonoBehaviour
    {
        [Header("Configuración")]
        public float displayDuration = 1.2f;

        [Header("Referencias de UI")]
        public GameObject headshotOBJ;
        [SerializeField] private Text damageText;

        private Coroutine hideCoroutine;

        private void Awake() {            
            damageText = GetComponentInChildren<Text>(true);
            damageText.text="";
            headshotOBJ.SetActive(false);
        }

        public void TriggerHeadshot(int damage)
        {
            if (damageText != null)
            {
                damageText.text = $"HEADSHOT!\nDAÑO {damage}";
            }

            if (headshotOBJ != null)
            {
                headshotOBJ.SetActive(true);
            }

            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            hideCoroutine = StartCoroutine(HideRoutine());
        }

        private IEnumerator HideRoutine()
        {
            yield return new WaitForSeconds(displayDuration);

            if (headshotOBJ != null)
            {
                headshotOBJ.SetActive(false);
            }
        }
    }
}