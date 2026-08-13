using UnityEngine;

namespace CompleteProject
{
    public class BulletDamage : MonoBehaviour
    {
        [Header("Configuración de Daño")]
        public int baseDamage = 20;
        public int headshotMultiplier = 2;

        public int FinalDamage { get; private set; }
        public bool IsHeadshot { get; private set; }

        private void Awake()
        {
            // 33.3% de probabilidad de crítico (1 de 3)
            IsHeadshot = (Random.Range(1, 4) == 1);
            FinalDamage = IsHeadshot ? (baseDamage * headshotMultiplier) : baseDamage;
        }

        public void ApplyDamageToTarget(GameObject target, Vector3 hitPoint)
        {
            EnemyHealth enemyHealth = target.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                if (IsHeadshot)
                {
                    Debug.Log($"<b>¡HEADSHOT CRÍTICO!</b> Golpeado: {target.name} | Daño enviado: {FinalDamage}");
                    
                    HeadshotUI headshotUI = FindObjectOfType<HeadshotUI>(true);
                    if (headshotUI != null)
                    {
                        headshotUI.TriggerHeadshot(FinalDamage);
                    }
                    else
                    {
                        Debug.LogWarning("[BulletDamage] Se produjo un Headshot pero no se encontró 'HeadshotUI' en la escena.");
                    }
                }
                else
                {
                    Debug.Log($"Tiro Normal: {target.name} | Daño enviado: {FinalDamage}");
                }

                // Aplicar el daño real
                enemyHealth.TakeDamage(FinalDamage, hitPoint);
            }
        }
    }
}