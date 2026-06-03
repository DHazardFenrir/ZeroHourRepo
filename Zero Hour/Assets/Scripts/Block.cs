using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] BoxCollider2D triggerBox;
    [SerializeField] GameObject block;
    [SerializeField] GameObject interiorDoor;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(BlockAfterTimer());
    }

    IEnumerator BlockAfterTimer()
    {
        yield return new WaitForSeconds(1.5f);
        triggerBox.isTrigger = false;
        block.SetActive(true);
        GameManager.Instance.activeBlock = this;
        interiorDoor.SetActive(true);
    }
}