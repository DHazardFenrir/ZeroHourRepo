using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enter : MonoBehaviour
{
    public TilemapCollider2D
        [] collisions;
    public GameObject player;
    [SerializeField] TilemapCollider2D lineCollision;
    public GameObject exit;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            {
            foreach (var tilemapCol in collisions)
            {

                tilemapCol.enabled = false;
                player.GetComponent<SpriteRenderer>().sortingOrder = 8;

            }

            foreach( var sr in player.GetComponentsInChildren<SpriteRenderer>())
            {
                if (sr.gameObject != player)
                {
                    sr.sortingOrder = 9;
                }
            }
            lineCollision.enabled = true;
            StartCoroutine(TimedActivation());
           
          
        }

        IEnumerator TimedActivation()
        {
           
                yield return new WaitForSeconds(2f);
               exit.SetActive(true);
           
                this.gameObject.SetActive(false);

        }

    }
}
