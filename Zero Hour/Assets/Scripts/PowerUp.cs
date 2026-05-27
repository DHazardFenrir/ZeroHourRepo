using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] FireMode fireMode;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerMovement>().SetFireMode(fireMode);
        Destroy(gameObject);
    }
}
