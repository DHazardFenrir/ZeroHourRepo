using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpawnerStatus { CLEARED, NOTCLEARED }

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerCorner = 1;
    [SerializeField] private float overlapCheckRadius = 0.5f;
    [SerializeField] GameObject keyPrefab;
    [SerializeField] Transform keyTransform;
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] float powerUpOffset = 1f;

   HealthSpawner healthSpawner;
    public SpawnerStatus status = SpawnerStatus.NOTCLEARED;
    public event Action OnRoomClearedEvent;

    void OnEnable()
    {
       healthSpawner = FindAnyObjectByType<HealthSpawner>();
    }

    private Vector2[] GetCorners()
    {
        Bounds b = spawnArea.bounds;
        return new Vector2[]
        {
            new Vector2(b.min.x, b.min.y),
            new Vector2(b.max.x, b.min.y),
            new Vector2(b.min.x, b.max.y),
            new Vector2(b.max.x, b.max.y)
        };
    }

    public void SpawnEnemies()
    {
        Vector2[] corners = GetCorners();
        foreach (Vector2 corner in corners)
            for (int i = 0; i < enemiesPerCorner; i++)
                TrySpawnAt(corner);
    }

    public void OnRoomCleared()
    {
        Instantiate(keyPrefab, keyTransform.position, Quaternion.identity);

        if (powerUpPrefabs.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, powerUpPrefabs.Length);
            Vector3 powerUpPos = keyTransform.position + Vector3.right * powerUpOffset;
            Instantiate(powerUpPrefabs[randomIndex], powerUpPos, Quaternion.identity);
        }

        status = SpawnerStatus.CLEARED;
        OnRoomClearedEvent?.Invoke(); 
        healthSpawner.SpawnLoot();
        this.gameObject.SetActive(false);
    }

    private void TrySpawnAt(Vector2 position)
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D hit = Physics2D.OverlapCircle(position, overlapCheckRadius, enemyLayer);
        if (hit == null)
        {
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            GameManager.Instance.activeEnemies.Add(enemy);
            enemy.GetComponent<EnemyHealth>().SetSpawner(this);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.activeSpawner = this;

        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnEnemies();

            // Registrar este spawner en el InteriorZone activo
            // sin depender del orden de ejecución
            if (GameManager.Instance.door != null)
                GameManager.Instance.door
                    .GetComponent<InteriorZone>()
                    ?.SetSpawner(this);
        }

        GetComponent<BoxCollider2D>().enabled = false;
    }
}