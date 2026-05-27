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
  [SerializeField] ParticleSystem bloodParticles;
    private EnemySpawner spawner;
    public void SetSpawner(EnemySpawner s) => spawner = s;
 
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        impulseSource  = GameObject.FindWithTag("Player").GetComponent<CinemachineImpulseSource>();
         bloodParticles = GetComponentInChildren<ParticleSystem>();
    }
   

   
  IEnumerator FlashDamage()
    {
        sr.color = Color.red;
         ScaleDamage();
        yield return  new WaitForSeconds(0.2f);
        transform.localScale = new Vector3(1,1,1);

        sr.color = Color.white;
       
       
    }
   
   void Die()
    {
        GameManager.Instance.EnemyDied(gameObject);
         Destroy(this.gameObject);
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
         bloodParticles?.Play();
       
       if(health <= 0)
        {
            if(spawner != null)
            {
                Die();
           
            } 
            Destroy(gameObject);
           
        }
       
        
        
    }
}

