using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerCorner = 1;
    [SerializeField] private float overlapCheckRadius = 0.5f;

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
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
    }
   
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player in");
        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnEnemies();
        }
    }
}
