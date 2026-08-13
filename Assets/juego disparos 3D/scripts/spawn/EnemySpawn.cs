using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{
    public class EnemySpawn : MonoBehaviour
    {
        [Header("Configuración de Enemigos")]
        public GameObject[] enemies;           // Prefabs de enemigos
        public Transform[] spawnPoints;         // Puntos de aparición en el mapa

        [Header("Tiempos de Spawn")]
        [Range(1f, 15f)] public float initialTimeBetweenWaves = 10f;
        [Range(1f, 15f)] public float timeBetweenWaves = 4f;
        [Range(0.1f, 5f)] public float spawnDelay = 1.5f;

        [Header("Configuración de Oleadas")]
        [Range(1, 50)] public int initialWaveEnemies = 10;
        [Range(1, 20)] public int enemiesPerWaveIncrease = 5;

        [Header("Estado Actual (Debug)")]
        public int currentWave = 0;
        public int totalEnemiesInWave;
        public int enemiesSpawnedSoFar;
        public int enemiesAlive;

        private List<GameObject> activeEnemies = new List<GameObject>();
        private bool isSpawningWave = false;
        private bool isWaitingForNextWave = false;

        private bool IsPlayerAlive => GameManager.Instance != null && GameManager.Instance.hearts > 0;

        void Start()
        {
            currentWave = 0;
            StartCoroutine(StartFirstWaveWithDelay());
        }

        IEnumerator StartFirstWaveWithDelay()
        {
            isWaitingForNextWave = true;

            BulletSpawner ammoSpawner = FindObjectOfType<BulletSpawner>();
            if (ammoSpawner != null)
            {
                ammoSpawner.SpawnWaveAmmo();
            }

            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            float remainingTime = initialTimeBetweenWaves;

            while (remainingTime > 0)
            {
                if (waveUI != null)
                {
                    waveUI.ShowInitialCountdown(Mathf.CeilToInt(remainingTime));
                }

                yield return new WaitForSeconds(1f);
                remainingTime -= 1f;
            }

            if (waveUI != null)
            {
                waveUI.ClearCountdownText();
            }

            isWaitingForNextWave = false;
            StartNextWave();
        }

        void StartNextWave()
        {
            currentWave++;
            enemiesSpawnedSoFar = 0;

            totalEnemiesInWave = initialWaveEnemies + (currentWave - 1) * enemiesPerWaveIncrease;
            enemiesAlive = totalEnemiesInWave;

            Debug.Log($"<color=cyan><b>--- INICIANDO OLEADA {currentWave} ---</b></color> Total enemigos: {totalEnemiesInWave}");

            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            if (waveUI != null)
            {
                waveUI.ShowWave(currentWave);
            }

            if (currentWave > 1)
            {
                BulletSpawner ammoSpawner = FindObjectOfType<BulletSpawner>();
                if (ammoSpawner != null)
                {
                    ammoSpawner.SpawnWaveAmmo();
                }
            }

            StartCoroutine(SpawnWaveRoutine());
        }

        IEnumerator WaitAndStartNextWave()
        {
            isWaitingForNextWave = true;
            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            float remainingTime = timeBetweenWaves;

            Debug.Log($"<b>¡Oleada {currentWave} completada!</b> Próxima oleada en {timeBetweenWaves} segundos...");

            while (remainingTime > 0)
            {
                if (waveUI != null)
                {
                    waveUI.ShowInitialCountdown(Mathf.CeilToInt(remainingTime));
                }

                yield return new WaitForSeconds(1f);
                remainingTime -= 1f;
            }

            if (waveUI != null)
            {
                waveUI.ClearCountdownText();
            }

            isWaitingForNextWave = false;
            StartNextWave();
        }

        IEnumerator SpawnWaveRoutine()
        {
            isSpawningWave = true;

            while (enemiesSpawnedSoFar < totalEnemiesInWave)
            {
                if (!IsPlayerAlive)
                {
                    isSpawningWave = false;
                    yield break;
                }

                SpawnOneEnemy();
                enemiesSpawnedSoFar++;

                yield return new WaitForSeconds(spawnDelay);
            }

            isSpawningWave = false;

            // Verificación de seguridad si todos los enemigos murieron mientras aún se instanciaban
            if (enemiesAlive <= 0 && !isWaitingForNextWave && IsPlayerAlive)
            {
                StartCoroutine(WaitAndStartNextWave());
            }
        }

        void SpawnOneEnemy()
        {
            if (enemies == null || enemies.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
                return;

            int enemyIndex = Random.Range(0, enemies.Length);
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            GameObject newEnemy = Instantiate(
                enemies[enemyIndex],
                spawnPoints[spawnPointIndex].position,
                spawnPoints[spawnPointIndex].rotation
            );

            activeEnemies.Add(newEnemy);
        }

        public void OnEnemyDied()
        {
            enemiesAlive--;
            if (enemiesAlive < 0) enemiesAlive = 0;

            Debug.Log($"Enemigo eliminado. Quedan vivos: {enemiesAlive}/{totalEnemiesInWave}");

            // Al morir el último enemigo de la oleada, se inicia el temporizador de la siguiente
            if (enemiesAlive == 0 && !isSpawningWave && !isWaitingForNextWave && IsPlayerAlive)
            {
                StartCoroutine(WaitAndStartNextWave());
            }
        }

        public void ClearAllEnemies()
        {
            StopAllCoroutines();
            isSpawningWave = false;
            isWaitingForNextWave = false;

            foreach (GameObject enemy in activeEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy);
                }
            }

            activeEnemies.Clear();
            enemiesAlive = 0;

            WaveUI waveUI = FindObjectOfType<WaveUI>(true);
            if (waveUI != null)
            {
                waveUI.ClearWaveText();
                waveUI.ClearCountdownText();
            }

            Debug.Log("<color=orange><b>[EnemySpawn]</b> Enemigos en juego y UI de oleada limpiados.</color>");
        }
    }
}