using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [Header("Prefabs de los PowerUps")]
    [SerializeField] GameObject healPickupPrefab;       // El botiquín común
    [SerializeField] GameObject maxHealthPickupPrefab;  // El contenedor que expande la barra
    
    [Header("Puntos de Spawn")]
    [SerializeField] Transform[] pointToSpawn;

    [Header("Probabilidades (0 a 100)")]
    [Range(0, 100)] [SerializeField] float BaseDropChance = 40f;

    // Llama a este método cuando el enemigo muera
    public void SpawnLoot()
    {
        // Validación extra: Si no asignaste puntos en el Inspector, no hacemos nada para evitar errores
        if (pointToSpawn == null || pointToSpawn.Length == 0)
        {
            Debug.LogWarning("¡No hay puntos de spawn asignados en el HealthSpawner!");
            return;
        }

        // Conseguimos la referencia al Player de forma segura
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth == null) return;

        float randomRoll = Random.Range(0f, 100f);
        
        // ¿Pasó la probabilidad base de tirar algún objeto?
        if (randomRoll <= BaseDropChance)
        {
            
            int randomIndex = Random.Range(0, pointToSpawn.Length);
            Transform chosenPoint = pointToSpawn[randomIndex];

            // Si por alguna razón el objeto en el arreglo se borró o es null, usamos la posición actual como respaldo
            Vector3 spawnPosition = chosenPoint != null ? chosenPoint.position : transform.position;

            // LÓGICA DE FILTRADO INTELIGENTE
            
            // Regla 1: Si está herido y su vida máxima es menor a 15, puede salir CUALQUIERA de los dos
            if (playerHealth.currentHealth < playerHealth.maxHealth && playerHealth.maxHealth < 15)
            {
                // Elegimos al azar entre curar o expandir barra
                GameObject selected = Random.Range(0, 2) == 0 ? healPickupPrefab : maxHealthPickupPrefab;
                
                // CAMBIO: Ahora spawnea en spawnPosition
                Instantiate(selected, spawnPosition, Quaternion.identity);
            }
            // Regla 2: Si ya llegó al límite de 15 de vida máxima, SOLO permitimos botiquines normales de cura
            else if (playerHealth.currentHealth < playerHealth.maxHealth && playerHealth.maxHealth >= 15)
            {
                // CAMBIO: Ahora spawnea en spawnPosition
                Instantiate(healPickupPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
