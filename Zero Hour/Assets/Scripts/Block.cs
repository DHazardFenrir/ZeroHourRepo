using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]BoxCollider2D triggerBox;
    [SerializeField]GameObject[] blocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(BlockAfterTimer());

        }
    }

    IEnumerator BlockAfterTimer()
    {
        yield return new WaitForSeconds(2.5f);
        triggerBox.isTrigger = false;
        foreach( var block in blocks)
        {
            block.SetActive(true);
        }
    }
}
