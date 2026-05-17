using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorTrigger : MonoBehaviour
{
   
    [SerializeField] GameObject[] tilemapGameObjects;
    [SerializeField] BoxCollider2D[] interior;
    [SerializeField] Tilemap interiorTilemap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          foreach(var col in interior)
            {
                col.enabled = true;
            }

            interiorTilemap.gameObject.SetActive(true);

            foreach(var go in tilemapGameObjects)
            {
                go.SetActive(false);
            }

            this.gameObject.SetActive(false);
        }
    }
}
