using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] bool isBossKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            
            if (player != null)
            {
                if (isBossKey)
                {
                    Inventory.hasBossKey = true;
                    Debug.Log("¡Recogiste la LLAVE DEL BOSS!");
                }
                else
                {
                    Inventory.hasNormalKey = true;
                    Debug.Log("¡Recogiste una llave normal!");
                }

                
                player.SynchronizeInventory();

                Destroy(gameObject);
            }
        }
    }
}