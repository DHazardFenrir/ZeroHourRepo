using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;
   public enum InteriorType
{
    BoxCollider,  // para interiores normales
    Tilemap       // para subir/bajar niveles
}
public class InteriorZone : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] GameObject[] exteriorObjects;
    [SerializeField] Tilemap interiorTilemap;

    [Header("Tipo")]
    [SerializeField] InteriorType interiorType;

    // Colisiones BoxCollider (interiores)
    [SerializeField] BoxCollider2D[] activeOnEnter;
    [SerializeField] BoxCollider2D[] activeOnExit;

    // Colisiones Tilemap (subir/bajar)
    [SerializeField] TilemapCollider2D[] tilemapActiveOnEnter;
    [SerializeField] TilemapCollider2D[] tilemapActiveOnExit;
  

    [Header("Config")]
    [SerializeField] float restoreDelay = 2.5f;
    [SerializeField] int sortingOrderInside = 8;
    [SerializeField] int sortingOrderWeapon = 9;
    [SerializeField] int sortingOrderWeaponNormal = 3;
    [SerializeField] int sortingOrderOutside = 2;
    private int previousSortingOrder;
    private int previousWeaponSortingOrder;

    private PlayerHealth player;
    private bool playerInside = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (playerInside) return;

        player = collision.GetComponent<PlayerHealth>();
        playerInside = true;
        Enter();
       
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.Instance.isCleared)
        {
            if (!collision.CompareTag("Player")) return;
            if (!playerInside) return;
          

            playerInside = false;
            StartCoroutine(ExitRoutine());
        }
      
    }

    void Enter()
    {
        previousSortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder;
    
    SpriteRenderer[] sprites = player.GetComponentsInChildren<SpriteRenderer>();
    foreach (var sprite in sprites)
    {
        if (sprite.gameObject != player.gameObject)
        {
            previousWeaponSortingOrder = sprite.sortingOrder;
            break; 
        }
    }

   
    player.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderInside;
    foreach (var sprite in player.GetComponentsInChildren<SpriteRenderer>())
    {
        if (sprite.gameObject != player.gameObject)
            sprite.sortingOrder = sortingOrderWeapon;
    }
        foreach (var go in exteriorObjects)
            go.SetActive(false);

        if(interiorTilemap != null) interiorTilemap.gameObject.SetActive(true);

        switch (interiorType)
        {
            case InteriorType.BoxCollider:
                foreach (var col in activeOnEnter) col.enabled = true;
                foreach (var col in activeOnExit) col.enabled = false;
                break;

            case InteriorType.Tilemap:
                foreach (var col in tilemapActiveOnEnter) col.enabled = true;
                foreach (var col in tilemapActiveOnExit) col.enabled = false;
                break;
        }


        if (player != null)
            player.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderInside;
        
        SpriteRenderer[] spriteRenderersInPlayer = player.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sprite in spriteRenderersInPlayer)
        {
            if(sprite.gameObject != player)
            {
                sprite.sortingOrder = sortingOrderWeapon;
            }
        }
        GameManager.Instance.door = this.gameObject;
    }
   

    IEnumerator ExitRoutine()
    {
        yield return new WaitForSeconds(restoreDelay);
        
        if (player != null)
            {
                player.GetComponent<SpriteRenderer>().sortingOrder = previousSortingOrder;
                foreach (var sprite in player.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (sprite.gameObject != player.gameObject)
                        sprite.sortingOrder = previousWeaponSortingOrder;
                }
            }

        foreach (var go in exteriorObjects)
            go.SetActive(true);

         if(interiorTilemap != null) interiorTilemap.gameObject.SetActive(false);
        switch (interiorType)
            {
                case InteriorType.BoxCollider:
                    foreach (var col in activeOnEnter) col.enabled = false;
                    foreach (var col in activeOnExit) col.enabled = true;
                    break;

                case InteriorType.Tilemap:
                    foreach (var col in tilemapActiveOnEnter) col.enabled = false;
                    foreach (var col in tilemapActiveOnExit) col.enabled = true;
                    break;
            }

        if (player != null)
            player.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderOutside;

        SpriteRenderer[] spriteRenderersInPlayer = player.GetComponentsInChildren<SpriteRenderer>();
        foreach(var sprite in spriteRenderersInPlayer)
        {
            if(sprite.gameObject != player)
            {
                sprite.sortingOrder = sortingOrderWeaponNormal;
            }
        }
    }
}
