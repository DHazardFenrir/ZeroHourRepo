using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;

public enum InteriorType { BoxCollider, Tilemap }
public enum InteriorState { ENTER, EXIT }

public class InteriorZone : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] GameObject[] exteriorObjects;
    [SerializeField] Tilemap interiorTilemap;

    [Header("Tipo")]
    [SerializeField] InteriorType interiorType;

    [SerializeField] BoxCollider2D[] activeOnEnter;
    [SerializeField] BoxCollider2D[] activeOnExit;

    [SerializeField] TilemapCollider2D[] tilemapActiveOnEnter;
    [SerializeField] TilemapCollider2D[] tilemapActiveOnExit;

    [Header("Config")]
    [SerializeField] float restoreDelay = 2.5f;
    [SerializeField] int sortingOrderInside = 8;
    [SerializeField] int sortingOrderWeapon = 9;
    [SerializeField] int sortingOrderOutside = 2;

    private int previousSortingOrder;
    private int previousWeaponSortingOrder;
    public InteriorState intState = InteriorState.ENTER;

    private PlayerHealth player;
    private bool playerInside = false;
    private EnemySpawner localSpawner;

    // Llamado por EnemySpawner cuando el jugador entra a la sala
    public void SetSpawner(EnemySpawner spawner)
    {
        localSpawner = spawner;
        localSpawner.OnRoomClearedEvent += OnSalaLimpia;
    }

    void OnSalaLimpia()
    {
        intState = InteriorState.EXIT;

        // Activar puerta de salida
        foreach (var col in activeOnExit) col.enabled = true;

        // Desactivar bloque de entrada
        if (GameManager.Instance.activeBlock != null)
            GameManager.Instance.activeBlock
                .GetComponent<BoxCollider2D>().enabled = false;

        // Desuscribirse
        if (localSpawner != null)
            localSpawner.OnRoomClearedEvent -= OnSalaLimpia;
    }

    void OnDestroy()
    {
        if (localSpawner != null)
            localSpawner.OnRoomClearedEvent -= OnSalaLimpia;
    }

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
    if (!collision.CompareTag("Player")) return;
    if (!playerInside) return;

    switch (interiorType)
    {
        case InteriorType.BoxCollider:
            if (intState == InteriorState.EXIT)
            {
                playerInside = false;
                StartCoroutine(ExitRoutine());
            }
            break;

        case InteriorType.Tilemap:
            // Tilemap no tiene sala que limpiar — siempre puede salir
            playerInside = false;
            StartCoroutine(ExitRoutine());
            break;
    }
}

    void Enter()
    {
        // Guardar sorting orders anteriores
        previousSortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder;
        foreach (var sprite in player.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sprite.gameObject != player.gameObject)
            {
                previousWeaponSortingOrder = sprite.sortingOrder;
                break;
            }
        }

        // Aplicar sorting orders del interior
        player.GetComponent<SpriteRenderer>().sortingOrder = sortingOrderInside;
        foreach (var sprite in player.GetComponentsInChildren<SpriteRenderer>())
            if (sprite.gameObject != player.gameObject)
                sprite.sortingOrder = sortingOrderWeapon;

        // Tiles
        foreach (var go in exteriorObjects) go.SetActive(false);
        if (interiorTilemap != null) interiorTilemap.gameObject.SetActive(true);

        // Colisiones
        switch (interiorType)
        {
            case InteriorType.BoxCollider:
                foreach (var col in activeOnEnter) col.enabled = true;
                foreach (var col in activeOnExit) col.enabled = false; // bloquear salida
                break;

            case InteriorType.Tilemap:
                foreach (var col in tilemapActiveOnEnter) col.enabled = true;
                foreach (var col in tilemapActiveOnExit) col.enabled = false;
                
                break;
        }

        GameManager.Instance.door = this.gameObject;
    }

   IEnumerator ExitRoutine()
{
    yield return new WaitForSeconds(restoreDelay);

    // Si el jugador volvió a entrar durante el delay, cancelar
    if (playerInside) yield break;

    // Restaurar sorting orders
    if (player != null)
    {
        player.GetComponent<SpriteRenderer>().sortingOrder = previousSortingOrder;
        foreach (var sprite in player.GetComponentsInChildren<SpriteRenderer>())
            if (sprite.gameObject != player.gameObject)
                sprite.sortingOrder = previousWeaponSortingOrder;
    }

    // Restaurar tiles y colisiones
    foreach (var go in exteriorObjects) go.SetActive(true);
    if (interiorTilemap != null) interiorTilemap.gameObject.SetActive(false);

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
}
}