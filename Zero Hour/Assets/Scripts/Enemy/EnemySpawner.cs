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
   
   
    [SerializeField] GameObject keyPrefab;
    [SerializeField] Transform keyTransform;
    [SerializeField] GameObject[] powerUpPrefabs;
    [SerializeField] float powerUpOffset = 1f;
   
   


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

    public void OnRoomCleared()
    {
        Instantiate(keyPrefab, keyTransform.position, Quaternion.identity);

        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        Vector3 powerUpPos = keyTransform.position + Vector3.right * powerUpOffset;
        Instantiate(powerUpPrefabs[randomIndex], powerUpPos, Quaternion.identity);
        GameManager.Instance.activeBlock.gameObject.SetActive(false);
        GameManager.Instance.door.gameObject.GetComponent<BoxCollider2D>().enabled = true;
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
        GameManager.Instance.isCleared = false;
        GameManager.Instance.activeSpawner = this;
        Debug.Log("Player in");
        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnEnemies();
        }
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
