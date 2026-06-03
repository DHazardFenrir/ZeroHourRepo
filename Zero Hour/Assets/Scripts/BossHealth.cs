using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.UI; // 👈 Recuerda dejar esto para el arreglo de Image

public class BossHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 20f;
    private float currentHealth;

    [SerializeField] SpriteRenderer sr;
    [SerializeField] Color hitColor = Color.red;
    [SerializeField] float hitFlashDuration = 0.1f;
    [SerializeField] GameObject dropBossKey;
    [SerializeField] Transform pointToDrop;
    
    [Header("UI del Jefe (Iconos de Vida)")]
    [SerializeField] GameObject bossHUD;
    [SerializeField] Image[] bossHealthIcons; // Imagenes que componen la barra del boss. 

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();

        // Forzamos a que el HUD empiece sincronizado con la vida actual
        UpdateBossUI();
    }

    // Propiedad que devuelve la vida en un formato de 0.0 a 1.0 (Porcentaje)
    public float HealthPercent => currentHealth / maxHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(HitFlash());

        // 📊 Actualizamos las imágenes de vida solo cuando hay cambios
        UpdateBossUI();

        if (currentHealth <= 0)
            Die();
    }

    // 🛠️ Función encargada de encender/apagar los iconos de vida del Boss
    void UpdateBossUI()
    {
        if (bossHealthIcons == null || bossHealthIcons.Length == 0) return;

        for (int i = 0; i < bossHealthIcons.Length; i++)
        {
            // Si el índice actual de la imagen es menor a la vida restante, se queda activa.
            // Si la vida bajó de ese índice, el icono se apaga.
            bossHealthIcons[i].enabled = i < currentHealth;
        }
    }

    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        sr.color = Color.white;
    }

    void Die()
    {
        GameManager.Instance.OnBossDefeated();
        Instantiate(dropBossKey, pointToDrop.transform.position, Quaternion.identity);
        bossHUD.SetActive(false);
        Destroy(gameObject);
    }
}