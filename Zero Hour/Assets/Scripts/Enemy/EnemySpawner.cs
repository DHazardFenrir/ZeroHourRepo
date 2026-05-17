using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerCorner = 1;
    [SerializeField] private float overlapCheckRadius = 0.5f;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool spawnKey = false;
    [SerializeField] GameObject keyPrefab;
    [SerializeField] Transform keyTransform;
    [SerializeField] GameObject exit;
    [SerializeField] GameObject block;


    private Vector2[] GetCorners()
    {
        Bounds b = spawnArea.bounds;
        return new Vector2[]
        {
            new Vector2(b.min.x, b.min.y), // bottom-left
            new Vector2(b.max.x, b.min.y), // bottom-right
            new Vector2(b.min.x, b.max.y), // top-left
            new Vector2(b.max.x, b.max.y)  // top-right
        };
    }

    public void SpawnEnemies()
    {
        Vector2[] corners = GetCorners();

        foreach (Vector2 corner in corners)
        {
            for (int i = 0; i < enemiesPerCorner; i++)
            {
                TrySpawnAt(corner);
            }
        }
    }

    private void TrySpawnAt(Vector2 position)
    {
       LayerMask enemyLayer = LayerMask.GetMask("Enemy");
    Collider2D hit = Physics2D.OverlapCircle(position, overlapCheckRadius, enemyLayer);
    
    if (hit == null)
    {
       GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            activeEnemies.Add(enemy);
            enemy.GetComponent<EnemyHealth>().SetSpawner(this);
    }
    }
   public void EnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if(activeEnemies.Count <= 0 && !spawnKey)
        {
            SpawnKey();
        }
    }

    void SpawnKey()
    {
        spawnKey = true;
        Instantiate(keyPrefab, keyTransform.position, Quaternion.identity);
        Debug.Log("Llave spawn");
        exit.SetActive(true);
        block.SetActive(false);
        Debug.Log("Sala despejada");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in");
        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnEnemies();
        }
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
