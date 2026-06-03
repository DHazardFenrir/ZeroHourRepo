using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Menu menus;

    private PlayerHealth currHealth;

    public List<GameObject> activeEnemies = new List<GameObject>();
    public EnemySpawner activeSpawner;
    public Block activeBlock;
    public GameObject door;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterPlayer(PlayerHealth player)
    {
        currHealth = player;
    }

    public void OnPlayerDied()
    {
        currHealth = null;
        menus.ShowGameOver();
    }

    public bool IsPlayerAlive()
    {
        return currHealth != null;
    }

    public void EnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        if (activeEnemies.Count <= 0)
        {
            if (activeSpawner != null && activeSpawner.gameObject.activeSelf)
                activeSpawner.OnRoomCleared();
            activeSpawner = null;
            
            
            if (activeBlock != null)
            {
                activeBlock.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void GameOver()
    {
        menus.ShowGameOver();
    }

    
    public void OnBossDefeated()
    {
        
            Debug.LogWarning("¡Boss derrotado! Pero no se encontró la referencia a 'menus' en el GameManager.");
        
    }
}