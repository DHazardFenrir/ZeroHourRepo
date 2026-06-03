using UnityEngine;

public class OpenCell : MonoBehaviour
{
    [SerializeField] bool requiresBossKey; 


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player == null) return;

            Debug.Log($"[Choque] ¿Requiere Boss Key?: {requiresBossKey} | Inventario Boss: {Inventory.hasBossKey} | Inventario Normal: {Inventory.hasNormalKey}");

            if (requiresBossKey && Inventory.hasBossKey)
            {
                OpenTheDoor(player);
                 TimeManager.Instance.escapeMenuVictory.SetActive(true);
            }
            else if (!requiresBossKey && Inventory.hasNormalKey)
            {
                OpenTheDoor(player);
            }
            else
            {
                Debug.Log("Intento de apertura fallido: No tienes la llave correcta.");
            }
        }
    }

    void OpenTheDoor(PlayerMovement player)
    {
        Debug.Log("¡Llave correcta! La celda se abre por completo.");
        
        if (requiresBossKey)
        {
            Inventory.hasBossKey = false;
        }
        else
        {
            Inventory.hasNormalKey = false;
        }

        player.SynchronizeInventory();

        this.gameObject.SetActive(false);
    }
}