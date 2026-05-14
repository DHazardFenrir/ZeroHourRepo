using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
 [SerializeField] float health = 5f;
 [SerializeField] SpriteRenderer sr;
  public CinemachineImpulseSource impulseSource;
    private EnemySpawner spawner;
    public void SetSpawner(EnemySpawner s) => spawner = s;
 
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
   

   
  IEnumerator FlashDamage()
    {
        sr.color = Color.red;
         ScaleDamage();
        yield return  new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(1,1,1);

        sr.color = Color.white;
       
       
    }

    void ScaleDamage()
    {
        transform.localScale = new Vector3(1, 0.8f,1f);
    }
    public void TakeDamage(float damage)
    {
        health-=damage;
        impulseSource.GenerateImpulse();
        StartCoroutine(FlashDamage());
       
        
        if(health <= 0)
        {
            spawner.EnemyDied(gameObject);
            Destroy(this.gameObject);
        }
    }
}

