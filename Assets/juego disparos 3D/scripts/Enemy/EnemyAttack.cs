using UnityEngine;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;
        public int attackDamage = 10;
        public int pointsDeductedOnPlayerDeath = 10; // Puntos que quita al matar al jugador

        [Header("Pausa Post-Ataque")]
        public float postAttackMoveDelay = 1.5f;

        private Animator anim;
        private GameObject player;
        private EnemyHealth enemyHealth;
        private EnemyMovement enemyMovement;
        private bool playerInRange;
        private float timer;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag(GameTags.Player);
            enemyHealth = GetComponent<EnemyHealth>();
            enemyMovement = GetComponent<EnemyMovement>();
            anim = GetComponent<Animator>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInRange = false;

                if (enemyMovement != null && enemyHealth != null && enemyHealth.currentHealth > 0)
                {
                    enemyMovement.ApplyAttackCooldown(postAttackMoveDelay);
                }
            }
        }

        void Update()
        {
            timer += Time.deltaTime;

            bool isPlayerAlive = GameManager.Instance != null && GameManager.Instance.hearts > 0;

            if (timer >= timeBetweenAttacks && playerInRange && enemyHealth != null && enemyHealth.currentHealth > 0)
            {
                Attack(isPlayerAlive);
            }

            if (!isPlayerAlive && anim != null)
            {
                anim.SetTrigger("PlayerDead");
            }
        }

        void Attack(bool isPlayerAlive)
        {
            timer = 0f;

            if (isPlayerAlive && HealthManager.Instance != null)
            {
                HealthManager.Instance.TakeDamage(attackDamage);

                // Comprobar si el golpe fue letal para el jugador
                if (GameManager.Instance != null && GameManager.Instance.hearts <= 0)
                {
                    // Restar puntos por la derrota
                    GameManager.Instance.AddPoints(-pointsDeductedOnPlayerDeath);

                    Debug.Log($"<color=red><b>[EnemyAttack]</b> Enemigo eliminó al jugador. Puntos restados: {pointsDeductedOnPlayerDeath}. Destruyendo enemigo...</color>");

                    // Autodestruir el enemigo que mató al jugador
                    Destroy(gameObject);
                }
            }
        }
    }
}