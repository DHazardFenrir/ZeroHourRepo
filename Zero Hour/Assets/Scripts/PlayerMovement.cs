using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField]float horizontal;
    [SerializeField] float vertical; 
    [SerializeField] float speed = 5;
    [SerializeField] GameObject bulletObj;
    [SerializeField] GameObject shootPoint;
    [SerializeField] float lastAttackTime = 0f;
    [SerializeField] float fireRate = 0.5f;


    public Vector2 MoveDirection{get; set;}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
     horizontal = Input.GetAxisRaw("Horizontal");
    vertical = Input.GetAxisRaw("Vertical");
   MoveDirection = new Vector2(horizontal, vertical);

if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime) {
        Shoot();
        lastAttackTime = Time.time + fireRate;
    }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
       
        Vector2 move = new Vector2(horizontal, vertical).normalized;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);

    }

    void Shoot()
    {
        Instantiate(bulletObj, shootPoint.transform.position, shootPoint.transform.rotation);
       
    }
}
