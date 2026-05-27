using System;
using UnityEngine;
public enum FireMode
{
    Single,
    Double,
    Triple,
    Fan,
    Charged
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float horizontal;
    [SerializeField] float vertical;
    [SerializeField] float speed = 5;
    [SerializeField] GameObject bulletObj;
    [SerializeField] GameObject shootPoint;
    [SerializeField] float lastAttackTime = 0f;
    [SerializeField] float bulletDamage = 1f;
    [SerializeField] float chargedFireRate = 1f;  
    [SerializeField] float chargedDamage = 5f;     
    [SerializeField] float normalFireRate = 0.5f; 
    [SerializeField] float fireRate;

    private FireMode currentFireMode = FireMode.Single;
    public bool playerHasKey = false;
    public Vector2 MoveDirection { get; private set; }
    
    [SerializeField] ParticleSystem dustParticles;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        fireRate = normalFireRate;
    }

    void Update()
    {
       horizontal = Input.GetAxisRaw("Horizontal");
       vertical = Input.GetAxisRaw("Vertical");
    MoveDirection = new Vector2(horizontal, vertical);

    if (horizontal > 0)
        transform.localScale = new Vector3(1, 1, 1);
    else if (horizontal < 0)
        transform.localScale = new Vector3(-1, 1, 1);

    if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime)
        Shoot();
    }

    void FixedUpdate()
    {
        Move();
    }
    public void SetFireMode(FireMode mode)
    {
        currentFireMode = mode;
        
        fireRate = mode == FireMode.Charged ? chargedFireRate : normalFireRate;
    }

    void SpawnBullet(float angleOffset, float damage)
    {
       
        Vector2 baseDir = shootPoint.transform.right * Mathf.Sign(transform.localScale.x);

       
        float angle = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;
        angle += angleOffset * Mathf.Sign(transform.localScale.x); 
        Vector2 finalDir = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        GameObject bullet = Instantiate(
            bulletObj,
            shootPoint.transform.position,
            Quaternion.identity
        );

        Bullet b = bullet.GetComponent<Bullet>();
        b.SetDirection(finalDir);
        b.SetDamage(damage);

        Physics2D.IgnoreCollision(
            bullet.GetComponent<Collider2D>(),
            GetComponent<Collider2D>()
        );
    }
    void Move()
    {
        Vector2 move = new Vector2(horizontal, vertical).normalized;
        if(move.magnitude > 0)
        {
            dustParticles.Play();
        }else if(move.magnitude < 0)
        {
            dustParticles.Stop();
        }
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
       switch (currentFireMode)
    {
        case FireMode.Single:
            SpawnBullet(0f, bulletDamage);
            break;

        case FireMode.Double:
            SpawnBullet(15f, bulletDamage);
            SpawnBullet(-15f, bulletDamage);
            break;

        case FireMode.Triple:
            SpawnBullet(0f, bulletDamage);
            SpawnBullet(20f, bulletDamage);
            SpawnBullet(-20f, bulletDamage);
            break;

        case FireMode.Fan:
            SpawnBullet(0f, bulletDamage);
            SpawnBullet(30f, bulletDamage);
            SpawnBullet(-30f, bulletDamage);
            SpawnBullet(60f, bulletDamage);
            SpawnBullet(-60f, bulletDamage);
            break;

        case FireMode.Charged:
            SpawnBullet(0f, chargedDamage);
            break;
    }

    lastAttackTime = Time.time + fireRate;
    }
}