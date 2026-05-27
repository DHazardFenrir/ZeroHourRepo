using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float bulletLifetime = 15f;
    [SerializeField] float damage = 1f;

    private Vector2 direction;

    void Awake()
    {
    Debug.Log("Parent de la bala: " + (transform.parent != null ? transform.parent.name : "ninguno"));
    Destroy(gameObject, bulletLifetime);
    }   

    public void SetDirection(Vector2 dir)
{
    direction = dir.normalized;
    Debug.Log("SetDirection llamado con: " + direction);
}

void Update()
{


    if (direction == Vector2.zero)
        Debug.Log("direction es ZERO en update");
    else
        Debug.Log("Moviendo hacia: " + direction);
        
    transform.Translate(direction * speed * Time.deltaTime, Space.World);

}
    public void SetDamage(float d)
    {
        damage = d;
    }

   
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") )
    {
        collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        Destroy(gameObject);
    }
        if (collision.gameObject.CompareTag("Player"))
    {
        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        Destroy(gameObject);
    }
    }


}