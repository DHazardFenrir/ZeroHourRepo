using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class BossIntro : MonoBehaviour
{
    [SerializeField] CinemachineCamera virtualCamera;
    [SerializeField] Transform bossTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] float panDuration = 2f;
    [SerializeField] float holdDuration = 1.5f;
    [SerializeField] CinemachineImpulseSource impulseSource;

    private PlayerMovement playerMovement;

    void OnEnable()
    {
        playerMovement = playerTransform.GetComponent<PlayerMovement>();
        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        // 1. Desactivar input del jugador
        playerMovement.enabled = false;

        // ASEGURARNOS de que el boss no se mueva al iniciar la intro
        bossTransform.GetComponent<BossMovement>().enabled = false;

        // 2. Pan hacia el boss
        virtualCamera.Follow = bossTransform;
        yield return new WaitForSeconds(panDuration);

        // 3. Boss aparece con squash/bounce
        yield return StartCoroutine(BossEntrance());

        // 4. Pausa dramática
        yield return new WaitForSeconds(holdDuration);

        // 5. Regresar al jugador
        virtualCamera.Follow = playerTransform;
        yield return new WaitForSeconds(panDuration);

        // 6. Reactivar input del jugador
        playerMovement.enabled = true;

        // 7. ¡AHORA SÍ! Activamos al boss para que empiece a atacar
        bossTransform.GetComponent<BossMovement>().enabled = true;

        // Destruir este script de intro
        Destroy(this);
    }

    IEnumerator BossEntrance()
    {
        // El boss empieza invisible y diminuto
        SpriteRenderer sr = bossTransform.GetComponent<SpriteRenderer>();
        bossTransform.localScale = Vector3.zero;
        sr.color = new Color(1, 1, 1, 0);

        // Fase 1: Fade in + crece con Overshoot (Sube de 0 a 2.4 para que se pase de su tamaño)
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.3f;
            float scale = Mathf.Lerp(0f, 2.4f, t); 
            bossTransform.localScale = new Vector3(scale, scale, 1f);
            sr.color = new Color(1, 1, 1, t);
            yield return null;
        }

        // Fase 2: Bounce (Regresa del exceso de 2.4 a su tamaño real que es 2.0)
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.15f;
            float scale = Mathf.Lerp(2.4f, 2.0f, t);
            bossTransform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        // Fijamos la escala final exactamente en 2 en lugar de Vector3.one
        bossTransform.localScale = new Vector3(2f, 2f, 1f);
        
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(); // Sacudida de pantalla al asentarse
        }
    }
}