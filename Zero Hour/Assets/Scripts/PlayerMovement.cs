using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float horizontal;
    [SerializeField] float vertical;
    [SerializeField] float speed = 5;
    [SerializeField] GameObject bulletObj;
    [SerializeField] GameObject shootPoint;
    [SerializeField] float lastAttackTime = 0f;
    [SerializeField] float fireRate = 0.5f;
    public bool playerHasKey = false;
    public Vector2 MoveDirection { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
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
        {
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
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mouseWorld - shootPoint.transform.position).normalized;

        GameObject bullet = Instantiate(bulletObj, shootPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(shootDir);
        Physics2D.IgnoreCollision(
            bullet.GetComponent<Collider2D>(),
            GetComponent<Collider2D>()
        );
    }
}