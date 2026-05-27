using UnityEngine;

public class OpenCell : MonoBehaviour
{
 [SerializeField] BoxCollider2D myBoxCollider;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().playerHasKey)
            {
                myBoxCollider.gameObject.SetActive(false);
                collision.gameObject.GetComponent<PlayerMovement>().playerHasKey = false;
                Inventory.hasKey = false;
            }
        }
    }
}
