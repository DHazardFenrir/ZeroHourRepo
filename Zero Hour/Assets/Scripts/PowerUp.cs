using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] FireMode fireMode;
    [SerializeField] bool affectFireMode;
    [SerializeField] HealthPowerUp health;
    [SerializeField] float healthAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!affectFireMode)
        {
            other.GetComponent<PlayerHealth>().SetHealth(health, healthAmount);
        }
        other.GetComponent<PlayerMovement>().SetFireMode(fireMode);
        Destroy(gameObject);
    }
}
