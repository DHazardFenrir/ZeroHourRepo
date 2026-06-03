using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] bool cellForBossKey; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player == null) return;

          
            if (cellForBossKey && Inventory.hasBossKey)
            {
                OpenDoor(player);
                TimeManager.Instance.SaveEscapeTime();
            }
           
            else if (!cellForBossKey && Inventory.hasNormalKey)
            {
                OpenDoor(player);
            }
            else
            {
                Debug.Log(cellForBossKey ? "Esta puerta requiere la llave del Boss." : "Esta puerta requiere una llave normal.");
            }
        }
    }

    void OpenDoor(PlayerMovement player)
    {
        Debug.Log("¡Llave correcta! Abriendo puerta física por colisión.");

       
        if (cellForBossKey)
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