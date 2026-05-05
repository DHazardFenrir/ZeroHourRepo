using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float bulletLifetime = 15f;
    [SerializeField] float damage = 1f;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Move();
        Destroy(gameObject, bulletLifetime);
        
    }


    void Move()
    {
        rb.linearVelocity = transform.right * speed;
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
         if (collision.gameObject.CompareTag("Enemy")) 
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    
}
