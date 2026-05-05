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
 
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            
            Destroy(this.gameObject);
        }
    }
}

