using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Exit : MonoBehaviour
{
    public TilemapCollider2D
      [] collisions;
    public GameObject player;
    [SerializeField] TilemapCollider2D lineCollision;
    public GameObject enter;

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.gameObject.CompareTag("Player"))
        {
            foreach (var collision in collisions)
            {

                collision.enabled = true;
                player.GetComponent<SpriteRenderer>().sortingOrder = 1;

            }
            foreach( SpriteRenderer sr in player.GetComponentsInChildren<SpriteRenderer>())
            {
                if(sr.gameObject != player)
                {
                    sr.sortingOrder = 2;
                }
              
            }
           
           
            lineCollision.enabled = false;
            if (!gameObject.activeInHierarchy || !enabled)
                return;
           StartCoroutine(TimedActivation());
            
        }

    }

    IEnumerator TimedActivation()
    {

        yield return new WaitForSeconds(3f);
        enter.SetActive(true);
        this.gameObject.SetActive(false);

    }


}



