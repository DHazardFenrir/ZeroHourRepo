using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossIntroTrigger : MonoBehaviour
{
    // Arrastra aquí el script BossIntro que está en tu Boss
    [SerializeField] private BossIntro bossIntroScript; 
    [SerializeField] private GameObject bossHUD;
    
    private void Awake()
    {
        if (bossIntroScript != null)
        {
            bossIntroScript.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el que entró a la zona es el jugador
        if (other.CompareTag("Player"))
        {
            if (bossIntroScript != null)
            {
                // Encendemos el script de la intro para que ejecute su Start() y empiece la Corrutina
                StartCoroutine(WaitUntilPlayerIsIn());
            }

            // Opcional: Aquí podrías cerrar las puertas de la sala para encerrar al jugador

            // Destruimos este trigger para que la cinemática no se vuelva a repetir si pasas por ahí
            
        }
    }
    IEnumerator WaitUntilPlayerIsIn()
    {
        yield return new WaitForSeconds(1.5f);
        bossIntroScript.gameObject.SetActive(true);
        bossHUD.SetActive(true);
                bossIntroScript.enabled = true;
    }
}