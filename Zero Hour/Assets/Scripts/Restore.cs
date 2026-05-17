using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
public class Restore : MonoBehaviour
{
    [SerializeField]GameObject[] tilemapGameObjects;
    [SerializeField] BoxCollider2D[] interior;
    [SerializeField] Tilemap interiorTilemap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
          
        StartCoroutine(BlockAfterTimer());
           
        }
    }

     IEnumerator BlockAfterTimer()
    {
        yield return new WaitForSeconds(2.5f);
        foreach(var col in interior)
            {
                col.enabled = false;
            }

            interiorTilemap.gameObject.SetActive(false);

            foreach(var go in tilemapGameObjects)
            {
                go.SetActive(true);
            }
        this.gameObject.SetActive(false);
        
    }
}
