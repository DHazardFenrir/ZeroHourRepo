
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isPlayerAlive;
    public  PlayerHealth currHealth;
    [SerializeField] Menu menus;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public bool isCleared = false;
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
    public void OnPlayerDied()
    {
        currHealth = null;
        menus.ShowGameOver();
    }
    public void RegisterPlayer(PlayerHealth player)
    {
        currHealth = player;
    }
    public bool IsPlayerAlive()
    {
        if(currHealth) return true;
        return false; 
    }
   

    public void EnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        if (activeEnemies.Count <= 0)
        {
            isCleared = true;
            if (activeSpawner != null && activeSpawner.gameObject.activeSelf)
                activeSpawner.OnRoomCleared();
            activeSpawner = null;
        }
    }
}
