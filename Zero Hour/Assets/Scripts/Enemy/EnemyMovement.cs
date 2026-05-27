using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType 
{
    Ground,
    Flying

}
 
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 3f;
    [SerializeField] Transform playerTransform;
    [SerializeField] EnemyType type;
 
    
    [SerializeField] ParticleSystem deathParticles;
 
    
    [SerializeField] float minDistance = 1.5f;
    [SerializeField] float tacklePower = 10f;
    [SerializeField] float attackCooldown = 2f;
    bool isAttacking = false;
    float lastAttackTime = -99f;
 
   
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float shootRange = 5f;
    [SerializeField] float shootCooldown = 2f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    int currentPoint = 0;
    float lastShootTime = -99f;
    bool isShooting = false;
 
    
    float distance;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = FindAnyObjectByType<PlayerMovement>().transform;
    }
 
    void Update()
    {
        if (!GameManager.Instance.IsPlayerAlive()) return;
 
        distance = Vector2.Distance(transform.position, playerTransform.position);
 
        switch (type)
        {
            case EnemyType.Ground:
                if (distance <= minDistance && !isAttacking &&
                    Time.time >= lastAttackTime + attackCooldown)
                    StartCoroutine(AttackRoutine());
                break;
 
            case EnemyType.Flying:
                if (distance <= shootRange && !isShooting &&
                    Time.time >= lastShootTime + shootCooldown)
                    StartCoroutine(ShootRoutine());
                break;
        }
    }
 
    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlayerAlive()) return;
         
        
        switch (type)
        {
            case EnemyType.Ground:
                if (isAttacking) return;
                FollowPlayer();
                break;
 
            case EnemyType.Flying:
                if (isShooting) return; 
                
                if (distance > shootRange)
                    Patrol();
                break;
               

        }
    }
 
   
 
    void FollowPlayer()
    {
        rb.MovePosition(Vector2.MoveTowards(
            rb.position,
            playerTransform.position,
            speed * Time.fixedDeltaTime
        ));
    }
 
   
 
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
 
        Vector2 dir = (playerTransform.position - transform.position).normalized;
 
       
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.2f;
            float squash = Mathf.Lerp(1f, 0.6f, t);
            transform.localScale = new Vector3(1f, squash, 1f);
            yield return null;
        }
 
       
        rb.AddForce(dir * tacklePower, ForceMode2D.Impulse);
 
        // Recover de escala
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.15f;
            float squash = Mathf.Lerp(0.6f, 1f, t);
            transform.localScale = new Vector3(1f, squash, 1f);
            yield return null;
        }
        transform.localScale = Vector3.one;
 
        
        if (distance <= minDistance)
            Retreat(dir);
 
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }
 
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isAttacking && col.gameObject.CompareTag("Player"))
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
    }
 
    void Retreat(Vector2 dir)
    {
        rb.AddForce(-dir * 2, ForceMode2D.Impulse);
    }
 
  
 
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;
 
        Transform target = patrolPoints[currentPoint];
        Vector2 direction = (target.position - transform.position).normalized;
 
        
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
 
        rb.MovePosition(Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.fixedDeltaTime
        ));
 
       
        if (Vector2.Distance(rb.position, target.position) < 0.1f)
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
 
    // ── AÉREO: DISPARO ───────────────────────────────────────────────
 
    IEnumerator ShootRoutine()
    {
        isShooting = true;
        lastShootTime = Time.time;
 
        // Squash horizontal de preparación
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.15f;
            float squash = Mathf.Lerp(1f, 0.8f, t);
            transform.localScale = new Vector3(squash, 1f / squash, 1f);
            yield return null;
        }
 
        Shoot();
 
        // Recover
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.1f;
            float squash = Mathf.Lerp(0.8f, 1f, t);
            transform.localScale = new Vector3(squash, 1f / squash, 1f);
            yield return null;
        }
        transform.localScale = Vector3.one;
 
        yield return new WaitForSeconds(0.3f);
 
        // Si el jugador ya no está en rango, volver a patrullar
        isShooting = false;
    }
 
    void Shoot()
    {
        Vector2 direction = (playerTransform.position - shootPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
 
    // ── MUERTE ───────────────────────────────────────────────────────
 
    public void Die()
    {
        if (deathParticles != null)
        {
            // Desparentar las partículas para que no se destruyan con el enemigo
            deathParticles.transform.SetParent(null);
            deathParticles.Play();
            Destroy(deathParticles.gameObject, deathParticles.main.duration);
        }
 
        GameManager.Instance.EnemyDied(gameObject);
        Destroy(gameObject);
    }
}