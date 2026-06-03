using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class BossMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] BoxCollider2D arenaCollider;
    
    private BossHealth bossHealth;


    [Header("Lunge (Fase 1: Vida > 66%)")]
    [SerializeField] private float lungeSpeed = 15f;
    [SerializeField] private float lungeCooldown = 2f;
    private float lastLungeTime = -99f;
    private bool isLunging = false;

    [Header("Invocación (Fase 2: Vida > 33%)")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float summonCooldown = 5f;
    [SerializeField] private int enemiesPerSummon = 3;
    [SerializeField] private float summonRadius = 2f;
    private float lastSummonTime = -99f;

    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] float normalSize = 4f;
    [SerializeField] float summonSize = 6f;
    [SerializeField] float zoomDuration = 0.5f;

    [Header("Hacha Bumerán (Fase 3: Vida <= 33%)")]
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float axeCooldown = 3f;
    private float lastAxeTime = -99f;
    private bool waitingForReturn = false;
    [SerializeField] float speed = 5f;
    [SerializeField] float minDistance = 2f;
    private float distance;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bossHealth = GetComponent<BossHealth>();
        
        PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
       
        if (!GameManager.Instance.IsPlayerAlive()) return;

        float hp = bossHealth.HealthPercent;

        if (hp > 0.66f)
        {
            HandleLunge();
        }
        else if (hp > 0.33f)
        {
            HandleSummon();
        }
        else
        {
            HandleAxe();
        }

        

       distance = Vector2.Distance(this.transform.position, playerTransform.position);
            
    }

    

    void HandleLunge()
    {
        if (isLunging) return;
        if (Time.time < lastLungeTime + lungeCooldown) return;

        StartCoroutine(LungeRoutine());
    }

    IEnumerator LungeRoutine()
    {
        isLunging = true;
        lastLungeTime = Time.time;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.2f;
            transform.localScale = new Vector3(
                Mathf.Lerp(1f, 1.4f, t),
                Mathf.Lerp(1f, 0.6f, t),
                1f
            );
            yield return null;
        }

        
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        rb.AddForce(dir * lungeSpeed, ForceMode2D.Impulse);
        
        if (impulseSource != null) 
            impulseSource.GenerateImpulse(); 

        
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.15f;
            transform.localScale = new Vector3(
                Mathf.Lerp(1.4f, 1f, t),
                Mathf.Lerp(0.6f, 1f, t),
                1f
            );
            yield return null;
        }
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(0.5f);
        isLunging = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (isLunging && col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(2);
        }
    }

    Vector3 GetRandomPosInArena()
    {
        Bounds b = arenaCollider.bounds;
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            0f
        );
    }

    void HandleSummon()
    {
        if (Time.time < lastSummonTime + summonCooldown) return;

        StartCoroutine(SummonRoutine());
    }

    IEnumerator SummonRoutine()
    {
        lastSummonTime = Time.time;

    
        yield return StartCoroutine(ZoomCamera(summonSize));
    
        if (impulseSource != null) impulseSource.GenerateImpulse();
        yield return new WaitForSeconds(0.5f);

    for (int i = 0; i < enemiesPerSummon; i++)
    {
        Vector3 spawnPos = GetRandomPosInArena();
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        GameObject enemy = Instantiate(enemyPrefabs[randomEnemy], spawnPos, Quaternion.identity);
        GameManager.Instance.activeEnemies.Add(enemy);
        yield return new WaitForSeconds(0.3f);
    }

    
    yield return StartCoroutine(ZoomCamera(normalSize));
       
    }

       IEnumerator ZoomCamera(float targetSize)
    {
        float startSize = virtualCamera.Lens.OrthographicSize;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomDuration;
            var lens = virtualCamera.Lens;
            lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);
            virtualCamera.Lens = lens;
            yield return null;
        }
    }

   
        void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlayerAlive()) return;
        
        float hp = bossHealth.HealthPercent;
        
       
        if (hp <= 0.33f)
            FollowPlayer();
    }
    void FollowPlayer()
    {
        
        Vector2 target = playerTransform.position;
            if (distance > minDistance)
        {
            rb.MovePosition(Vector2.MoveTowards(
                rb.position, 
                target, 
                speed * Time.fixedDeltaTime
            ));
        }
        
    }
    void HandleAxe()
    {
        if (waitingForReturn) return; 
        if (Time.time < lastAxeTime + axeCooldown) return;

        ThrowAxe();
    }

    void ThrowAxe()
    {
        
        lastAxeTime = Time.time;
        waitingForReturn = true;

        
        Vector2 dir = (playerTransform.position - throwPoint.position).normalized;
        GameObject axe = Instantiate(axePrefab, throwPoint.position, Quaternion.identity);

       
        AxeProjectile axeScript = axe.GetComponent<AxeProjectile>();
        if (axeScript != null)
        {
            axeScript.Launch(dir, transform); 
            
            axeScript.OnReturn = () => waitingForReturn = false; 
        }
    }
}