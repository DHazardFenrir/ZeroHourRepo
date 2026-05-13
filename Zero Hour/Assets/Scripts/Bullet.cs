using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float bulletLifetime = 15f;
    [SerializeField] float damage = 1f;

    private Vector2 direction = Vector2.right;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, bulletLifetime);
    }
    private void Start()
    {
        BulletMove();
    }
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    

    private void BulletMove()
    {
        rb.linearVelocity = direction * speed;
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