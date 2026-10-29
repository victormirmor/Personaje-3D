using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private AudioClip deadClip;
    [SerializeField] private SoundFXManagerv fxManager;

    void Start()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.FindWithTag("Player");
        }
    }

    public void OnPlayerTakeDamage(int damage)
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.hearts -= damage;

        if (GameManager.Instance.hearts <= 0)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        if (fxManager != null && deadClip != null)
        {
            fxManager.SoundPlay(deadClip, 1);
        }

        if (playerObj != null)
        {
            playerObj.SetActive(false);
        }

        GameManager.Instance.lives--;
        GameManager.Instance.hearts = 100;

        if (GameManager.Instance.lives <= 0)
        {
            FindAnyObjectByType<SceneFlowManager>()?.TriggerGameOver();
            yield break;
        }

        yield return new WaitForSecondsRealtime(respawnDelay);

        if (playerObj != null)
        {
            playerObj.transform.position = spawnPosition;
            playerObj.SetActive(true);
        }
    }
}