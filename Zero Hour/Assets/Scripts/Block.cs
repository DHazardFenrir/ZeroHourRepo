using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]BoxCollider2D triggerBox;
    [SerializeField]GameObject block;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.isCleared)
        {
            StartCoroutine(BlockAfterTimer());

        }

    }

    IEnumerator BlockAfterTimer()
    {
        yield return new WaitForSeconds(2.5f);
        triggerBox.isTrigger = false;
        block.SetActive(true);
        GameManager.Instance.activeBlock = this;
        
    }

   

}
