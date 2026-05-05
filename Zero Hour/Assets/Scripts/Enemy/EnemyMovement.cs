using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
     [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed = 3f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float minDistance = 1.5f;
    [SerializeField] float tacklePower = 10f;
    [SerializeField] float attackCooldown = 2f;

    float distance;
    bool isAttacking = false;
    float lastAttackTime = -99f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (isAttacking) return; // no mover mientras ataca
        FollowPlayer();
    }

    void Update() {
        distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= minDistance && !isAttacking && Time.time >= lastAttackTime + attackCooldown) {
            StartCoroutine(AttackRoutine());
        }
    }

    void FollowPlayer() {
        rb.MovePosition(Vector2.MoveTowards(rb.position, playerTransform.position, speed * Time.fixedDeltaTime));
    }


    IEnumerator AttackRoutine() {
        isAttacking = true;
        lastAttackTime = Time.time;
       
              // 1. Squash — preparación
        Vector2 dir = (playerTransform.position - transform.position).normalized;
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime / 0.2f; // 0.2s de squash
            float squash = Mathf.Lerp(1f, 0.6f, t);
            transform.localScale = new Vector3(1f, squash, 1f);
            yield return null;
        }

        // 2. Tackle — impulso hacia el jugador
        rb.AddForce(dir * tacklePower, ForceMode2D.Impulse);

        // 3. Recover — vuelve a escala normal mientras viaja
        t = 0f;
        while (t < 1f) {
            t += Time.deltaTime / 0.15f; // 0.15s de recover
            float squash = Mathf.Lerp(0.6f, 1f, t);
            transform.localScale = new Vector3(1f, squash, 1f);
            yield return null;
        }
        transform.localScale = Vector3.one;

        // 4. Espera a que termine el impulso antes de seguir caminando
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
        
      
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (isAttacking && col.gameObject.CompareTag("Player")) {
            col.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(1);
        }
    }
}
