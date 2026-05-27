using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 5;
    [SerializeField] float currentHealth;
    [SerializeField] float invencibilityDuration = 0.5f;
    private float invencibilityTimer;
    public CinemachineImpulseSource impulseSourcePlayer;
    public SpriteRenderer sr;
    [SerializeField]private Color tmp;
     public Image[] healthBar;
     [SerializeField] ParticleSystem bloodParticles;
     
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        tmp = sr.color;
       
        GameManager.Instance.RegisterPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(invencibilityTimer > 0)
        {
            Debug.Log("I am invencible!");
            Physics.IgnoreLayerCollision(0,6);
            Debug.Log(tmp.a);
            invencibilityTimer -= Time.deltaTime;
            Debug.Log(tmp.a);
        }
       

        for(int i =0; i < healthBar.Length; i++)
        {
            if(i < currentHealth-1)
            {
                healthBar[i].enabled = true; 
            }else if (i > currentHealth-1)
            {
                healthBar[i].enabled = false;
            }
            
        }
    }
IEnumerator Transparent()
    {
    tmp.a = 0.3f;
    sr.color = tmp;
    yield return new WaitForSeconds(invencibilityDuration);
    tmp.a = 1f;
    sr.color = tmp;
    }
     public  void TakeDamage(float damage)
    {
        if(invencibilityTimer >0) return;
        bloodParticles.Play();
        currentHealth-=damage;
        impulseSourcePlayer.GenerateImpulse();
         invencibilityTimer = invencibilityDuration;
         StartCoroutine(Transparent());
        if (currentHealth <= 0)
        {
        GameManager.Instance.OnPlayerDied();
        Destroy(gameObject);
        }
    }
}
