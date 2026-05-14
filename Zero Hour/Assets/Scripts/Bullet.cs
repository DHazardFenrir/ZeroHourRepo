using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float bulletLifetime = 15f;
    [SerializeField] float damage = 1f;

    private Vector2 moveDirection;

    void Awake()
    {
        Destroy(gameObject, bulletLifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
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