using UnityEngine;
using UnityEngine.AI;
using System.Collections;

    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Referencia a la posición del jugador
        EnemyHealth enemyHealth;        // Referencia a la salud de este enemigo
        NavMeshAgent nav;// Referencia al NavMeshAgent
        Animator anim;                  // Referencia al Animator

        [Header("Configuración de Velocidad")]
        [Tooltip("Velocidad de movimiento de este prefab específico")]
        public float speed = 3.5f;

        [Header("Variación de Ruta / Dispersión")]
        [Tooltip("Radio de variación alrededor del player para evitar amontonamientos")]
        public float targetOffsetRadius = 1.5f;

        [Tooltip("Cada cuántos segundos recalculamos la variación del destino")]
        public float recalculateInterval = 1.0f;

        [Header("Mecánica de Pausa / Duda")]
        [Tooltip("Tiempo mínimo entre pausas de duda")]
        public float minTimeBetweenPauses = 5f;
        [Tooltip("Tiempo máximo entre pausas de duda")]
        public float maxTimeBetweenPauses = 10f;
        [Tooltip("Duración de la animación/espera de duda")]
        public float pauseDuration = 2f;

        private Vector3 currentOffset;
        private float nextRecalculateTime;
        private float pauseTimer;
        private bool isPausing = false;
        private bool isCoolingDownFromAttack = false;

        // Propiedad helper para verificar la integridad del NavMeshAgent
        private bool IsAgentActiveAndOnNavMesh => nav != null && nav.enabled && nav.isOnNavMesh;

        void Awake()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }

            enemyHealth = GetComponent<EnemyHealth>();
            nav = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            if (IsAgentActiveAndOnNavMesh)
            {
                nav.speed = speed;
            }

            GenerateNewOffset();
            ScheduleNextPause();
        }

        void Update()
        {
            bool isPlayerDead = GameManager.Instance == null || GameManager.Instance.hearts <= 0;
            bool isEnemyDead = enemyHealth != null && enemyHealth.currentHealth <= 0;

            // Si el enemigo o el jugador murieron, aseguramos desactivar el agente
            if (isEnemyDead || isPlayerDead)
            {
                if (nav != null && nav.enabled)
                {
                    nav.enabled = false;
                }
                return;
            }

            if (!IsAgentActiveAndOnNavMesh || player == null) return;

            // Si está pausado o en cooldown de ataque, frenar el agente
            if (isPausing || isCoolingDownFromAttack)
            {
                nav.isStopped = true;
                return;
            }

            nav.isStopped = false;

            // Control del ciclo de pausa/duda
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                StartCoroutine(DoubtRoutine());
                return;
            }

            // Recalcular la variación de destino periódicamente
            if (Time.time >= nextRecalculateTime)
            {
                GenerateNewOffset();
                nextRecalculateTime = Time.time + recalculateInterval;
            }

            // Definir el destino con el offset aplicado
            Vector3 targetDestination = player.position + currentOffset;
            nav.SetDestination(targetDestination);
        }

        void GenerateNewOffset()
        {
            Vector2 randomCircle = Random.insideUnitCircle * targetOffsetRadius;
            currentOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        void ScheduleNextPause()
        {
            pauseTimer = Random.Range(minTimeBetweenPauses, maxTimeBetweenPauses);
        }

        IEnumerator DoubtRoutine()
        {
            isPausing = true;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = true;
            }

            if (anim != null)
            {
                //anim.SetBool("IsDoubting", true);
            }

            yield return new WaitForSeconds(pauseDuration);

            if (anim != null)
            {
                //anim.SetBool("IsDoubting", false);
            }

            isPausing = false;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = false;
            }

            ScheduleNextPause();
        }

        public void ApplyAttackCooldown(float cooldownTime)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(AttackCooldownRoutine(cooldownTime));
            }
        }

        IEnumerator AttackCooldownRoutine(float cooldownTime)
        {
            isCoolingDownFromAttack = true;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = true;
            }

            yield return new WaitForSeconds(cooldownTime);

            isCoolingDownFromAttack = false;

            if (IsAgentActiveAndOnNavMesh)
            {
                nav.isStopped = false;
            }
    }


    
    // Reposiciona al enemigo en un punto de spawn y resetea su navegación.
    
    public void ResetToSpawn(Vector3 spawnPosition)
    {
        if (nav != null)
        {
            nav.enabled = false;
            transform.position = spawnPosition;
            nav.enabled = true;
            if (nav.isOnNavMesh)
            {
                nav.isStopped = false;
            }
        }
        else
        {
            transform.position = spawnPosition;
        }

        isPausing = false;
        isCoolingDownFromAttack = false;
    }
}