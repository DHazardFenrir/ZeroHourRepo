using UnityEngine;
using System;

public class AxeProjectile : MonoBehaviour
{
    [SerializeField] float speed = 8f;
    [SerializeField] float maxDistance = 4f;
    [SerializeField] float returnSpeed = 10f;
    [SerializeField] float damage = 2f;
    [SerializeField] float rotationSpeed = 720f; 

    private Vector2 direction;
    private Transform bossTransform;
    private Vector3 startPosition;
    private bool returning = false;

    
    public Action OnReturn; 

    public void Launch(Vector2 dir, Transform boss)
    {
        direction = dir;
        bossTransform = boss;
        startPosition = transform.position;
    }

    void Update()
    {
       
        if (bossTransform == null)
        {
            Destroy(gameObject);
            return; 
        }

        // Efecto visual: rotación constante en el eje Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (!returning)
        {
            
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, startPosition) >= maxDistance)
                returning = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                bossTransform.position,
                returnSpeed * Time.deltaTime
            );

           
            if (Vector3.Distance(transform.position, bossTransform.position) < 0.3f)
            {
                OnReturn?.Invoke(); 
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }
}